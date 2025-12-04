namespace MercenariesAndBeasts.Domain.AI;

public sealed class LocalizedNameResult
{
    // English – základ / můžeš lehce „polishnout“
    public string NameEn { get; set; } = string.Empty;
    public string? DescriptionEn { get; set; }

    // Czech
    public string? NameCs { get; set; }
    public string? DescriptionCs { get; set; }

    // German
    public string? NameDe { get; set; }
    public string? DescriptionDe { get; set; }
}