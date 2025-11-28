

using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.AI;

public sealed class DungeonStageDefinition
{
    public int StageNumber { get; set; }            // 1–10
    public DungeonStageType StageType { get; set; } // DS1_... DS10_...

    public DungeonMonsterDefinition Monster { get; set; } = new();
}