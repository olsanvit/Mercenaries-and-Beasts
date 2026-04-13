

using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Progress
{


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
    public string Code { get; set; } = null!; // 1 z 22
    public int Required { get; set; }
    public int Current { get; set; }
    public bool IsCompleted { get; set; }
public Guid ProgressId { get; set; }
public PlayerExpeditionProgress Progress { get; set; } = null!;
        public Guid AchievementId { get; set; }
        public ExpeditionAchievementDefinition Achievement { get; set; } = null!;

        public DateTime CompletedAtUtc { get; set; }
    public int DifficultyTier { get; set; } // “náhodně se zvyšuje”
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

    public Guid ProgressId { get; set; }
    public PlayerDungeonProgress Progress { get; set; } = null!;
        public Guid PlayerId { get; set; }
        public PlayerProfile Player { get; set; } = null!;

        public Guid AchievementId { get; set; }
        public DungeonAchievementDefinition Achievement { get; set; } = null!;

        public DateTime CompletedAtUtc { get; set; }

            public bool IsCompleted { get; set; }
    public string Code { get; set; } = null!; // např. "Boss", "WinStreak", ...
            public int Current { get; set; }
            public int Required { get; set; }
    public int DifficultyTier { get; set; } // náročnost
    }
}