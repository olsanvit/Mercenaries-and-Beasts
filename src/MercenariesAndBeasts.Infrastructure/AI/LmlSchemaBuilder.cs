using System.Reflection;
using System.Text;

namespace MercenariesAndBeasts.Infrastructure.AI;

// Jednoduchý runtime "schema builder" pro LLM
public static class LlmSchemaBuilder
{
    public static string BuildJsonShape<T>() => BuildJsonShape(typeof(T));

    public static string BuildJsonShape(Type type)
    {
        var visited = new HashSet<Type>();
        var sb = new StringBuilder();
        BuildTypeShape(type, sb, 0, visited);
        return sb.ToString();
    }

    public static string BuildEnumValues<TEnum>() where TEnum : Enum
    {
        var names = Enum.GetNames(typeof(TEnum));
        return "[\"" + string.Join("\",\"", names) + "\"]";
    }

    private static void BuildTypeShape(Type type, StringBuilder sb, int indent, HashSet<Type> visited)
    {
        string Indent(int i) => new string(' ', i * 2);

        if (visited.Contains(type))
        {
            sb.Append("\"<recursive>\"");
            return;
        }

        if (type == typeof(string))
        {
            sb.Append("\"string\"");
            return;
        }
        if (type.IsPrimitive || type == typeof(decimal))
        {
            sb.Append("\"number\"");
            return;
        }
        if (type.IsEnum)
        {
            sb.Append("\"enum\"");
            return;
        }

        // Kolekce / pole → [ shape(T) ]
        var elementType = GetEnumerableElementType(type);
        if (elementType != null && elementType != typeof(byte)) // byte[] → radši neřešit jako kolekci
        {
            sb.AppendLine("[");
            BuildTypeShape(elementType, sb, indent + 1, visited);
            sb.AppendLine();
            sb.Append(Indent(indent));
            sb.Append("]");
            return;
        }

        // Složitý typ → objekt
        visited.Add(type);

        sb.AppendLine("{");
        var props = type
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead)
            .ToArray();

        for (int i = 0; i < props.Length; i++)
        {
            var p = props[i];
            sb.Append(Indent(indent + 1));
            sb.Append('\"');
            sb.Append(char.ToLowerInvariant(p.Name[0]) + p.Name.Substring(1));
            sb.Append("\": ");

            BuildTypeShape(p.PropertyType, sb, indent + 1, visited);

            if (i < props.Length - 1)
                sb.Append(',');

            sb.AppendLine();
        }

        sb.Append(Indent(indent));
        sb.Append('}');

        visited.Remove(type);
    }

    private static Type? GetEnumerableElementType(Type type)
    {
        if (type.IsArray)
            return type.GetElementType();

        if (type.IsGenericType &&
            (typeof(IEnumerable<>).IsAssignableFrom(type.GetGenericTypeDefinition())
             || type.GetInterfaces().Any(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>))))
        {
            return type.GetGenericArguments()[0];
        }

        var ienum = type.GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEnumerable<>));

        return ienum?.GetGenericArguments()[0];
    }
}