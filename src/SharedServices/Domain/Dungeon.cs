using System.ComponentModel.DataAnnotations;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Dto;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain;

public class Dungeon : BaseGuid
{

    // Physical, Fire, Ice, Poison, Lightning, Shadow, Light...
    [Required, MaxLength(32)]
    public ElementType Element { get; set; } = ElementType.None;

    [Range(1, 999)]
    public int UnlockOrder { get; set; } = 1;
    [Range(1, 999)]
    public int MinLevel { get; set; }

    [Range(1, 999)]
    public int MaxLevel { get; set; }


        public List<DungeonStage> Stages { get; set; } = new List<DungeonStage>();

}
public class DungeonStage : BaseGuid
    {
        

        public Guid DungeonId { get; set; }
        public Dungeon Dungeon { get; set; } = null!;

        public int StageIndex { get; set; }

        public DungeonStageType StageType { get; set; } = DungeonStageType.DS1_Feeble;

        public Guid MonsterTemplateId { get; set; }
        public MonsterTemplate MonsterTemplate { get; set; } = null!;

        public int RecommendedLevel { get; set; }
        public int DifficultyRating { get; set; }
    }
    /// <summary>
    /// Typ monstra – základní definice (template).
    /// </summary>
    public class MonsterTemplate :BaseGuid
    {

        public ElementType Element { get; set; } = ElementType.None;
        public UnitRole Role { get; set; }

        public StatBlock BaseStats { get; set; } = StatBlock.Zero;
    }
    /// <summary>
    /// Konkrétní monstrum vlastněné hráčem.
    /// </summary>
    public class PlayerMonster : BaseGuid
    {

        public Guid PlayerId { get; set; }
        public PlayerProfile Player { get; set; } = null!;

        public Guid TemplateId { get; set; }
        public MonsterTemplate Template { get; set; } = null!;

        public int Level { get; set; } = 1;
        public QualityTier Rank { get; set; } = QualityTier.Q1_Common;

        public StatBlock BonusStats { get; set; } = StatBlock.Zero;
        public ICollection<BeastEquipmentSlot> Equipment { get; set; } = new List<BeastEquipmentSlot>();
    }

    public sealed class DungeonGenerationResult
{
    public string Code { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string Element { get; set; } = string.Empty;

    public int MinLevel { get; set; }
    public int MaxLevel { get; set; }

    public IReadOnlyList<DungeonStageDefinition> Stages { get; set; } = Array.Empty<DungeonStageDefinition>();
}
public sealed class DungeonGenerationOptions
{
    public string Name { get; set; }
    public string Code { get; set; }
    public int TargetMinLevel { get; set; }
    public int TargetMaxLevel { get; set; }

    // volitelné, můžeš rozšířit:
    public ElementType? FocusElement { get; set; }
}
public sealed class DungeonMonsterDefinition
{
    public string Role { get; set; } = string.Empty; // nebo UnitRole jako string kvůli JSON
    public string Code { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;

    public string Element { get; set; } = string.Empty;
    public int BaseLevel { get; set; }

    public StatBlock Stats { get; set; } = new();
}
public sealed class DungeonStageDefinition
{
    public string Code { get; set; } = string.Empty;
    public int StageNumber { get; set; }            // 1–10
    
    public string StageType { get; set; } = string.Empty;

    public DungeonMonsterDefinition Monster { get; set; } = new();
}