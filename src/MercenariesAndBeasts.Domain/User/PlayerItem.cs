using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;
public class PlayerItem : BaseGuid
    {
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
        public ItemEquipSlot EquipSlot { get; set; } = ItemEquipSlot.None;

        public Guid? LinkedMercenaryInstanceId { get; set; }  // pro Contract
        public Guid? LinkedMonsterInstanceId { get; set; }    // pro Egg

private ItemBadgeTier _badgeTier = ItemBadgeTier.None;
       public ItemBadgeTier BadgeTier => _badgeTier;

        // “XP” na badge – doporučuju wins, protože to chceš od winů
        public int Wins { get; set; } = 0;

        // aby šlo debugovat / anti-cheat / statistika
        public int TimesUsedInWins { get; set; } = 0;
        
        public void RecomputeBadgeTier(int wins)
        => _badgeTier = ItemBadgeRules.ComputeTier(wins);
    }