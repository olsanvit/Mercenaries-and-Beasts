using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Dto;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MercenariesAndBeasts.Web.Services;

/// <summary>
/// Seeds 8 expedition locations with 10 stages each and 24 mercenary templates.
/// Each location has a unique elemental theme and escalating difficulty.
/// Idempotent — skips if locations already exist.
/// </summary>
public sealed class TestLocationsAndMercenariesSeeder
{
    private readonly IDbContextFactory<AppDbContextMercenariesAndBeasts> _factory;

    public TestLocationsAndMercenariesSeeder(IDbContextFactory<AppDbContextMercenariesAndBeasts> factory)
        => _factory = factory;

    public async Task<string> SeedAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);

        if (await db.Locations.AnyAsync(ct))
            return "Locations already exist — skipped.";

        // ── 24 Mercenary templates (3 per element) ───────────────────────────
        var mercDefs = new (string Code, string Name, string Desc, ElementType El, UnitRole Role, float Hp, float Atk, float Def, float Spd)[]
        {
            // Fire
            ("MERC_FIRE_KNIGHT",   "Ohnivý rytíř",     "Seká mečem a planutím.",          ElementType.Fire,   UnitRole.Bruiser,   140, 60, 35, 50),
            ("MERC_FIRE_MAGE",     "Mág ohně",         "Vyvolává ohnivé kulové blesky.",  ElementType.Fire,   UnitRole.Caster,      90,  80, 15, 65),
            ("MERC_FIRE_ARCHER",   "Plamenný lučištník","Šípy v plamenech.",               ElementType.Fire,   UnitRole.Sniper,    100, 70, 20, 75),
            // Water
            ("MERC_WATER_HEALER",  "Mořský léčitel",   "Obnovuje HP spojencům.",          ElementType.Water,  UnitRole.Healer,    110, 30, 40, 60),
            ("MERC_WATER_KNIGHT",  "Oceánský rytíř",   "Štít z vodního ledu.",            ElementType.Water,  UnitRole.Tank,      170, 40, 55, 35),
            ("MERC_WATER_ROGUE",   "Přílivový zloděj", "Krade energii nepřátel.",         ElementType.Water,  UnitRole.Assassin,     95,  65, 20, 85),
            // Earth
            ("MERC_EARTH_WARRIOR", "Skalní válečník",  "Nejodolnější bojovník.",          ElementType.Earth,  UnitRole.Tank,      200, 35, 65, 20),
            ("MERC_EARTH_DRUID",   "Lesní druid",      "Léčí a přivolává kořeny.",        ElementType.Earth,  UnitRole.Healer,    120, 40, 30, 50),
            ("MERC_EARTH_RANGER",  "Lesní stopař",     "Jed a pasti.",                    ElementType.Earth,  UnitRole.Sniper,    105, 60, 25, 80),
            // Air
            ("MERC_AIR_SCOUT",     "Větrný průzkumník","Nejrychlejší na bitevním poli.",  ElementType.Air,    UnitRole.Assassin,  80,  75, 10, 100),
            ("MERC_AIR_MAGE",      "Bouřkový mág",     "Blesky a tornáda.",               ElementType.Air,    UnitRole.Caster,      95,  80, 15, 70),
            ("MERC_AIR_PALADIN",   "Nebeský paladín",  "Ochrana a boží zásah.",           ElementType.Air,    UnitRole.Battlemage,   150, 55, 45, 50),
            // Light
            ("MERC_LIGHT_CLERIC",  "Světelný klerikus","Svaté kouzlo a léčení.",          ElementType.Light,  UnitRole.Healer,    115, 45, 35, 55),
            ("MERC_LIGHT_CRUSADER","Křižák světla",    "Paladin s oslnivým štítem.",      ElementType.Light,  UnitRole.Battlemage,   160, 55, 50, 40),
            ("MERC_LIGHT_ARCHER",  "Sluneční střelec", "Světelné šípy zaslepují.",        ElementType.Light,  UnitRole.Sniper,    100, 65, 20, 75),
            // Shadow
            ("MERC_SHADOW_ROGUE",  "Stínový zloděj",  "Útočí ze tmy.",                   ElementType.Shadow, UnitRole.Assassin,     85,  80, 10, 95),
            ("MERC_SHADOW_WARLOCK","Temný čaroděj",    "Prokletí a krádež duše.",         ElementType.Shadow, UnitRole.Caster,      100, 75, 20, 65),
            ("MERC_SHADOW_KNIGHT", "Rytíř temnoty",    "Temnota mu dává sílu.",           ElementType.Shadow, UnitRole.Bruiser,   145, 65, 30, 55),
            // Poison
            ("MERC_POISON_ALCH",   "Jedovatý alchymista","Výbušné jedy a kysely.",       ElementType.Poison, UnitRole.Caster,      95,  70, 20, 65),
            ("MERC_POISON_HUNTER", "Lovce jedu",       "Dýky potřené jedem.",             ElementType.Poison, UnitRole.Assassin,     90,  75, 15, 90),
            ("MERC_POISON_GUARD",  "Bažinný strážce",  "Imunní vůči jedům.",              ElementType.Poison, UnitRole.Tank,      180, 30, 60, 25),
            // Ice
            ("MERC_ICE_MAGE",      "Mrazivý mág",      "Mrazí nepřátele na místě.",       ElementType.Ice,    UnitRole.Caster,      100, 75, 20, 65),
            ("MERC_ICE_WARRIOR",   "Ledový válečník",  "Zbroj z ledu.",                   ElementType.Ice,    UnitRole.Tank,      175, 40, 60, 25),
            ("MERC_ICE_RANGER",    "Arktický lovec",   "Šípy z ledu.",                    ElementType.Ice,    UnitRole.Sniper,    105, 65, 20, 78),
        };

        var mercs = mercDefs.Select(m => new MercenaryTemplate
        {
            Code          = m.Code,
            NameEn        = m.Name,
            DescriptionEn = m.Desc,
            Element       = m.El,
            Role          = m.Role,
            IsActive      = true,
            BaseLevel     = 1,
            BaseStats     = new StatBlock { MaxHp = m.Hp, Attack = m.Atk, Defense = m.Def, Speed = m.Spd }
        }).ToList();

        db.MercenaryTemplates.AddRange(mercs);
        await db.SaveChangesAsync(ct);

        // ── 8 Expedition Locations with 10 stages each ───────────────────────
        var locationDefs = new (string Code, string Name, string Desc, ElementType El, int Order, int MinLvl, int MaxLvl, string[] MercCodes)[]
        {
            ("LOC_FOREST",    "Temný les",          "Hustý prales plný nebezpečí.",     ElementType.Earth,  1, 1,  5,  new[]{"MERC_EARTH_RANGER","MERC_EARTH_WARRIOR","MERC_EARTH_DRUID"}),
            ("LOC_COAST",     "Bouřlivé pobřeží",   "Přívalové vlny a mořské příšery.", ElementType.Water,  2, 3,  8,  new[]{"MERC_WATER_ROGUE","MERC_WATER_KNIGHT","MERC_WATER_HEALER"}),
            ("LOC_VOLCANO",   "Sopečné útroby",     "Žhavá láva a jedovaté výpary.",    ElementType.Fire,   3, 6,  12, new[]{"MERC_FIRE_ARCHER","MERC_FIRE_KNIGHT","MERC_FIRE_MAGE"}),
            ("LOC_SKY",       "Létající ostrovy",   "Vzdušné pevnosti nad mraky.",      ElementType.Air,    4, 9,  15, new[]{"MERC_AIR_SCOUT","MERC_AIR_MAGE","MERC_AIR_PALADIN"}),
            ("LOC_RUINS",     "Zapomenuté ruiny",   "Chrámy dávno zaniklé civilizace.", ElementType.Light,  5, 12, 20, new[]{"MERC_LIGHT_ARCHER","MERC_LIGHT_CRUSADER","MERC_LIGHT_CLERIC"}),
            ("LOC_CATACOMBS", "Katakomby",          "Nekonečné chodby plné přízraků.",  ElementType.Shadow, 6, 16, 25, new[]{"MERC_SHADOW_ROGUE","MERC_SHADOW_KNIGHT","MERC_SHADOW_WARLOCK"}),
            ("LOC_SWAMP",     "Jedovaté bažiny",    "Toxická mlha a skrytá nebezpečí.", ElementType.Poison, 7, 20, 30, new[]{"MERC_POISON_HUNTER","MERC_POISON_ALCH","MERC_POISON_GUARD"}),
            ("LOC_TUNDRA",    "Arktická tundra",    "Mrazivá poušť bez milosti.",       ElementType.Ice,    8, 25, 40, new[]{"MERC_ICE_RANGER","MERC_ICE_MAGE","MERC_ICE_WARRIOR"}),
        };

        var mercMap     = mercs.ToDictionary(m => m.Code);
        var stageTypes  = Enum.GetValues<ExpeditionStageType>().OrderBy(x => (int)x).ToArray();

        foreach (var (code, name, desc, el, order, minLvl, maxLvl, mCodes) in locationDefs)
        {
            var loc = new Location
            {
                Code          = code,
                NameEn        = name,
                DescriptionEn = desc,
                Element       = el,
                UnlockOrder   = order,
                MinLevel      = minLvl,
                MaxLevel      = maxLvl,
                IsActive      = true,
                BaseLevel     = minLvl
            };
            db.Locations.Add(loc);
            await db.SaveChangesAsync(ct);

            // 10 stages — cycle through the 3 mercenary templates
            for (int i = 1; i <= 10; i++)
            {
                var mercCode  = mCodes[(i - 1) % mCodes.Length];
                var stageTier = stageTypes[Math.Min(i - 1, stageTypes.Length - 1)];

                db.ExpeditionStages.Add(new ExpeditionStage
                {
                    Code          = $"{code}_S{i:D2}",
                    NameEn        = $"{name} — fáze {i}",
                    DescriptionEn = $"Stage {i} of expedition to {name}.",
                    LocationId    = loc.Guid,
                    StageNumber   = i,
                    Difficulty    = stageTier,
                    EnemyId       = mercMap[mercCode].Guid,
                    FixedLevel    = minLvl + (i - 1),
                    IsActive      = true,
                    BaseLevel     = minLvl + (i - 1)
                });
            }
            await db.SaveChangesAsync(ct);
        }

        return $"✅ Seeded {mercs.Count} mercenary templates + {locationDefs.Length} locations (10 stages each).";
    }
}
