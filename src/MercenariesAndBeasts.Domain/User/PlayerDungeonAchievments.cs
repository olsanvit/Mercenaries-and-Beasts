using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;

public class PlayerDungeonAchievements : BaseGuid
{
    public Guid PlayerId { get; set; }
    public PlayerProfile Player { get; set; } = null!;

    public Guid DungeonId { get; set; }
    public Dungeon Dungeon { get; set; } = null!;

    // 1) porazení bosse
    public bool BossDefeated { get; set; } = false;

    // 2) porazení všech fází
    public int StagesCleared { get; set; } = 0;
    public int TotalStages { get; set; } = 11;

    public bool AllStagesCleared => StagesCleared >= TotalStages;

    // 3) poražení v řadě
    public int BestWinStreak { get; set; } = 0;
    public int RequiredWinStreak { get; set; } = 3;

    public bool WinStreakAchieved => BestWinStreak >= RequiredWinStreak;

    public bool IsFullyCompleted =>
        BossDefeated && AllStagesCleared && WinStreakAchieved;
}