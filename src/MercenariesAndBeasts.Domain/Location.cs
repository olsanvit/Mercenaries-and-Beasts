using System.ComponentModel.DataAnnotations;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Units;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain;

public class Location: BaseGuid
{

    [Required]
    [MaxLength(64)]
    public string Code { get; set; } = default!; // např. "NORTHERN_TRADE_ROAD"

    [Required]
    [MaxLength(128)]
    public string NameEn { get; set; } = default!;

    [Required, MaxLength(32)]
    public string Element { get; set; } = "Physical";

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