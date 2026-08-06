using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Infrastructure;
using Microsoft.EntityFrameworkCore;
using SharedServices;

namespace MercenariesAndBeasts.Web.Services;

/// <summary>
/// Handles player level-up checks and level progression for Mercenaries &amp; Beasts.
///
/// Level-up conditions (all must be satisfied simultaneously):
///   1. Player has completed at least (CurrentLevel) expedition locations
///   2. Player has completed at least (CurrentLevel) dungeons
///   3. Player has accumulated at least (CurrentLevel × 10) completed expedition achievements
///   4. Player has accumulated at least (CurrentLevel × 10) completed dungeon achievements
///
/// When conditions are met the player's level increases by 1.
/// This service is idempotent: calling it multiple times only levels up once per threshold.
/// </summary>
public class LevelUpService
{
    private readonly IDbContextFactory<AppDbContextMercenariesAndBeasts> _dbFactory;
    private readonly ToastService _toast;

    public LevelUpService(
        IDbContextFactory<AppDbContextMercenariesAndBeasts> dbFactory,
        ToastService toast)
    {
        _dbFactory = dbFactory;
        _toast     = toast;
    }

    /// <summary>
    /// Checks whether the player meets level-up conditions and applies the level-up if so.
    /// Should be called after each expedition or dungeon fight completion.
    /// </summary>
    /// <returns>True if the player leveled up, false otherwise.</returns>
    // AUDIT:FIXED|byl: race condition – dva souběžné requesty mohly oba level-up aplikovat;
    // nyní optimistická concurrency: ExecuteUpdateAsync WHERE Level = currentLevel (jen jeden uspěje)
    public async Task<bool> CheckAndApplyAsync(Guid playerId)
    {
        await using var db = await _dbFactory.CreateDbContextAsync();

        var player = await db.Players
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Guid == playerId);

        if (player is null) return false;

        int currentLevel = player.Level;

        // 1 – Completed expeditions
        int completedExpeditions = await db.PlayerExpeditionProgresses
            .CountAsync(p => p.PlayerId == playerId && p.IsCompleted);

        // 2 – Completed dungeons
        int completedDungeons = await db.PlayerDungeonProgresses
            .CountAsync(p => p.PlayerId == playerId && p.IsCompleted);

        // 3 – Completed expedition achievements (total)
        int expeditionAchievements = await db.PlayerExpeditionAchievementRows
            .CountAsync(a => a.PlayerId == playerId && a.IsCompleted);

        // 4 – Completed dungeon achievements (total)
        int dungeonAchievements = await db.PlayerDungeonAchievements
            .CountAsync(a => a.PlayerId == playerId && a.IsCompleted);

        // Thresholds scale with current level so each level-up requires more progress
        int requiredCompleted    = currentLevel;
        int requiredAchievements = currentLevel * 10;

        bool canLevelUp =
            completedExpeditions   >= requiredCompleted    &&
            completedDungeons      >= requiredCompleted    &&
            expeditionAchievements >= requiredAchievements &&
            dungeonAchievements    >= requiredAchievements;

        if (!canLevelUp) return false;

        int newLevel     = currentLevel + 1;
        int newMaxEnergy = 100 + currentLevel * 10;

        // WHERE Level = currentLevel zajišťuje, že při souběžném requestu uspěje jen jeden
        int rows = await db.Players
            .Where(p => p.Guid == playerId && p.Level == currentLevel)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.Level,      newLevel)
                .SetProperty(p => p.MaxEnergy,  newMaxEnergy)
                .SetProperty(p => p.SoftCurrency, p => p.SoftCurrency + 500L * newLevel));

        if (rows == 0) return false; // jiný request byl rychlejší

        _toast.ShowSuccess(
            $"🎉 Level {newLevel}!",
            $"Dosáhl jsi úrovně {newLevel}! " +
            $"Energie +10 ({newMaxEnergy} max), měna +{500 * newLevel:N0}.");

        return true;
    }
}
