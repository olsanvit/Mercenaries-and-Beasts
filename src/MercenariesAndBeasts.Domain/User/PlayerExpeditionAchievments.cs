
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;
public class PlayerExpeditionAchievements : BaseGuid
{
    public Guid PlayerId { get; set; }
    public PlayerProfile Player { get; set; } = null!;

    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    // (1) porazeni bosse
    public bool BossDefeated { get; set; } = false;

    // (2) porazeni vsech nepratel (např. počet unikátních poražených v lokaci)
    public int EnemiesDefeatedCount { get; set; } = 0;
    public int EnemiesTotalCount { get; set; } = 3; // pro expedici fixně 3, nebo dopočítáš dynamicky

    public bool AllEnemiesDefeated => EnemiesDefeatedCount >= EnemiesTotalCount;

    // (3) ziskani vsech orders do inventory
    // nejjednodušší: kolik unikátních “Order templates” z dané lokace už hráč “discovered”
    public int OrdersDiscoveredCount { get; set; } = 0;
    public int OrdersTotalCount { get; set; } = 3; // nebo per-lokace pool

    public bool AllOrdersDiscovered => OrdersDiscoveredCount >= OrdersTotalCount;

    // (4) porazeni v rade
    public int BestWinStreak { get; set; } = 0;
    public int RequiredWinStreak { get; set; } = 3;

    public bool WinStreakAchieved => BestWinStreak >= RequiredWinStreak;

    public bool IsFullyCompleted =>
        BossDefeated && AllEnemiesDefeated && AllOrdersDiscovered && WinStreakAchieved;
}