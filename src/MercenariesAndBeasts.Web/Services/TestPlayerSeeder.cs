using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Utils;
using MercenariesAndBeasts.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MercenariesAndBeasts.Web.Services;

/// <summary>
/// Seeds test player profiles with progression data for Mercenaries &amp; Beasts.
/// Creates 5 test accounts at different progression levels so the full game loop
/// (dungeons, expeditions, achievements, leaderboard) can be verified without manual play.
///
/// Idempotent — skips if test players already exist.
/// </summary>
public sealed class TestPlayerSeeder
{
    private readonly IDbContextFactory<AppDbContextMercenariesAndBeasts> _factory;
    private readonly UserManager<AppUser> _userManager;

    public TestPlayerSeeder(
        IDbContextFactory<AppDbContextMercenariesAndBeasts> factory,
        UserManager<AppUser> userManager)
    {
        _factory     = factory;
        _userManager = userManager;
    }

    public async Task<string> SeedAsync(CancellationToken ct = default)
    {
        await using var db = await _factory.CreateDbContextAsync(ct);

        // Idempotent check
        if (await db.Players.AnyAsync(p => p.UserId != null && p.UserId.StartsWith("test_"), ct))
            return "Test players already exist — skipped.";

        var locations = await db.Locations.Where(l => l.IsActive).OrderBy(l => l.UnlockOrder).Take(5).ToListAsync(ct);
        var dungeons  = await db.Dungeons.Where(d => d.IsActive).OrderBy(d => d.UnlockOrder).Take(5).ToListAsync(ct);

        var testPlayers = new[]
        {
            ("test_novice",    "Novice Hrach",   1,  100,     500L,   0L),
            ("test_explorer",  "Průzkumník Jan", 3,  3_200,   5_000L, 50L),
            ("test_veteran",   "Veterán Tomáš",  6,  15_000,  25_000L,200L),
            ("test_champion",  "Šampion Petr",   10, 55_000,  100_000L,500L),
            ("test_legend",    "Legenda Karel",  15, 200_000, 500_000L,2_000L),
        };

        int created = 0;
        var rng = new Random(1337);

        foreach (var (userId, name, level, xp, soft, premium) in testPlayers)
        {
            // Identity user (dummy — not loginable, just for FK)
            var email = $"{userId}@test.local";
            var appUser = await _userManager.FindByEmailAsync(email);
            if (appUser is null)
            {
                appUser = new AppUser
                {
                    Id            = userId,
                    UserName      = userId,
                    Email         = email,
                    EmailConfirmed = true
                };
                var result = await _userManager.CreateAsync(appUser, "TestPass123!");
                if (!result.Succeeded) continue;
            }

            var player = new PlayerProfile
            {
                UserId         = userId,
                Level          = level,
                Experience     = xp,
                SoftCurrency   = soft,
                PremiumCurrency = (int)premium,
                Energy         = 100,
                MaxEnergy      = 100 + (level - 1) * 10,
                IsInitialized  = true
            };
            db.Players.Add(player);
            await db.SaveChangesAsync(ct);

            // Expedition progress based on level
            int expLocations = Math.Min(level / 3 + 1, locations.Count);
            for (int i = 0; i < expLocations && i < locations.Count; i++)
            {
                var expProg = new PlayerExpeditionProgress
                {
                    PlayerId    = player.Guid,
                    LocationId  = locations[i].Guid,
                    IsCompleted = i < expLocations - 1,
                    TotalWins   = rng.Next(5, 30),
                    TotalLosses = rng.Next(0, 10),
                    WinStreak   = rng.Next(0, 5),
                    MaxStageReached = i < expLocations - 1 ? 10 : rng.Next(1, 10)
                };
                db.PlayerExpeditionProgresses.Add(expProg);
            }

            // Dungeon progress based on level
            int dungeonCount = Math.Min(level / 2 + 1, dungeons.Count);
            for (int i = 0; i < dungeonCount && i < dungeons.Count; i++)
            {
                var dungProg = new PlayerDungeonProgress
                {
                    PlayerId       = player.Guid,
                    DungeonId      = dungeons[i].Guid,
                    IsCompleted    = i < dungeonCount - 1,
                    CurrentStage   = i < dungeonCount - 1 ? 11 : rng.Next(1, 11),
                    MaxStageReached= i < dungeonCount - 1 ? 11 : rng.Next(1, 11),
                    WinStreak      = rng.Next(0, 4),
                    LastUpdatedUtc = DateTime.UtcNow.AddDays(-rng.Next(0, 30))
                };
                db.PlayerDungeonProgresses.Add(dungProg);
            }

            await db.SaveChangesAsync(ct);
            created++;
        }

        return $"✅ Seeded {created} test players (Novice → Legend) with expedition/dungeon progress.";
    }
}
