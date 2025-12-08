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
    "A future-fantasy role-playing universe where mystical forces intertwine with advanced technology. " +
    "Aesthetic: atmospheric, enigmatic, slightly grim, with echoes of ancient mysteries integrated into futuristic constructs. " +
    "World tone: immersive, mysterious, premium RPG feeling. " +
    "Technology appears as crystalline conduits, mystic energy cores, shimmering glyph-interfaces, and resonant constructs. " +
    "Mystic energy feels ancient, cosmic, and intertwined with unknown dimensional principles. " +
    "Naming style: unique, evocative, lore-friendly, and easy to pronounce; never generic. " +
    "Names must NOT include numbers, hyphens, or MMO-style tags. " +
    "Forbidden clichés: 'of Doom', 'of the Ancients', 'Ultimate', 'Extreme', 'Legendary', and similar patterns. " +
    "Descriptions should be vivid, atmospheric, immersive, and consistent with the mystical future-fantasy tone. " +
    "Output must respect linguistic accuracy and natural fluency in every target language.";
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
- The ""Code"" MUST be generated directly from English translated ""NameEn"":
    - Start with ""LOC_""
    - Convert NameEn to UPPERCASE
    - Remove all characters except A–Z, 0–9 and spaces
    - Replace spaces with ""_""
    - No randomness, no suffix hashes, no diacritics.
";

   var userPrompt = $@"
{currentOrNext}

Dungeon naming rules:
- The dungeon ""NameEn"" must be short (2–4 words), vivid, unique and clearly future-fantasy themed.
- It must strongly express the dungeon's identity, history or function, not just its element.
- The new dungeon name must feel clearly different in tone and wording from the previous dungeon ""{previousName}"".
- Avoid generic names like ""Fire Dungeon"", ""Dark Cave"", ""Poison Lair"", ""Ancient Dungeon"", ""Mystic Temple"", ""Forgotten Ruins"".

Dungeon–monster cohesion rules:
- All monsters must clearly belong in THIS specific dungeon.
- Their visual concept, behaviour and role should feel shaped by the dungeon's environment, purpose and element(s).
- At least some monster names should echo key ideas from the dungeon name or description
  (materials, shapes, energies, relics, functions, myths).
- The MiniBoss and Boss must be the strongest direct expression of the dungeon's core theme.

Monster uniqueness rules:
- Every monster in this dungeon MUST have a unique ""NameEn"" (no duplicates, no tiny variations like ""Guard"" / ""Guardian"").
- Assume many other dungeons exist in the game: avoid overly generic or overused fantasy names.
- Each monster should feel like a specific, memorable creature that could have its own bestiary entry.

Monster naming rules:
- Each monster name must fit a high-quality future-fantasy RPG bestiary: atmospheric, flavorful and easy to pronounce.
- Avoid plain labels like ""Fire Beast"", ""Stone Golem"", ""Dark Archer"", ""Poison Spider"", ""Mutant Soldier"".
- Names may include elemental or mystical flavoring only when it makes sense
  (e.g. ""Cindercoil Warden"", ""Glassborn Pulse-Seer""), but this is optional.
- MiniBoss and Boss monsters should have more epic, memorable names than regular monsters, but still avoid clichés
  like ""of Doom"", ""the Ultimate"", ""the Eternal"", ""of the Ancients"".

Constraints:
- recommendedMinLevel = {nextMin}
- recommendedMaxLevel = {nextMax}
- The 'stages' array MUST contain exactly {stageCount} stages.
- stageIndex must go from 1 to {stageCount} (no duplicates, no gaps).
- Each stageType value from the allowed enum list must be used exactly once across all stages
  (no stageType is repeated, no stageType is missing).

Level scaling rules:
- Each monster has a field ""BaseLevel"".
- Stage 1 monster BaseLevel MUST be exactly MinLevel ({nextMin}).
- Last stage (stageIndex = {stageCount}) monster BaseLevel MUST be exactly MaxLevel ({nextMax}).
- For every stageIndex i < j, the monster at stage j MUST NOT have lower BaseLevel than the monster at stage i.
- All BaseLevel values MUST be between MinLevel and MaxLevel (inclusive).
- Stats (MaxHp, Attack, Defense, Speed, CritChance, CritMultiplier, ElementalDamageBonus, ElementalResistance)
  MUST scale with BaseLevel so that later stages feel clearly harder.

Element rules:
- Each monster's ""element"" must be one of: {elementEnumValues}.
- Across the WHOLE dungeon (all stages), you may use at most TWO distinct elements in total.
- First, implicitly choose up to two elements that fit the dungeon theme, then use only those elements for all monsters and the boss.
- Do NOT use more than two different element values in the entire dungeon.

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
You are an assistant generating procedural expedition locations for {GameTheme}.

You MUST ALWAYS return ONLY valid JSON.
The JSON MUST be deserializable into this C#-like shape:

{jsonShape}

Rules:
- ""StageType"" must be one of: {stageTypeEnumValues}
- ""Element"" must be one of: {elementEnumValues}
- The 'Stages' array MUST contain exactly {stageCount} items.
- The ""Code"" MUST be generated directly from English translated ""NameEn"":
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
- ""NameEn"" must be 2–4 words, atmospheric, memorable and clearly future-fantasy themed.
- It should evoke a place you would want to explore in a premium future-fantasy RPG
  (e.g. ""Ashen Lantern Fields"", ""Glimmering Rift"", ""Shattered Grove"", ""Neon Ember Warrens"").
- The new location name must feel clearly different in tone and wording from the previous location ""{previousName}"".
- Avoid ultra-generic names like ""Forest Camp"", ""Old Village"", ""Mountain Pass"", ""Ancient Ruins"", ""Mystic Valley"".

Location–mercenary cohesion rules:
- All mercenaries must clearly belong in THIS specific location.
- Their gear, behavior and backstory should feel shaped by the location's terrain, hazards, culture and element(s).
- At least some mercenary names should echo key ideas from the location name or description
  (materials, energies, factions, relics, rituals, technology).
- The final stage enemy (boss / leader) must be the strongest, most iconic expression of the location's core theme.

Mercenary uniqueness rules:
- Every mercenary in this location MUST have a unique ""NameEn"" (no duplicates, no tiny variants like ""Raider"" / ""Raider Captain"").
- Assume many other locations exist in the game: avoid generic mercenary names that could fit anywhere.
- Each mercenary should feel like a distinct, memorable character or notable foe.

Mercenary naming rules:
- Mercenary names must sound like individual characters or notable foes, not generic mobs.
- Avoid plain labels like ""Fire Soldier"", ""Water Archer"", ""Poison Bandit"", ""Dark Mage"", ""Cyber Thug"".
- Prefer evocative names like ""Ashbreaker Scout"", ""Gloomsworn Cutthroat"", ""Verdant Pike-Leader"", ""Lumenwire Saboteur"", etc.
- Boss-type mercenaries can have slightly grander titles, but still without clichés like ""of Doom"", ""the Eternal"", ""of the Ancients"".

Constraints:
- MinLevel = {nextMin}
- MaxLevel = {nextMax}
- The 'Stages' array MUST contain exactly {stageCount} stages.
- StageNumber must go from 1 to {stageCount} (no duplicates, no gaps).
- Each StageType value from the allowed enum list must be used exactly once across all stages
  (no StageType is repeated, no StageType is missing).
- Enemy stats (attack, defense, health, etc.) should scale with BaseLevel so that later stages feel harder.

Level scaling rules:
- Each mercenary (enemy) has a field ""BaseLevel"".
- Stage 1 mercenary BaseLevel MUST be exactly MinLevel ({nextMin}).
- Last stage (StageNumber = {stageCount}) mercenary BaseLevel MUST be exactly MaxLevel ({nextMax}).
- For every StageNumber i < j, the mercenary at stage j MUST NOT have lower BaseLevel than the one at stage i.
- All BaseLevel values MUST be between MinLevel and MaxLevel (inclusive).
- Enemy stats (MaxHp, Attack, Defense, Speed, CritChance, CritMultiplier, ElementalDamageBonus, ElementalResistance)
  MUST scale with BaseLevel so that later stages feel clearly harder.

Element rules:
- Each mercenary's ""element"" must be one of: {elementEnumValues}.
- Across the WHOLE location (all stages), you may use at most TWO distinct elements in total.
- First, implicitly choose up to two elements that fit the location theme, then use only those elements for all mercenaries and the boss.
- Do NOT use more than two different element values in the entire location.

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
You are a professional localization assistant for {GameTheme}.
You translate game entity names and descriptions into Czech (cs) and German (de),
following strict linguistic rules.

You ALWAYS output ONLY valid JSON that matches this C# structure:

{jsonShape}

### LANGUAGE RULES (MANDATORY)

1) NameEn and DescriptionEn:
   - Remain fully in English.
   - You MAY polish style slightly, but DO NOT change meaning.

2) NameCs and DescriptionCs:
   - MUST be valid Czech.
   - MUST contain ONLY Czech vocabulary, grammar and diacritics.
   - NO English, NO German words allowed.

3) NameDe and DescriptionDe:
   - MUST be valid German.
   - MUST contain ONLY German vocabulary and grammar.
   - NO English, NO Czech words allowed.

4) Style:
   - High-quality fantasy RPG tone.
   - Keep fantasy flavor, do not add new story or lore.
   - If literal translation sounds bad, prefer a natural localized equivalent.

5) JSON output:
   - Output ONLY the JSON object.
   - No comments, no markdown, no explanations.
";

   var userPrompt = $@"
EntityKind: {entityKind}

Base English Input:
- NameEn: {nameEn}
- DescriptionEn: {(string.IsNullOrWhiteSpace(descriptionEn) ? "(none)" : descriptionEn)}

Translate into Czech and German according to the language rules.
Return ONLY JSON.
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