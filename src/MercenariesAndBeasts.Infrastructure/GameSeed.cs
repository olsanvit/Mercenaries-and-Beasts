using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Interface;
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain.Localization;
using MercenariesAndBeasts.Domain.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MercenariesAndBeasts.Infrastructure;

public sealed class GameSeed
{
    private readonly GameDbContext _db;
    private readonly IUnitAiGenerator _ai;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _cfg;
    private bool makeAll { get; set; } = false;

    public GameSeed(
        GameDbContext db,
        IUnitAiGenerator ai,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration cfg)
    {
        _db = db;
        _ai = ai;
        _userManager = userManager;
        _roleManager = roleManager;
        _cfg = cfg;
    }

    public async Task SeedAsync(bool makeAll = false, CancellationToken ct = default)
    {
        this.makeAll = makeAll;

        await SeedIdentityAsync(_userManager, _roleManager);
        await SeedCountriesFromGeoNamesAsync(ct);
        await SeedLocationsAndDungeonsFromLabyrinthJsonAsync(ct);
        await SeedItemTemplatesFromItemsFullJsonAsync(ct);
        await EnsureDomainLocalizationsAsync(ct);

        await _db.SaveChangesAsync(ct);
    }

    // ==========================================================
    // 1) IDENTITY
    // ==========================================================

    public async Task SeedIdentityAsync(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        const string adminRoleName = "Admin";

        if (!await roleManager.RoleExistsAsync(adminRoleName))
            await roleManager.CreateAsync(new IdentityRole(adminRoleName));

        await EnsureAdminAsync(userManager, adminRoleName,
            email: "admin@local",
            username: "admin",
            password: "Admin123.");

        await EnsureAdminAsync(userManager, adminRoleName,
            email: "olsanskyvitek@gmail.com",
            username: "vitek",
            password: "Vitek575");

        await EnsureAdminAsync(userManager, adminRoleName,
            email: "korba99@seznam.cz",
            username: "Bausisaur",
            password: "Abeceda123");
    }

    private static async Task EnsureAdminAsync(
        UserManager<AppUser> userManager,
        string adminRoleName,
        string email,
        string username,
        string password)
    {
        var user = await userManager.FindByEmailAsync(email);

        if (user is null)
        {
            user = new AppUser
            {
                UserName = username,
                Email = email,
                EmailConfirmed = true,
                IsAdmin = true
            };

            var result = await userManager.CreateAsync(user, password);
            if (!result.Succeeded)
                throw new Exception($"Failed to create user {email}: " +
                    string.Join(", ", result.Errors.Select(e => e.Description)));
        }

        if (!await userManager.IsInRoleAsync(user, adminRoleName))
        {
            var roleRes = await userManager.AddToRoleAsync(user, adminRoleName);
            if (!roleRes.Succeeded)
                throw new Exception($"Failed to add {email} to role {adminRoleName}: " +
                    string.Join(", ", roleRes.Errors.Select(e => e.Description)));
        }
    }

    // ==========================================================
    // 2) LOCATIONS + DUNGEONS from JSON (Orders/Beasts)
    // ==========================================================

    private static readonly JsonSerializerOptions JsonOpts = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public sealed class BeastRow
    {
        public int labyrinth_id { get; set; }
        public string? labyrinth_name { get; set; }
        public string? theme { get; set; }
        public string? beast { get; set; }
        public string? quality { get; set; }
    }

    public sealed class OrderRow
    {
        public int labyrinth_id { get; set; }
        public string? labyrinth_name { get; set; }
        public string? theme { get; set; }
        public string order { get; set; } = "";
        public string? quality { get; set; }
    }

    private static List<(string NameEn, string? Quality)> ExtractOrders(List<OrderRow> rows)
        => rows
            .Select(r => (NameEn: (r.order ?? "").Trim(), Quality: r.quality))
            .Where(x => x.NameEn.Length > 0)
            .DistinctBy(x => x.NameEn)
            .ToList();

    private static List<(string NameEn, string? Quality)> ExtractBeasts(List<BeastRow> rows)
        => rows
            .Select(r => (NameEn: (r.beast ?? "").Trim(), Quality: r.quality))
            .Where(x => x.NameEn.Length > 0)
            .DistinctBy(x => x.NameEn)
            .ToList();

    private async Task SeedLocationsAndDungeonsFromLabyrinthJsonAsync(CancellationToken ct)
    {
        const int targetCount = 5;

        var beastsPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "Dungeons_Beasts_Labyrinths_250_redistributed_beasts_fantasy_mix.json");
        var ordersPath = Path.Combine(AppContext.BaseDirectory, "SeedData", "Orders_Locations_250_orders_fantasy_mix.json");

        if (!File.Exists(beastsPath))
            throw new FileNotFoundException($"Missing beasts json: {beastsPath}");

        if (!File.Exists(ordersPath))
            throw new FileNotFoundException($"Missing orders json: {ordersPath}");

        var beastsJson = await File.ReadAllTextAsync(beastsPath, ct);
        var ordersJson = await File.ReadAllTextAsync(ordersPath, ct);

        var beasts = JsonSerializer.Deserialize<List<BeastRow>>(beastsJson, JsonOpts) ?? new();
        var orders = JsonSerializer.Deserialize<List<OrderRow>>(ordersJson, JsonOpts) ?? new();

        var beastsById = beasts
            .Where(x => x.labyrinth_id != 0)
            .GroupBy(x => x.labyrinth_id)
            .ToDictionary(g => g.Key, g => g.ToList());

        var ordersById = orders
            .Where(x => x.labyrinth_id != 0)
            .GroupBy(x => x.labyrinth_id)
            .ToDictionary(g => g.Key, g => g.ToList());

        var dbLocationCount = await _db.Locations.CountAsync(ct);
        var dbDungeonCount = await _db.Dungeons.CountAsync(ct);

        if (!makeAll && dbLocationCount >= targetCount && dbDungeonCount >= targetCount)
            return;

        // --------------------------
        // Locations (Orders)
        // --------------------------
        int missingLocations = makeAll ? int.MaxValue : Math.Max(0, targetCount - dbLocationCount);
        int createdLocations = 0;

        var lastLocation = await _db.Locations
            .OrderByDescending(x => x.UnlockOrder)
            .FirstOrDefaultAsync(ct);

        int minLevel = (lastLocation?.MinLevel ?? 0) + 1;
        int maxLevel = (lastLocation?.MaxLevel ?? 2) + 1;
        int unlockOrder = (lastLocation?.UnlockOrder ?? 0) + 1;

        foreach (var (labId, orderRows) in ordersById.OrderBy(kv => kv.Key))
        {
            ct.ThrowIfCancellationRequested();

            if (!makeAll && createdLocations >= missingLocations)
                break;

            var first = orderRows.FirstOrDefault();
            var name = first?.labyrinth_name ?? $"Labyrinth {labId}";
            var theme = first?.theme;

            var locCode = $"LOC_{labId}";
            if (await _db.Locations.AnyAsync(x => x.Code == locCode, ct))
                continue;

            var loc = new Location
            {
                Id = Guid.NewGuid(),
                Code = locCode,
                NameEn = name,
                Element = MapThemeToElement(theme),
                MinLevel = minLevel,
                MaxLevel = maxLevel,
                UnlockOrder = unlockOrder
            };

            _db.Locations.Add(loc);
            await _db.SaveChangesAsync(ct);

            var ordersCsv = string.Join(", ",
                orderRows
                    .Select(x => x.order)
                    .Where(o => !string.IsNullOrWhiteSpace(o))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
            );

            await _ai.GenerateNextLocationAsync(null, loc, true, ordersCsv, ct);
            await _db.SaveChangesAsync(ct);

            minLevel++;
            maxLevel++;
            unlockOrder++;
            createdLocations++;
        }

        // --------------------------
        // Dungeons (Beasts)
        // --------------------------
        int missingDungeons = makeAll ? int.MaxValue : Math.Max(0, targetCount - dbDungeonCount);
        int createdDungeons = 0;

        var lastDungeon = await _db.Dungeons
            .OrderByDescending(x => x.UnlockOrder)
            .FirstOrDefaultAsync(ct);

        minLevel = (lastDungeon?.MinLevel ?? 0) + 1;
        maxLevel = (lastDungeon?.MaxLevel ?? 2) + 1;
        unlockOrder = (lastDungeon?.UnlockOrder ?? 0) + 1;

        foreach (var (labId, beastRows) in beastsById.OrderBy(kv => kv.Key))
        {
            ct.ThrowIfCancellationRequested();

            if (!makeAll && createdDungeons >= missingDungeons)
                break;

            var first = beastRows.FirstOrDefault();
            var name = first?.labyrinth_name ?? $"Labyrinth {labId}";
            var theme = first?.theme;

            var dngCode = $"DUN_{labId}";
            if (await _db.Dungeons.AnyAsync(x => x.Code == dngCode, ct))
                continue;

            var dng = new Dungeon
            {
                Id = Guid.NewGuid(),
                Code = dngCode,
                NameEn = name,
                Element = MapThemeToElement(theme),
                MinLevel = minLevel,
                MaxLevel = maxLevel,
                UnlockOrder = unlockOrder
            };

            _db.Dungeons.Add(dng);
            await _db.SaveChangesAsync(ct);

            AttachPredefinedBeastStages(dng, beastRows);

            var beastsCsv = string.Join(", ",
                beastRows
                    .Select(x => x.beast)
                    .Where(o => !string.IsNullOrWhiteSpace(o))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
            );

            await _ai.GenerateNextDungeonAsync(null, dng, true, beastsCsv, ct);
            await _db.SaveChangesAsync(ct);

            minLevel++;
            maxLevel++;
            unlockOrder++;
            createdDungeons++;
        }

        // --------------------------
        // Local helpers
        // --------------------------

        void AttachPredefinedMercStages(Location loc, List<OrderRow> rows)
        {
            var stageCount = Enum.GetNames(typeof(ExpeditionStageType)).Length;

            var pairs = rows
                .Select(r => (Stage: TryGetInt(r, "stage", "stage_index", "stageIndex", "stageNumber", "stage_number") ?? 0,
                              Name: TryGetString(r, "mercenary_name", "enemy_name", "order_name", "name", "unit_name")))
                .Where(x => x.Stage > 0 && !string.IsNullOrWhiteSpace(x.Name))
                .ToList();

            if (pairs.Count == 0)
            {
                var names = rows
                    .Select(r => TryGetString(r, "mercenary_name", "enemy_name", "order_name", "name", "unit_name"))
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Select(n => n!.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Take(stageCount)
                    .ToList();

                loc.Stages = new List<ExpeditionStage>();
                for (int i = 0; i < names.Count; i++)
                {
                    loc.Stages.Add(new ExpeditionStage
                    {
                        Id = Guid.NewGuid(),
                        LocationId = loc.Id,
                        StageNumber = i + 1,
                        Enemy = new MercenaryTemplate { NameEn = names[i] }
                    });
                }

                return;
            }

            var stageToName = pairs
                .GroupBy(x => x.Stage)
                .OrderBy(g => g.Key)
                .Select(g => (Stage: g.Key, Name: g.Select(z => z.Name!.Trim()).First()))
                .Take(stageCount)
                .ToList();

            loc.Stages = new List<ExpeditionStage>();
            foreach (var (st, nm) in stageToName)
            {
                loc.Stages.Add(new ExpeditionStage
                {
                    Id = Guid.NewGuid(),
                    LocationId = loc.Id,
                    StageNumber = st,
                    Enemy = new MercenaryTemplate { NameEn = nm }
                });
            }
        }

        void AttachPredefinedBeastStages(Dungeon dng, List<BeastRow> rows)
        {
            var stageCount = Enum.GetNames(typeof(DungeonStageType)).Length;

            var pairs = rows
                .Select(r => (Stage: TryGetInt(r, "stage", "stage_index", "stageIndex", "stageNumber", "stage_number") ?? 0,
                              Name: TryGetString(r, "beast_name", "monster_name", "name", "unit_name")))
                .Where(x => x.Stage > 0 && !string.IsNullOrWhiteSpace(x.Name))
                .ToList();

            if (pairs.Count == 0)
            {
                var names = rows
                    .Select(r => TryGetString(r, "beast_name", "monster_name", "name", "unit_name"))
                    .Where(n => !string.IsNullOrWhiteSpace(n))
                    .Select(n => n!.Trim())
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .Take(stageCount)
                    .ToList();

                dng.Stages = new List<DungeonStage>();
                for (int i = 0; i < names.Count; i++)
                {
                    dng.Stages.Add(new DungeonStage
                    {
                        Id = Guid.NewGuid(),
                        DungeonId = dng.Id,
                        StageIndex = i + 1,
                        MonsterTemplate = new MonsterTemplate { NameEn = names[i] }
                    });
                }

                return;
            }

            var stageToName = pairs
                .GroupBy(x => x.Stage)
                .OrderBy(g => g.Key)
                .Select(g => (Stage: g.Key, Name: g.Select(z => z.Name!.Trim()).First()))
                .Take(stageCount)
                .ToList();

            dng.Stages = new List<DungeonStage>();
            foreach (var (st, nm) in stageToName)
            {
                dng.Stages.Add(new DungeonStage
                {
                    Id = Guid.NewGuid(),
                    DungeonId = dng.Id,
                    StageIndex = st,
                    MonsterTemplate = new MonsterTemplate { NameEn = nm }
                });
            }
        }

        static string? TryGetString(object obj, params string[] names)
        {
            var t = obj.GetType();
            foreach (var n in names)
            {
                var p = t.GetProperty(n, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.IgnoreCase);
                if (p == null) continue;

                var v = p.GetValue(obj);
                if (v == null) continue;

                var s = v.ToString();
                if (!string.IsNullOrWhiteSpace(s))
                    return s;
            }
            return null;
        }

        static int? TryGetInt(object obj, params string[] names)
        {
            var s = TryGetString(obj, names);
            if (string.IsNullOrWhiteSpace(s)) return null;

            if (int.TryParse(s, out var i)) return i;

            var digits = new string(s.Where(char.IsDigit).ToArray());
            if (int.TryParse(digits, out i)) return i;

            return null;
        }
    }

    private static string ToCode(string prefix, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return prefix + "UNKNOWN";

        var upper = name.Trim().ToUpperInvariant();

        var sb = new StringBuilder();
        foreach (var ch in upper)
        {
            if (ch >= 'A' && ch <= 'Z') sb.Append(ch);
            else if (ch >= '0' && ch <= '9') sb.Append(ch);
            else sb.Append('_');
        }

        var code = sb.ToString();

        while (code.Contains("__"))
            code = code.Replace("__", "_");

        code = code.Trim('_');

        return prefix + code;
    }

    private static ElementType MapThemeToElement(string? theme)
    {
        if (string.IsNullOrWhiteSpace(theme))
            return ElementType.None;

        return theme.Trim().ToLowerInvariant() switch
        {
            "fire" or "flame" or "lava" or "ember" or "inferno" => ElementType.Fire,
            "water" or "ocean" or "sea" or "river" or "flood" => ElementType.Water,
            "ice" or "frost" or "snow" or "glacier" or "cold" => ElementType.Ice,
            "nature" or "forest" or "jungle" or "swamp" or "meadow" => ElementType.Nature,
            "earth" or "stone" or "rock" or "mountain" or "cave" => ElementType.Earth,
            "air" or "wind" or "storm" or "sky" => ElementType.Air,
            "lightning" or "thunder" or "stormlight" => ElementType.Lightning,
            "shadow" or "dark" or "darkness" or "night" => ElementType.Shadow,
            "light" or "holy" or "radiant" => ElementType.Light,
            "poison" or "toxic" or "venom" or "plague" => ElementType.Poison,
            "metal" or "iron" or "steel" or "forge" => ElementType.Metal,
            "arcane" or "magic" or "mana" => ElementType.Arcane,
            "plasma" or "energy" => ElementType.Plasma,
            "void" or "abyss" or "eldritch" => ElementType.Void,
            "physical" or "brutal" => ElementType.Physical,
            _ => ElementType.None
        };
    }

    // ==========================================================
    // 3) ITEM TEMPLATES from items_full.json
    // ==========================================================

    public sealed record ItemsFile(
        [property: JsonPropertyName("items")] List<ItemRow> Items
    );

    public sealed record ItemRow(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("itemType")] string? ItemType,
        [property: JsonPropertyName("group")] string? Group
    );

    private async Task SeedItemTemplatesFromItemsFullJsonAsync(CancellationToken ct)
    {
        var pocet = _db.ItemTemplates.Count();
        if (pocet > 22 && !makeAll)
            return;

        var path = Path.Combine(AppContext.BaseDirectory, "SeedData", "items_full.json");
        if (!File.Exists(path))
            throw new FileNotFoundException($"Missing items json: {path}");

        var json = await File.ReadAllTextAsync(path, ct);

        var file = JsonSerializer.Deserialize<ItemsFile>(json, new JsonSerializerOptions(JsonSerializerDefaults.Web));
        var rows = file?.Items ?? new List<ItemRow>();

        if (!makeAll)
        {
            rows = rows
                .Where(r => !string.IsNullOrWhiteSpace(r.Group))
                .GroupBy(r => r.Group!.Trim(), StringComparer.OrdinalIgnoreCase)
                .SelectMany(g => g
                    .OrderBy(x => (x.Name ?? "").Trim())
                    .Take(5))
                .ToList();
        }

        foreach (var r in rows)
        {
            ct.ThrowIfCancellationRequested();

            var name = (r.Name ?? "").Trim();
            if (name.Length == 0)
                continue;

            if (string.IsNullOrWhiteSpace(r.Group))
                continue;

            if (!Enum.TryParse<ItemEquipSlot>(
                    r.Group,
                    ignoreCase: true,
                    out var equipSlot)
                || equipSlot == ItemEquipSlot.None)
                continue;

            var ownerKind = ItemEquipSlotRules.OwnerOf(equipSlot);
            if (ownerKind == ItemOwnerKind.None)
                continue;

            var code = ToCode(name);

            var existing = await _db.ItemTemplates.FirstOrDefaultAsync(x => x.Code == code, ct);
            if (existing is not null)
                continue;

            var tpl = new ItemTemplate
            {
                Id = Guid.NewGuid(),
                Code = code,
                NameEn = name,
                BaseQuality = QualityTier.Q1_Common,
                BaseStats = StatBlock.Zero,
                OwnerKind = ownerKind,
                MercenarySlot = ownerKind == ItemOwnerKind.Mercenary ? equipSlot : null,
                MonsterSlot = ownerKind == ItemOwnerKind.Beast ? equipSlot : null
            };

            var a = await _ai.GenerateItemTemplateAsync(tpl, r.Group, ct);
            _db.ItemTemplates.Add(a);
            await _db.SaveChangesAsync(ct);
        }
    }

    private static string ToCode(string nameEn)
    {
        var s = nameEn.Trim().ToUpperInvariant();

        var chars = s.Select(c => char.IsLetterOrDigit(c) ? c : ' ').ToArray();
        var cleaned = new string(chars);

        while (cleaned.Contains("  "))
            cleaned = cleaned.Replace("  ", " ");

        return cleaned.Replace(" ", "_");
    }

    private async Task SeedItemTemplatesViaAiAsync(CancellationToken ct)
    {
        var allSlots = Enum.GetValues<ItemEquipSlot>()
            .Where(s => s != ItemEquipSlot.None)
            .OrderBy(s => (int)s)
            .ToList();

        foreach (var slot in allSlots)
        {
            ct.ThrowIfCancellationRequested();

            var owner = ItemEquipSlotRules.OwnerOf(slot);

            ItemTemplate? existing = owner == ItemOwnerKind.Mercenary
                ? await _db.ItemTemplates.FirstOrDefaultAsync(x => x.MercenarySlot == slot, ct)
                : await _db.ItemTemplates.FirstOrDefaultAsync(x => x.MonsterSlot == slot, ct);

            if (existing != null)
                continue;

            var themeHint = owner == ItemOwnerKind.Mercenary ? "orders gear" : "beast biology";

            await _ai.GenerateItemTemplateAsync(existing, themeHint, ct);
        }
    }

    private async Task SeedCountriesFromGeoNamesAsync(CancellationToken ct)
    {
        if (await _db.Countries.AnyAsync(ct))
            return;

        var seedDir = Path.Combine(AppContext.BaseDirectory, "SeedData");
        Directory.CreateDirectory(seedDir);

        var localPath = Path.Combine(seedDir, "countryInfo.txt");

        if (!File.Exists(localPath))
        {
            using var http = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(15)
            };

            var url = "https://download.geonames.org/export/dump/countryInfo.txt";
            var data = await http.GetStringAsync(url, ct);

            await File.WriteAllTextAsync(localPath, data, ct);
        }

        var lines = await File.ReadAllLinesAsync(localPath, ct);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                continue;

            var cols = line.Split('\t');
            if (cols.Length < 9)
                continue;

            var iso2 = cols[0].Trim();
            var iso3 = cols[1].Trim();
            var name = cols[4].Trim();
            var continent = cols[8].Trim();

            if (iso2.Length != 2)
                continue;

            long.TryParse(cols[7], out var population);

            _db.Countries.Add(new Country
            {
                Code = iso2,
                Iso3 = iso3,
                NameEn = name,
                Continent = continent,
                Population = population,
                IsActive = true
            });
        }

        await _db.SaveChangesAsync(ct);
    }

    private static bool UpsertLocalized(
        GameDbContext db,
        string entityType,
        Guid entityId,
        string code,
        LocalizedNameResult res)
    {
        if (res?.Locales is null || res.Locales.Count == 0)
            return false;

        bool changed = false;

        foreach (var culture in LocalizationConfig.SupportedLocales)
        {
            if (!res.Locales.TryGetValue(culture, out var dto) || dto is null)
                continue;

            var name = (dto.Name ?? "").Trim();
            if (string.IsNullOrWhiteSpace(name))
                continue;

            string? desc = string.IsNullOrWhiteSpace(res.DescriptionEn)
                ? null
                : string.IsNullOrWhiteSpace(dto.Description) ? null : dto.Description!.Trim();

            changed |= UpsertOne(db, entityType, entityId, code, culture, name, desc);
        }

        return changed;
    }

    private static bool UpsertOne(
        GameDbContext db,
        string entityType,
        Guid entityId,
        string code,
        string culture,
        string name,
        string? description)
    {
        var existing = db.LocalizedTexts.FirstOrDefault(x =>
            x.EntityType == entityType &&
            x.EntityId == entityId &&
            x.Culture == culture);

        if (existing is null)
        {
            db.LocalizedTexts.Add(new LocalizedText
            {
                Id = Guid.NewGuid(),
                EntityType = entityType,
                EntityId = entityId,
                Culture = culture,
                Code = code ?? "",
                Name = name,
                Description = description,
                UpdatedUtc = DateTime.UtcNow
            });
            return true;
        }

        bool changed = false;

        if (!string.Equals(existing.Name, name, StringComparison.Ordinal))
        {
            existing.Name = name;
            changed = true;
        }

        if (!string.Equals(existing.Description, description, StringComparison.Ordinal))
        {
            existing.Description = description;
            changed = true;
        }

        if (!string.IsNullOrWhiteSpace(code) &&
            !string.Equals(existing.Code, code, StringComparison.Ordinal))
        {
            existing.Code = code;
            changed = true;
        }

        if (changed)
        {
            existing.UpdatedUtc = DateTime.UtcNow;
            db.LocalizedTexts.Update(existing);
        }

        return changed;
    }

    private async Task EnsureDomainLocalizationsAsync(CancellationToken ct)
    {
        var pocet = _db.LocalizedTexts.Count();
        if (pocet >= 111 && !makeAll)
            return;

        await EnsureEntityLocalizationsAsync<Location>(
            entityType: nameof(Location),
            query: _db.Locations.AsNoTracking(),
            getId: x => x.Id,
            getCode: x => x.Code,
            getNameEn: x => x.NameEn,
            getDescEn: x => x.DescriptionEn,
            entityKind: "location",
            makeAll: makeAll,
            ct: ct
        );

        await EnsureEntityLocalizationsAsync<Dungeon>(
            entityType: nameof(Dungeon),
            query: _db.Dungeons.AsNoTracking(),
            getId: x => x.Id,
            getCode: x => x.Code,
            getNameEn: x => x.NameEn,
            getDescEn: x => x.DescriptionEn,
            entityKind: "dungeon",
            makeAll: makeAll,
            ct: ct
        );

        await EnsureEntityLocalizationsAsync<MonsterTemplate>(
            entityType: nameof(MonsterTemplate),
            query: _db.MonsterTemplates.AsNoTracking(),
            getId: x => x.Id,
            getCode: x => x.Code,
            getNameEn: x => x.NameEn,
            getDescEn: x => x.DescriptionEn,
            entityKind: "beast",
            makeAll: makeAll,
            ct: ct
        );

        await EnsureEntityLocalizationsAsync<MercenaryTemplate>(
            entityType: nameof(MercenaryTemplate),
            query: _db.MercenaryTemplates.AsNoTracking(),
            getId: x => x.Id,
            getCode: x => x.Code,
            getNameEn: x => x.NameEn,
            getDescEn: x => x.DescriptionEn,
            entityKind: "order",
            makeAll: makeAll,
            ct: ct
        );

        await EnsureEntityLocalizationsAsync<ItemTemplate>(
            entityType: nameof(ItemTemplate),
            query: _db.ItemTemplates.AsNoTracking(),
            getId: x => x.Id,
            getCode: x => x.Code,
            getNameEn: x => x.NameEn,
            getDescEn: x => x.DescriptionEn,
            entityKind: "item",
            makeAll: makeAll,
            ct: ct
        );

        await EnsureEntityLocalizationsAsync<ItemUpgradeResource>(
            entityType: nameof(ItemUpgradeResource),
            query: _db.ItemUpgradeResources.AsNoTracking(),
            getId: x => x.Id,
            getCode: x => x.Code,
            getNameEn: x => x.NameEn,
            getDescEn: x => x.DescriptionEn,
            entityKind: "item_upgrade_resource",
            makeAll: makeAll,
            ct: ct
        );
    }

    private async Task EnsureEntityLocalizationsAsync<T>(
        string entityType,
        IQueryable<T> query,
        Expression<Func<T, Guid>> getId,
        Expression<Func<T, string>> getCode,
        Expression<Func<T, string>> getNameEn,
        Expression<Func<T, string?>> getDescEn,
        string entityKind,
        bool makeAll,
        CancellationToken ct)
        where T : class
    {
        ct.ThrowIfCancellationRequested();

        var list = await query
            .AsNoTracking()
            .OrderBy(getCode)
            .Select(x => new
            {
                Id = EF.Property<Guid>(x, ((MemberExpression)getId.Body).Member.Name),
                Code = EF.Property<string>(x, ((MemberExpression)getCode.Body).Member.Name),
                NameEn = EF.Property<string>(x, ((MemberExpression)getNameEn.Body).Member.Name),
                DescEn = EF.Property<string?>(x, ((MemberExpression)getDescEn.Body).Member.Name),
            })
            .ToListAsync(ct);

        if (!makeAll)
            list = list.Take(1).ToList();

        var ids = list.Select(x => x.Id).ToList();

        var existing = await _db.LocalizedTexts
            .Where(x => x.EntityType == entityType && ids.Contains(x.EntityId))
            .Select(x => new { x.EntityId, x.Culture })
            .ToListAsync(ct);

        var map = existing
            .GroupBy(x => x.EntityId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Culture).ToHashSet(StringComparer.OrdinalIgnoreCase));

        var pocet = 11;
        foreach (var e in list)
        {
            ct.ThrowIfCancellationRequested();

            var have = map.TryGetValue(e.Id, out var set)
                ? set
                : new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var missing = LocalizationConfig.SupportedLocales
                .Where(c => !have.Contains(c))
                .ToList();

            if (missing.Count == 0)
                continue;

            if (missing.Count > pocet)
            {
                var rng = Random.Shared;
                for (int i = missing.Count - 1; i > 0; i--)
                {
                    int j = rng.Next(i + 1);
                    (missing[i], missing[j]) = (missing[j], missing[i]);
                }

                missing = missing.Take(pocet).ToList();
            }

            var loc = await _ai.GenerateLocalizedNamesAsync(entityKind, e.NameEn, e.DescEn, missing, ct);
            if (loc is null) continue;

            UpsertLocalized(_db, entityType, e.Id, e.Code, loc);
            await _db.SaveChangesAsync(ct);
        }
    }
}