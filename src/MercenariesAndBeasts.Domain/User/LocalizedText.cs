using System.ComponentModel.DataAnnotations;

namespace MercenariesAndBeasts.Domain;
public class LocalizedText
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(8)]
    public string Culture { get; set; } = "en"; // en, cs, de, fr, es, ja, zh-Hans…

    [Required, MaxLength(64)]
    public string EntityType { get; set; } = string.Empty; // Dungeon, Monster, Item…

    public Guid EntityId { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    public string? Description { get; set; }
    [MaxLength(64)]
    public string Code { get; set; } = string.Empty;

    public DateTime UpdatedUtc { get; set; } = DateTime.UtcNow;
}
public sealed class LocalizedNameResult
{
    public string NameEn { get; set; } = string.Empty;
    public string? DescriptionEn { get; set; }

    // ISO-639-1 (nebo i BCP47), např.: "cs", "de", "fr", "es", "ja", "zh-Hans"
    public Dictionary<string, LocalizedLocaleDto> Locales { get; set; } = new();
}

public sealed class LocalizedLocaleDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}