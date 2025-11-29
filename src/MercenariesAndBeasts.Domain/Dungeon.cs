using System.ComponentModel.DataAnnotations;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain;

public class Dungeon : BaseGuid
{

    [Required]
    [MaxLength(64)]
    public string Code { get; set; } = default!; 

    // Physical, Fire, Ice, Poison, Lightning, Shadow, Light...
    [Required, MaxLength(32)]
    public string Element { get; set; } = "Physical";

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
        public Units.MonsterTemplate MonsterTemplate { get; set; } = null!;

        public int RecommendedLevel { get; set; }
        public int DifficultyRating { get; set; }
    }