using System.Globalization;
using System.Text.RegularExpressions;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Items;

namespace MercenariesAndBeasts.Domain.Utils;

public static class ItemTemplateSlotHelpers
{
    /// <summary>
    /// Vrátí slot itemu (merc nebo beast). Pokud je non-gear => null.
    /// </summary>
    public static ItemEquipSlot? Slot(this ItemTemplate t)
        => t.MercenarySlot ?? t.MonsterSlot;

    /// <summary>
    /// SlotKey podle tvého pravidla:
    /// merc: 1..11
    /// beast: 101..111 (100 + 1..11)
    /// </summary>
    public static int SlotKey(this ItemTemplate t)
    {
        if (t.MercenarySlot.HasValue) return (int)t.MercenarySlot.Value;          // 1..11
        if (t.MonsterSlot.HasValue)  return 100 + (int)t.MonsterSlot.Value;       // 101..111
        return 0;
    }

    public static bool IsMercGear(this ItemTemplate t) => t.MercenarySlot.HasValue;
    public static bool IsBeastGear(this ItemTemplate t) => t.MonsterSlot.HasValue;

    public static ItemOwnerKind OwnerKind(this ItemTemplate t)
        => t.MercenarySlot.HasValue ? ItemOwnerKind.Mercenary
         : t.MonsterSlot.HasValue ? ItemOwnerKind.Beast
         : ItemOwnerKind.None;
         public static StatId ParseOrDefault(string? s, StatId fallback = StatId.Attack)
    {
        if (string.IsNullOrWhiteSpace(s)) return fallback;

        // normalizace: "Max HP" -> "MaxHp", "DOT Damage Bonus" -> "DotDamageBonus"
        var cleaned = Regex.Replace(s.Trim(), @"[^A-Za-z0-9]+", " ");
        cleaned = CultureInfo.InvariantCulture.TextInfo.ToTitleCase(cleaned.ToLowerInvariant())
            .Replace(" ", "");

        // nejdřív přímý enum parse
        if (Enum.TryParse<StatId>(s.Trim(), ignoreCase: true, out var id))
            return id;

        if (Enum.TryParse<StatId>(cleaned, ignoreCase: true, out id))
            return id;

        // pár synonym/aliasů (podle potřeb)
        return cleaned switch
        {
            "Hp" or "Health" or "MaxHealth" => StatId.MaxHp,
            "Def" => StatId.Defense,
            "Pen" or "ArmorPen" => StatId.ArmorPenetration,
            _ => fallback
        };
    }
}

public static class ItemEquipSlotHelpers
{
    // tyhle extension metody ti chyběly v OnboardingService
    public static bool IsMercenarySlot(this ItemEquipSlot s)
        => (int)s >= 1 && (int)s <= 99;

    public static bool IsBeastSlot(this ItemEquipSlot s)
        => (int)s >= 100 && (int)s <= 199; // Beast_Fang..Beast_Core (100..110)

    // Když chceš dělat slotKey přímo ze slotu:
    public static int ToMercSlotKey(this ItemEquipSlot s) => (int)s;
    public static int ToBeastSlotKey(this ItemEquipSlot s) => 100 + ((int)s - 99); // nepoužívej pokud máš Beast 100..110 jako už hotové
}
public static class ElementEffectiveness
{
    // ručně definované výjimky
    private static readonly Dictionary<(ElementType atk, ElementType def), int> Overrides =
        new()
        {
            // 🔥 FIRE
            {(ElementType.Fire, ElementType.Water), 70},
            {(ElementType.Fire, ElementType.Ice), 130},
            {(ElementType.Fire, ElementType.Nature), 130},
            {(ElementType.Fire, ElementType.Metal), 70},

            // 💧 WATER
            {(ElementType.Water, ElementType.Fire), 130},
            {(ElementType.Water, ElementType.Lightning), 70},
            {(ElementType.Water, ElementType.Poison), 130},

            // ❄ ICE
            {(ElementType.Ice, ElementType.Fire), 70},
            {(ElementType.Ice, ElementType.Nature), 130},
            {(ElementType.Ice, ElementType.Metal), 70},

            // 🌿 NATURE
            {(ElementType.Nature, ElementType.Fire), 70},
            {(ElementType.Nature, ElementType.Poison), 70},
            {(ElementType.Nature, ElementType.Water), 130},

            // ⚡ LIGHTNING
            {(ElementType.Lightning, ElementType.Water), 130},
            {(ElementType.Lightning, ElementType.Earth), 70},

            // 🪨 EARTH
            {(ElementType.Earth, ElementType.Lightning), 130},
            {(ElementType.Earth, ElementType.Air), 70},

            // 🌪 AIR
            {(ElementType.Air, ElementType.Earth), 130},
            {(ElementType.Air, ElementType.Ice), 70},

            // ☠ POISON
            {(ElementType.Poison, ElementType.Nature), 130},
            {(ElementType.Poison, ElementType.Metal), 70},

            // 🔩 METAL
            {(ElementType.Metal, ElementType.Ice), 130},
            {(ElementType.Metal, ElementType.Fire), 130},

            // 🌑 SHADOW
            {(ElementType.Shadow, ElementType.Light), 130},

            // ✨ LIGHT
            {(ElementType.Light, ElementType.Shadow), 130},

            // 🌀 ARCANE
            {(ElementType.Arcane, ElementType.Physical), 130},

            // 🕳 VOID
            {(ElementType.Void, ElementType.Arcane), 130},
            {(ElementType.Void, ElementType.Light), 70},

            // 🔥 PLASMA
            {(ElementType.Plasma, ElementType.Metal), 130},
            {(ElementType.Plasma, ElementType.Water), 70},
        };

    /// <summary>
    /// Vrátí účinnost v procentech (100 = neutrální)
    /// </summary>
    public static int GetPercent(ElementType attacker, ElementType defender)
    {
        if (attacker == ElementType.None || defender == ElementType.None)
            return 100;

        if (attacker == defender)
            return 100;

        return Overrides.TryGetValue((attacker, defender), out var v)
            ? v
            : 100;
    }

    /// <summary>
    /// Vrátí multiplier (např. 1.3 / 0.7)
    /// </summary>
    public static double GetMultiplier(ElementType attacker, ElementType defender)
        => GetPercent(attacker, defender) / 100.0;
}