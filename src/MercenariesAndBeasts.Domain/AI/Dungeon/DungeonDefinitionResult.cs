namespace MercenariesAndBeasts.Domain.AI;

public sealed class DungeonGenerationResult
{
    public string Code { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;

    public int MinLevel { get; set; }
    public int MaxLevel { get; set; }

    public IReadOnlyList<DungeonStageDefinition> Stages { get; set; } = Array.Empty<DungeonStageDefinition>();
}