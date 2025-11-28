
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Items{
public class ItemEffect : BaseGuid
{
    public ItemEffectType EffectType { get; set; }
    public double Value { get; set; }              // např. +5%, +10 ATK
    public TimeSpan? Duration { get; set; }        // pokud je časový buff
    public string? TargetStat { get; set; }        // "ATK", "DEF", "HP", "FireDamage"
}
}