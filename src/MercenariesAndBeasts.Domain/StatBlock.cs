using System.Reflection;
using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.Combat;

public class StatBlock
{
    // =========================================================
    // CORE (primární staty – “velká čísla”)
    // =========================================================

    /// <summary>Max HP jednotky / itemu. Zvyšuje přežití.</summary>
    public float MaxHp { get; set; } = 0;

    /// <summary>Armor – “tvrdá” obranná vrstva nad DEF (např. pláty, carapace, štíty).</summary>
    public float Armor { get; set; }= 0;

    /// <summary>Útočná síla – zvyšuje damage (podle tvého damage vzorce).</summary>
    public float Attack { get; set; }= 0;

    /// <summary>Obranná síla – snižuje incoming damage (podle tvého damage vzorce).</summary>
    public float Defense { get; set; }= 0;

    /// <summary>Rychlost – určuje pořadí tahů / tempo (initiative/turn order).</summary>
    public float Speed { get; set; }= 0;

    /// <summary>Max energie/mana – kapacita resource (navazuje na EnergyRegen).</summary>
    public float MaxEnergy { get; set; }= 0;

    // =========================================================
    // CRIT / PEN (kritiky a průraz)
    // =========================================================

    /// <summary>Šance na kritický zásah. 0..1 (0.25 = 25%).</summary>
    public double CriticalChance { get; set; }= 0;

    /// <summary>Kritický násobič. Typicky 1.5 = 150% damage.</summary>
    public double CriticalMultiplier { get; set; }= 0;

    /// <summary>Průraz armoru/defense. 0..1 (0.2 = ignoruje 20%).</summary>
    public double ArmorPenetration { get; set; }= 0;

    // =========================================================
    // HIT / AVOID (zásah, vyhnutí, blok)
    // =========================================================

    /// <summary>Přesnost. 1.0 = baseline, může být i > 1.0.</summary>
    public double Accuracy { get; set; }= 0;

    /// <summary>Vyhýbání. Obvykle 0..1.</summary>
    public double Evasion { get; set; }= 0;

    /// <summary>Šance na blok. 0..1.</summary>
    public double BlockChance { get; set; }= 0;

    /// <summary>Šance na counterattack po zásahu / po bloknutí. 0..1.</summary>
    public double CounterChance { get; set; }= 0;

    // =========================================================
    // DAMAGE MODIFIERS (globální modifikátory damage)
    // =========================================================

    /// <summary>Bonus k damage. Obvykle 0..1.</summary>
    public double DamageBonus { get; set; }= 0;

    /// <summary>Redukce damage (incoming). Obvykle 0..1.</summary>
    public double DamageReduction { get; set; }= 0;

    /// <summary>Bonus k true damage / části damage co ignoruje obranu. 0..1.</summary>
    public double TrueDamageBonus { get; set; }= 0;

    /// <summary>“Thorns” – část přijatého dmg se vrací útočníkovi. 0..1.</summary>
    public double Thorns { get; set; }= 0;

    // =========================================================
    // TEMPO (rychlost hry bez cooldownů)
    // =========================================================

    /// <summary>Bonus k získávání turn meteru / rychlejší nabíhání tahu. 0..1.</summary>
    public double TurnMeterGain { get; set; }= 0;

    /// <summary>Snížení energy costu schopností/akcí (bez cooldownů). 0..1.</summary>
    public double EnergyCostReduction { get; set; }= 0;

    // =========================================================
    // SUSTAIN (udržitelnost, self-heal, resource regen)
    // =========================================================

    /// <summary>Lifesteal – část uděleného damage se léčí. 0..1.</summary>
    public double LifeSteal { get; set; }= 0;

    /// <summary>HP regen (per turn / tick). 0..1.</summary>
    public double HpRegen { get; set; }= 0;

    /// <summary>Energy regen (mana/energy). 0..1.</summary>
    public double EnergyRegen { get; set; }= 0;

    /// <summary>Bonus k veškerému léčení (lifesteal/regen/heal). 0..1.</summary>
    public double HealingBonus { get; set; }= 0;

    /// <summary>Redukce přijatého léčení (wounds/anti-heal). 0..1.</summary>
    public double HealingReduction { get; set; }= 0;

    /// <summary>Bonus k síle štítů/barrierů, pokud je používáš. 0..1.</summary>
    public double ShieldBonus { get; set; }= 0;

    // =========================================================
    // ELEMENT (typový – ne-sčítá se jako číslo)
    // =========================================================

    /// <summary>Element jako string (např. "Fire"). Kvůli serializaci / LLM.</summary>
    public string Element { get; set; } = ElementType.None.ToString();

    // =========================================================
    // STATUS EFFECTS (šance + obrana proti nim + DOT)
    // =========================================================

    /// <summary>Šance aplikovat Bleed. 0..1.</summary>
    public double BleedChance { get; set; }= 0;

    /// <summary>Šance aplikovat Poison. 0..1.</summary>
    public double PoisonChance { get; set; }= 0;

    /// <summary>Šance aplikovat Burn. 0..1.</summary>
    public double BurnChance { get; set; }= 0;

    /// <summary>Šance aplikovat Shock. 0..1.</summary>
    public double ShockChance { get; set; }= 0;

    /// <summary>Šance aplikovat Freeze. 0..1.</summary>
    public double FreezeChance { get; set; }= 0;

    /// <summary>Bonus k délce statusů (např. +20% duration). 0..1.</summary>
    public double StatusDurationBonus { get; set; }= 0;

    /// <summary>Odolnost vůči statusům. 0..1.</summary>
    public double StatusResistance { get; set; }= 0;

    /// <summary>Bonus k DOT damage (bleed/poison/burn/shock). 0..1.</summary>
    public double DotDamageBonus { get; set; }= 0;

    /// <summary>Redukce DOT damage (incoming). 0..1.</summary>
    public double DotDamageReduction { get; set; }= 0;

    /// <summary>Šance očistit (cleanse) negativní efekty. 0..1.</summary>
    public double CleanseChance { get; set; }= 0;

    /// <summary>Potency statusů – knob pro status build (šance/proc/rezist-check). 0..1.</summary>
    public double StatusPotency { get; set; }= 0;

    // =========================================================
    // PRESET
    // =========================================================

    public static StatBlock Zero => new()
    {
        // 1.5 = default crit multiplier (150% dmg)
        CriticalMultiplier = 1.5,

        // 1.0 = baseline hit chance scaling (tvůj “normál”)
        Accuracy = 1.0,

        // element jako string – default None
        Element = ElementType.None.ToString()
    };
}

public static class StatBlockExtensions
{
    // cache reflection (rychlejší, nepočítá to pořád dokola)
    private static readonly PropertyInfo[] NumericProps = typeof(StatBlock)
        .GetProperties(BindingFlags.Instance | BindingFlags.Public)
        .Where(p => p.CanRead && p.CanWrite)
        .Where(p =>
            p.PropertyType == typeof(int) ||
            p.PropertyType == typeof(long) ||
            p.PropertyType == typeof(float) ||
            p.PropertyType == typeof(double))
        .ToArray();

    /// <summary>
    /// Vrátí NOVÝ StatBlock = a + b (bez mutace).
    /// </summary>
    public static StatBlock Plus(this StatBlock a, StatBlock? b)
    {
        var res = Clone(a);
        res.AddInPlace(b);
        return res;
    }

    /// <summary>
    /// Přičte b do a (mutace). Pokud b == null, nic nedělá.
    /// Element řeší: pokud a.Element == None a b.Element != None, vezme b.Element.
    /// </summary>
    public static void AddInPlace(this StatBlock a, StatBlock? b)
    {
        if (a is null) throw new ArgumentNullException(nameof(a));
        if (b is null) return;

        foreach (var p in NumericProps)
        {
            if (p.PropertyType == typeof(double))
            {
                var av = (double)p.GetValue(a)!;
                var bv = (double)p.GetValue(b)!;
                p.SetValue(a, av + bv);
            }
            else if (p.PropertyType == typeof(float))
            {
                var av = (float)p.GetValue(a)!;
                var bv = (float)p.GetValue(b)!;
                p.SetValue(a, av + bv);
            }
            else if (p.PropertyType == typeof(int))
            {
                var av = (int)p.GetValue(a)!;
                var bv = (int)p.GetValue(b)!;
                p.SetValue(a, av + bv);
            }
            else if (p.PropertyType == typeof(long))
            {
                var av = (long)p.GetValue(a)!;
                var bv = (long)p.GetValue(b)!;
                p.SetValue(a, av + bv);
            }
        }

        if (a.Element == ElementType.None.ToString() && b.Element != ElementType.None.ToString())
            a.Element = b.Element;
    }

    /// <summary>
    /// Převede StatBlock na (Name, Value) list - použitelné pro foreach v UI.
    /// Zahrnuje numeric + enum.
    /// </summary>
    public static (string Name, object? Value)[] ToKeyValues(this StatBlock? sb)
    {
        if (sb is null) return Array.Empty<(string, object?)>();

        var t = typeof(StatBlock);

        var props = t.GetProperties(BindingFlags.Instance | BindingFlags.Public)
            .Where(p => p.CanRead)
            .Where(p =>
                p.PropertyType == typeof(int) ||
                p.PropertyType == typeof(long) ||
                p.PropertyType == typeof(float) ||
                p.PropertyType == typeof(double) ||
                p.PropertyType.IsEnum)
            .OrderBy(p => p.Name)
            .ToArray();

        var arr = new (string, object?)[props.Length];
        for (int i = 0; i < props.Length; i++)
            arr[i] = (props[i].Name, props[i].GetValue(sb));

        return arr;
    }

    /// <summary>
    /// Ruční clone bez reflection.
    /// Když přidáš nový stat do StatBlock, sem ho přidej taky.
    /// </summary>
    public static StatBlock Clone(this StatBlock s)
{
    if (s is null) throw new ArgumentNullException(nameof(s));

    return new StatBlock
    {
        // CORE
        MaxHp = s.MaxHp,
        Armor = s.Armor,
        Attack = s.Attack,
        Defense = s.Defense,
        Speed = s.Speed,
        MaxEnergy = s.MaxEnergy,

        // CRIT / PEN
        CriticalChance = s.CriticalChance,
        CriticalMultiplier = s.CriticalMultiplier,
        ArmorPenetration = s.ArmorPenetration,

        // HIT / AVOID
        Accuracy = s.Accuracy,
        Evasion = s.Evasion,
        BlockChance = s.BlockChance,
        CounterChance = s.CounterChance,

        // DAMAGE MODIFIERS
        DamageBonus = s.DamageBonus,
        DamageReduction = s.DamageReduction,
        TrueDamageBonus = s.TrueDamageBonus,
        Thorns = s.Thorns,

        // TEMPO
        TurnMeterGain = s.TurnMeterGain,
        EnergyCostReduction = s.EnergyCostReduction,

        // SUSTAIN
        LifeSteal = s.LifeSteal,
        HpRegen = s.HpRegen,
        EnergyRegen = s.EnergyRegen,
        HealingBonus = s.HealingBonus,
        HealingReduction = s.HealingReduction,
        ShieldBonus = s.ShieldBonus,

        // ELEMENT
        Element = string.IsNullOrWhiteSpace(s.Element)
            ? ElementType.None.ToString()
            : s.Element,

        // STATUS
        BleedChance = s.BleedChance,
        PoisonChance = s.PoisonChance,
        BurnChance = s.BurnChance,
        ShockChance = s.ShockChance,
        FreezeChance = s.FreezeChance,
        StatusDurationBonus = s.StatusDurationBonus,
        StatusResistance = s.StatusResistance,
        DotDamageBonus = s.DotDamageBonus,
        DotDamageReduction = s.DotDamageReduction,
        CleanseChance = s.CleanseChance,
        StatusPotency = s.StatusPotency
    };
}
}