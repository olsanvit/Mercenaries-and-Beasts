using System.Globalization;

namespace MercenariesAndBeasts.Domain.Localization;

public static class CountryCatalog
{
    public sealed record CountryOpt(string Code, string Name);

    public static IReadOnlyList<CountryOpt> GetAll()
    {
        // ISO2 -> EnglishName
        var map = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        foreach (var c in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
        {
            try
            {
                var r = new RegionInfo(c.Name);
                var code = r.TwoLetterISORegionName?.ToUpperInvariant();

                if (!string.IsNullOrWhiteSpace(code) && code.Length == 2)
                    map[code] = r.EnglishName;
            }
            catch
            {
                // ignore invalid cultures
            }
        }

        return map
            .Select(kv => new CountryOpt(kv.Key, kv.Value))
            .OrderBy(x => x.Name)
            .ToList();
    }

    public static bool IsValidIso2(string? code)
        => !string.IsNullOrWhiteSpace(code)
           && code.Length == 2
           && code.All(char.IsLetter);
}