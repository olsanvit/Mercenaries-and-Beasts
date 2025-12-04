using System.Net.Http.Json;
using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.AI;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Dto;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Interface;
using Microsoft.Extensions.Logging;

namespace MercenariesAndBeasts.Infrastructure.AI;
public class AiUnitGeneratorService : IUnitAiGenerator
{
    private string GameTheme =>
    "a dark fantasy role-playing game with subtle arcane-tech elements. " +
    "The tone is mysterious, atmospheric and slightly grim. " +
    "Naming style: premium RPG, evocative, memorable and lore-friendly. " +
    "Names should feel unique, not generic, easy to pronounce, without numbers or MMO-style tags. " +
    "Avoid clichés like 'of Doom', 'of the Ancients', 'the Ultimate', 'Extreme', etc."; 
    /*private string GameTheme => 
        "a fantasy role-playing game with subtle futuristic and arcane-tech elements. " +
        "The world is rich in magic, advanced technology, and diverse landscapes, inhabited by various monsters and mercenaries." +
        "Names should be atmospheric, imaginative, lore-friendly, and unique — taking inspiration from both fantasy and light futuristic. " +
        "Avoid generic names.";    */
    private readonly ChatGptAsker _asker;
    private readonly GameDbContext _db;
    private readonly ILogger<AiUnitGeneratorService> _logger;

    // JEDEN veřejný konstruktor, žádný HttpClient
    public AiUnitGeneratorService(
        ChatGptAsker asker,
        GameDbContext db,
        ILogger<AiUnitGeneratorService> logger)
    {
        _asker = asker;
        _db = db;
        _logger = logger;
    }

    public async Task<DungeonGenerationResult?> GenerateNextDungeonAsync(Dungeon? previousDungeon, Dungeon? currDungeon, bool currentDungeon, CancellationToken ct = default)
    {
        int nextMin;
        int nextMax;

        if (currentDungeon)
        {

            if (previousDungeon is null)
            {
                // vůbec nic ještě neexistuje → první lokace
                nextMin = 1;
                nextMax = 5;   // nebo co používáš jako default
            }
            else
            {
                var prevMin = previousDungeon.MinLevel;
                var prevMax = previousDungeon.MaxLevel;

                var span = Math.Max(1, prevMax - prevMin);
                nextMin = prevMax + 1;
                nextMax = nextMin + span;
            }
        }
        else
        {
            // ✅ Generujeme „další“ lokaci v progresi
            if (previousDungeon is null)
            {
                // první lokace
                nextMin = 1;
                nextMax = 5;
            }
            else
            {
                var prevMin = previousDungeon.MinLevel;
                var prevMax = previousDungeon.MaxLevel;

                var span = Math.Max(1, prevMax - prevMin);
                nextMin = prevMax + 1;
                nextMax = nextMin + span;
            }
        }

        var stageTypeNames = Enum.GetNames(typeof(DungeonStageType));
        var stageCount = stageTypeNames.Length; // ← počet stagí = počet enum hodnot
        var previousName = previousDungeon?.NameEn ?? "None";

       // ⚙️ schéma z classy – automaticky
    var jsonShape = LlmSchemaBuilder.BuildJsonShape<DungeonGenerationResult>();

    // ⚙️ enum hodnoty – taky automaticky
    var stageTypeEnumValues = LlmSchemaBuilder.BuildEnumValues<DungeonStageType>();
    var elementEnumValues   = LlmSchemaBuilder.BuildEnumValues<ElementType>();
    string currentOrNext = string.Empty;
        if (currentDungeon)
        {
           var originalName = currDungeon?.NameEn ?? "Unknown";

    currentOrNext = $@"
The player provided a manual dungeon name in natural language: ""{originalName}"".

IMPORTANT:
- The provided name may be Czech or any other non-English language.
- You MUST translate/adapt this name into smooth, natural ENGLISH.
- The translated English name MUST preserve the meaning and theme.
- Put the translated English name into ""NameEn"".

STRICT RULES:
- ALWAYS return the English version in ""NameEn"".
- NEVER return the non-English input name in the JSON.
- NEVER include diacritics or non-ASCII characters in ""NameEn"" or ""Code"".

Generate THIS dungeon (based on the translated English name), do NOT create the next one.
";
        } else
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
- The 'stages' array MUST contain exactly {stageCount} items.
";

    var userPrompt = $@"
{currentOrNext}

Dungeon naming rules:
- The dungeon ""NameEn"" must be short (2–4 words), vivid and unique.
- It should hint at the place's identity or history, not just its element.
- Avoid generic names like ""Fire Dungeon"", ""Dark Cave"", ""Poison Lair"", ""Ancient Dungeon"".

Constraints:
- recommendedMinLevel = {nextMin}
- recommendedMaxLevel = {nextMax}
- The 'stages' array MUST contain exactly {stageCount} stages.
- stageIndex must go from 1 to {stageCount} (no duplicates, no gaps).
- Each stageType value from the allowed enum list must be used exactly once across all stages
  (no stageType is repeated, no stageType is missing).
- Stats (attack, defense, health, etc.) should scale with baseLevel so that later stages feel harder.

Element rules:
- Each monster's ""element"" must be one of: {elementEnumValues}.
- Across the WHOLE dungeon (all stages), you may use at most TWO distinct elements in total.
- First, implicitly choose up to two elements that fit the dungeon theme, then use only those elements for all monsters and the boss.
- Do NOT use more than two different element values in the entire dungeon.

Monster naming rules:
- Each monster must have a distinct, flavorful name that would fit a high-quality RPG bestiary.
- Avoid plain labels like ""Fire Beast"", ""Stone Golem"", ""Dark Archer"", ""Poison Spider"".
- Names may include elemental flavoring only when it makes sense (e.g. ""Cinderfang Hound""), but it is optional.
- MiniBoss and Boss monsters should have more epic, memorable names than regular monsters.

Return ONLY JSON object that matches the described shape. No markdown, no comments, no extra text.
";

        try
        {
            var result = await _asker.AskJsonAsync<DungeonGenerationResult>(systemPrompt, userPrompt, ct);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating dungeon via OpenAI.");
            return null;
        }
    }
    public async Task<ExpeditionGenerationResult?> GenerateNextLocationAsync(
    Location? previousLocation,
    Location? currLocation,
    bool currentLocation,
    CancellationToken ct = default)
{
        int nextMin;
        int nextMax;

        if (currentLocation)
        {
            // ✅ Generujeme „aktuální“ lokaci – nepřehazuj level range

            if (previousLocation is null)
            {
                // vůbec nic ještě neexistuje → první lokace
                nextMin = 1;
                nextMax = 5;   // nebo co používáš jako default
            }
            else
            {
                var prevMin = previousLocation.MinLevel;
                var prevMax = previousLocation.MaxLevel;

                var span = Math.Max(1, prevMax - prevMin);
                nextMin = prevMax + 1;
                nextMax = nextMin + span;
            }
        }
        else
        {
            // ✅ Generujeme „další“ lokaci v progresi
            if (previousLocation is null)
            {
                // první lokace
                nextMin = 1;
                nextMax = 5;
            }
            else
            {
                var prevMin = previousLocation.MinLevel;
                var prevMax = previousLocation.MaxLevel;

                var span = Math.Max(1, prevMax - prevMin);
                nextMin = prevMax + 1;
                nextMax = nextMin + span;
            }
        }

        // počet stagí podle enumu expedic
        var stageTypeNames = Enum.GetNames(typeof(ExpeditionStageType));
    var stageCount     = stageTypeNames.Length;

    var previousName = previousLocation?.NameEn ?? "None";

    // schéma z DTO
    var jsonShape = LlmSchemaBuilder.BuildJsonShape<ExpeditionGenerationResult>();

    // enum values
    var stageTypeEnumValues = LlmSchemaBuilder.BuildEnumValues<ExpeditionStageType>();
    var elementEnumValues   = LlmSchemaBuilder.BuildEnumValues<ElementType>();

    string currentOrNext;
    if (currentLocation)
    {
      var originalName = currLocation?.NameEn ?? "Unknown";

    currentOrNext = $@"
The player provided a manual location name in natural language: ""{originalName}"".
Generate THIS expedition location, do NOT create a next one.

IMPORTANT:
- The provided name may be Czech or any other non-English language.
- You MUST translate/adapt this name into smooth, natural ENGLISH.
- The translated English name MUST preserve the meaning and theme.
- Put the translated English name into ""NameEn"".

STRICT RULES:
- ALWAYS return the English version in ""NameEn"".
- NEVER return the non-English input name in the JSON.
- NEVER include diacritics or non-ASCII characters in ""NameEn"" or ""Code"".
";
    }
    else
    {
         currentOrNext = $@"
The player has just completed an expedition location named ""{previousName}"".
Generate the NEXT location in the progression.";
    }

    var systemPrompt = $@"
You are an assistant generating procedural expedition locations for {GameTheme}
with subtle futuristic/arcane-tech elements.

You MUST ALWAYS return ONLY valid JSON.
The JSON MUST be deserializable into this C#-like shape:

{jsonShape}

Rules:
- ""StageType"" must be one of: {stageTypeEnumValues}
- ""Element"" must be one of: {elementEnumValues}
- The 'Stages' array MUST contain exactly {stageCount} items.
- The ""Code"" MUST be generated directly from ""NameEn"":
    - Start with ""LOC_""
    - Convert NameEn to UPPERCASE
    - Remove all characters except A–Z, 0–9 and spaces
    - Replace spaces with ""_""
    - Examples:
        NameEn = ""Bausia Outskirts"" → ""LOC_BAUSIA_OUTSKIRTS""
        NameEn = ""Shattered Coastline"" → ""LOC_SHATTERED_COASTLINE""
    - No randomness, no suffix hashes, no diacritics.
";

   var userPrompt = $@"
{currentOrNext}

Location naming rules:
- ""NameEn"" must be 2–4 words, atmospheric and memorable.
- It should evoke a place you would want to explore in a fantasy RPG (e.g. ""Ashen Lantern Fields"", ""Glimmering Rift"", ""Shattered Grove"").
- Avoid ultra-generic names like ""Forest Camp"", ""Old Village"", ""Mountain Pass"".

Constraints:
- MinLevel = {nextMin}
- MaxLevel = {nextMax}
- The 'Stages' array MUST contain exactly {stageCount} stages.
- StageNumber must go from 1 to {stageCount} (no duplicates, no gaps).
- Each StageType value from the allowed enum list must be used exactly once across all stages
  (no StageType is repeated, no StageType is missing).
- Enemy stats (attack, defense, health, etc.) should scale with BaseLevel so that later stages feel harder.

Element rules:
- Each mercenary's ""element"" must be one of: {elementEnumValues}.
- Across the WHOLE location (all stages), you may use at most TWO distinct elements in total.
- First, implicitly choose up to two elements that fit the location theme, then use only those elements for all mercenaries and the boss.
- Do NOT use more than two different element values in the entire location.

Mercenary naming rules:
- Mercenary names must sound like individual characters or notable foes, not generic mobs.
- Avoid plain labels like ""Fire Soldier"", ""Water Archer"", ""Poison Bandit"", ""Dark Mage"".
- Prefer evocative names like ""Ashbreaker Scout"", ""Gloomsworn Cutthroat"", ""Verdant Pike-Leader"", etc.
- Boss-type mercenaries can have slightly grander titles, but still without clichés like ""of Doom"".

Return ONLY a JSON object that matches the described shape. No markdown, no comments, no extra text.
";

    try
    {
        var result = await _asker.AskJsonAsync<ExpeditionGenerationResult>(systemPrompt, userPrompt, ct);
        return result;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error generating location via OpenAI.");
        return null;
    }
}
public async Task<LocalizedNameResult?> GenerateLocalizedNamesAsync(
    string entityKind,            // "dungeon", "location", "monster", "mercenary"...
    string nameEn,
    string? descriptionEn,
    CancellationToken ct = default)
{
    // C# shape pro LLM
    var jsonShape = LlmSchemaBuilder.BuildJsonShape<LocalizedNameResult>();

    var systemPrompt = $@"
You are a localization assistant for {GameTheme}.

You receive the BASE ENGLISH name and optional English description
of a game entity (dungeon, location, monster, mercenary, etc.).
Your job is to produce localized names and descriptions in Czech (cs)
and German (de), keeping the style, tone and flavor.

You MUST ALWAYS return ONLY valid JSON that can be deserialized into
this C#-like structure:

{jsonShape}

General rules:
- Keep NameEn and DescriptionEn in English. You MAY slightly polish them
  (fix grammar, make them sound more premium), but preserve the meaning.
- NameCs and DescriptionCs must be natural, fluent Czech with correct diacritics.
- NameDe and DescriptionDe must be natural, fluent German.
- Preserve the fantasy tone and the entity role (dungeon, location, monster, mercenary).
- Do NOT invent completely new lore; just adapt style and nuance.
- Avoid overly literal translations if they sound awkward; prioritize what
  would sound cool in a high-quality RPG in that language.
";

    var userPrompt = $@"
EntityKind: {entityKind}

Base English:
- NameEn: {nameEn}
- DescriptionEn: {(string.IsNullOrWhiteSpace(descriptionEn) ? "(none)" : descriptionEn)}
";

    try
    {
        var result = await _asker.AskJsonAsync<LocalizedNameResult>(
            systemPrompt,
            userPrompt,
            ct);

        return result;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error generating localized names via OpenAI.");
        return null;
    }
}
}