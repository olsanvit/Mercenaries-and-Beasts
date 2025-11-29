using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Units;

namespace MercenariesAndBeasts.Domain.AI;

public sealed class DungeonMonsterDefinition
{
    public string Code { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;

    public string Element { get; set; } = string.Empty;
    public int BaseLevel { get; set; }

    public StatBlock Stats { get; set; } = new();
}