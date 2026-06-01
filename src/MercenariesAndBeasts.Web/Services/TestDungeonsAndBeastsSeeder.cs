using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Dto;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace MercenariesAndBeasts.Web.Services;

/// <summary>
/// Seeds 8 test dungeons with 11 stages each and 24 monster (beast) templates.
/// Each dungeon has a unique elemental affinity and progressive difficulty.
/// Idempotent — skips if dungeons already exist.
/// </summary>
public sealed class TestDungeonsAndBeastsSeeder
{
    private readonly IDbContextFactory<AppDbContextMercenariesAndBeasts> _factory;

    public TestDungeonsAndBeastsSeeder(IDbContextFactory<AppDbContextMercenariesAndBeasts> factory)
        => _factory = factory;

    public async Task<string> SeedAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);

        if (await db.Dungeons.AnyAsync(ct))
            return "Dungeons already exist — skipped.";

        // ── 24 Monster templates (3 per element) ─────────────────────────────
        var monsterDefs = new (string Code, string Name, string Desc, ElementType El, UnitRole Role, float Hp, float Atk, float Def, float Spd)[]
        {
            // Fire
            ("FIRE_WYRM",      "Ohnivý drak",        "Starý drak chrlící plameny.",       ElementType.Fire,    UnitRole.Bruiser,   180, 55, 30, 40),
            ("FIRE_GOLEM",     "Magmatický golem",   "Kamenný golem plný lávy.",           ElementType.Fire,    UnitRole.Tank,      250, 35, 60, 20),
            ("FIRE_SPRITE",    "Ohnivý skřítek",     "Rychlý elementál ohně.",             ElementType.Fire,    UnitRole.Assassin,  80,  70, 15, 90),
            // Water
            ("WATER_SERPENT",  "Mořský had",         "Obrovský had z hlubin.",             ElementType.Water,   UnitRole.Vanguard,  160, 50, 40, 55),
            ("WATER_JELLYFISH","Obří medúza",        "Chapadla paralyzují nepřátele.",     ElementType.Water,   UnitRole.Controller,120, 40, 35, 65),
            ("WATER_KRAKEN",   "Mini kraken",        "Tentacle beast from the deep.",      ElementType.Water,   UnitRole.Tank,      220, 45, 55, 30),
            // Earth
            ("EARTH_TROLL",    "Lesní troll",        "Regeneruje HP každé kolo.",          ElementType.Earth,   UnitRole.Juggernaut,300, 40, 50, 25),
            ("EARTH_SPIDER",   "Obří pavouk",        "Jedovaté kousnutí.",                 ElementType.Earth,   UnitRole.Debuffer,  110, 60, 20, 75),
            ("EARTH_GOLEM",    "Kamenný golem",      "Pomalý, ale extrémně odolný.",       ElementType.Earth,   UnitRole.Tank,      280, 30, 70, 15),
            // Air
            ("AIR_HARPY",      "Harpia",             "Létá vysoko a útočí shora.",         ElementType.Air,     UnitRole.Assassin,  90,  75, 15, 95),
            ("AIR_DJINN",      "Vzdušný džin",       "Mistr iluzí a větru.",               ElementType.Air,     UnitRole.Caster,      130, 65, 25, 70),
            ("AIR_WYVERN",     "Vyvern",             "Menší bratranec draka.",             ElementType.Air,     UnitRole.Bruiser,   170, 60, 35, 60),
            // Light
            ("LIGHT_ANGEL",    "Padlý anděl",        "Kdysi strážce, nyní zkažený.",      ElementType.Light,   UnitRole.Battlemage,   200, 50, 50, 45),
            ("LIGHT_SPHINX",   "Sfinga",             "Klade hádanky, trestá chyby.",       ElementType.Light,   UnitRole.Controller,150, 55, 40, 55),
            ("LIGHT_GOLEM",    "Světelný golem",     "Oslňuje a omráčuje.",                ElementType.Light,   UnitRole.Tank,      240, 35, 65, 20),
            // Shadow
            ("SHADOW_WRAITH",  "Přízrak",            "Nehmotný, téměř nezranitelný.",      ElementType.Shadow,  UnitRole.Assassin,  70,  80, 10, 100),
            ("SHADOW_LICH",    "Lich",               "Pán nemrtvých s mocnou magií.",      ElementType.Shadow,  UnitRole.Caster,      140, 70, 30, 60),
            ("SHADOW_VAMPIRE", "Upír",               "Krade životy nepřátel.",             ElementType.Shadow,  UnitRole.Striker, 160, 65, 25, 65),
            // Poison
            ("POISON_BASILISK","Bazilišek",          "Pohled zkamení, jed zabíjí.",        ElementType.Poison,  UnitRole.Controller,145, 55, 35, 50),
            ("POISON_NAGA",    "Naga",               "Hadí kmen s smrtelným jedem.",       ElementType.Poison,  UnitRole.Bruiser,   155, 60, 30, 55),
            ("POISON_SLIME",   "Jedovatý sliz",      "Rozmnožuje se při zásahu.",          ElementType.Poison,  UnitRole.Tank,      200, 25, 55, 20),
            // Ice
            ("ICE_YETI",       "Yeti",               "Obr ze zasněžených hor.",            ElementType.Ice,     UnitRole.Juggernaut,270, 50, 55, 22),
            ("ICE_FROST_WOLF", "Mrazivý vlk",        "Smečka ledových vlků.",             ElementType.Ice,     UnitRole.Vanguard,  120, 65, 25, 80),
            ("ICE_ELEMENTAL",  "Ledový elementál",   "Mrzne vše kolem sebe.",              ElementType.Ice,     UnitRole.Caster,      130, 60, 30, 65),
        };

        var monsters = monsterDefs.Select(m => new MonsterTemplate
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

        db.MonsterTemplates.AddRange(monsters);
        await db.SaveChangesAsync(ct);

        // ── 8 Dungeons with 11 stages each ───────────────────────────────────
        var dungeonDefs = new (string Code, string Name, string Desc, ElementType El, int UnlockOrder, int MinLvl, int MaxLvl, string[] MonsterCodes)[]
        {
            ("DUNGEON_FIRE",   "Vulkánská kobka",     "Žár lávy a spalující plameny.",     ElementType.Fire,   1, 1,  5,  new[]{"FIRE_SPRITE","FIRE_GOLEM","FIRE_WYRM"}),
            ("DUNGEON_WATER",  "Podmořská říše",      "Hluboké vody plné nebezpečí.",      ElementType.Water,  2, 3,  8,  new[]{"WATER_JELLYFISH","WATER_SERPENT","WATER_KRAKEN"}),
            ("DUNGEON_EARTH",  "Kamenné labyrinty",   "Temné chodby plné pastí.",          ElementType.Earth,  3, 6,  12, new[]{"EARTH_SPIDER","EARTH_GOLEM","EARTH_TROLL"}),
            ("DUNGEON_AIR",    "Věžová citadela",     "Vysoké věže a vzdušné hrozby.",     ElementType.Air,    4, 9,  15, new[]{"AIR_HARPY","AIR_WYVERN","AIR_DJINN"}),
            ("DUNGEON_LIGHT",  "Chrám světla",        "Kdysi posvátné místo, nyní zkaž.",  ElementType.Light,  5, 12, 20, new[]{"LIGHT_SPHINX","LIGHT_GOLEM","LIGHT_ANGEL"}),
            ("DUNGEON_SHADOW", "Temná citadela",      "Brána do stínové říše.",            ElementType.Shadow, 6, 16, 25, new[]{"SHADOW_WRAITH","SHADOW_VAMPIRE","SHADOW_LICH"}),
            ("DUNGEON_POISON", "Jedovaté bažiny",     "Každý nádech otravuje krev.",       ElementType.Poison, 7, 20, 30, new[]{"POISON_SLIME","POISON_NAGA","POISON_BASILISK"}),
            ("DUNGEON_ICE",    "Věčné ledovce",       "Mrazivé hory bez milosti.",         ElementType.Ice,    8, 25, 40, new[]{"ICE_FROST_WOLF","ICE_YETI","ICE_ELEMENTAL"}),
        };

        var monsterMap = monsters.ToDictionary(m => m.Code);
        var stageTypes = Enum.GetValues<DungeonStageType>().OrderBy(x => (int)x).ToArray();

        foreach (var (code, name, desc, el, order, minLvl, maxLvl, mCodes) in dungeonDefs)
        {
            var dungeon = new Dungeon
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
            db.Dungeons.Add(dungeon);
            await db.SaveChangesAsync(ct);

            // 11 stages — cycle through the 3 monster templates
            for (int i = 1; i <= 11; i++)
            {
                var monsterCode = mCodes[(i - 1) % mCodes.Length];
                var stageType   = stageTypes[Math.Min(i - 1, stageTypes.Length - 1)];

                db.DungeonStages.Add(new DungeonStage
                {
                    Code               = $"{code}_S{i:D2}",
                    NameEn             = $"{name} — Stage {i}",
                    DescriptionEn      = $"Floor {i} of {name}.",
                    DungeonId          = dungeon.Guid,
                    StageIndex         = i,
                    StageType          = stageType,
                    MonsterTemplateId  = monsterMap[monsterCode].Guid,
                    RecommendedLevel   = minLvl + (i - 1),
                    DifficultyRating   = i,
                    IsActive           = true,
                    BaseLevel          = minLvl + (i - 1)
                });
            }
            await db.SaveChangesAsync(ct);
        }

        return $"✅ Seeded {monsters.Count} beast templates + {dungeonDefs.Length} dungeons (11 stages each).";
    }
}
