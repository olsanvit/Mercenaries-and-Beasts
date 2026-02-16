
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Items
{
public class ItemEffect : BaseGuid
{
    public ItemEffectType EffectType { get; set; }
    public double Value { get; set; }              // např. +5%, +10 ATK
    public TimeSpan? Duration { get; set; }        // pokud je časový buff
    public string? TargetStat { get; set; }        // "ATK", "DEF", "HP", "FireDamage"

    public QualityTier MinQuality { get; set; }
}
public static class StatRegistry
{
    // Metadata (pro UI + validace + prompty)
    public sealed record StatMeta(
        StatId Id,
        string Name,          // pro UI / LLM
        bool IsPercentLike    // jestli se má formátovat jako % (0..1), nebo je to "velké číslo"
    );

    public static readonly StatMeta[] All =
    {
        new(StatId.MaxHp, "Max HP", false),
        new(StatId.Attack, "Attack", false),
        new(StatId.Defense, "Defense", false),
        new(StatId.Speed, "Speed", false),

        new(StatId.CriticalChance, "Crit Chance", true),
        new(StatId.CriticalMultiplier, "Crit Multiplier", false), // tohle je spíš násobič než %
        new(StatId.ArmorPenetration, "Armor Penetration", true),

        new(StatId.Accuracy, "Accuracy", false), // u tebe baseline 1.0, není to % v 0..1
        new(StatId.Evasion, "Evasion", true),
        new(StatId.BlockChance, "Block Chance", true),

        new(StatId.DamageBonus, "Damage Bonus", true),
        new(StatId.DamageReduction, "Damage Reduction", true),
        new(StatId.TrueDamageBonus, "True Damage Bonus", true),

        new(StatId.TurnMeterGain, "Turn Meter Gain", true),

        new(StatId.LifeSteal, "Life Steal", true),
        new(StatId.HpRegen, "HP Regen", true),
        new(StatId.EnergyRegen, "Energy Regen", true),

        new(StatId.BleedChance, "Bleed Chance", true),
        new(StatId.PoisonChance, "Poison Chance", true),
        new(StatId.BurnChance, "Burn Chance", true),
        new(StatId.ShockChance, "Shock Chance", true),
        new(StatId.FreezeChance, "Freeze Chance", true),

        new(StatId.StatusDurationBonus, "Status Duration Bonus", true),
        new(StatId.StatusResistance, "Status Resistance", true),
        new(StatId.DotDamageBonus, "DOT Damage Bonus", true),
        new(StatId.DotDamageReduction, "DOT Damage Reduction", true),
        new(StatId.CleanseChance, "Cleanse Chance", true),
        new(StatId.StatusPotency, "Status Potency", true),
    };

    // Getter
    public static double Get(StatBlock s, StatId id) => id switch
    {
        StatId.MaxHp => s.MaxHp,
        StatId.Attack => s.Attack,
        StatId.Defense => s.Defense,
        StatId.Speed => s.Speed,

        StatId.CriticalChance => s.CriticalChance,
        StatId.CriticalMultiplier => s.CriticalMultiplier,
        StatId.ArmorPenetration => s.ArmorPenetration,

        StatId.Accuracy => s.Accuracy,
        StatId.Evasion => s.Evasion,
        StatId.BlockChance => s.BlockChance,

        StatId.DamageBonus => s.DamageBonus,
        StatId.DamageReduction => s.DamageReduction,
        StatId.TrueDamageBonus => s.TrueDamageBonus,

        StatId.TurnMeterGain => s.TurnMeterGain,

        StatId.LifeSteal => s.LifeSteal,
        StatId.HpRegen => s.HpRegen,
        StatId.EnergyRegen => s.EnergyRegen,

        StatId.BleedChance => s.BleedChance,
        StatId.PoisonChance => s.PoisonChance,
        StatId.BurnChance => s.BurnChance,
        StatId.ShockChance => s.ShockChance,
        StatId.FreezeChance => s.FreezeChance,

        StatId.StatusDurationBonus => s.StatusDurationBonus,
        StatId.StatusResistance => s.StatusResistance,
        StatId.DotDamageBonus => s.DotDamageBonus,
        StatId.DotDamageReduction => s.DotDamageReduction,
        StatId.CleanseChance => s.CleanseChance,
        StatId.StatusPotency => s.StatusPotency,

        _ => 0
    };

    // Setter/Add (zde rovnou přičítání, protože bonusy jsou "additive")
    public static void Add(StatBlock s, StatId id, double delta)
    {
        switch (id)
        {
            case StatId.MaxHp: s.MaxHp += (float)delta; break;
            case StatId.Attack: s.Attack += (float)delta; break;
            case StatId.Defense: s.Defense += (float)delta; break;
            case StatId.Speed: s.Speed += (float)delta; break;

            case StatId.CriticalChance: s.CriticalChance += delta; break;
            case StatId.CriticalMultiplier: s.CriticalMultiplier += delta; break;
            case StatId.ArmorPenetration: s.ArmorPenetration += delta; break;

            case StatId.Accuracy: s.Accuracy += delta; break;
            case StatId.Evasion: s.Evasion += delta; break;
            case StatId.BlockChance: s.BlockChance += delta; break;

            case StatId.DamageBonus: s.DamageBonus += delta; break;
            case StatId.DamageReduction: s.DamageReduction += delta; break;
            case StatId.TrueDamageBonus: s.TrueDamageBonus += delta; break;

            case StatId.TurnMeterGain: s.TurnMeterGain += delta; break;

            case StatId.LifeSteal: s.LifeSteal += delta; break;
            case StatId.HpRegen: s.HpRegen += delta; break;
            case StatId.EnergyRegen: s.EnergyRegen += delta; break;

            case StatId.BleedChance: s.BleedChance += delta; break;
            case StatId.PoisonChance: s.PoisonChance += delta; break;
            case StatId.BurnChance: s.BurnChance += delta; break;
            case StatId.ShockChance: s.ShockChance += delta; break;
            case StatId.FreezeChance: s.FreezeChance += delta; break;

            case StatId.StatusDurationBonus: s.StatusDurationBonus += delta; break;
            case StatId.StatusResistance: s.StatusResistance += delta; break;
            case StatId.DotDamageBonus: s.DotDamageBonus += delta; break;
            case StatId.DotDamageReduction: s.DotDamageReduction += delta; break;
            case StatId.CleanseChance: s.CleanseChance += delta; break;
            case StatId.StatusPotency: s.StatusPotency += delta; break;
        }
    }
    
}
}