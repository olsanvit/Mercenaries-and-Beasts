
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Units
{
    /// <summary>
    /// Typ monstra – základní definice (template).
    /// </summary>
    public class MonsterTemplate :BaseGuid
    {
        public string Code { get; set; } = string.Empty;   // např. "BEAST_FIRE_WOLF"

        public ElementType Element { get; set; }

        public StatBlock BaseStats { get; set; } = StatBlock.Zero;
    }
    /// <summary>
    /// Konkrétní monstrum vlastněné hráčem.
    /// </summary>
    public class PlayerMonster
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }
        public PlayerProfile Player { get; set; } = null!;

        public Guid TemplateId { get; set; }
        public MonsterTemplate Template { get; set; } = null!;

        public int Level { get; set; } = 1;
        public QualityTier Rank { get; set; } = QualityTier.Q1_Common;

        public StatBlock BonusStats { get; set; } = StatBlock.Zero;
    }
}