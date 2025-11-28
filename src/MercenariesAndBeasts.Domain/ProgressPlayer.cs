

using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Progress
{
    public class PlayerExpeditionProgress: BaseGuid
    {

        public Guid PlayerId { get; set; }
        public PlayerProfile Player { get; set; } = null!;

        public Guid LocationId { get; set; }
        public Location Location { get; set; } = null!;

        /// <summary>
        /// Nejvyšší dosažený stage (1–10).
        /// </summary>
        public int MaxStageReached { get; set; }

        /// <summary>
        /// true, pokud byl poražen boss (stage 10).
        /// </summary>
        public bool IsCompleted { get; set; }

        public DateTime LastUpdatedUtc { get; set; }
    }

    public class PlayerDungeonProgress: BaseGuid
    {

        public Guid PlayerId { get; set; }
        public PlayerProfile Player { get; set; } = null!;

        public Guid DungeonId { get; set; }
        public Dungeon Dungeon { get; set; } = null!;

        public int MaxStageReached { get; set; }
        public bool IsCompleted { get; set; }

        public DateTime LastUpdatedUtc { get; set; }
    }

    /// <summary>
    /// Definice achievementu pro konkrétní výpravu (např. 5 per location).
    /// </summary>
    public class ExpeditionAchievementDefinition
    {
        public Guid Id { get; set; }

        public Guid LocationId { get; set; }
        public Location Location { get; set; } = null!;

        public int Index { get; set; }           // 1–5
        public string Code { get; set; } = string.Empty; // "EXP1_NO_DEATH"
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
    }

    public class PlayerExpeditionAchievement
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }
        public PlayerProfile Player { get; set; } = null!;

        public Guid AchievementId { get; set; }
        public ExpeditionAchievementDefinition Achievement { get; set; } = null!;

        public DateTime CompletedAtUtc { get; set; }
    }

    public class DungeonAchievementDefinition
    {
        public Guid Id { get; set; }

        public Guid DungeonId { get; set; }
        public Dungeon Dungeon { get; set; } = null!;

        public int Index { get; set; }           // 1–5
        public string Code { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string DescriptionEn { get; set; } = string.Empty;
    }

    public class PlayerDungeonAchievement
    {
        public Guid Id { get; set; }

        public Guid PlayerId { get; set; }
        public PlayerProfile Player { get; set; } = null!;

        public Guid AchievementId { get; set; }
        public DungeonAchievementDefinition Achievement { get; set; } = null!;

        public DateTime CompletedAtUtc { get; set; }
    }
}