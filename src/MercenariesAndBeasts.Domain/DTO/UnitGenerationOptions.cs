
using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.Dto;
public enum UnitRole
{
    Bruiser = 1,
    Tank = 2,
    Assassin = 3,
    Caster = 4,
    Support = 5
}
public sealed class UnitGenerationOptions
{
    public bool IsMercenary { get; set; }

    public ElementType Element { get; set; }

    public ExpeditionStageType? ExpeditionStageType { get; set; }
    public DungeonStageType? DungeonStageType { get; set; }

    public UnitRole Role { get; set; }

    public int TargetLevel { get; set; }
}