namespace MercenariesAndBeasts.Domain.Enums
{
    public enum ElementType
    {
        None = 0,
        Fire = 1,
        Water = 2,
        Earth = 3,
        Air = 4,
        Light = 5,
        Shadow = 6,
        Arcane = 7,
        Poison = 8,
        Metal = 9,
        Plasma = 10,
        Void = 11,
        Nature = 12,
        Ice = 13,
        Physical = 14,
        Lightning = 15
    }

    /// <summary>
    /// 11-tier kvalita itemů i jednotek.
    /// </summary>
    public enum QualityTier : int
    {
         Q1_Common = 1,
        Q2_Uncommon = 2,
        Q3_Rare = 3,
        Q4_Superior = 4,
        Q5_Epic = 5,
        Q6_Mythic = 6,
        Q7_Ancient = 7,
        Q8_Arcane = 8,
        Q9_Celestial = 9,
        Q10_Ethereal = 10,
        Q11_Transcendent = 11
    }

    public enum UnitType
    {
        Mercenary = 0,
        Monster = 1
    }

    public enum DungeonStageType
    {
        DS1_Feeble = 1,
        DS2_Lesser = 2,
        DS3_Wild = 3,
        DS4_Hardened = 4,
        DS5_Corrupted = 5,
        DS6_Empowered = 6,
        DS7_Twisted = 7,
        DS8_Prime = 8,
        DS9_Monstrous = 9,
        DS10_Tyrant = 10,
        DS11_Abyssal = 11
    }

    public enum ExpeditionStageType
    {
        ES1_Novice = 1,
        ES2_Trained = 2,
        ES3_Disciplined = 3,
        ES4_Skilled = 4,
        ES5_Hardened = 5,
        ES6_Elite = 6,
        ES7_Veteran = 7,
        ES8_Master = 8,
        ES9_Commander = 9,
        ES10_Dominion = 10,
        ES11_Ascendant = 11
    }
public enum ItemEquipSlot
{
    None = 0,

    // =========================================================
    // MERCENARIES (ORDERS) — 1–99
    // =========================================================

    // --- Core combat gear (1–19)
    Merc_Entity      = 1,   // weapons, attack-focused
    Merc_MainHand      = 2,   // weapons, attack-focused
    Merc_OffHand       = 3,   // shields, foci, defense
    Merc_Head          = 4,
    Merc_Chest         = 5,
    Merc_Legs          = 6,
    Merc_Boots         = 7,
    Merc_Shoulder      = 8,
    Merc_Back          = 9,
    Merc_Belt          = 10,
    Merc_Trinket       = 11,
    Merc_Relic         = 12,

    // --- Utility / tactical (30–49)
    Merc_Consumable    = 30,  // potions, combat actives (choose one)
    Merc_Mineral       = 31,  // battle crystals, one-use or passive
    Merc_Artifact      = 32,  // powerful but limited passive
    Merc_MiniPet       = 33,  // companion providing bonuses
    Merc_Plant         = 34,  // seeds, spores, terrain buffs

    // --- Future expansion (50–99 reserved)
    Merc_Utility1      = 50,
    Merc_Utility2      = 51,


    // =========================================================
    // BEASTS — 100–199
    // =========================================================

    // --- Core body parts (100–119)
    Beast_Entity         = 100,
    Beast_Fang         = 101,
    Beast_Claw         = 102,
    Beast_Skull        = 103,
    Beast_Carapace     = 104,
    Beast_HindLeg      = 105,
    Beast_Talon        = 106,
    Beast_Spine        = 107,
    Beast_Dorsal       = 108,
    Beast_Girth        = 109,
    Beast_Essence      = 110,
    Beast_Core         = 111,

    // --- Utility / combat additions (130–149)
    Beast_Consumable   = 130, // combat organs, glands, injections
    Beast_Mineral      = 131, // embedded crystals, ores
    Beast_Artifact     = 132, // symbiotic relics
    Beast_MiniPet      = 133, // parasites / symbiotes
    Beast_Plant        = 134, // living growths, spores

    // --- Future expansion (150–199 reserved)
    Beast_Utility1     = 150,
    Beast_Utility2     = 151
}

    public enum StatId
{
    // CORE
    MaxHp,
    Armor,
    Attack,
    Defense,
    Speed,
    MaxEnergy,

    // CRIT / PEN
    CriticalChance,
    CriticalMultiplier,
    ArmorPenetration,

    // HIT / AVOID
    Accuracy,
    Evasion,
    BlockChance,
    CounterChance,

    // DAMAGE MODIFIERS
    DamageBonus,
    DamageReduction,
    TrueDamageBonus,
    Thorns,

    // TEMPO
    TurnMeterGain,
    EnergyCostReduction,

    // SUSTAIN
    LifeSteal,
    HpRegen,
    EnergyRegen,
    HealingBonus,
    HealingReduction,
    ShieldBonus,

    // STATUS
    BleedChance,
    PoisonChance,
    BurnChance,
    ShockChance,
    FreezeChance,
    StatusDurationBonus,
    StatusResistance,
    DotDamageBonus,
    DotDamageReduction,
    CleanseChance,
    StatusPotency,
}
    public enum ItemEffectType
{
    None = 0,

    // PASIVNÍ EFEKTY (permanentní)
    PassiveStatBoost = 1,          // +ATK, +DEF, +HP…
    PassiveElementBonus = 2,       // +Fire DMG, +Void Res
    PassiveDropBonus = 3,          // +% drop rate
    PassiveGoldBonus = 4,          // +% gold reward
    PassiveXpBonus = 5,            // +% XP
    PassiveSpeedBonus = 6,         // +% attack/turn speed
    PassiveEnergyRegen = 7,        // +Energy regeneration
    PassiveHealthRegen = 8,        // +Health regeneration

    // DOČASNÉ EFEKTY (časové buffy)
    TimedBoostCombat = 20,         // buff pro jeden souboj
    TimedBoostDuration = 21,       // buff na X minut
    TimedDamageBuff = 22,          // +DMG na čas
    TimedDefenseBuff = 23,         // +DEF na čas
    TimedElementBurst = 24,        // aktivuje element burst na čas
    TimedLootBoost = 25,           // zvýšený loot na čas
    TimedRarityBoost = 26,         // zvýšená kvalita dropů na čas
    TimedSpeedBoost = 27,          // haste buff
    TimedEvasionBoost = 28,        // +dodge na čas

}
public enum ItemOwnerKind
{
    None = 0,         // pro věci co se neequippují
    Mercenary = 1,
    Beast = 2
}
public enum UpgradeTarget
{
    None = 0,
    Unit = 1,  // level/rank merc/beast
    Gear = 2   // level/quality gearu
}
public enum ItemUpgradeResourceType
{
    Core,      // level
    Catalyst, // quality
    Essence   // traits / enchant / stability
}
public enum ItemBadgeTier
{
    None = 0,
    // === ORGANIC / SOFT MATERIALS ===
    Wood,
    Peat,
    Lignite,
    Coal,
    Amber,

    // === SEDIMENTARY ROCKS ===
    Clay,
    Shale,
    Sandstone,
    Limestone,
    Chalk,
    Marl,
    Siltstone,

    // === METAMORPHIC (LOW) ===
    Slate,
    Phyllite,
    Schist,
    Gneiss,
    Quartzite,
    Marble,

    // === COMMON IGNEOUS ===
    Basalt,
    Andesite,
    Diorite,
    Granite,
    Obsidian,
    Pumice,

    // === COMMON METALS ===
    Copper,
    Tin,
    Lead,
    Zinc,
    Nickel,
    Iron,

    // === ALLOYS ===
    Bronze,
    Brass,
    Steel,
    CastIron,
    StainlessSteel,

    // === SEMI-PRECIOUS MINERALS ===
    Calcite,
    Fluorite,
    Dolomite,
    Apatite,
    Feldspar,
    Tourmaline,
    Garnet,

    // === HARD METALS ===
    Chromium,
    Manganese,
    Cobalt,
    Vanadium,
    Tungsten,
    Molybdenum,

    // === PRECIOUS METALS ===
    Silver,
    Gold,
    Platinum,
    Palladium,
    Rhodium,
    Iridium,
    Osmium,

    // === GEM MATERIALS ===
    Jade,
    Onyx,
    Opal,
    Topaz,
    Sapphire,
    Ruby,
    Emerald,
    Diamond,

    // === RARE / EXTREME MATERIALS ===
    Spinel,
    Zircon,
    Alexandrite,
    Moissanite,
    Tantalum,
    Hafnium,
    Rhenium,

    // === ULTRA-DENSE / ENDGAME ===
    Neutronium,
    Adamantite,
    Vibranium
}
public enum DungeonAchCode
{
    BossDefeat,
    ClearAllStages,
    WinStreak,
    NoDeaths,
    FinishUnderTurns,
    // ... až do 22 TODO
}
}