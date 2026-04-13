using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Infrastructure.Players;

/// <summary>
/// Creates a PlayerProfile on first login and seeds starter party + starter inventory.
/// 
/// Current behavior:
/// - seeds 5 merc unit items + 5 beast unit items
/// - creates 5+5 party slots if missing
/// - auto-equips up to 5 units into empty slots
/// - creates equipment slots only for newly created unit instances
/// - keeps one instance at most once in party
/// </summary>
public class PlayerOnboardingService
{
    private readonly GameDbContext _db;
    private readonly IErrorService _errors;
    private readonly ILogger<PlayerOnboardingService> _logger;

    public PlayerOnboardingService(GameDbContext db, ILogger<PlayerOnboardingService> logger, IErrorService errors)
    {
        _db = db;
        _logger = logger;
        _errors = errors;
    }

    public async Task EnsurePlayerInitializedAsync(string userId, CancellationToken ct = default)
    {
        var profile = await _db.Players
            .Include(p => p.MercenarySlots)
            .Include(p => p.BeastSlots)
            .Include(p => p.Inventory)
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (profile is not null && profile.IsInitialized)
            return;

        if (profile is null)
        {
            profile = new PlayerProfile
            {
                UserId = userId,
                Level = 1,
                Experience = 0,
                SoftCurrency = 200,
                PremiumCurrency = 0,
                Energy = 100,
                MaxEnergy = 100,
                IsInitialized = false
            };

            _db.Players.Add(profile);
            await _db.SaveChangesAsync(ct);
        }

        // 1) Create party slots (5 + 5) if missing
        if (profile.MercenarySlots.Count == 0)
        {
            for (int i = 1; i <= 5; i++)
            {
                _db.PlayerMercenarySlots.Add(new PlayerMercenarySlot
                {
                    PlayerProfileId = profile.Id,
                    SlotIndex = i
                });
            }
        }

        if (profile.BeastSlots.Count == 0)
        {
            for (int i = 1; i <= 5; i++)
            {
                _db.PlayerBeastSlots.Add(new PlayerBeastSlot
                {
                    PlayerProfileId = profile.Id,
                    SlotIndex = i
                });
            }
        }

        await _db.SaveChangesAsync(ct);

        // 2) Get starter unit templates (5 + 5)
        var beastUnitTpls = await _db.ItemTemplates
            .AsNoTracking()
            .Where(t => t.OwnerKind == ItemOwnerKind.Beast)
            .Where(t => t.MonsterTemplateId != null)
            .OrderBy(t => t.Code)
            .Take(5)
            .ToListAsync(ct);

        var mercUnitTpls = await _db.ItemTemplates
            .AsNoTracking()
            .Where(t => t.OwnerKind == ItemOwnerKind.Mercenary)
            .Where(t => t.MercenaryTemplateId != null)
            .OrderBy(t => t.Code)
            .Take(5)
            .ToListAsync(ct);

        if (mercUnitTpls.Count < 5 || beastUnitTpls.Count < 5)
        {
            throw new InvalidOperationException(
                "Missing starter UNIT item templates (need at least 5 merc unit templates and 5 beast unit templates).");
        }

        // 3) Existing unit item template ids for this player
        var existingUnitTemplateIds = await _db.PlayerItems
            .AsNoTracking()
            .Join(
                _db.ItemTemplates.AsNoTracking(),
                pi => pi.TemplateId,
                t => t.Id,
                (pi, t) => new { pi.PlayerId, pi.TemplateId, t.OwnerKind, t.MercenaryTemplateId, t.MonsterTemplateId })
            .Where(x => x.PlayerId == profile.Id)
            .Where(x =>
                (x.OwnerKind == ItemOwnerKind.Mercenary && x.MercenaryTemplateId != null) ||
                (x.OwnerKind == ItemOwnerKind.Beast && x.MonsterTemplateId != null))
            .Select(x => x.TemplateId)
            .ToHashSetAsync(ct);

        // 4) Add missing unit items only
        foreach (var tpl in mercUnitTpls.Where(t => !existingUnitTemplateIds.Contains(t.Id)))
        {
            _db.PlayerItems.Add(new PlayerItem
            {
                Id = Guid.NewGuid(),
                PlayerId = profile.Id,
                TemplateId = tpl.Id,
                Level = 1,
                Quality = tpl.BaseQuality,
                BonusStats = StatBlock.Zero,
                Wins = 0,
                LinkedMercenaryInstanceId = null
            });
        }

        foreach (var tpl in beastUnitTpls.Where(t => !existingUnitTemplateIds.Contains(t.Id)))
        {
            _db.PlayerItems.Add(new PlayerItem
            {
                Id = Guid.NewGuid(),
                PlayerId = profile.Id,
                TemplateId = tpl.Id,
                Level = 1,
                Quality = tpl.BaseQuality,
                BonusStats = StatBlock.Zero,
                Wins = 0,
                LinkedMonsterInstanceId = null
            });
        }

        await _db.SaveChangesAsync(ct);

        // 5) Starter gear pack: 2 per equip slot
        await EnsureStarterEquipPackAsync(profile, ct);

        // 6) Load slots and unit items fresh
        var mercSlots = await _db.PlayerMercenarySlots
    .Include(s => s.Mercenary)
        .ThenInclude(b => b.Template)
            .Where(s => s.PlayerProfileId == profile.Id)
            .OrderBy(s => s.SlotIndex)
            .ToListAsync(ct);

            var beastSlots = await _db.PlayerBeastSlots
        .Include(s => s.Beast)
            .ThenInclude(b => b.Template)
            .Where(s => s.PlayerProfileId == profile.Id)
            .OrderBy(s => s.SlotIndex)
            .ToListAsync(ct);

        var unitItems = await _db.PlayerItems
    .Include(x => x.Template)
    .Where(pi => pi.PlayerId == profile.Id)
    .ToListAsync(ct);

        var mercUnitItems = unitItems
            .Where(x => x.Template.OwnerKind == ItemOwnerKind.Mercenary && x.Template.MercenaryTemplateId != null)
            .OrderBy(x => x.Template.Code)
            .Select(x => x)
            .Take(5)
            .ToList();

        var beastUnitItems = unitItems
            .Where(x => x.Template.OwnerKind == ItemOwnerKind.Beast && x.Template.MonsterTemplateId != null)
            .OrderBy(x => x.Template.Code)
            .Select(x => x)
            .Take(5)
            .ToList();

        // 7) Auto-equip into empty slots only
        var mercCount = Math.Min(5, Math.Min(mercSlots.Count, mercUnitItems.Count));
        for (int i = 0; i < mercCount; i++)
        {
            if (mercSlots[i].MercenaryInstanceId is null)
            {
                await EnsureMercUnitEquippedAsync(
                    profile.Id,
                    mercSlots[i].Id,
                    mercUnitItems[i].Id,
                    ct);
            }
        }

        var beastCount = Math.Min(5, Math.Min(beastSlots.Count, beastUnitItems.Count));
        for (int i = 0; i < beastCount; i++)
        {
            if (beastSlots[i].BeastInstanceId is null)
            {
                await EnsureBeastUnitEquippedAsync(
                    profile.Id,
                    beastSlots[i].Id,
                    beastUnitItems[i].Id,
                    ct);
            }
        }

        await _db.SaveChangesAsync(ct);

var mercDebug = await _db.PlayerMercenarySlots
    .Where(s => s.PlayerProfileId == profile.Id)
    .Include(s => s.Mercenary)
        .ThenInclude(m => m.Template)
    .OrderBy(s => s.SlotIndex)
    .Select(s => new
    {
        s.SlotIndex,
        InstanceId = s.MercenaryInstanceId,
        MercId = s.Mercenary != null ? s.Mercenary.Id : (Guid?)null,
        TemplateName = s.Mercenary != null && s.Mercenary.Template != null
            ? s.Mercenary.Template.NameEn
            : "NULL"
    })
    .ToListAsync(ct);

_logger.LogWarning("MERC AFTER EQUIP: " +
    string.Join(" | ", mercDebug.Select(x =>
        $"{x.SlotIndex}: fk={(x.InstanceId != null ? "Y" : "N")}, inst={(x.MercId != null ? "OK" : "NULL")}, tpl={x.TemplateName}")));
// =====================
// 🔍 DEBUG VÝPIS BEASTŮ
// =====================
var beastDebug = await _db.PlayerBeastSlots
    .Where(s => s.PlayerProfileId == profile.Id)
    .Include(s => s.Beast)
        .ThenInclude(b => b.Template)
    .OrderBy(s => s.SlotIndex)
    .Select(s => new
    {
        s.SlotIndex,
        InstanceId = s.BeastInstanceId,
        BeastId = s.Beast != null ? s.Beast.Id : (Guid?)null,
        TemplateName = s.Beast != null && s.Beast.Template != null
            ? s.Beast.Template.NameEn
            : "NULL"
    })
    .ToListAsync(ct);

_logger.LogWarning("BEASTS AFTER EQUIP: " +
    string.Join(" | ", beastDebug.Select(x =>
        $"{x.SlotIndex}: fk={(x.InstanceId != null ? "Y" : "N")}, inst={(x.BeastId != null ? "OK" : "NULL")}, tpl={x.TemplateName}")));
        profile.IsInitialized = true;
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation(
            "Player initialized for UserId={UserId}, PlayerId={PlayerId}",
            userId,
            profile.Id);
    }
private async Task EnsureMercUnitEquippedAsync(
    Guid playerId,
    Guid targetSlotId,
    Guid playerItemId,
    CancellationToken ct)
{
    var slot = await _db.PlayerMercenarySlots
        .FirstOrDefaultAsync(s => s.Id == targetSlotId && s.PlayerProfileId == playerId, ct);

    if (slot is null)
        return;

    var item = await _db.PlayerItems
        .Include(i => i.Template)
        .FirstOrDefaultAsync(i => i.Id == playerItemId && i.PlayerId == playerId, ct);

    if (item?.Template is null)
        return;

    if (item.Template.OwnerKind != ItemOwnerKind.Mercenary || item.Template.MercenaryTemplateId is null)
        return;

    PlayerMercenary? merc = null;

    if (item.LinkedMercenaryInstanceId is not null)
    {
        merc = await _db.PlayerMercenaries
            .FirstOrDefaultAsync(m => m.Id == item.LinkedMercenaryInstanceId.Value && m.PlayerId == playerId, ct);
    }

    if (merc is null)
    {
        merc = new PlayerMercenary
        {
            Id = Guid.NewGuid(),
            PlayerId = playerId,
            TemplateId = item.Template.MercenaryTemplateId.Value,
            Level = item.Level
        };

        merc.Equipment = BuildMercEquipment(merc.Id);

        _db.PlayerMercenaries.Add(merc);
        item.LinkedMercenaryInstanceId = merc.Id;
    }
    else
    {
        merc.Level = item.Level;
    }

    var mercId = merc.Id;

    var otherSlots = await _db.PlayerMercenarySlots
        .Where(s => s.PlayerProfileId == playerId)
        .ToListAsync(ct);

    foreach (var s in otherSlots)
    {
        if (s.Id != slot.Id && s.MercenaryInstanceId == mercId)
            s.MercenaryInstanceId = null;
    }

    slot.MercenaryInstanceId = mercId;
}

    private async Task EnsureBeastUnitEquippedAsync(
    Guid playerId,
    Guid targetSlotId,
    Guid playerItemId,
    CancellationToken ct)
{
    var slot = await _db.PlayerBeastSlots
        .FirstOrDefaultAsync(s => s.Id == targetSlotId && s.PlayerProfileId == playerId, ct);

    if (slot is null)
        return;

    var item = await _db.PlayerItems
        .Include(i => i.Template)
        .FirstOrDefaultAsync(i => i.Id == playerItemId && i.PlayerId == playerId, ct);

    if (item?.Template is null)
        return;

    if (item.Template.OwnerKind != ItemOwnerKind.Beast || item.Template.MonsterTemplateId is null)
        return;

    PlayerMonster? beast = null;

    if (item.LinkedMonsterInstanceId is not null)
    {
        beast = await _db.PlayerMonsters
            .FirstOrDefaultAsync(b => b.Id == item.LinkedMonsterInstanceId.Value && b.PlayerId == playerId, ct);
    }

    if (beast is null)
    {
        beast = new PlayerMonster
        {
            Id = Guid.NewGuid(),
            PlayerId = playerId,
            TemplateId = item.Template.MonsterTemplateId.Value,
            Level = item.Level
        };

        beast.Equipment = BuildBeastEquipment(beast.Id);

        _db.PlayerMonsters.Add(beast);
        item.LinkedMonsterInstanceId = beast.Id;
    }
    else
    {
        beast.Level = item.Level;
    }

    var beastId = beast.Id;

    var otherSlots = await _db.PlayerBeastSlots
        .Where(s => s.PlayerProfileId == playerId)
        .ToListAsync(ct);

    foreach (var s in otherSlots)
    {
        if (s.Id != slot.Id && s.BeastInstanceId == beastId)
            s.BeastInstanceId = null;
    }

    slot.BeastInstanceId = beastId;
}

    /// <summary>
    /// Gives the player TWO items for each equip slot (merc and beast),
    /// if missing.
    /// </summary>
    private async Task EnsureStarterEquipPackAsync(PlayerProfile profile, CancellationToken ct)
    {
        var owned = await (
            from pi in _db.PlayerItems.AsNoTracking()
            join t in _db.ItemTemplates.AsNoTracking() on pi.TemplateId equals t.Id
            where pi.PlayerId == profile.Id
            select new
            {
                pi.TemplateId,
                Slot = (ItemEquipSlot?)(t.MercenarySlot ?? t.MonsterSlot)
            }
        ).ToListAsync(ct);

        var ownedTemplateIds = owned.Select(x => x.TemplateId).ToHashSet();

        var ownedCountBySlot = owned
            .Where(x => x.Slot.HasValue && x.Slot.Value != ItemEquipSlot.None)
            .GroupBy(x => x.Slot!.Value)
            .ToDictionary(g => g.Key, g => g.Count());

        var templates = await _db.ItemTemplates
            .AsNoTracking()
            .Where(t => t.MercenarySlot != null || t.MonsterSlot != null)
            .Select(t => new
            {
                t.Id,
                t.Code,
                t.OwnerKind,
                t.BaseQuality,
                Slot = (ItemEquipSlot?)(t.MercenarySlot ?? t.MonsterSlot)
            })
            .ToListAsync(ct);

        var templatesBySlot = templates
            .Where(t => t.Slot.HasValue && t.Slot.Value != ItemEquipSlot.None)
            .GroupBy(t => t.Slot!.Value)
            .ToDictionary(g => g.Key, g => g.OrderBy(x => x.Code).ToList());

        var allSlots = Enum.GetValues<ItemEquipSlot>()
            .Where(s => s != ItemEquipSlot.None)
            .OrderBy(s => (int)s)
            .ToList();

        foreach (var slot in allSlots)
        {
            var have = ownedCountBySlot.TryGetValue(slot, out var cnt) ? cnt : 0;
            var need = 2 - have;
            if (need <= 0)
                continue;

            if (!templatesBySlot.TryGetValue(slot, out var candidates) || candidates.Count == 0)
                continue;

            var expectedOwner = slot.IsMercenarySlot()
                ? ItemOwnerKind.Mercenary
                : ItemOwnerKind.Beast;

            var pool = candidates
                .Where(t => t.OwnerKind == expectedOwner)
                .ToList();

            if (pool.Count == 0)
                continue;

            var uniquePicks = pool
                .Where(t => !ownedTemplateIds.Contains(t.Id))
                .Take(need)
                .ToList();

            foreach (var tpl in uniquePicks)
            {
                AddStarterItem(profile.Id, tpl.Id, tpl.BaseQuality, slot);
                ownedTemplateIds.Add(tpl.Id);
                have++;
                need--;
                ownedCountBySlot[slot] = have;
                if (need == 0)
                    break;
            }

            if (need > 0)
            {
                foreach (var tpl in pool.Take(need))
                {
                    AddStarterItem(profile.Id, tpl.Id, tpl.BaseQuality, slot);
                    have++;
                    ownedCountBySlot[slot] = have;
                }
            }
        }

        await _db.SaveChangesAsync(ct);
    }

    private void AddStarterItem(Guid playerId, Guid templateId, QualityTier quality, ItemEquipSlot slot)
    {
        try
        {
            _db.PlayerItems.Add(new PlayerItem
            {
                Id = Guid.NewGuid(),
                PlayerId = playerId,
                TemplateId = templateId,
                Level = 1,
                Quality = quality,
                BonusStats = StatBlock.Zero,
                Wins = 0
            });
        }
        catch (Exception ex)
        {
            _errors.Capture(ex, "Failed to add starter PlayerItem (2-per-slot)",
                new
                {
                    PlayerId = playerId,
                    Slot = slot.ToString(),
                    SlotValue = (int)slot,
                    TemplateId = templateId
                });
        }
    }

    /// <summary>
    /// DEV helper to fully reset a player profile and re-run onboarding.
    /// </summary>
    public async Task RecreateProfileAsync(string userId, CancellationToken ct = default)
    {
        var profile = await _db.Players
            .Include(p => p.Inventory)
            .Include(p => p.MercenarySlots)
            .Include(p => p.BeastSlots)
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (profile is not null)
        {
            var items = await _db.PlayerItems
                .Include(x => x.Template)
                .Where(x => x.PlayerId == profile.Id)
                .Where(pi => pi.Template.MercenarySlot != null || pi.Template.MonsterSlot != null)
                .ToListAsync(ct);

            _db.PlayerItems.RemoveRange(items);

            var mercIds = await _db.PlayerMercenaries
                .Where(m => m.PlayerId == profile.Id)
                .Select(m => m.Id)
                .ToListAsync(ct);

            if (mercIds.Count > 0)
            {
                var mercEq = await _db.MercenaryEquipmentSlots
                    .Where(e => mercIds.Contains(e.MercenaryInstanceId))
                    .ToListAsync(ct);
                _db.MercenaryEquipmentSlots.RemoveRange(mercEq);

                var mercs = await _db.PlayerMercenaries
                    .Where(m => mercIds.Contains(m.Id))
                    .ToListAsync(ct);
                _db.PlayerMercenaries.RemoveRange(mercs);
            }

            var beastIds = await _db.PlayerMonsters
                .Where(b => b.PlayerId == profile.Id)
                .Select(b => b.Id)
                .ToListAsync(ct);

            if (beastIds.Count > 0)
            {
                var beastEq = await _db.BeastEquipmentSlots
                    .Where(e => beastIds.Contains(e.BeastInstanceId))
                    .ToListAsync(ct);
                _db.BeastEquipmentSlots.RemoveRange(beastEq);

                var beasts = await _db.PlayerMonsters
                    .Where(b => beastIds.Contains(b.Id))
                    .ToListAsync(ct);
                _db.PlayerMonsters.RemoveRange(beasts);
            }

            var mercSlots = await _db.PlayerMercenarySlots
                .Where(s => s.PlayerProfileId == profile.Id)
                .ToListAsync(ct);
            _db.PlayerMercenarySlots.RemoveRange(mercSlots);

            var beastSlots = await _db.PlayerBeastSlots
                .Where(s => s.PlayerProfileId == profile.Id)
                .ToListAsync(ct);
            _db.PlayerBeastSlots.RemoveRange(beastSlots);

            var expProg = await _db.PlayerExpeditionProgresses.Where(x => x.PlayerId == profile.Id).ToListAsync(ct);
            _db.PlayerExpeditionProgresses.RemoveRange(expProg);

            var dunProg = await _db.PlayerDungeonProgresses.Where(x => x.PlayerId == profile.Id).ToListAsync(ct);
            _db.PlayerDungeonProgresses.RemoveRange(dunProg);

            var expAch = await _db.PlayerExpeditionAchievementSummaries.Where(x => x.PlayerId == profile.Id).ToListAsync(ct);
            _db.PlayerExpeditionAchievementSummaries.RemoveRange(expAch);

            var dunAch = await _db.PlayerDungeonAchievements.Where(x => x.PlayerId == profile.Id).ToListAsync(ct);
            _db.PlayerDungeonAchievements.RemoveRange(dunAch);

            _db.Players.Remove(profile);

            await _db.SaveChangesAsync(ct);
        }

        await EnsurePlayerInitializedAsync(userId, ct);
    }

    private static List<MercenaryEquipmentSlot> BuildMercEquipment(Guid mercId)
    {
        var mercSlots = Enum.GetValues<ItemEquipSlot>()
            .Where(s => s.IsMercenarySlot() && s != ItemEquipSlot.None)
            .OrderBy(s => (int)s)
            .ToList();

        return mercSlots.Select(s => new MercenaryEquipmentSlot
        {
            Id = Guid.NewGuid(),
            MercenaryInstanceId = mercId,
            Slot = s,
            PlayerItemId = null
        }).ToList();
    }

    private static List<BeastEquipmentSlot> BuildBeastEquipment(Guid beastId)
    {
        var beastSlots = Enum.GetValues<ItemEquipSlot>()
            .Where(s => s.IsBeastSlot() && s != ItemEquipSlot.None)
            .OrderBy(s => (int)s)
            .ToList();

        return beastSlots.Select(s => new BeastEquipmentSlot
        {
            Id = Guid.NewGuid(),
            BeastInstanceId = beastId,
            Slot = s,
            PlayerItemId = null
        }).ToList();
    }
}