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
/// ✅ New rules applied:
/// - NO contracts / eggs
/// - NO LinkedMonsterInstanceId / LinkedMercenaryInstanceId
/// - PlayerItem always references ItemTemplate (TemplateId)
/// - ItemTemplate uses:
///     - OwnerKind (Mercenary / Beast)
///     - EquipSlot (ItemEquipSlot, 1..99 merc, 100..199 beast)
/// - Starter inventory gives: 1× item for EACH EquipSlot per owner kind (if templates exist)
/// - Party slots equip UNIT instances (merc/beast) same as before
/// - Equipment slots on units are created empty (player decides what to equip)
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
        // 1) Load profile + slots + inventory
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

        // 2) Create party slots (5+5) if missing
        if (profile.MercenarySlots.Count == 0)
        {
            for (int i = 1; i <= 5; i++)
                _db.PlayerMercenarySlots.Add(new PlayerMercenarySlot
                {
                    PlayerProfileId = profile.Id,
                    SlotIndex = i
                });
        }

        if (profile.BeastSlots.Count == 0)
        {
            for (int i = 1; i <= 5; i++)
                _db.PlayerBeastSlots.Add(new PlayerBeastSlot
                {
                    PlayerProfileId = profile.Id,
                    SlotIndex = i
                });
        }

        await _db.SaveChangesAsync(ct);

        // 3) Pick 2 starter templates (merc + beast)
        //    (Later you can add IsStarter flag. For now: "first two")
        var mercTemplates = await _db.MercenaryTemplates
            .OrderBy(t => t.BaseLevel).ThenBy(t => t.Code)
            .Take(2)
            .ToListAsync(ct);

        var beastTemplates = await _db.MonsterTemplates
            .OrderBy(t => t.BaseLevel).ThenBy(t => t.Code)
            .Take(2)
            .ToListAsync(ct);

        if (mercTemplates.Count < 2 || beastTemplates.Count < 2)
            throw new InvalidOperationException("Missing starter templates (need at least 2 mercenary templates and 2 monster templates).");

        // 4) Create 2 + 2 unit instances (lvl 1) with empty equipment slots
        //    NOTE: These builders assume your equipment-slot entities use ItemEquipSlot.
        var pm1 = new PlayerMercenary { Id = Guid.NewGuid(), PlayerId = profile.Id, TemplateId = mercTemplates[0].Id, Level = 1 };
        pm1.Equipment = BuildMercEquipment(pm1.Id);

        var pm2 = new PlayerMercenary { Id = Guid.NewGuid(), PlayerId = profile.Id, TemplateId = mercTemplates[1].Id, Level = 1 };
        pm2.Equipment = BuildMercEquipment(pm2.Id);

        _db.PlayerMercenaries.AddRange(pm1, pm2);

        var bm1 = new PlayerMonster { Id = Guid.NewGuid(), PlayerId = profile.Id, TemplateId = beastTemplates[0].Id, Level = 1 };
        bm1.Equipment = BuildBeastEquipment(bm1.Id);

        var bm2 = new PlayerMonster { Id = Guid.NewGuid(), PlayerId = profile.Id, TemplateId = beastTemplates[1].Id, Level = 1 };
        bm2.Equipment = BuildBeastEquipment(bm2.Id);

        _db.PlayerMonsters.AddRange(bm1, bm2);

        await _db.SaveChangesAsync(ct);

        // 5) Assign units to party slots (1 and 2)
        var mercSlots = await _db.PlayerMercenarySlots
            .Where(s => s.PlayerProfileId == profile.Id)
            .OrderBy(s => s.SlotIndex)
            .ToListAsync(ct);

        var beastSlots = await _db.PlayerBeastSlots
            .Where(s => s.PlayerProfileId == profile.Id)
            .OrderBy(s => s.SlotIndex)
            .ToListAsync(ct);

        mercSlots[0].MercenaryInstanceId = pm1.Id;
        mercSlots[1].MercenaryInstanceId = pm2.Id;

        beastSlots[0].BeastInstanceId = bm1.Id;
        beastSlots[1].BeastInstanceId = bm2.Id;

        var beastUnitTpls = await _db.ItemTemplates
            .AsNoTracking()
            .Where(t => t.OwnerKind == ItemOwnerKind.Beast)
            .Where(t => t.MonsterTemplateId != null)     // ✅ unit templates
            .Where(t => t.MercenarySlot == null && t.MonsterSlot != null)
            .OrderBy(t => Guid.NewGuid())
            .Take(2)
            .ToListAsync(ct);

        foreach (var tpl in beastUnitTpls)
        {
            _db.PlayerItems.Add(new PlayerItem
            {
                Id = Guid.NewGuid(),
                PlayerId = profile.Id,
                TemplateId = tpl.Id,
                Level = 1,
                Quality = tpl.BaseQuality,
                BonusStats = StatBlock.Zero,
                Wins = 0
            });
        }

        var orderUnitTpls = await _db.ItemTemplates
            .AsNoTracking()
            .Where(t => t.OwnerKind == ItemOwnerKind.Mercenary)
            .Where(t => t.MercenaryTemplateId != null)     // ✅ unit templates
            .Where(t => t.MercenarySlot != null && t.MonsterSlot == null)
            .OrderBy(t => Guid.NewGuid())
            .Take(2)
            .ToListAsync(ct);

        foreach (var tpl in orderUnitTpls)
        {
            _db.PlayerItems.Add(new PlayerItem
            {
                Id = Guid.NewGuid(),
                PlayerId = profile.Id,
                TemplateId = tpl.Id,
                Level = 1,
                Quality = tpl.BaseQuality,
                BonusStats = StatBlock.Zero,
                Wins = 0
            });
        }
        await _db.SaveChangesAsync(ct);

        // 6) Starter inventory: 1× per equip slot (merc + beast)
        await EnsureStarterEquipPackAsync(profile, ct);

        profile.IsInitialized = true;
        await _db.SaveChangesAsync(ct);

        _logger.LogInformation("Player initialized for UserId={UserId}, PlayerId={PlayerId}", userId, profile.Id);
    }

    /// <summary>
    /// Gives the player ONE item for each equip slot (for merc and for beast),
    /// based on ItemTemplate.EquipSlot.
    ///
    /// Idempotent: if player already owns ANY item with that EquipSlot, it skips it.
    ///
    /// This assumes your GameSeed created ItemTemplates with:
    /// - EquipSlot filled
    /// - OwnerKind filled
    /// </summary>
   private async Task EnsureStarterEquipPackAsync(PlayerProfile profile, CancellationToken ct)
{
    // 0) co už hráč má (sloty + templateIds)
    var owned = await _db.PlayerItems
        .AsNoTracking()
        .Where(pi => pi.PlayerId == profile.Id)
        .Select(pi => new
        {
            pi.TemplateId,
            Merc = pi.Template.MercenarySlot,
            Beast = pi.Template.MonsterSlot
        })
        .ToListAsync(ct);

    var ownedTemplateIds = owned.Select(x => x.TemplateId).ToHashSet();

    // slot -> kolik itemů pro ten slot už hráč má
    var ownedCountBySlot = owned
        .SelectMany(x => new ItemEquipSlot?[] { x.Merc, x.Beast })
        .Where(s => s.HasValue && s.Value != ItemEquipSlot.None)
        .Select(s => s!.Value)
        .GroupBy(s => s)
        .ToDictionary(g => g.Key, g => g.Count());

    // 1) načti všechny item templates, co mají slot
    //    a připrav per-slot seznam kandidatů (deterministicky podle Code)
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

    // 2) pro každý slot: zajisti aspoň 2 kusy
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

        // vyber max 2 kandidáty, kteří ještě nejsou v inventory (bez duplicit TemplateId)
        // a sedí owner na slot (merc/beast)
        var expectedOwner = slot.IsMercenarySlot() ? ItemOwnerKind.Mercenary : ItemOwnerKind.Beast;

        var picks = candidates
            .Where(t => t.OwnerKind == expectedOwner)
            .Where(t => !ownedTemplateIds.Contains(t.Id))
            .Take(need)
            .ToList();

        foreach (var tpl in picks)
        {
            try
            {
                _db.PlayerItems.Add(new PlayerItem
                {
                    Id = Guid.NewGuid(),
                    PlayerId = profile.Id,
                    TemplateId = tpl.Id,
                    Level = 1,
                    Quality = tpl.BaseQuality,
                    BonusStats = StatBlock.Zero
                });

                ownedTemplateIds.Add(tpl.Id);

                if (ownedCountBySlot.ContainsKey(slot))
                    ownedCountBySlot[slot]++;
                else
                    ownedCountBySlot[slot] = 1;
            }
            catch (Exception ex)
            {
                _errors.Capture(ex, "Failed to add starter PlayerItem (2-per-slot)",
                    new
                    {
                        PlayerId = profile.Id,
                        Slot = slot.ToString(),
                        SlotValue = (int)slot,
                        TemplateId = tpl.Id,
                        TemplateCode = tpl.Code,
                        TemplateOwner = tpl.OwnerKind.ToString()
                    });
            }
        }
    }

    await _db.SaveChangesAsync(ct);
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
            // 1) PlayerItems
            var items = await _db.PlayerItems
            .Include(x => x.Template)
                .Where(x => x.PlayerId == profile.Id)
            .Where(pi => pi.Template.MercenarySlot != null || pi.Template.MonsterSlot != null)
                .ToListAsync(ct);
            _db.PlayerItems.RemoveRange(items);

            // 2) Mercs + equipment slots
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

            // 3) Beasts + equipment slots
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

            // 4) Party slots
            var mercSlots = await _db.PlayerMercenarySlots
                .Where(s => s.PlayerProfileId == profile.Id)
                .ToListAsync(ct);
            _db.PlayerMercenarySlots.RemoveRange(mercSlots);

            var beastSlots = await _db.PlayerBeastSlots
                .Where(s => s.PlayerProfileId == profile.Id)
                .ToListAsync(ct);
            _db.PlayerBeastSlots.RemoveRange(beastSlots);

            // 5) Progress / achievements (optional)
            var expProg = await _db.PlayerExpeditionProgresses.Where(x => x.PlayerId == profile.Id).ToListAsync(ct);
            _db.PlayerExpeditionProgresses.RemoveRange(expProg);

            var dunProg = await _db.PlayerDungeonProgresses.Where(x => x.PlayerId == profile.Id).ToListAsync(ct);
            _db.PlayerDungeonProgresses.RemoveRange(dunProg);

            var expAch = await _db.PlayerExpeditionAchievementSummaries.Where(x => x.PlayerId == profile.Id).ToListAsync(ct);
            _db.PlayerExpeditionAchievementSummaries.RemoveRange(expAch);

            var dunAch = await _db.PlayerDungeonAchievements.Where(x => x.PlayerId == profile.Id).ToListAsync(ct);
            _db.PlayerDungeonAchievements.RemoveRange(dunAch);

            // 6) Profile
            _db.Players.Remove(profile);

            await _db.SaveChangesAsync(ct);
        }

        await EnsurePlayerInitializedAsync(userId, ct);
    }

    // ==========================================================
    // Equipment slot builders
    // ==========================================================
    //
    // These assume:
    // - MercenaryEquipmentSlot.Slot is ItemEquipSlot (Merc_*)
    // - BeastEquipmentSlot.Slot is ItemEquipSlot (Beast_*)
    //
    // If your entities still use MercenaryItemSlot / MonsterItemSlot,
    // you MUST migrate them to ItemEquipSlot (recommended),
    // otherwise the whole "unified slot enum" concept is broken.
    //

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