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

    public async Task<DungeonGenerationResult?> GenerateNextDungeonAsync(Dungeon? previousDungeon, bool currentDungeon, CancellationToken ct = default)
    {
        int prevMin = previousDungeon?.MinLevel ?? 1;
        int prevMax = previousDungeon?.MaxLevel ?? 5;

        int nextMin = previousDungeon is null ? 1 : prevMax + 1;
        int span = previousDungeon is null ? 4 : Math.Max(1, prevMax - prevMin);
        int nextMax = nextMin + span;
        if(prevMin == 1)
        {
            nextMin = prevMin;
            nextMax=prevMax;
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
            currentOrNext = $@"
        The player provided a manual dungeon name: ""{previousName}"".
        Generate THIS dungeon, do NOT create a next one.
        Use exactly this name for the dungeon.
        ";
        } else
        {  
            currentOrNext = $@"
The player has just completed a dungeon named ""{previousName}"".
Generate the NEXT dungeon in the progression.
";
        }
    var systemPrompt = $@"
You are an assistant generating procedural dungeons for a dark fantasy RPG with subtle futuristic/arcane-tech elements.

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

Constraints:
- recommendedMinLevel = {nextMin}
- recommendedMaxLevel = {nextMax}
- The 'stages' array MUST contain exactly {stageCount} stages.
- stageIndex must go from 1 to {stageCount} (no duplicates, no gaps).
- Each stageType value from the allowed enum list must be used exactly once across all stages
  (no stageType is repeated, no stageType is missing).
- Stats (attack, defense, health, etc.) should scale with baseLevel so that later stages feel harder.
Monster naming rules:
- Monster names MUST NOT be forced to include the element.
- Names may include element flavoring only when it makes sense, but it is optional.
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
    bool currentLocation,
    CancellationToken ct = default)
{
    int prevMin = previousLocation?.MinLevel ?? 1;
    int prevMax = previousLocation?.MaxLevel ?? 5;

    int nextMin = previousLocation is null ? 1 : prevMax + 1;
        int span = previousLocation is null ? 4 : Math.Max(1, prevMax - prevMin);
        int nextMax = nextMin + span;
        if(prevMin == 1)
        {
            nextMin = prevMin;
            nextMax=prevMax;
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
        currentOrNext = $@"
The player provided a manual location name: ""{previousName}"".
Generate THIS expedition location, do NOT create a next one.
Use exactly this name for the location (NameEn).";
    }
    else
    {
        currentOrNext = $@"
The player has just completed an expedition location named ""{previousName}"".
Generate the NEXT location in the progression.";
    }

    var systemPrompt = $@"
You are an assistant generating procedural expedition locations for a fantasy RPG
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

Constraints:
- MinLevel = {nextMin}
- MaxLevel = {nextMax}
- The 'Stages' array MUST contain exactly {stageCount} stages.
- StageNumber must go from 1 to {stageCount} (no duplicates, no gaps).
- Each StageType value from the allowed enum list must be used exactly once across all stages
  (no StageType is repeated, no StageType is missing).
- Enemy stats (attack, defense, health, etc.) should scale with BaseLevel so that later stages feel harder.

Enemy naming rules:
- Enemy names MUST NOT be forced to include the element.
- Names may include elemental flavoring only when it makes sense, but this is optional.
- Avoid literal names like ""Fire Soldier"", ""Water Archer"", ""Poison Bandit"".
- Use creative, mercenary/bandit/raider-like names fitting a fantasy-tech world.

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
}