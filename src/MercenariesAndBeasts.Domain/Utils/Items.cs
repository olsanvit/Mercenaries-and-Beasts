using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Utils;
using MercenariesAndBeasts.Items;

namespace MercenariesAndBeasts.Domain.Items
{
    /// <summary>
    /// ItemTemplate = "šablona itemu" (definice / katalog itemů).
    ///
    /// - Reálný item v inventáři je PlayerItem (má Level, Quality, BonusStats...).
    /// - PlayerItem vždy odkazuje na ItemTemplate (TemplateId).
    ///
    /// V tomhle designu NENÍ ItemType – "co to je" určujeme kombinací:
    /// - OwnerKind + Slot => Gear (equippovatelný item)
    /// - GrantedXxxTemplateId => Summon item (contract/egg)
    /// - jinak => misc item (materiály, měny, consumables… přijdou později)
    /// </summary>
    public class ItemTemplate : BaseGuid
    {
        // ------------------------------------------------------------
        // 1) OWNER (kdo může item použít / equipnout)
        // ------------------------------------------------------------

        /// <summary>
        /// Pro koho je item určený.
        ///
        /// - None      => item není gear (typicky: materiál, měna, consumable...)
        /// - Mercenary => gear / upgrade určený pro Mercenary
        /// - Beast     => gear / upgrade určený pro Beast
        /// </summary>
        public ItemOwnerKind OwnerKind { get; set; } = ItemOwnerKind.None;

        public ElementType Element { get; set; } = ElementType.None;
        public List<ItemUpgradeResource> UpgradeResources { get; set; } = new();
        /// <summary>
        /// Pohodlné zkratky pro čitelnost (UI, filtry).
        /// </summary>
        public bool IsForMercenary => OwnerKind == ItemOwnerKind.Mercenary;

        /// <summary>
        /// Pohodlné zkratky pro čitelnost (UI, filtry).
        /// </summary>
        public bool IsForBeast => OwnerKind == ItemOwnerKind.Beast;

        // ------------------------------------------------------------
        // 2) EQUIPMENT SLOT (pokud je item gear)
        // ------------------------------------------------------------

        /// <summary>
        /// Pokud je vyplněno => item je gear pro Mercenary a patří do tohoto slotu.
        ///
        /// Pravidlo:
        /// - Pokud MercenarySlot != null => OwnerKind musí být Mercenary
        /// - MercenarySlot a MonsterSlot nesmí být oba vyplněné zároveň
        /// </summary>
        public ItemEquipSlot? MercenarySlot { get; set; }

        /// <summary>
        /// Pokud je vyplněno => item je gear pro Beast a patří do tohoto slotu.
        ///
        /// Pravidlo:
        /// - Pokud MonsterSlot != null => OwnerKind musí být Beast
        /// - MercenarySlot a MonsterSlot nesmí být oba vyplněné zároveň
        /// </summary>
        public ItemEquipSlot? MonsterSlot { get; set; }

        // ------------------------------------------------------------
        // 3) STATS (základní staty šablony)
        // ------------------------------------------------------------

        /// <summary>
        /// Základní staty itemu.
        ///
        /// - Pro gear: staty, které se škálují s levelem PlayerItem přes Scaling.
        /// - Pro non-gear: typicky StatBlock.Zero.
        /// </summary>
        public StatBlock BaseStats { get; set; } = StatBlock.Zero;

        /// <summary>
        /// Základní kvalita šablony (např. common/uncommon…).
        /// PlayerItem může mít Quality odlišnou, ale BaseQuality je "doporučená default".
        /// </summary>
        public QualityTier BaseQuality { get; set; } = QualityTier.Q1_Common;
    public bool IsUnitItem => MercenaryTemplateId.HasValue || MonsterTemplateId.HasValue;

        // ------------------------------------------------------------
        // 4) EFFECTS / UPGRADES (rozšíření do budoucna)
        // ------------------------------------------------------------

        /// <summary>
        /// Efekty itemu (např. pro consumable/buffy, pasivní bonusy atd.).
        /// U gearu můžeš taky používat (prefixy/suffixy), pokud budeš chtít.
        /// </summary>
        public List<ItemEffect> Effects { get; set; } = new();

            public Guid? MercenaryTemplateId { get; set; }
            public MercenaryTemplate? MercenaryTemplate { get; set; }

            public Guid? MonsterTemplateId { get; set; }
            public MonsterTemplate? MonsterTemplate { get; set; }

        /// <summary>
        /// Pokud item funguje jako "upgrade", sem můžeš dát cíl upgradu.
        /// (zatím klidně None).
        /// </summary>
        public UpgradeTarget UpgradeTarget { get; set; } = UpgradeTarget.None;

        /// <summary>
        /// Efekty odemčené kvalitou (např. rare+ přidává extra modifikátory).
        /// </summary>
        public List<ItemEffect> QualityEffects { get; set; } = new();

        // ------------------------------------------------------------
        // 5) SUMMON ITEMS (contract / egg)
        // ------------------------------------------------------------

        /// <summary>
        /// Pokud je vyplněno => item je "contract" a grantuje MercenaryTemplate.
        ///
        /// Pravidlo:
        /// - Summon item nesmí být gear (nesmí mít sloty).
        /// - Tj. pokud GrantedMercenaryTemplateId != null => MercenarySlot/MonsterSlot musí být null.
        /// </summary>
        public Guid? GrantedMercenaryTemplateId { get; set; }

        public MercenaryTemplate? GrantedMercenaryTemplate { get; set; }

        /// <summary>
        /// Pokud je vyplněno => item je "egg" a grantuje MonsterTemplate.
        ///
        /// Pravidlo:
        /// - Summon item nesmí být gear (nesmí mít sloty).
        /// </summary>
        public Guid? GrantedMonsterTemplateId { get; set; }

        public MonsterTemplate? GrantedMonsterTemplate { get; set; }

        /// <summary>
        /// Helper: je to summon item?
        /// </summary>
        public bool IsSummon => GrantedMercenaryTemplateId.HasValue || GrantedMonsterTemplateId.HasValue;

        // ------------------------------------------------------------
        // 6) STAT SCALING (FK do tabulky StatScaling)
        // ------------------------------------------------------------

        /// <summary>
        /// FK do StatScaling tabulky.
        /// Pokud null => použije se StatScaling.Default (fallback).
        ///
        /// POZOR:
        /// - Pokud máš FK constraint v DB (ScalingId -> StatScaling.Id),
        ///   musí v DB existovat odpovídající řádek StatScaling (včetně DefaultId),
        ///   jinak budeš dostávat FK error při migraci nebo seedování.
        /// </summary>
        public Guid? ScalingId { get; set; } = null;

        /// <summary>
        /// Navigace na scaling parametry.
        /// </summary>
        public StatScaling? Scaling { get; set; }

        // ------------------------------------------------------------
        // 7) VALIDATION (chrání integritu dat)
        // ------------------------------------------------------------

        /// <summary>
        /// Hlídá konzistenci:
        /// - item nemůže být zároveň merc gear i beast gear
        /// - gear musí mít odpovídající OwnerKind
        /// - summon item nesmí být gear
        /// </summary>
        public void Validate()
        {
            var isMercGear = MercenarySlot.HasValue;
            var isBeastGear = MonsterSlot.HasValue;

            // Nemůžeš být v obou slotech (nedává smysl).
            if (isMercGear && isBeastGear)
                throw new InvalidOperationException("Item cannot have both MercenarySlot and MonsterSlot.");

            // Slot určuje ownera (jinak bude UI/logic chaos).
            if (isMercGear && OwnerKind != ItemOwnerKind.Mercenary)
                throw new InvalidOperationException("Mercenary gear must have OwnerKind = Mercenary.");

            if (isBeastGear && OwnerKind != ItemOwnerKind.Beast)
                throw new InvalidOperationException("Beast gear must have OwnerKind = Beast.");

            // Summon item nesmí být gear (contract/egg není equip).
            if (IsSummon && (isMercGear || isBeastGear))
                throw new InvalidOperationException("Summon items cannot be equip gear.");
        }

        // ------------------------------------------------------------
        // 8) STATS CALCULATION (PlayerItem level -> výsledné staty)
        // ------------------------------------------------------------

        /// <summary>
        /// Spočítá finální staty pro konkrétní PlayerItem podle jeho levelu
        /// a scaling křivky (Scaling).
        ///
        /// Poznámka:
        /// - Tady počítáš jen 4 staty (ATK/DEF/HP/SPD). Ostatní můžeš doplnit později.
        /// - lvl = item.Level - 1 => Level 1 = base stats bez bonusu.
        /// </summary>
        public StatBlock CalculateStats(PlayerItem item)
{
    if (item is null) throw new ArgumentNullException(nameof(item));
    if (item.Template is null) throw new InvalidOperationException("PlayerItem.Template is null.");

    var baseStats = item.Template.BaseStats ?? StatBlock.Zero;
    var s = item.Template.Scaling ?? StatScaling.Default;

    var lvl = Math.Max(0, item.Level - 1);

    static float MulF(float v, float perLevel, int l) => v * (1f + perLevel * l);
    static double MulD(double v, float perLevel, int l) => v * (1.0 + (double)perLevel * l);

    static double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);

    // ---------- build scaled ----------
    var scaled = new StatBlock
    {
        // CORE
        MaxHp   = MulF(baseStats.MaxHp,   s.HpPerLevel,      lvl),
        Attack  = MulF(baseStats.Attack,  s.AttackPerLevel,  lvl),
        Defense = MulF(baseStats.Defense, s.DefensePerLevel, lvl),
        Speed   = MulF(baseStats.Speed,   s.SpeedPerLevel,   lvl),

        // CRIT / PEN
        CriticalChance       = Clamp01(MulD(baseStats.CriticalChance, s.CritChancePerLevel, lvl)),
        CriticalMultiplier   = Math.Max(1.0, MulD(baseStats.CriticalMultiplier, s.CritMultiplierPerLevel, lvl)),
        ArmorPenetration = Clamp01(MulD(baseStats.ArmorPenetration, s.ArmorPenetrationPerLevel, lvl)),

        // HIT / AVOID
        Accuracy    = MulD(baseStats.Accuracy, s.AccuracyPerLevel, lvl),            // klidně bez clamp
        Evasion     = Clamp01(MulD(baseStats.Evasion, s.EvasionPerLevel, lvl)),
        BlockChance = Clamp01(MulD(baseStats.BlockChance, s.BlockChancePerLevel, lvl)),

        // DAMAGE MODS
        DamageBonus     = Clamp01(MulD(baseStats.DamageBonus, s.DamageBonusPerLevel, lvl)),
        DamageReduction = Clamp01(MulD(baseStats.DamageReduction, s.DamageReductionPerLevel, lvl)),
        TrueDamageBonus = Clamp01(MulD(baseStats.TrueDamageBonus, s.TrueDamageBonusPerLevel, lvl)),

        // TEMPO (bez cooldownů nechávám jen TM gain)
        TurnMeterGain = Clamp01(MulD(baseStats.TurnMeterGain, s.TurnMeterGainPerLevel, lvl)),

        // SUSTAIN
        LifeSteal   = Clamp01(MulD(baseStats.LifeSteal, s.LifeStealPerLevel, lvl)),
        HpRegen     = Clamp01(MulD(baseStats.HpRegen, s.HpRegenPerLevel, lvl)),
        EnergyRegen = Clamp01(MulD(baseStats.EnergyRegen, s.EnergyRegenPerLevel, lvl)),

        // ELEMENT
        Element = baseStats.Element,

        // STATUS CHANCE / DEF
        BleedChance  = Clamp01(MulD(baseStats.BleedChance,  s.StatusChancePerLevel, lvl)),
        PoisonChance = Clamp01(MulD(baseStats.PoisonChance, s.StatusChancePerLevel, lvl)),
        BurnChance   = Clamp01(MulD(baseStats.BurnChance,   s.StatusChancePerLevel, lvl)),
        ShockChance  = Clamp01(MulD(baseStats.ShockChance,  s.StatusChancePerLevel, lvl)),
        FreezeChance = Clamp01(MulD(baseStats.FreezeChance, s.StatusChancePerLevel, lvl)),

        StatusDurationBonus = Clamp01(MulD(baseStats.StatusDurationBonus, s.StatusDurationPerLevel, lvl)),
        StatusResistance    = Clamp01(MulD(baseStats.StatusResistance,    s.StatusResistancePerLevel, lvl)),
        DotDamageBonus      = Clamp01(MulD(baseStats.DotDamageBonus,      s.DotDamagePerLevel, lvl)),
        DotDamageReduction  = Clamp01(MulD(baseStats.DotDamageReduction,  s.DotDamagePerLevel, lvl)),
        CleanseChance       = Clamp01(MulD(baseStats.CleanseChance,       s.StatusChancePerLevel, lvl)),

        // POTENCY (pokud chceš – je to univerzální knob)
        StatusPotency = Clamp01(MulD(baseStats.StatusPotency, s.StatusChancePerLevel, lvl)),
    };

    // ---------- apply bonus stats (additive) ----------
    var b = item.BonusStats ?? StatBlock.Zero;

    scaled.MaxHp   += b.MaxHp;
    scaled.Attack  += b.Attack;
    scaled.Defense += b.Defense;
    scaled.Speed   += b.Speed;

    scaled.CriticalChance       = Clamp01(scaled.CriticalChance + b.CriticalChance);
    scaled.CriticalMultiplier   = Math.Max(1.0, scaled.CriticalMultiplier + (b.CriticalMultiplier - 1.5)); // pokud Zero má 1.5
    scaled.ArmorPenetration = Clamp01(scaled.ArmorPenetration + b.ArmorPenetration);

    scaled.Accuracy         = scaled.Accuracy + b.Accuracy;
    scaled.Evasion          = Clamp01(scaled.Evasion + b.Evasion);
    scaled.BlockChance      = Clamp01(scaled.BlockChance + b.BlockChance);

    scaled.DamageBonus      = Clamp01(scaled.DamageBonus + b.DamageBonus);
    scaled.DamageReduction  = Clamp01(scaled.DamageReduction + b.DamageReduction);
    scaled.TrueDamageBonus  = Clamp01(scaled.TrueDamageBonus + b.TrueDamageBonus);

    scaled.TurnMeterGain    = Clamp01(scaled.TurnMeterGain + b.TurnMeterGain);

    scaled.LifeSteal        = Clamp01(scaled.LifeSteal + b.LifeSteal);
    scaled.HpRegen          = Clamp01(scaled.HpRegen + b.HpRegen);
    scaled.EnergyRegen      = Clamp01(scaled.EnergyRegen + b.EnergyRegen);

    scaled.BleedChance      = Clamp01(scaled.BleedChance + b.BleedChance);
    scaled.PoisonChance     = Clamp01(scaled.PoisonChance + b.PoisonChance);
    scaled.BurnChance       = Clamp01(scaled.BurnChance + b.BurnChance);
    scaled.ShockChance      = Clamp01(scaled.ShockChance + b.ShockChance);
    scaled.FreezeChance     = Clamp01(scaled.FreezeChance + b.FreezeChance);

    scaled.StatusDurationBonus = Clamp01(scaled.StatusDurationBonus + b.StatusDurationBonus);
    scaled.StatusResistance    = Clamp01(scaled.StatusResistance + b.StatusResistance);
    scaled.DotDamageBonus      = Clamp01(scaled.DotDamageBonus + b.DotDamageBonus);
    scaled.DotDamageReduction  = Clamp01(scaled.DotDamageReduction + b.DotDamageReduction);
    scaled.CleanseChance       = Clamp01(scaled.CleanseChance + b.CleanseChance);

    scaled.StatusPotency       = Clamp01(scaled.StatusPotency + b.StatusPotency);

    return scaled;
}

private static float Mul(float baseVal, float perLevelPct, int lvl)
    => baseVal * (1f + perLevelPct * lvl);

private static double Add(double baseVal, double perLevelFlat, int lvl)
    => baseVal + perLevelFlat * lvl;

private static double Clamp01(double v) => Math.Clamp(v, 0.0, 1.0);
private static double ClampPct(double v) => Math.Clamp(v, 0.0, 100.0); // pokud držíš 0..100 místo 0..1
        // ------------------------------------------------------------
        // 9) STAT SCALING ENTITY
        // ------------------------------------------------------------

        /// <summary>
        /// Parametry škálování statů s levelem.
        ///
        /// DefaultId je konstantní Guid, který by měl existovat v DB,
        /// pokud máš ScalingId jako FK.
        /// </summary>
        public class StatScaling
{
    public static readonly Guid DefaultId =
        new("11111111-1111-1111-1111-111111111111");

    public Guid Id { get; set; } = DefaultId;

    // ================= CORE =================
    public float AttackPerLevel { get; set; } = 0.05f;
    public float DefensePerLevel { get; set; } = 0.04f;
    public float HpPerLevel { get; set; } = 0.08f;
    public float SpeedPerLevel { get; set; } = 0.01f;

    // ============ CRIT / PEN =================
    public float CritChancePerLevel { get; set; } = 0.002f;
    public float CritMultiplierPerLevel { get; set; } = 0.01f;
    public float ArmorPenetrationPerLevel { get; set; } = 0.02f;

    // ============ HIT / AVOID ================
    public float AccuracyPerLevel { get; set; } = 0.01f;
    public float EvasionPerLevel { get; set; } = 0.01f;
    public float BlockChancePerLevel { get; set; } = 0.005f;

    // ============ DAMAGE =====================
    public float DamageBonusPerLevel { get; set; } = 0.02f;
    public float DamageReductionPerLevel { get; set; } = 0.015f;
    public float TrueDamageBonusPerLevel { get; set; } = 0.01f;

    // ============ TEMPO ======================
    public float TurnMeterGainPerLevel { get; set; } = 0.01f;
    public float CooldownReductionPerLevel { get; set; } = 0.005f;
    public float ActionCostReductionPerLevel { get; set; } = 0.005f;

    // ============ SUSTAIN ====================
    public float LifeStealPerLevel { get; set; } = 0.005f;
    public float HpRegenPerLevel { get; set; } = 0.01f;
    public float EnergyRegenPerLevel { get; set; } = 0.01f;

    // ============ STATUS EFFECTS =============
    public float StatusChancePerLevel { get; set; } = 0.005f;
    public float StatusDurationPerLevel { get; set; } = 0.01f;
    public float StatusResistancePerLevel { get; set; } = 0.01f;
    public float DotDamagePerLevel { get; set; } = 0.02f;

    public static StatScaling Default => new();
}
        public class PlayerItemPieces : BaseGuid
        {
            public Guid PlayerId { get; set; }
            public PlayerProfile Player { get; set; } = null!;

            public Guid TemplateId { get; set; }
            public ItemTemplate Template { get; set; } = null!;

            public long Pieces { get; set; } = 0;
        }
        public class PlayerUpgradeStones : BaseGuid
        {
            public Guid PlayerId { get; set; }
            public PlayerProfile Player { get; set; } = null!;

            public ItemOwnerKind OwnerKind { get; set; } // Mercenary / Beast
            
            public int SlotKey { get; set; }

            public long Stones { get; set; } = 0;
        }
        public class ItemLevelScalingProfile : BaseGuid
        {
            public int SlotKey { get; set; }

            // multipliers per level (lineární nebo %)
            public float AttackPerLevelPct { get; set; }
            public float DefensePerLevelPct { get; set; }
            public float HpPerLevelPct { get; set; }
            public float SpeedPerLevelPct { get; set; }

            public float CritChancePerLevel { get; set; }       // +flat
            public float CritMultPerLevel { get; set; }         // +flat
            public float PenetrationPerLevel { get; set; }      // pokud přidáš stat
            public float EvasionPerLevel { get; set; }          // atd.
        }
        public class QualityBonusSet : BaseGuid
        {   
            public StatId Bonus1Type { get; set; }
            public float Bonus1Base { get; set; }     // hodnota na tier 1
            public float Bonus1PerTier { get; set; }  // přírůstek na tier

            public StatId Bonus2Type { get; set; }
            public float Bonus2Base { get; set; }
            public float Bonus2PerTier { get; set; }

            public StatId Bonus3Type { get; set; }
            public float Bonus3Base { get; set; }
            public float Bonus3PerTier { get; set; }
        }
    }
    public static class ItemSlotKey
{
    public static int FromMerc(ItemEquipSlot s) => (int)s;          // 1..99
    public static int FromBeast(ItemEquipSlot s) => 100 + (int)s;     // 101..199
    public static bool IsMerc(int slotKey)  => slotKey >= 1 && slotKey <= 11;
    public static bool IsBeast(int slotKey) => slotKey >= 101 && slotKey <= 111;

    public static ItemOwnerKind OwnerKindOf(int slotKey) =>
        IsMerc(slotKey) ? ItemOwnerKind.Mercenary :
        IsBeast(slotKey) ? ItemOwnerKind.Beast :
        ItemOwnerKind.None;
}
public sealed class GeneratedBonusResult
{
    public string? StatId { get; set; } = "None";  // ✅ string z LLM
    public float Base { get; set; }
    public float PerTier { get; set; }
}

public sealed class ItemTemplateGenerationResult
{
    public string NameEn { get; set; } = "";
    public string DescriptionEn { get; set; } = "";

    public string EquipSlot { get; set; } = "None";
    public string BaseQuality { get; set; } = "Q1_Common";

    public StatBlock Stats { get; set; } = new();

    // ✅ místo List<GeneratedBonus>
    //public List<GeneratedBonusResult> Bonuses { get; set; } = new();
}
public sealed class ItemUpgradeResourceNamesResult
{
    public string CoreNameEn { get; set; } = string.Empty;
    public string? CoreDescriptionEn { get; set; }

    public string CatalystNameEn { get; set; } = string.Empty;
    public string? CatalystDescriptionEn { get; set; }

    public string EssenceNameEn { get; set; } = string.Empty;
    public string? EssenceDescriptionEn { get; set; }
}

public sealed class ItemBonusGenerationResult
{
    public List<GeneratedBonusResult> Bonuses { get; set; } = new();
}

public sealed class GeneratedBonus
{
    public StatId Type { get; set; }

    // hodnoty v procentech jsou v "pct bodech" (např. 3 = +3%)
    public float Base { get; set; }
    public float PerTier { get; set; }
}
public static class ItemEquipSlotRules
{
    public static bool IsMercSlot(ItemEquipSlot s) => (int)s >= 1 && (int)s <= 99;
    public static bool IsBeastSlot(ItemEquipSlot s) => (int)s >= 100 && (int)s <= 199;

    public static ItemOwnerKind OwnerOf(ItemEquipSlot s)
        => IsBeastSlot(s) ? ItemOwnerKind.Beast
         : IsMercSlot(s) ? ItemOwnerKind.Mercenary
         : ItemOwnerKind.None;
}
public sealed class ItemUpgradeResource : BaseGuid
{
    public Guid ItemTemplateId { get; set; }
    public ItemTemplate ItemTemplate { get; set; } = null!;

    public ItemUpgradeResourceType Type { get; set; }

    // kolik kusů padá / má player / costy řeš v inventory tabulkách
    // tady může být jen “default drop quantity” nebo “upgrade cost base”
    public int DefaultAmount { get; set; } = 0;
}

public static class StatIdParser
{
    // Normalizace: "Crit Chance" => "critchance", "HP%" => "hp", "Armor_Shred_Pct" => "armorshredpct"
    private static string Norm(string s)
        => (s ?? string.Empty)
            .Trim()
            .ToLowerInvariant()
            .Replace(" ", "")
            .Replace("_", "")
            .Replace("-", "")
            .Replace("%", "pct");

    public static StatId ParseOrThrow(string raw)
    {
    // fallback pool = všechny hodnoty enumu
    var all = Enum.GetValues<StatId>();

    // když je to úplně prázdné → rovnou random
    if (string.IsNullOrWhiteSpace(raw))
        return DeterministicPick(raw,all);

    // odstraní mezery, pomlčky, podtržítka, atd.
    var cleaned = new string(raw
        .Where(char.IsLetterOrDigit)
        .ToArray());

    if (Enum.TryParse<StatId>(raw, ignoreCase: true, out var direct))
        return direct;

    if (!string.IsNullOrWhiteSpace(cleaned) &&
        Enum.TryParse<StatId>(cleaned, ignoreCase: true, out var normalized))
        return normalized;

    // ❗ fallback místo throw
    return DeterministicPick(raw,all);
}
private static StatId DeterministicPick(string seed, StatId[] values)
{
    var hash = seed.GetHashCode();
    var idx = Math.Abs(hash % values.Length);
    return values[idx];
}

}
}