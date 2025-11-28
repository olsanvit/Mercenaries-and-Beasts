using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Units
{
    public class MercenaryTemplate: BaseGuid
    {
        public string Code { get; set; } = string.Empty;   

        public ElementType Element { get; set; }

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
    }

}