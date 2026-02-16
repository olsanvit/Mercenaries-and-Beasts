using System.ComponentModel.DataAnnotations;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Dto;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Players;

using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain;

public class Location: BaseGuid
{

    [Required, MaxLength(32)]
    public ElementType Element { get; set; } = ElementType.None;

    [Range(1, 999)]
    public int UnlockOrder { get; set; } = 1;

    [Range(1, 999)]
    public int MinLevel { get; set; }

    [Range(1, 999)]
    public int MaxLevel { get; set; }

    public List<ExpeditionStage> Stages { get; set; } = new();
}
public class ExpeditionStage: BaseGuid
    {

        public Guid LocationId { get; set; }
        public Location Location { get; set; } = null!;

        public int StageNumber { get; set; }   // 1–10

        public ExpeditionStageType Difficulty { get; set; } = ExpeditionStageType.ES1_Novice;

        // Enemy mercenary for this stage
        public Guid EnemyId { get; set; }
        public MercenaryTemplate Enemy { get; set; } = null!;

        public int? FixedLevel { get; set; }   // volitelné stupňování podle lokace
    }
    public class MercenaryTemplate: BaseGuid
    {

        public ElementType Element { get; set; } = ElementType.None;
        public UnitRole Role { get; set; }
        public StatBlock BaseStats { get; set; } = StatBlock.Zero;
    }
    public class PlayerMercenary : BaseGuid
    {

        public Guid PlayerId { get; set; }
        public PlayerProfile Player { get; set; } = null!;

        public Guid TemplateId { get; set; }
        public MercenaryTemplate Template { get; set; } = null!;

        public int Level { get; set; } = 1;

        public QualityTier Rank { get; set; } = QualityTier.Q1_Common;

        public StatBlock BonusStats { get; set; } = StatBlock.Zero;
        public ICollection<MercenaryEquipmentSlot> Equipment { get; set; } = new List<MercenaryEquipmentSlot>();
    }

    public sealed class ExpeditionGenerationOptions
{    public string Name { get; set; } = String.Empty;
    public int TargetMinLevel { get; set; }
    public int TargetMaxLevel { get; set; }

    public ElementType? FocusElement { get; set; }
}
public sealed class ExpeditionGenerationResult
{
    public string Code { get; set; } = string.Empty;
    public string NameEn { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;
    public string Element { get; set; } = string.Empty;
    public int MinLevel { get; set; }
    public int MaxLevel { get; set; }

    public IReadOnlyList<ExpeditionStageDefinition> Stages { get; set; } = Array.Empty<ExpeditionStageDefinition>();
}
public sealed class ExpeditionMercenaryDefinition
{
    public string NameEn { get; set; } = string.Empty;
    public string DescriptionEn { get; set; } = string.Empty;

    public string Element { get; set; }
    public int BaseLevel { get; set; }

    public string Role { get; set; } = string.Empty;   // ✅ přidat

    public StatBlock Stats { get; set; } = new();
}
public sealed class ExpeditionStageDefinition
{
    public int StageNumber { get; set; }                // 1–10
    public string StageType { get; set; }  // ES1_… ES11_…

    public ExpeditionMercenaryDefinition Mercenary { get; set; } = new();
}