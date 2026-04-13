using Microsoft.EntityFrameworkCore;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain.Players;
using MercenariesAndBeasts.Domain.Interface;
using MercenariesAndBeasts.Domain.Utils; // Plus/AddInPlace
using MercenariesAndBeasts.Infrastructure;

public sealed class StatAggregator : IStatAggregator
{
    private readonly GameDbContext _db;

    public StatAggregator(GameDbContext db) => _db = db;

    public async Task<UnitSnapshot> BuildMercenaryAsync(Guid playerMercenaryId, CancellationToken ct = default)
    {
        var merc = await _db.PlayerMercenaries
            .Include(m => m.Template)
            .Include(m => m.Equipment)
                .ThenInclude(es => es.PlayerItem)
                    .ThenInclude(pi => pi!.Template)
            .FirstAsync(m => m.Id == playerMercenaryId, ct);

        // 1) base (template)
        var stats = merc.Template.BaseStats.Clone();

        // 2) level scaling (unit level)
        ApplyLevelScaling(stats, merc.Level);

        // 3) equipped items
        var equipped = new List<EquippedItemSnapshot>();

        foreach (var eqSlot in merc.Equipment)
        {
            if (eqSlot.PlayerItem is null) continue;

            var pi = eqSlot.PlayerItem;
            var tpl = pi.Template;

            // Merc gear musí mít MercenarySlot
            if (!tpl.MercenarySlot.HasValue) continue;

            // ✅ POZOR: CalculateStats() už bonusy často přičítá samo (podle tvé implementace)
            // => nezdvojuj BonusStats
            var itemStats = tpl.CalculateStats(pi);

            stats.AddInPlace(itemStats);

            equipped.Add(new EquippedItemSnapshot(
                pi.Id,
                tpl.Code,
                tpl.NameEn,
                tpl.MercenarySlot.Value,
                pi.Quality,
                itemStats
            ));
        }

        NormalizeAndClamp(stats);

        return new UnitSnapshot(
            merc.Id,
            merc.Template.NameEn,
            merc.Level,
            IsMercenary: true,
            Stats: stats,
            EquippedItems: equipped
        );
    }

    public async Task<UnitSnapshot> BuildBeastAsync(Guid playerMonsterId, CancellationToken ct = default)
    {
        var beast = await _db.PlayerMonsters
            .Include(b => b.Template)
            .Include(b => b.Equipment)
                .ThenInclude(es => es.PlayerItem)
                    .ThenInclude(pi => pi!.Template)
            .FirstAsync(b => b.Id == playerMonsterId, ct);

        var stats = beast.Template.BaseStats.Clone();

        ApplyLevelScaling(stats, beast.Level);

        var equipped = new List<EquippedItemSnapshot>();

        foreach (var eqSlot in beast.Equipment)
        {
            if (eqSlot.PlayerItem is null) continue;

            var pi = eqSlot.PlayerItem;
            var tpl = pi.Template;

            // Beast gear musí mít MonsterSlot
            if (!tpl.MonsterSlot.HasValue) continue;

            // ✅ bez dvojitého BonusStats
            var itemStats = tpl.CalculateStats(pi);

            stats.AddInPlace(itemStats);

            equipped.Add(new EquippedItemSnapshot(
                pi.Id,
                tpl.Code,
                tpl.NameEn,
                tpl.MonsterSlot.Value,
                pi.Quality,
                itemStats
            ));
        }

        NormalizeAndClamp(stats);

        return new UnitSnapshot(
            beast.Id,
            beast.Template.NameEn,
            beast.Level,
            IsMercenary: false,
            Stats: stats,
            EquippedItems: equipped
        );
    }

    // ---------- helpers ----------

    private static void ApplyLevelScaling(StatBlock s, int level)
    {
        var lvl = Math.Max(1, level);

        // baseline: +3% core per level (HP/ATK/DEF/Armor)
        var kCore = 1.0 + 0.03 * (lvl - 1);

        s.MaxHp   = (float)(s.MaxHp   * kCore);
        s.Attack  = (float)(s.Attack  * kCore);
        s.Defense = (float)(s.Defense * kCore);

        // speed jemněji
        s.Speed = (float)(s.Speed * (1.0 + 0.01 * (lvl - 1)));
    }

    private static void NormalizeAndClamp(StatBlock s)
    {
        // chances / reductions / bonuses drž v 0..1
        s.CriticalChance = Clamp01(s.CriticalChance);
        s.ArmorPenetration = Clamp01(s.ArmorPenetration);
        s.BlockChance = Clamp01(s.BlockChance);

        s.DamageReduction = Clamp01(s.DamageReduction);
        s.DamageBonus = Clamp01NonNegative(s.DamageBonus);
        s.TrueDamageBonus = Clamp01NonNegative(s.TrueDamageBonus);

        s.TurnMeterGain = Clamp01NonNegative(s.TurnMeterGain);

        s.LifeSteal = Clamp01NonNegative(s.LifeSteal);
        s.HpRegen = Clamp01NonNegative(s.HpRegen);
        s.EnergyRegen = Clamp01NonNegative(s.EnergyRegen);

        s.BleedChance = Clamp01(s.BleedChance);
        s.PoisonChance = Clamp01(s.PoisonChance);
        s.BurnChance = Clamp01(s.BurnChance);
        s.ShockChance = Clamp01(s.ShockChance);
        s.FreezeChance = Clamp01(s.FreezeChance);

        s.StatusDurationBonus = Clamp01NonNegative(s.StatusDurationBonus);
        s.StatusResistance = Clamp01(s.StatusResistance);
        s.DotDamageBonus = Clamp01NonNegative(s.DotDamageBonus);
        s.DotDamageReduction = Clamp01(s.DotDamageReduction);
        s.CleanseChance = Clamp01(s.CleanseChance);

        // crit mult min 1
        s.CriticalMultiplier = Math.Max(1.0, s.CriticalMultiplier);

        // accuracy může být > 1 (pokud chceš), ale nesmí být nesmysl
        s.Accuracy = Math.Max(0.1, s.Accuracy);
        s.Evasion = Clamp01NonNegative(s.Evasion); // pokud u tebe evasion je chance
    }

    private static double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);
    private static double Clamp01NonNegative(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);
}