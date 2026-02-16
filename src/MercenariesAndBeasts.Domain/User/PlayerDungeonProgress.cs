
using MercenariesAndBeasts.Domain.Progress;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;
public class PlayerDungeonProgress : BaseGuid
{
    public Guid PlayerId { get; set; }
    public PlayerProfile Player { get; set; } = null!;

    public Guid DungeonId { get; set; }
    public Dungeon Dungeon { get; set; } = null!;

    public int CurrentStage { get; set; } = 1;     // 1..11
    public int MaxStageReached { get; set; } = 1;  // 1..11
    public bool IsCompleted { get; set; } = false;

    public int WinStreak { get; set; } = 0;
    public DateTime LastUpdatedUtc { get; set; } = DateTime.UtcNow;

    public ICollection<PlayerDungeonAchievement> Achievements { get; set; } = new List<PlayerDungeonAchievement>();

    }