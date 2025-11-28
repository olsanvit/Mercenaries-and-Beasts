
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Utils;
using MercenariesAndBeasts.Items;

namespace MercenariesAndBeasts.Domain.Items
{
    /// <summary>
    /// Šablona itemu – základní typ.
    /// </summary>
    public class ItemTemplate : BaseGuid
    {
        public string Code { get; set; } = string.Empty; // "WPN_FIRE_SWORD_01"
        public string NameEn { get; set; } = string.Empty;

        public ItemType ItemType { get; set; }

        public MonsterItemSlot? Slot { get; set; } // null pro materiály, vejce, kontrakty

        /// <summary>
        /// Základní staty (pro gear).
        /// </summary>
        public StatBlock BaseStats { get; set; } = StatBlock.Zero;

        /// <summary>
        /// Základní kvalita šablony (např. Tier3 = rare).
        /// </summary>
        public QualityTier BaseQuality { get; set; } = QualityTier.Q1_Common;

        public List<ItemEffect> Effects { get; set; } = new();
    }

    /// <summary>
    /// Konkrétní item vlastněný hráčem.
    /// </summary>
    public class PlayerItem
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }
        public PlayerProfile Player { get; set; } = null!;

        public Guid TemplateId { get; set; }
        public ItemTemplate Template { get; set; } = null!;

        /// <summary>
        /// Level itemu – zvyšuje se pomocí sběratelských itemů.
        /// </summary>
        public int Level { get; set; } = 1;

        /// <summary>
        /// Kvalita 1–11. Pro získání vyšší kvality se spojují 2 stejné itemy.
        /// </summary>
        public QualityTier Quality { get; set; } = QualityTier.Q1_Common;

        /// <summary>
        /// Dodatečné bonusy (prefix/suffix, enchanty, atd.).
        /// </summary>
        public StatBlock BonusStats { get; set; } = StatBlock.Zero;

        /// <summary>
        /// Případně prefix/suffix string – nebo to rozsekáš na samostatné entity.
        /// </summary>
        public string? PrefixCode { get; set; }
        public string? SuffixCode { get; set; }
    }
}