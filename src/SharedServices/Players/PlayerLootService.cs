using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Combat;

namespace MercenariesAndBeasts.Infrastructure.Players;

public class PlayerLootService
{
    private readonly MercenariesAndBeastsDbContext _db;
    private readonly ILogger<PlayerLootService> _logger;

    public PlayerLootService(MercenariesAndBeastsDbContext db, ILogger<PlayerLootService> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Přidá item hráči:
    /// - první nález => discovered + item do inventáře
    /// - další nález => XP do discovery + item do inventáře
    /// </summary>
    public async Task GrantItemAsync(
        Guid playerProfileId,
        Guid itemTemplateId,
        CancellationToken ct = default)
    {
        var tpl = await _db.ItemTemplates
            .FirstAsync(t => t.Id == itemTemplateId, ct);

        var now = DateTime.UtcNow;

        var discovery = await _db.PlayerItemDiscoveries
            .FirstOrDefaultAsync(d =>
                d.PlayerId == playerProfileId &&
                d.TemplateId == itemTemplateId,
                ct);

        if (discovery is null)
        {
            discovery = new PlayerItemDiscovery
            {
                Id = Guid.NewGuid(),
                PlayerId = playerProfileId,
                TemplateId = itemTemplateId,
                IsDiscovered = true,
                TimesFound = 1,
                ItemXp = 0,
                MasteryLevel = 1,
                FirstDiscoveredUtc = now,
                LastFoundUtc = now
            };

            _db.PlayerItemDiscoveries.Add(discovery);

            _logger.LogInformation(
                "Item discovered: Player={PlayerId}, Item={ItemCode}",
                playerProfileId, tpl.Code);
        }
        else
        {
            discovery.TimesFound += 1;
            discovery.LastFoundUtc = now;

            var gainedXp = ComputeItemXp(tpl);
            discovery.ItemXp += gainedXp;

            // MasteryLevel = level z XP (tvoje jednoduchá mastery křivka)
            discovery.MasteryLevel = ComputeMasteryLevel(discovery.ItemXp);

            _logger.LogInformation(
                "Item XP gained: Player={PlayerId}, Item={ItemCode}, XP=+{Xp}",
                playerProfileId, tpl.Code, gainedXp);
        }

        // VŽDY přidáme instanci itemu do inventáře
        var playerItem = new PlayerItem
        {
            Id = Guid.NewGuid(),
            PlayerId = playerProfileId,
            TemplateId = itemTemplateId,
            Level = 1,
            Quality = tpl.BaseQuality,
            BonusStats = StatBlock.Zero,

            // pokud chceš, můžeš ukládat slot i na instanci
            EquipSlot = (tpl.MercenarySlot ?? tpl.MonsterSlot) ?? ItemEquipSlot.None
        };

        _db.PlayerItems.Add(playerItem);

        await _db.SaveChangesAsync(ct);
    }

    // ===============================
    // XP / Mastery logika
    // ===============================

    private static long ComputeItemXp(ItemTemplate tpl)
{
    var rarity = (int)tpl.BaseQuality; // Q1..Q11

    // typ odvodíme z toho co existuje:
    // - Gear: má slot
    // - Summon: IsSummon (pokud to ještě někde zůstalo)
    // - jinak "misc"
    var typeFactor =
        (tpl.MercenarySlot.HasValue || tpl.MonsterSlot.HasValue) ? 10 :
        (tpl.IsSummon) ? 15 :
        4;

    return rarity * typeFactor;
}

    private static int ComputeMasteryLevel(long xp)
{
    if (xp < 100) return 1;
    if (xp < 250) return 2;
    if (xp < 500) return 3;
    if (xp < 900) return 4;
    return 5;
}

    // Pokud chceš místo "MasteryLevel" mít i skutečný "ItemLevel" křivkou,
    // nechávám ti to tu jako util (zatím se nikde nepoužívá).
    private static int ComputeItemLevel(long xp)
    {
        const long baseXp = 50;      // cena za lvl 2
        const double growth = 1.15;  // 15% navýšení každým levelem

        int level = 1;
        long spent = 0;

        while (true)
        {
            long cost = (long)Math.Ceiling(baseXp * Math.Pow(growth, level - 1));
            if (spent + cost > xp) break;

            spent += cost;
            level++;
        }

        return level;
    }

    private static long XpToNextLevel(long xp)
    {
        const long baseXp = 50;
        const double growth = 1.15;

        int level = 1;
        long spent = 0;

        while (true)
        {
            long cost = (long)Math.Ceiling(baseXp * Math.Pow(growth, level - 1));
            if (spent + cost > xp)
                return (spent + cost) - xp;

            spent += cost;
            level++;
        }
    }
}