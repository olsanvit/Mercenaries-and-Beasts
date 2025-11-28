using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Units;

namespace MercenariesAndBeasts.Domain.AI;

public sealed class ExpeditionMercenaryDefinition
{
    public string NameEn { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;

    public ElementType Element { get; set; }
    public int BaseLevel { get; set; }

    public StatBlock Stats { get; set; } = new();
}