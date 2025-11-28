using System.Net.Http.Json;
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

    public async Task<ExpeditionGenerationResult> GenerateAsync(ExpeditionGenerationOptions options, CancellationToken ct = default)
    {
        
          var systemPrompt = @"You are an assistant that returns ONLY valid JSON ... (viz minule)";
          var userPrompt = $@"
You are an assistant generating procedural expedition locations for a fantasy–tech RPG.

The player has just completed the expedition location named: ""{options.Name}"".

Generate the NEXT expedition location in progression.
It must be slightly harder, thematically connected, but original — not just a variant of the previous name.

### Location Requirements:
- Create 1 new expedition location.
- Name must feel fantasy with a subtle futuristic/arcane-tech flavor.
- Provide a short English description (2–3 sentences).
- recommendedMinLevel = previous location's recommendedMaxLevel + 1
- recommendedMaxLevel = recommendedMinLevel + 4
- Location has exactly 10 stages.

### Stage Requirements:
Each of the 10 stages must include:
- stageIndex (1–10)
- stageType: choose one of:
  - ""Patrol""    (light encounter)
  - ""Skirmish""  (standard fight)
  - ""Ambush""    (harder wave)
  - ""Vanguard""  (elite group)
  - ""Commander"" (final boss, stage 10 only)
- difficultyLevel: 1–11 (use increasing order as stages progress)
- enemy:
  - For stages 1–9 → generate 1 enemy mercenary or small squad with name + description + element + stats (Attack, Defense, Health)
  - For stage 10 → generate a commander/boss unit with name + description + element + stronger stats

### Elements:
Enemy element must be one of: Fire, Water, Earth, Air, Light, Shadow, Arcane, Nature, Frost, Metal, Chaos.

### Stats:
Attack, Defense, Health: choose reasonable RPG-like values (not too big), each higher on average than in the previous location.

### Output:
Return ONLY valid JSON in this exact shape (no wrapper object, no extra fields):

{{
  ""name"": ""..."",
  ""description"": ""..."",
  ""recommendedMinLevel"": 0,
  ""recommendedMaxLevel"": 0,
  ""stages"": [
    {{
      ""stageIndex"": 0,
      ""stageType"": ""Patrol"",
      ""difficultyLevel"": 0,
      ""enemy"": {{
        ""name"": ""..."",
        ""description"": ""..."",
        ""element"": ""..."",
        ""attack"": 0,
        ""defense"": 0,
        ""health"": 0
      }}
    }}
  ]
}}

DO NOT write anything outside JSON.
";

        var res =  _asker.AskJsonAsync<ExpeditionStageDefinition>(systemPrompt, userPrompt, ct);
        // AI (OpenAI,  backend)
        _logger.LogInformation("AI dungeon generator called for {Min}-{Max}", options.TargetMinLevel, options.TargetMaxLevel);

        var stages = new List<ExpeditionStageDefinition>();

        var stageTypes = new[]
        {
            ExpeditionStageType.ES1_Novice,
            ExpeditionStageType.ES2_Trained,
            ExpeditionStageType.ES3_Disciplined,
            ExpeditionStageType.ES4_Skilled,
            ExpeditionStageType.ES5_Hardened,  
            ExpeditionStageType.ES6_Elite,
            ExpeditionStageType.ES7_Veteran,
            ExpeditionStageType.ES8_Master,
            ExpeditionStageType.ES9_Commander,
            ExpeditionStageType.ES10_Dominion,
            ExpeditionStageType.ES11_Ascendant
        };

        for (int i = 0; i < 10; i++)
        {
            var lvl = options.TargetMinLevel + i; // dummy scaling

            stages.Add(new ExpeditionStageDefinition
            {
                StageNumber = i + 1,
                StageType = stageTypes[i],
                Mercenary = new ExpeditionMercenaryDefinition
                {
                    NameEn = $"AI Mercenary {i + 1}",
                    DescriptionEn = "Dummy AI-generated mercenary. Replace with real AI output.",
                    Element = options.FocusElement ?? ElementType.Fire,
                    BaseLevel = lvl,
                    Stats = new StatBlock
                    {
                        MaxHp = 80 + lvl * 15,
                        Attack = 12 + lvl * 3,
                        Defense = 6 + lvl * 2,
                        Speed = 6 + i,
                        CritChance = 5 + i,
                         CritMultiplier = 50
                    }
                }
            });
        }

        return await Task.FromResult(new ExpeditionGenerationResult
        {
            Code = $"AI_EXP_{options.TargetMinLevel}_{options.TargetMaxLevel}",
            NameEn = $"AI Expedition {options.TargetMinLevel}-{options.TargetMaxLevel}",
            DescriptionEn = "AI generated expedition (placeholder).",
            MinLevel = options.TargetMinLevel,
            MaxLevel = options.TargetMaxLevel,
            Stages = stages
        });
    }
    public async Task<DungeonGenerationResult> GenerateAsync(DungeonGenerationOptions options, CancellationToken ct = default)
    {
          var systemPrompt = @"You are an assistant that returns ONLY valid JSON ... (viz minule)";
        var userPrompt = $@"
            You are an assistant generating procedural dungeon content for a fantasy–tech RPG.

            The player has just completed the dungeon named: ""{options.Name}"".

            Generate the NEXT dungeon in progression.  
            It must be slightly harder, thematically connected, but original — not a variant of the previous name.

            ### Dungeon Requirements:
            - Create 1 new dungeon.
            - Name must feel fantasy with subtle futuristic/arcane-tech flavor.
            - Provide a short English description (2–3 sentences).
            - RecommendedMinLevel = previous dungeon's RecommendedMaxLevel + 1
            - RecommendedMaxLevel = RecommendedMinLevel + 4
            - Dungeon has exactly 10 stages.

            ### Stage Requirements:
            Each of the 10 stages must include:
            - stageIndex (1–10)
            - stageType: choose one of:
            - ""Encounter""   (standard fight)
            - ""Elite""       (tougher enemy)
            - ""MiniBoss""    (should appear at stage 5 only)
            - ""Boss""        (stage 10 only)
            - difficultyLevel: 1–11 (use increasing order)
            - monster: 
            - For stages 1–9 → generate 1 monster with name + description + element + stats (Attack, Defense, Health)
            - For stage 10 → generate a boss with name + description + element + stats (stronger)

            ### Elements:
            Enemy element must be one of: Fire, Water, Earth, Air, Light, Shadow, Arcane, Nature, Frost, Metal, Chaos.

            ### Stats:
            Attack, Defense, Health: choose reasonable RPG-like values (not too big), each higher than previous dungeon’s average.

            ### Output:
            Return **ONLY valid JSON** in this format:

            DO NOT write anything outside JSON.
            ";

        var res =  _asker.AskJsonAsync<DungeonStageDefinition>(systemPrompt, userPrompt, ct);
        // AI (OpenAI,  backend)
        _logger.LogInformation("AI dungeon generator called for {Min}-{Max}", options.TargetMinLevel, options.TargetMaxLevel);

        var stages = new List<DungeonStageDefinition>();

        var stageTypes = new[]
        {
            DungeonStageType.DS1_Feeble,
            DungeonStageType.DS2_Lesser,
            DungeonStageType.DS3_Wild,
            DungeonStageType.DS4_Hardened,
            DungeonStageType.DS5_Corrupted,
            DungeonStageType.DS6_Empowered,
            DungeonStageType.DS7_Twisted,
            DungeonStageType.DS8_Prime,
            DungeonStageType.DS9_Monstrous,
            DungeonStageType.DS10_Tyrant
        };

        for (int i = 0; i < 10; i++)
        {
            var lvl = options.TargetMinLevel + i; // dummy scaling

            stages.Add(new DungeonStageDefinition
            {
                StageNumber = i + 1,
                StageType = stageTypes[i],
                Monster = new DungeonMonsterDefinition
                {
                    NameEn = $"AI Beast {i + 1}",
                    DescriptionEn = "Dummy AI-generated beast. Replace with real AI output.",
                    BaseLevel = lvl,
                    Stats = new StatBlock
                    {
                        MaxHp = 100 + lvl * 20,
                        Attack = 10 + lvl * 3,
                        Defense = 8 + lvl * 2,
                        Speed = 5 + i,
                        CritChance = 5 + i,
                        CritMultiplier = 50
                    }
                }
            });
        }

        return await Task.FromResult(new DungeonGenerationResult
        {
            Code = $"AI_DUN_{options.TargetMinLevel}_{options.TargetMaxLevel}",
            NameEn = $"AI Dungeon {options.TargetMinLevel}-{options.TargetMaxLevel}",
            DescriptionEn = "AI generated dungeon (placeholder).",
            MinLevel = options.TargetMinLevel,
            MaxLevel = options.TargetMaxLevel,
            Stages = stages
        });
    }
}