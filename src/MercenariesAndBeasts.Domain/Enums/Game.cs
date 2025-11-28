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
        Void = 11
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

    public enum MercenaryItemSlot
    {
        MainHand = 1,
        OffHand = 2,
        Headgear = 3,
        Chestguard = 4,
        Legwear = 5,
        Footgear = 6,
        Shoulder = 7,
        Back = 8,
        Belt = 9,
        Trinket = 10,
        Relic = 11
    }

    public enum MonsterItemSlot
    {
        Fang = 1,
        Claw = 2,
        Skull = 3,
        Carapace = 4,
        HindLeg = 5,
        Talon = 6,
        Spine = 7,
        Dorsal = 8,
        Girth = 9,
        Essence = 10,
        Core = 11
    }
    public enum ItemType
    {
        // EQUIPMENT
    Gear = 0,          // equip do slotů (merc/monster)

    // RESOURCES
    Material = 1,      // crafting, upgrade, enhancement
    Currency = 2,      // gold, shards, tokens

    // SUMMONING / UNIT-GAIN
    Contract = 3,      // získání žoldáka
    Egg = 4,           // získání monstra

    // BOOST ITEMS
    Consumable = 5,    // jednorázové (potiony, boost orby)
    Buff = 6,          // časové buffy (XP boost, drop boost)
    Passive = 7,       // permanentní pasivní bonus (account-wide / hero-wide)

    // PROGRESSION ITEMS
    Key = 8,           // odemyká dungeon / výpravu / bránu
    Blueprint = 9,     // schémata na výrobu gearu
    UpgradeCore = 10,  // zlepšení kvality, tierů, evoluce

    // SPECIAL
    QuestItem = 11,    // pro questy nebo eventy
    Seasonal = 12,     // sezónní itemy
    Cosmetic = 13,     // skiny, efekty, vizuály

    // META
    Container = 14,    // truhly, loot boxy, balíčky
    Randomizer = 15    // reroll-točení (stats, prefixy, suffixy)
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

    // SPECIAL
    OneTimeUse = 40,               // spotřební item (potion, orb…)
    AccountWide = 41,              // permanentní účetní odemykání
    PartyWide = 42                 // ovlivní celý tým (žoldáky/monstra)
}
}