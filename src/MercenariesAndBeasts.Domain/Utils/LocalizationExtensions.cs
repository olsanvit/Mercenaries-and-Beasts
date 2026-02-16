using System.Globalization;

namespace MercenariesAndBeasts.Domain.Utils;

public static class LocalizationExtensions
{
    // culture = CurrentUICulture
    public static string NameForCurrentCulture(
        this BaseGuid entity,
        IEnumerable<LocalizedText>? locs,
        CultureInfo? culture = null)
    {
        if (entity is null) return string.Empty;
        if (locs is null) return entity.NameEn;

        culture ??= CultureInfo.CurrentUICulture;

        // prefer přesný match (např. "zh-Hans"), pak fallback na TwoLetter
        var full = culture.Name; // "cs-CZ"
        var two = culture.TwoLetterISOLanguageName; // "cs"

        var type = entity.GetType().Name;

        var hit =
            locs.FirstOrDefault(x => x.EntityType == type && x.EntityId == entity.Id && x.Culture == full)?.Name
            ?? locs.FirstOrDefault(x => x.EntityType == type && x.EntityId == entity.Id && x.Culture == two)?.Name;

        return string.IsNullOrWhiteSpace(hit) ? entity.NameEn : hit;
    }

    public static string DescriptionForCurrentCulture(
        this BaseGuid entity,
        IEnumerable<LocalizedText>? locs,
        CultureInfo? culture = null)
    {
        if (entity is null) return string.Empty;
        if (locs is null) return entity.DescriptionEn ?? string.Empty;

        culture ??= CultureInfo.CurrentUICulture;

        var full = culture.Name;
        var two = culture.TwoLetterISOLanguageName;

        var type = entity.GetType().Name;

        var hit =
            locs.FirstOrDefault(x => x.EntityType == type && x.EntityId == entity.Id && x.Culture == full)?.Description
            ?? locs.FirstOrDefault(x => x.EntityType == type && x.EntityId == entity.Id && x.Culture == two)?.Description;

        return string.IsNullOrWhiteSpace(hit) ? (entity.DescriptionEn ?? string.Empty) : hit!;
    }
    public sealed class LocalizationBatchResult
{
    public IReadOnlyList<LocalizationEntry> Items { get; set; } = Array.Empty<LocalizationEntry>();
}

public sealed class LocalizationEntry
{
    public string Culture { get; set; } = "en";  // "cs", "de", "fr"...
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public static string NormalizeCulture(string cultureName)
{
    if (string.IsNullOrWhiteSpace(cultureName))
        return "en";

    cultureName = cultureName.Trim();

    // Keep full BCP47 tag for zh-Hans
    if (cultureName.Equals("zh-Hans", StringComparison.OrdinalIgnoreCase))
        return "zh-Hans";

    // cs-CZ -> cs, de-DE -> de, pt-BR -> pt, ...
    var dash = cultureName.IndexOf('-');
    return dash > 0
        ? cultureName[..dash].ToLowerInvariant()
        : cultureName.ToLowerInvariant();
}
}