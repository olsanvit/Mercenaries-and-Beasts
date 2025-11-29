

using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.AI;

public sealed class DungeonStageDefinition
{
    public string Code { get; set; } = string.Empty;
    public int StageNumber { get; set; }            // 1–10
    
    public string StageType { get; set; } = string.Empty;

    public DungeonMonsterDefinition Monster { get; set; } = new();
}