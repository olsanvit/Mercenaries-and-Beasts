using System.Text;
using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Dto;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Interface;
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain.Localization;
using MercenariesAndBeasts.Domain.Utils;
using MercenariesAndBeasts.Items;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MercenariesAndBeasts.Infrastructure.AI;

public class AiUnitGeneratorService : IUnitAiGenerator
{
    private string GameTheme =>
        "A future-fantasy role-playing universe where ancient magic and subtle arcane-tech coexist. " +
        "Aesthetic: atmospheric, enigmatic, slightly grim, with echoes of lost civilizations, relic-cults, and forbidden rites. " +
        "World tone: immersive, mysterious, premium RPG feeling. " +
        "Sci-fi is allowed only as rare, mystical 'arcane-tech' (relic devices, resonant glyph-machines) — never modern or cyberpunk. " +
        "Avoid explicit modern/IT terms: interface, UI, database, smartphone, drone, android, chatbot, nanotech, cybernetics, laser rifle. " +
        "Naming style: unique, evocative, lore-friendly, and easy to pronounce; never generic. " +
        "Names must NOT include numbers, hyphens, or MMO-style tags. " +
        "Forbidden clichés: 'of Doom', 'of the Ancients', 'Ultimate', 'Extreme', 'Legendary', and similar patterns. " +
        "Descriptions should be vivid, atmospheric, immersive, and consistent with the future-fantasy tone."+
        "LANGUAGE RULE: Use ONLY American English spelling and terminology (e.g., Armor, Color, Defense). Never use British variants.";

    private readonly ChatGptAsker _asker;
    private readonly GameDbContext _db;
    private readonly ILogger<AiUnitGeneratorService> _logger;

    public AiUnitGeneratorService(
        ChatGptAsker asker,
        GameDbContext db,
        ILogger<AiUnitGeneratorService> logger)
    {
        _asker = asker;
        _db = db;
        _logger = logger;
    }
    static List<string> ParseNames(string csv)
{
    return csv
        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
        .Where(x => !string.IsNullOrWhiteSpace(x))
        .Distinct(StringComparer.OrdinalIgnoreCase)
        .ToList();
}

    // ==========================================================
    // DUNGEON
    // ==========================================================
    public async Task<Dungeon?> GenerateNextDungeonAsync(
        Dungeon? previousDungeon,
        Dungeon? currDungeon,
        bool currentDungeon,
    string? predefinedBeastsCsv = "",
        CancellationToken ct = default)
    {
        try
        {
            var predefinedBeasts =
    !string.IsNullOrWhiteSpace(predefinedBeastsCsv)
        ? ParseNames(predefinedBeastsCsv)
        : new List<string>();
        (int nextMin, int nextMax) = ComputeNextLevelRange(
            previousMin: previousDungeon?.MinLevel,
            previousMax: previousDungeon?.MaxLevel,
            currentMin: currDungeon?.MinLevel,
            currentMax: currDungeon?.MaxLevel,
            current: currentDungeon);

        var stageCount = Enum.GetNames(typeof(DungeonStageType)).Length;
        var previousName = previousDungeon?.NameEn ?? "None";

        var jsonShape = LlmSchemaBuilder.BuildJsonShape<DungeonGenerationResult>();
        var stageTypeEnumValues = LlmSchemaBuilder.BuildEnumValues<DungeonStageType>();
        var unitRoles = LlmSchemaBuilder.BuildEnumValues<UnitRole>();
        var elementEnumValues = LlmSchemaBuilder.BuildEnumValues<ElementType>();

        // ✅ PREDEFINED beasts (pokud jsou stages už předvyplněné)
        var predefinedBeastNames = currDungeon?.Stages?
            .Where(s => s?.MonsterTemplate?.NameEn != null && s.MonsterTemplate.NameEn != "")
            .OrderBy(s => s!.StageIndex)
            .Select(s => s!.MonsterTemplate!.NameEn!)
            .Distinct()
            .ToList() ?? new();

        string predefinedBeastsBlock = "";

if (predefinedBeasts.Count > 0)
{
    predefinedBeastsBlock = $@"

PREDEFINED BEASTS (STRICT):
The following Beast names are PREDEFINED and MUST be used exactly as provided.
You MUST NOT invent new Beast names.
You MUST NOT rename or stylize them.

Rules:
- Use ONLY these names for dungeon stages.
- Keep NameEn EXACTLY as provided (ASCII stays ASCII).
- Translate only if non-English characters are present.

Beast names:
{string.Join("\n", predefinedBeasts.Select(n => $"- \"{n}\""))}
";
}

        string currentOrNext;
        if (currentDungeon)
        {
            var originalName = currDungeon?.NameEn ?? "Unknown";

            currentOrNext = $@"
The player provided a manual dungeon name in natural language: ""{originalName}"".

You MUST use this name as the dungeon's NameEn EXACTLY, unless it contains non-English characters.
- If it is already plain English ASCII, keep it EXACTLY as-is.
- If it contains diacritics or non-English, translate it to English, but KEEP the SAME MEANING and keep it short.

STRICT:
- NameEn MUST equal the final chosen name (either the original unchanged or its direct translation).
- You MUST NOT invent a different name.
- You MUST NOT use synonyms, rebranding, or creative renaming.
- Generate THIS dungeon, do NOT create a next one.
{predefinedBeastsBlock}
";
        }
        else
        {
            currentOrNext = $@"
The player has just completed a dungeon named ""{previousName}"".
Generate the NEXT dungeon in the progression.
";
        }

        var systemPrompt = $@"
You are an assistant generating procedural dungeons for {GameTheme}.

You MUST ALWAYS return ONLY valid JSON.
The JSON MUST be deserializable into this C#-like shape:

{jsonShape}

Rules:
- ""stageType"" must be one of: {stageTypeEnumValues}
- ""element"" must be one of: {elementEnumValues}
- ""unitRole"" must be one of: {unitRoles}
- The 'stages' array MUST contain exactly {stageCount} items.
- Code must be derived from NameEn:
    - Start with ""DNG_""
    - Uppercase
    - Keep only A–Z, 0–9 and spaces
    - Replace spaces with ""_""
";

        var existingDungeonNames = await _db.Dungeons
            .Select(x => x.NameEn)
            .Where(x => x != null && x != "")
            .ToListAsync(ct);

        var existingBeastNames = await _db.MonsterTemplates
            .Select(x => x.NameEn)
            .Where(x => x != null && x != "")
            .ToListAsync(ct);

        var bannedDungeons = string.Join("\n", existingDungeonNames.Take(60).Select(n => $"- \"{n}\""));
        var bannedBeasts   = string.Join("\n", existingBeastNames.Take(60).Select(n => $"- \"{n}\""));

        // ✅ Pokud jsou predefined beasts, NECHCEŠ aplikovat "originality against existing beasts"
        // na tyto předurčené názvy (jinak si bude prompt odporovat).
        // Takže je z banlistu vyfiltrujeme.
        if (predefinedBeastNames.Count > 0)
        {
            var set = predefinedBeastNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
            bannedBeasts = string.Join("\n",
                existingBeastNames
                    .Where(n => !set.Contains(n))
                    .Take(300)
                    .Select(n => $"- \"{n}\""));
        }

        var userPrompt = $@"
{currentOrNext}
{predefinedBeastsBlock}

ORIGINALITY (HARD):
- Dungeon NameEn MUST NOT match any existing dungeon name.
- Must not be a near-duplicate either (no pluralization, no adding adjectives like 'Dark', no minor reordering).
- Beast (monster) NameEn MUST NOT match any existing beast name.
- Must not reuse the same root word (avoid the same distinctive noun across multiple beasts).

Existing Dungeon names (DO NOT reuse):
{bannedDungeons}

Existing Beast names (DO NOT reuse):
{bannedBeasts}

THEMATIC COHESION (HARD):
- Every beast must feel like it belongs to THIS dungeon's theme, element, and atmosphere.
- Avoid beasts that contradict the dungeon setting.

Constraints:
- recommendedMinLevel = {nextMin}
- recommendedMaxLevel = {nextMax}
- stages MUST contain exactly {stageCount} stages
- stageIndex 1..{stageCount}
- use each stageType exactly once

Return ONLY JSON.
";

            var generated = await _asker.AskJsonAsync<DungeonGenerationResult>(systemPrompt, userPrompt, ct);

            // decide target entity (update vs new)
            Dungeon dungeon;
            if (currentDungeon)
            {
                if (currDungeon is null)
                    throw new InvalidOperationException("currentDungeon=true but currDungeon is null.");

                dungeon = currDungeon;
            }
            else
            {
                dungeon = new Dungeon
                {
                    Id = Guid.NewGuid(),
                    UnlockOrder = (previousDungeon?.UnlockOrder ?? 0) + 1
                };
                _db.Dungeons.Add(dungeon);
            }

            // update dungeon fields
            dungeon.NameEn = CleanName(generated.NameEn);
            dungeon.DescriptionEn = generated.DescriptionEn;
            dungeon.Element = TryParseElement(generated.Element);
            dungeon.MinLevel = generated.MinLevel;
            dungeon.MaxLevel = generated.MaxLevel;
            if (!currentDungeon)
                dungeon.Code = ToCodePrefix("LOC_", dungeon.NameEn);

            // keep unlock order if already set (manual seeded)
            if (currentDungeon && currDungeon!.UnlockOrder > 0)
                dungeon.UnlockOrder = currDungeon.UnlockOrder;

            // wipe old stages (and optionally orphaned templates)
            if (currentDungeon)
            {
                var oldStages = await _db.DungeonStages
                    .Where(s => s.DungeonId == dungeon.Id)
                    .ToListAsync(ct);
                _db.DungeonStages.RemoveRange(oldStages);

                // POZN: MonsterTemplates seedované jen pro ten dungeon můžeš mazat taky,
                // ale musíš mít jistotu, že nejsou sdílené jinde.
            }

            // recreate stages + monsters
            foreach (var st in generated.Stages.OrderBy(s => s.StageNumber))
            {
                var m = st.Monster;

                var monster = new MonsterTemplate
                {
                    Id = Guid.NewGuid(),
                    Code = ToCodePrefix($"{dungeon.Code}_", CleanName(m.NameEn)),
                    NameEn = CleanName(m.NameEn),
                    DescriptionEn = m.DescriptionEn,
                    Element = TryParseElement(m.Element),
                    BaseLevel = m.BaseLevel,
                    BaseStats = m.Stats
                };

                _db.MonsterTemplates.Add(monster);
                await EnsureBeastItemTemplateAsync(monster, ct);
            
                var stage = new DungeonStage
                {
                    Id = Guid.NewGuid(),
                    Code = $"{dungeon.Code}_STAGE_{st.StageNumber}",
                    DungeonId = dungeon.Id,
                    StageIndex = st.StageNumber,
                    StageType = Enum.TryParse<DungeonStageType>(
                        st.StageType.ToString(),
                        ignoreCase: true,
                        out var stType)
                        ? stType
                        : DungeonStageType.DS1_Feeble,
                    MonsterTemplateId = monster.Id
                };

                _db.DungeonStages.Add(stage);
            }

            await _db.SaveChangesAsync(ct);
            return dungeon;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating dungeon via OpenAI.");
            return null;
        }
    }
    private async Task EnsureBeastItemTemplateAsync(
    MonsterTemplate monster,
    CancellationToken ct)
{
    // idempotence – 1 item per beast
    var code = $"ITM_BEAST_{monster.Code}";

    if (await _db.ItemTemplates.AnyAsync(x => x.Code == code, ct))
        return;

    var item = new ItemTemplate
    {
        Id = Guid.NewGuid(),
        Code = code,

        // výchozí ekvivalentní název (AI může lehce vylepšit bez změny významu)
        NameEn = monster.NameEn,
        DescriptionEn = monster.DescriptionEn,

        OwnerKind = ItemOwnerKind.Beast,

        MercenarySlot = null,
        MonsterSlot   = ItemEquipSlot.Beast_Entity,
        MonsterTemplateId = monster.Id,

        BaseQuality = QualityTier.Q1_Common,
        BaseStats   = StatBlock.Zero
    };
    item.BaseStats = monster.BaseStats;

    // ✅ zavolat AI generátor (slot se nesmí změnit)
    await ((IUnitAiGenerator)this).GenerateItemTemplateAsync(
        tpl: item,
        groupHint: $"Beast item linked to beast \"{monster.NameEn}\" ({monster.Code}). Keep identity consistent with the beast and its vibe.",
        ct: ct);

    await _db.SaveChangesAsync(ct);
}

    // ==========================================================
    // LOCATION
    // ==========================================================
    public async Task<Location?> GenerateNextLocationAsync(
        Location? previousLocation,
        Location? currLocation,
        bool currentLocation,
    string? predefinedOrdersCsv = "",
        CancellationToken ct = default)
    {
        try
        {
            var predefinedOrders =
    !string.IsNullOrWhiteSpace(predefinedOrdersCsv)
        ? ParseNames(predefinedOrdersCsv)
        : new List<string>();
             (int nextMin, int nextMax) = ComputeNextLevelRange(
            previousMin: previousLocation?.MinLevel,
            previousMax: previousLocation?.MaxLevel,
            currentMin: currLocation?.MinLevel,
            currentMax: currLocation?.MaxLevel,
            current: currentLocation);

        var stageCount = Enum.GetNames(typeof(ExpeditionStageType)).Length;
        var previousName = previousLocation?.NameEn ?? "None";

        var jsonShape = LlmSchemaBuilder.BuildJsonShape<ExpeditionGenerationResult>();
        var stageTypeEnumValues = LlmSchemaBuilder.BuildEnumValues<ExpeditionStageType>();
        var unitRoles = LlmSchemaBuilder.BuildEnumValues<UnitRole>();
        var elementEnumValues = LlmSchemaBuilder.BuildEnumValues<ElementType>();

        // ✅ PREDEFINED orders (pokud jsou stages už předvyplněné)
        var predefinedOrderNames = currLocation?.Stages?
            .Where(s => s?.Enemy?.NameEn != null && s.Enemy.NameEn != "")
            .OrderBy(s => s!.StageNumber)
            .Select(s => s!.Enemy!.NameEn!)
            .Distinct()
            .ToList() ?? new();


        string predefinedOrdersBlock = "";

if (predefinedOrders.Count > 0)
{
    predefinedOrdersBlock = $@"

PREDEFINED ORDERS (STRICT):
The following Order names are PREDEFINED and MUST be used exactly as provided.
You MUST NOT invent new Order names.
You MUST NOT rename, rephrase, or stylize them.

Rules:
- Use ONLY these names for stage enemies.
- Keep NameEn EXACTLY as provided (ASCII stays ASCII).
- If a name contains non-English characters, translate to English, KEEP MEANING.

Order names (use these only):
{string.Join("\n", predefinedOrders.Select(n => $"- \"{n}\""))}
";
}

        string currentOrNext;
        if (currentLocation)
        {
            var originalName = currLocation?.NameEn ?? "Unknown";

            currentOrNext = $@"
The player provided a manual location name in natural language: ""{originalName}"".

You MUST use this name as the location's NameEn EXACTLY, unless it contains non-English characters.
- If it is already plain English ASCII, keep it EXACTLY as-is.
- If it contains diacritics or non-English, translate it to English, but KEEP the SAME MEANING and keep it short.

STRICT:
- NameEn MUST equal the final chosen name.
- You MUST NOT invent a different name.
- Generate THIS location, do NOT create a next one.
{predefinedOrdersBlock}
";
        }
        else
        {
            currentOrNext = $@"
The player has just completed an expedition location named ""{previousName}"".
Generate the NEXT location in the progression.
";
        }

        var systemPrompt = $@"
You are an assistant generating procedural expedition locations for {GameTheme}.

You MUST ALWAYS return ONLY valid JSON.
The JSON MUST be deserializable into this C#-like shape:

{jsonShape}

Rules:
- ""StageType"" must be one of: {stageTypeEnumValues}
- ""Element"" must be one of: {elementEnumValues}
- ""unitRole"" must be one of: {unitRoles}
- The 'Stages' array MUST contain exactly {stageCount} items.
- Code must be derived from NameEn:
    - Start with ""LOC_""
    - Uppercase
    - Keep only A–Z, 0–9 and spaces
    - Replace spaces with ""_""
";

        var existingLocationNames = await _db.Locations
            .Select(x => x.NameEn)
            .Where(x => x != null && x != "")
            .ToListAsync(ct);

        var existingMercNames = await _db.MercenaryTemplates
            .Select(x => x.NameEn)
            .Where(x => x != null && x != "")
            .ToListAsync(ct);

        var bannedLocations = string.Join("\n", existingLocationNames.Take(300).Select(n => $"- \"{n}\""));

        var bannedMercs = string.Join("\n", existingMercNames.Take(300).Select(n => $"- \"{n}\""));

        // ✅ zase: pokud jsou predefined, vyhoď je z banlistu
        if (predefinedOrderNames.Count > 0)
        {
            var set = predefinedOrderNames.ToHashSet(StringComparer.OrdinalIgnoreCase);
            bannedMercs = string.Join("\n",
                existingMercNames
                    .Where(n => !set.Contains(n))
                    .Take(300)
                    .Select(n => $"- \"{n}\""));
        }

        var userPrompt = $@"
{currentOrNext}
{predefinedOrdersBlock}

ORIGINALITY (HARD):
- Location NameEn MUST NOT match any existing location name.
- Must not be a near-duplicate either.
- Mercenary NameEn MUST NOT match any existing mercenary name.
- Avoid reusing the same distinctive noun root across mercenaries.

Existing Location names (DO NOT reuse):
{bannedLocations}

Existing Mercenary names (DO NOT reuse):
{bannedMercs}

ORDER / LOCATION COHESION (HARD):
- Treat the stage enemies as 'Orders' that originate from this location.
- Each mercenary must feel tied to the location's ecosystem/ruins/curse/faction.

Constraints:
- MinLevel = {nextMin}
- MaxLevel = {nextMax}
- Stages MUST contain exactly {stageCount} stages
- StageNumber 1..{stageCount}
- Use each StageType exactly once

Return ONLY JSON.
";

            var generated = await _asker.AskJsonAsync<ExpeditionGenerationResult>(systemPrompt, userPrompt, ct);

            Location location;
            if (currentLocation)
            {
                if (currLocation is null)
                    throw new InvalidOperationException("currentLocation=true but currLocation is null.");

                location = currLocation;
            }
            else
            {
                location = new Location
                {
                    Id = Guid.NewGuid(),
                    UnlockOrder = (previousLocation?.UnlockOrder ?? 0) + 1
                };
                _db.Locations.Add(location);
            }

            location.NameEn = CleanName(generated.NameEn);
            location.DescriptionEn = generated.DescriptionEn;
            location.Element = TryParseElement(generated.Element);
            location.MinLevel = generated.MinLevel;
            location.MaxLevel = generated.MaxLevel;
            if (!currentLocation)
                location.Code = ToCodePrefix("LOC_", location.NameEn);

            if (currentLocation && currLocation!.UnlockOrder > 0)
                location.UnlockOrder = currLocation.UnlockOrder;

            if (currentLocation)
            {
                var oldStages = await _db.ExpeditionStages
                    .Where(s => s.LocationId == location.Id)
                    .ToListAsync(ct);
                _db.ExpeditionStages.RemoveRange(oldStages);
            }

            foreach (var st in generated.Stages.OrderBy(s => s.StageNumber))
            {
                var m = st.Mercenary;

                var merc = new MercenaryTemplate
                {
                    Id = Guid.NewGuid(),
                    Code = ToCodePrefix($"{location.Code}_", CleanName(m.NameEn)),
                    NameEn = CleanName(m.NameEn),
                    DescriptionEn = m.DescriptionEn,
                    Element = TryParseElement(m.Element),
                    BaseLevel = m.BaseLevel,
                    BaseStats = m.Stats
                };

                _db.MercenaryTemplates.Add(merc);
                await EnsureOrderItemTemplateAsync(merc, ct);

                var stage = new ExpeditionStage
                {
                    Id = Guid.NewGuid(),
                    Code = $"{location.Code}_STAGE_{st.StageNumber}",
                    LocationId = location.Id,
                    StageNumber = st.StageNumber,
                    Difficulty = Enum.TryParse<ExpeditionStageType>(
                        st.StageType.ToString(),
                        ignoreCase: true,
                        out var stType)
                        ? stType
                        : ExpeditionStageType.ES1_Novice,
                    EnemyId = merc.Id
                };

                _db.ExpeditionStages.Add(stage);
            }

            await _db.SaveChangesAsync(ct);
            return location;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating location via OpenAI.");
            return null;
        }
    }
private async Task EnsureOrderItemTemplateAsync(
    MercenaryTemplate merc,
    CancellationToken ct)
{
    // 1 item per order (idempotentní)
    var code = $"ITM_ORDER_{merc.Code}";

    if (await _db.ItemTemplates.AnyAsync(x => x.Code == code, ct))
        return;

    var item = new ItemTemplate
    {
        Id = Guid.NewGuid(),
        Code = code,

        // výchozí ekvivalentní název (AI může lehce vylepšit bez změny významu)
        NameEn = merc.NameEn,
        DescriptionEn = merc.DescriptionEn,

        OwnerKind = ItemOwnerKind.Mercenary,

        MercenarySlot = ItemEquipSlot.Merc_Entity,
        MonsterSlot   = null,
         MercenaryTemplateId = merc.Id,

        BaseQuality = QualityTier.Q1_Common,
        BaseStats   = StatBlock.Zero
    };

    item.BaseStats = merc.BaseStats;
    // ✅ zavolat AI generátor (slot se nesmí změnit)
    await ((IUnitAiGenerator)this).GenerateItemTemplateAsync(
        tpl: item,
        groupHint: $"Order item linked to mercenary template \"{merc.NameEn}\" ({merc.Code}). Must fit the same location/order theme and stay coherent.",
        ct: ct);

    await _db.SaveChangesAsync(ct);
}
    // ==========================================================
    // LOCALIZATION
    // ==========================================================
    public async Task<LocalizedNameResult?> GenerateLocalizedNamesAsync(
    string entityKind,
    string nameEn,
    string? descriptionEn,
    List<string?> missing,
    CancellationToken ct = default)
{
    var jsonShape = LlmSchemaBuilder.BuildJsonShape<LocalizedNameResult>();

    

var allowedLocaleKeysJson = string.Join(", ", missing.Select(x => $"\"{x}\""));

var systemPrompt = $@"
You are a professional game localization assistant for {GameTheme}.

Return ONLY valid JSON matching exactly this structure:

{{
  ""nameEn"": string,
  ""descriptionEn"": string|null,
  ""locales"": {{
    {string.Join(", ", missing.Select(k => $@"""{k}"": {{ ""name"": string, ""description"": string|null }}"))}
  }}
}}

HARD RULES:
- locales MUST be a JSON OBJECT (dictionary), NOT an array and NOT a string.
- locales MUST contain EXACTLY these keys: {allowedLocaleKeysJson}
- Each locales.<key> MUST be an OBJECT with exactly two properties:
  - ""name"": non-empty string
  - ""description"": string or null
- Do NOT include any other properties anywhere.
- If input DescriptionEn is NULL/empty, output descriptionEn must be null and ALL locales.<key>.description must be null.
- Do not mix languages.
HARD RULES (TRANSLATION ENFORCEMENT):
- For EVERY locale key: locales.<key>.name MUST be translated into that language.
- locales.<key>.name MUST NOT equal nameEn (case-insensitive).
- locales.<key>.name MUST NOT contain any English words from nameEn.
- locales.<key>.name MUST be 2–4 words (or natural equivalent for CJK), fully native.
- If a locale uses a non-Latin script, DO NOT output Latin characters.
- If a locale uses Latin script, DO NOT keep English words; translate them.
DESCRIPTION RULES (HARD):
- If input DescriptionEn is NULL/empty: output descriptionEn MUST be null AND every locales.<key>.description MUST be null.
- Otherwise (DescriptionEn is provided and not empty):
  - output descriptionEn MUST be a non-empty string (you may lightly polish English).
  - every locales.<key>.description MUST be a non-empty string translated into that locale.
  - locales.<key>.description MUST NOT be null.
  - locales.<key>.description MUST NOT contain any English words/sentences.
- Output ONLY JSON. No markdown, no comments.
";
    var userPrompt = $@"
EntityKind: {entityKind}

Base English Input:
NameEn: {nameEn}
DescriptionEn: {(string.IsNullOrWhiteSpace(descriptionEn) ? "NULL" : descriptionEn)}

Rules reminder:
- Keep meaning exactly.
- Do not add facts.
- Make each translation sound fully native and smooth.

Return ONLY JSON.
";
    try
    {
        var result = await _asker.AskJsonAsync<LocalizedNameResult>(systemPrompt, userPrompt, ct);

        // defensivní sanity: donuť whitelist (aby se ti to nerozsypalo, když AI ujede)
        if (result is null) return null;

        // 1) vyhoď klíče mimo whitelist (pokud AI udělá bordel)
        var allowed = new HashSet<string>(
            LocalizationConfig.SupportedLocales.Where(x => x != "en"),
            StringComparer.OrdinalIgnoreCase);  

        // 1) vyhoď mimo whitelist + prázdné
        var keys = result.Locales.Keys.ToList();
        foreach (var k in keys)
        {
            if (!allowed.Contains(k) ||
                result.Locales[k] is null ||
                string.IsNullOrWhiteSpace(result.Locales[k].Name))
            {
                result.Locales.Remove(k);
            }
        }

        // 2) vynuceně null popisky, když EN popisek není
        if (string.IsNullOrWhiteSpace(descriptionEn))
        {
            foreach (var loc in result.Locales.Values)
                loc.Description = null;
        }

        return result;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error generating localized names via OpenAI.");
        return null;
    }
}

    // ==========================================================
    // HELPERS
    // ==========================================================
    private static (int min, int max) ComputeNextLevelRange(
        int? previousMin, int? previousMax,
        int? currentMin, int? currentMax,
        bool current)
    {
        if (current)
        {
            var cm = currentMin ?? 0;
            var cx = currentMax ?? 0;
            if (cm > 0 && cx >= cm)
                return (cm, cx);

            // fallback
            return (1, 2);
        }

        if (previousMin is null || previousMax is null)
            return (1, 2);

        var span = Math.Max(1, previousMax.Value - previousMin.Value);
        var nextMin = previousMax.Value + 1;
        var nextMax = nextMin + span;
        return (nextMin, nextMax);
    }

    private static ElementType TryParseElement(object? elementValue)
    {
        if (elementValue is null)
            return ElementType.None;

        var s = elementValue.ToString();
        return Enum.TryParse<ElementType>(s, true, out var el) ? el : ElementType.None;
    }

    private static string CleanName(string? name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return "Unknown";

        // remove anything after dash-like separators (defensive)
        var n = name.Trim();
        n = n.Replace("—", "-").Replace("–", "-");
        var idx = n.IndexOf(" - ", StringComparison.Ordinal);
        if (idx >= 0) n = n[..idx].Trim();
        return n;
    }

    private static string ToCodePrefix(string prefix, string nameEn)
    {
        // prefix already contains trailing "_" (e.g. "DNG_" or "LOC_" or "LOC_X_")
        var upper = nameEn.Trim().ToUpperInvariant();

        var sb = new StringBuilder();
        foreach (var ch in upper)
        {
            if (ch >= 'A' && ch <= 'Z') sb.Append(ch);
            else if (ch >= '0' && ch <= '9') sb.Append(ch);
            else sb.Append(' ');
        }

        var cleaned = sb.ToString();
        while (cleaned.Contains("  "))
            cleaned = cleaned.Replace("  ", " ");

        cleaned = cleaned.Trim().Replace(" ", "_");
        if (cleaned.Length == 0) cleaned = "UNKNOWN";

        return prefix + cleaned;
    }

private static void ApplyGeneratedBonuses(ItemTemplate tpl, List<GeneratedBonus> bonuses)
{
    if (bonuses is null) return;

    var list = bonuses
        .GroupBy(b => b.Type)
        .Select(g => g.First()) 
        .ToList();

    if (list.Count == 0)
        return;

    // TODO: tady rozhodni, kam bonusy uložíš:
    // A) do tpl.QualityEffects (ItemEffect) nebo
    // B) do tpl.QualityBonusSet (pokud existuje)
    //
    // Ukázka pro ItemEffect (musíš mít ItemEffect ve svém modelu):
    //
     tpl.QualityEffects.Clear();
     foreach (var b in list)
     {
         tpl.QualityEffects.Add(new ItemEffect
         {
              EffectType = ItemEffectType.PassiveStatBoost,
            TargetStat = b.Type.ToString(),     // např. "BonusAttackPct"
            Value      = b.Base,                // ✅ jen base
            MinQuality = QualityTier.Q1_Common
         });
     }
}
    private static void ApplyBonusesToTemplate(ItemTemplate tpl, List<GeneratedBonus> bonuses)
{
    // Pokud máš v ItemTemplate místo (např.) QualityBonusSet, udělej to takhle.
    // Pokud ho tam nemáš, klidně to prozatím převeď do ItemEffect a ulož do tpl.Effects.
    if (bonuses is null || bonuses.Count == 0)
        return;

    // vyber max 3, bez None, unikátně
    var list = bonuses
        .GroupBy(b => b.Type)
        .Select(g => g.First())
        .Take(3)
        .ToList();

    // když nemáš QualityBonusSet na template, tuhle část vyhoď a udělej Effects
    tpl.QualityEffects ??= new List<ItemEffect>(); // pokud existuje
    // nebo: tpl.QualityBonusSet = new QualityBonusSet { ... } pokud máš property
}

 async Task<ItemTemplate?> IUnitAiGenerator.GenerateItemTemplateAsync(
    ItemTemplate tpl,
    string? groupHint,
    CancellationToken ct)
{
    if (tpl is null) throw new ArgumentNullException(nameof(tpl));

    // ✅ u tebe je slot v ItemTemplate jako EquipSlot (unified enum)
    var slot =
    tpl.IsForMercenary ? (tpl.MercenarySlot ?? ItemEquipSlot.None)
  : tpl.IsForBeast     ? (tpl.MonsterSlot   ?? ItemEquipSlot.None)
  : ItemEquipSlot.None;

        if (tpl.IsForMercenary && !slot.IsMercenarySlot())
            throw new InvalidOperationException($"EquipSlot {slot} invalid for Mercenary.");

        if (tpl.IsForBeast && !slot.IsBeastSlot())
            throw new InvalidOperationException($"EquipSlot {slot} invalid for Beast.");
    if (slot == ItemEquipSlot.None)
        throw new InvalidOperationException("ItemTemplate missing EquipSlot.");

    // schema + enum constraints
    var jsonShape   = LlmSchemaBuilder.BuildJsonShape<ItemTemplateGenerationResult>();
    var slotValues  = LlmSchemaBuilder.BuildEnumValues<ItemEquipSlot>();
    var statIdValues = LlmSchemaBuilder.BuildEnumValues<StatId>();
    var elementValues = LlmSchemaBuilder.BuildEnumValues<ElementType>(ElementType.None);

   var systemPrompt = $@"
You generate item templates for {GameTheme}.
Return ONLY valid JSON. No markdown, no code fences.

Shape:
{jsonShape}

HARD RULES:
- Do NOT change EquipSlot. The slot is fixed externally: {slot}.
- EquipSlot must be one of: {slotValues}.
- BaseQuality must be one of: Q1_Common..Q11_Transcendent (use enum identifiers).
- Use American English spelling ONLY (Armor, not Armour).
- No numbers/hyphens/MMO tags in NameEn.
- NameEn 2–4 words, vivid, unique, easy to pronounce.
- DescriptionEn: 1–3 short paragraphs, atmospheric, future-fantasy.
- Do NOT invent new JSON fields. Output ONLY fields defined by the Shape.

ELEMENT + STATS (HARD):
- Choose Element first.
- Element must be one of: {elementValues}.
- Stats.Element MUST be set and MUST NOT be None.
- Stats must contain ONLY properties that exist on StatBlock.
- You MUST NOT output any extra stat properties (examples of forbidden stats: FireResistance, NatureChance, IceDamage, HolyResist, etc.).

STATS (GUIDANCE):
- Do NOT set every stat. Many zeros are OK.
- For non-entity slots: you MAY use some negatives (tradeoffs), but keep values reasonable.
- Chances/bonuses are usually in 0..1 range unless explicitly flat (MaxHp/Armor/Attack/Defense/Speed/MaxEnergy).

- FORBIDDEN statId patterns (NEVER output these):
  - Any element name: Fire, Water, Ice, Nature, Lightning, etc.
  - Any Resistance/Resist/Immunity/Chance derived from elements: FireResistance, NatureChance, IceResist, etc.
  - Any renamed synonyms: CriticalChance (must be CritChance), ArmourPenetration (must be ArmorPenetration), MaxHP (must be MaxHp).
  - Any plural/typo variants: ShieldsBonus (must be ShieldBonus).

- If you are unsure about a statId spelling, DO NOT guess.
  Instead pick from this SAFE fallback set (choose unused ones first):
  MaxHp, Armor, Attack, Defense, Speed, MaxEnergy, CritChance, ArmorPenetration, DamageBonus, DamageReduction, StatusResistance, HpRegen, Evasion.

StatId MUST match the enum identifiers EXACTLY (case-sensitive).

VALID examples:
- CriticalChance
- CriticalMultiplier
- ArmorPenetration
- MaxHp
- ShieldBonus

INVALID examples:
- CritChance
- CritMultiplier
- Critical Chance
- Fire
- NatureChance
- ShieldsBonus

";


var existingItemNames = await _db.ItemTemplates
        .AsNoTracking()
        .Select(x => x.NameEn)
        .Where(x => x != null && x != "")
        .OrderByDescending(x => x) // libovolně; nebo podle CreatedUtc pokud máš
        .Take(350)
        .ToListAsync(ct);

    // Zamezí i tomu, aby AI opakovala "Current NameEn" jako 100 dalších itemů
    var bannedItemsBlock = string.Join("\n", existingItemNames.Select(n => $"- \"{n}\""));
     var userPrompt = $@"
We already have an item template skeleton:

- Current NameEn: ""{tpl.NameEn}""
- OwnerKind: {tpl.OwnerKind}
- EquipSlot: {slot}
- GroupHint: {(string.IsNullOrWhiteSpace(groupHint) ? "(none)" : groupHint)}

BANNED ITEM NAMES (DO NOT reuse, and avoid near-duplicates):
{bannedItemsBlock}

TASK:
- Improve/confirm NameEn (keep meaning; do NOT add numbers).
- Create DescriptionEn.
- Output Stats (StatBlock) + Element.

Return ONLY JSON.
";

    ItemTemplateGenerationResult gen;
    try
    {
        gen = await _asker.AskJsonAsync<ItemTemplateGenerationResult>(systemPrompt, userPrompt, ct);
    }
    catch(Exception e)
    {
        Console.WriteLine(e.Message);
        // když LLM failne, nechci rozbít seed — vrátím aspoň skeleton
        return tpl;
    }

    // ✅ hard-guard: slot se nesmí změnit ani v JSONu
   

    // apply (name + description)
    var newName = (gen.NameEn ?? "").Trim();
    if (!string.IsNullOrWhiteSpace(newName))
        tpl.NameEn = newName;
    tpl.DescriptionEn = (gen.DescriptionEn ?? "").Trim();
    if(tpl.MercenarySlot == ItemEquipSlot.Merc_Entity || tpl.MonsterSlot == ItemEquipSlot.Beast_Entity)
        {
            
        } else
        {
            tpl.BaseStats = gen.Stats.Clone();
        }
    tpl.OwnerKind = tpl.IsForBeast ? ItemOwnerKind.Beast
             : tpl.IsForMercenary ? ItemOwnerKind.Mercenary
             : ItemOwnerKind.None;
    tpl.BaseQuality = QualityTier.Q1_Common;
   
    await EnsureBonusesAsync(tpl);
    await EnsureUpgradeResourcesAsync(tpl, ct);
    
    _db.ItemTemplates.Add(tpl);
    return tpl;
}
private static List<GeneratedBonus> MapBonuses(List<GeneratedBonusResult>? src)
{
    if (src is null || src.Count == 0) return new();

    return src
        .Select(b => new GeneratedBonus
        {
            Type = StatIdParser.ParseOrThrow(b.StatId),
            Base = b.Base,
            PerTier = b.PerTier
        })
        .GroupBy(b => b.Type)
        .Select(g => g.First())
        .Take(10)
        .ToList();
}

public async Task<ItemUpgradeResourceNamesResult?> GenerateUpgradeResourceNamesAsync(
    ItemTemplate tpl,
    CancellationToken ct = default)
{
    var jsonShape = LlmSchemaBuilder.BuildJsonShape<ItemUpgradeResourceNamesResult>();

    var systemPrompt = $@"
You generate upgrade resource NAMES for a single RPG item in {GameTheme}.

Return ONLY valid JSON matching exactly:
{jsonShape}

STRICT:
- Output English only.
- Names must be 1–3 words each, evocative, lore-friendly.
- DO NOT use generic currency words: shard(s), stone(s), dust, fragment(s), crystal(s), token(s).
- Do NOT use numbers, hyphens, MMO tags.
- The three names must feel related to the item, but be clearly distinct in meaning:
  - Core => represents leveling the item (structural growth)
  - Catalyst => represents improving quality/tier (refinement)
  - Essence => represents traits/stability/enchanting (subtle imprint)
- If you output descriptions, keep them short (1–2 sentences) and consistent with the item fantasy.
";

    var userPrompt = $@"
Item template context:
- Item NameEn: {tpl.NameEn}
- Item DescriptionEn: {(string.IsNullOrWhiteSpace(tpl.DescriptionEn) ? "(none)" : tpl.DescriptionEn)}
- OwnerKind: {tpl.OwnerKind}
- EquipSlot: {(tpl.IsForMercenary ? tpl.MercenarySlot : tpl.IsForBeast ? tpl.MonsterSlot : ItemEquipSlot.None)}
- Element (if any): {(tpl.Element.ToString() ?? "None")}

Task:
Generate three resource names for this item: Core, Catalyst, Essence.
Return ONLY JSON.
";

    try
    {
        return await _asker.AskJsonAsync<ItemUpgradeResourceNamesResult>(systemPrompt, userPrompt, ct);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error generating upgrade resource names via OpenAI.");
        return null;
    }
}

public async Task EnsureBonusesAsync(ItemTemplate tpl, CancellationToken ct = default)
{
    /*
    var entry = _db.Entry(tpl);

    if (entry.State == EntityState.Detached)
    {
        // když má Id/Code, zkus to najít v DB a použít tracked instanci
        var tracked = await _db.ItemTemplates
            .Include(x => x.UpgradeResources)
            .FirstOrDefaultAsync(x => x.Id == tpl.Id || x.Code == tpl.Code, ct);

        if (tracked is not null)
        {
            tpl = tracked;
            entry = _db.Entry(tpl);
        }
        else
        {
            // jinak je to nový template => přidej a bude tracked
            _db.ItemTemplates.Add(tpl);
            entry = _db.Entry(tpl);
        }
    }

    // u Added nemá smysl Load
    if (entry.State != EntityState.Added)
        await entry.Collection(x => x.UpgradeResources).LoadAsync(ct);

    bool hasAll =
        tpl.UpgradeResources.Any(r => r.Type == ItemUpgradeResourceType.Core) &&
        tpl.UpgradeResources.Any(r => r.Type == ItemUpgradeResourceType.Catalyst) &&
        tpl.UpgradeResources.Any(r => r.Type == ItemUpgradeResourceType.Essence);

    if (hasAll) return;
    */
    var gen = await GenerateBonusesNamesAsync(tpl, "", ct);
    if (gen is null) return;

       var bonuses =  MapBonuses(gen.Bonuses);
        ApplyGeneratedBonuses(tpl, bonuses);
}

    private async Task<ItemBonusGenerationResult?> GenerateBonusesNamesAsync(
    ItemTemplate tpl,
    string? groupHint,
    CancellationToken ct)
{
    if (tpl is null) throw new ArgumentNullException(nameof(tpl));

    var slot =
        tpl.IsForMercenary ? (tpl.MercenarySlot ?? ItemEquipSlot.None)
      : tpl.IsForBeast ? (tpl.MonsterSlot ?? ItemEquipSlot.None)
      : ItemEquipSlot.None;

    if (slot == ItemEquipSlot.None)
        throw new InvalidOperationException("ItemTemplate missing EquipSlot.");

    var jsonShape = LlmSchemaBuilder.BuildJsonShape<ItemBonusGenerationResult>();
    var statIdValues = LlmSchemaBuilder.BuildEnumValues<StatId>();

    var systemPrompt = $@"
You generate QUALITY BONUSES for a single RPG item in {GameTheme}.
Return ONLY valid JSON. No markdown. No comments. No extra keys.

Shape:
{jsonShape}

YOU MUST FOLLOW THIS PROCEDURE:
1) Select EXACTLY 10 distinct statId tokens by COPYING them verbatim from the whitelist below.
2) For each selected statId, assign base and perTier values within allowed ranges.
3) Final self-check BEFORE output:
   - Every statId is EXACTLY one of the whitelist entries (case-sensitive).
   - No synonyms, no paraphrases, no invented words, no typos, no plurals.
   - If ANY statId is not an exact whitelist match, REPLACE it with another whitelist entry.

ABSOLUTE HARD RULES:
- Output MUST be a JSON object with ONE property: ""bonuses"".
- ""bonuses"" MUST be an array of EXACTLY 10 objects.
- Each bonus object MUST have EXACTLY these keys (spelling + casing):
  {{ ""statId"": ""<StatId>"", ""base"": <number>, ""perTier"": <number> }}

WHITELIST (COPY-PASTE ONLY, CASE-SENSITIVE):
{statIdValues}

statId MUST be copied EXACTLY from the whitelist.
Do NOT translate, rename, abbreviate, expand, or synonymize statId names.

Examples of WRONG behavior (never do this):
- HealthRegen instead of HpRegen
- CritChance instead of CriticalChance
- CritMultiplier instead of CriticalMultiplier
- ShieldsBonus instead of ShieldBonus
FORBIDDEN:
- Any synonym or related word not EXACTLY in the whitelist
  (examples: HemorrhageChance, CritChance, CritMultiplier, ShieldsBonus, ArmourPenetration).
- Any element names or element-derived words (Fire, Ice, NatureChance, FireResistance, etc.).
- Any spacing/punctuation variants (""Critical Chance"", ""Max HP"", ""DOT Damage Bonus"", etc.).
- Any extra fields (no ""type"", ""name"", ""desc"", etc.).
Only allowed statId values are exactly the whitelist. No other strings are allowed.
If a desired concept is magic mitigation, use DamageReduction.
BONUS VALUES:
- base: 1..6 (percent points)
- perTier: 0.5..2 (percent points)
- No zero, no negative.
- base should be integer.
- perTier should be one of: 0.5, 1, 1.5, 2.

FINAL OUTPUT REQUIREMENTS:
- Output ONLY valid JSON and nothing else.
";

    var userPrompt = $@"
Item context:
- NameEn: {tpl.NameEn}
- DescriptionEn: {(string.IsNullOrWhiteSpace(tpl.DescriptionEn) ? "(none)" : tpl.DescriptionEn)}
- OwnerKind: {tpl.OwnerKind}
- EquipSlot: {slot}
- GroupHint: {(string.IsNullOrWhiteSpace(groupHint) ? "(none)" : groupHint)}
- Element: {(tpl.BaseStats?.Element ?? ElementType.None.ToString())}

Task:
Pick EXACTLY 10 DISTINCT statId values by COPYING them verbatim from the whitelist.
Then assign base and perTier within allowed ranges.
Return ONLY JSON with the single key ""bonuses"".
";

    try
    {
        return await _asker.AskJsonAsync<ItemBonusGenerationResult>(systemPrompt, userPrompt, ct);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error generating bonuses via OpenAI.");
        return null;
    }
}

    public async Task EnsureUpgradeResourcesAsync(ItemTemplate tpl, CancellationToken ct = default)
{
    /*
    var entry = _db.Entry(tpl);

    if (entry.State == EntityState.Detached)
    {
        // když má Id/Code, zkus to najít v DB a použít tracked instanci
        var tracked = await _db.ItemTemplates
            .Include(x => x.UpgradeResources)
            .FirstOrDefaultAsync(x => x.Id == tpl.Id || x.Code == tpl.Code, ct);

        if (tracked is not null)
        {
            tpl = tracked;
            entry = _db.Entry(tpl);
        }
        else
        {
            // jinak je to nový template => přidej a bude tracked
            _db.ItemTemplates.Add(tpl);
            entry = _db.Entry(tpl);
        }
    }

    // u Added nemá smysl Load
    if (entry.State != EntityState.Added)
        await entry.Collection(x => x.UpgradeResources).LoadAsync(ct);

    bool hasAll =
        tpl.UpgradeResources.Any(r => r.Type == ItemUpgradeResourceType.Core) &&
        tpl.UpgradeResources.Any(r => r.Type == ItemUpgradeResourceType.Catalyst) &&
        tpl.UpgradeResources.Any(r => r.Type == ItemUpgradeResourceType.Essence);

    if (hasAll) return;
    */
    var gen = await GenerateUpgradeResourceNamesAsync(tpl, ct);
    if (gen is null) return;

    UpsertResource(tpl, ItemUpgradeResourceType.Core, gen.CoreNameEn, gen.CoreDescriptionEn);
    UpsertResource(tpl, ItemUpgradeResourceType.Catalyst, gen.CatalystNameEn, gen.CatalystDescriptionEn);
    UpsertResource(tpl, ItemUpgradeResourceType.Essence, gen.EssenceNameEn, gen.EssenceDescriptionEn);
}

private void UpsertResource(ItemTemplate tpl, ItemUpgradeResourceType type, string nameEn, string? descEn)
{
    if (string.IsNullOrWhiteSpace(nameEn)) return;

    var r = tpl.UpgradeResources.FirstOrDefault(x => x.Type == type);
    if (r is null)
    {
        r = new ItemUpgradeResource
        {
            Id = Guid.NewGuid(),
            Type = type
        };
        tpl.UpgradeResources.Add(r); // stačí navigace
    }

    r.NameEn = nameEn.Trim();
    r.DescriptionEn = string.IsNullOrWhiteSpace(descEn) ? null : descEn.Trim();
}
}