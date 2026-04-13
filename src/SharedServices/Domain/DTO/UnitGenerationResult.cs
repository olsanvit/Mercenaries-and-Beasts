

using MercenariesAndBeasts.Domain.Combat;

namespace MercenariesAndBeasts.Domain.Dto;
public sealed class UnitGenerationResult
{
    public string NameEn { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;

    public int BaseLevel { get; set; }
    public StatBlock Stats { get; set; } = new();

    public string? SuggestedImagePrompt { get; set; }
}