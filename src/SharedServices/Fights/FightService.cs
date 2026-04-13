using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MercenariesAndBeasts.Domain.Combat;

namespace MercenariesAndBeasts.Infrastructure.Fights;

public sealed class FightService : IFightService
{
    // Hard safety caps (aby se to nikdy nezacyklilo)
    private const int MaxActions = 300;
    private const double MinSpeed = 0.01;
    private const double EpsilonHp = 0.0001;

    /// <summary>
    /// Provede simulaci boje v dungenu a vrátí výsledek (výhra/prohra, log akcí, loot).
    /// Simulace je deterministická — stejný <see cref="FightRequest.Seed"/> vždy vrátí stejný výsledek.
    /// </summary>
    /// <param name="req">Parametry boje: hráčovy jednotky, nepřítel, stage a seed pro RNG.</param>
    /// <param name="ct">Cancellation token.</param>
    /// <returns>
    /// <see cref="FightResult"/> s příznakem vítězství, textovým shrnutím, logem a lootem.
    /// </returns>
    public Task<FightResult> ResolveDungeonFightAsync(FightRequest req, CancellationToken ct)
    {
        if (req is null) throw new ArgumentNullException(nameof(req));
        if (req.PlayerUnits is null || req.PlayerUnits.Count == 0)
            return Task.FromResult(new FightResult { DidWin = false, Summary = "No player units." });

        if (req.Enemy is null)
            return Task.FromResult(new FightResult { DidWin = false, Summary = "No enemy." });

        // Make combatants (copy stats so service is pure)
        var rng = new Random(req.Seed);

        var players = req.PlayerUnits
            .Select(u => new Combatant(u.Id, u.Name, side: Side.Player, u.Stats))
            .ToList();

        var enemy = new Combatant(req.Enemy.Id, req.Enemy.Name, side: Side.Enemy, req.Enemy.Stats);

        // Initialize derived pools
        foreach (var p in players) p.Initialize();
        enemy.Initialize();

        var all = new List<Combatant>(players.Count + 1);
        all.AddRange(players);
        all.Add(enemy);

        // Build scheduler: lower NextActAt acts first
        var queue = all.Select(c => new TimelineSlot(c)).ToList();

        var log = new List<string>(256);
        log.Add($"Seed={req.Seed} | Stage={req.Stage} | Party={players.Count} vs 1");

        int actions = 0;

        while (actions < MaxActions)
        {
            ct.ThrowIfCancellationRequested();

            // stop if someone dead
            bool playersAlive = players.Any(p => p.IsAlive);
            bool enemyAlive = enemy.IsAlive;

            if (!playersAlive || !enemyAlive)
                break;

            // pick next
            var slot = queue.OrderBy(s => s.NextActAt).First();
            var actor = slot.Unit;

            // advance "time" (not needed, but nice for logs)
            var now = slot.NextActAt;

            // schedule next action of the same actor
            // higher speed => smaller interval => more frequent actions
            var interval = SpeedInterval(actor.Stats.Speed, actor.Stats.TurnMeterGain, actor.Stats.EnergyCostReduction);
            slot.NextActAt += interval;

            // skip dead actor
            if (!actor.IsAlive)
                continue;

            // regen tick (uses HpRegen/EnergyRegen; + HealingBonus as amplifier)
            actor.TickRegen(log);

            // DOT tick (uses DotDamageBonus/DotDamageReduction/StatusDurationBonus)
            actor.TickDots(log);

            // pick target
            Combatant? target =
                actor.Side == Side.Player
                    ? enemy
                    : players.Where(p => p.IsAlive).OrderBy(_ => rng.Next()).FirstOrDefault();

            if (target is null || !target.IsAlive)
                continue;

            // attempt attack
            var attackResult = ResolveAttack(rng, actor, target);

            // log
            log.Add($"t={now:0.00} | {attackResult}");

            // counter (CounterChance) only if target alive and attack was a hit (not miss)
            if (attackResult.DidHit && target.IsAlive)
            {
                var counterRoll = rng.NextDouble();
                var counterChance = Clamp01(target.Stats.CounterChance);
                if (counterRoll < counterChance)
                {
                    var counter = ResolveAttack(rng, target, actor, isCounter: true);
                    log.Add($"t={now:0.00} | {counter}");
                }
            }

            actions++;
        }

        bool didWin = enemy.IsAlive == false && players.Any(p => p.IsAlive);
        var summary = didWin
            ? $"Victory. Actions={actions}. Enemy defeated."
            : $"Defeat. Actions={actions}. Party wiped or timeout.";

        var loot = BuildLootLines(req, didWin);

        return Task.FromResult(new FightResult
        {
            DidWin = didWin,
            Summary = summary,
            LogLines = log.Take(200).ToList(),
            LootLines = loot
        });
    }

    // ---------------------------
    // Attack resolution (uses StatBlock)
    // ---------------------------

    /// <summary>
    /// Vyřeší jeden útok od <paramref name="a"/> na <paramref name="t"/>.
    /// Zahrnuje kontrolu zásahu, bloku, critu, armor penetration, lifesteal, trny a aplikaci statusů.
    /// </summary>
    private static AttackOutcome ResolveAttack(Random rng, Combatant a, Combatant t, bool isCounter = false)
    {
        // HIT check: Accuracy vs Evasion
        // Accuracy baseline is 1.0 in your Zero preset; Evasion is 0..1
        var acc = Math.Max(0, a.Stats.Accuracy);
        var ev = Clamp01(t.Stats.Evasion);

        // A simple curve: baseHit = 0.75, +0.2*(acc-1), -0.4*evasion
        var hitChance = Clamp01(0.75 + 0.20 * (acc - 1.0) - 0.40 * ev);

        bool didHit = rng.NextDouble() < hitChance;
        if (!didHit)
        {
            return AttackOutcome.Miss(a, t, isCounter, hitChance);
        }

        // BLOCK check (reduces damage)
        var blockChance = Clamp01(t.Stats.BlockChance);
        bool didBlock = rng.NextDouble() < blockChance;

        // CRIT
        var critChance = Clamp01(a.Stats.CriticalChance);
        bool didCrit = rng.NextDouble() < critChance;

        // crit multiplier (>=1.0)
        var critMult = Math.Max(1.0, a.Stats.CriticalMultiplier);

        // Base damage from Attack vs Defense + Armor; includes ArmorPenetration
        // Use flat stats: Attack/Defense/Armor are floats
        var atk = Math.Max(0f, a.Stats.Attack);
        var def = t.Stats.Defense;
        var armor = t.Stats.Armor;

        // Penetration reduces armor+def (0..1)
        var pen = Clamp01(a.Stats.ArmorPenetration);
        var effArmor = (double)Math.Max(0f, armor * (float)(1.0 - pen));
        var effDef = (double)(def * (1.0 - 0.5 * pen)); // partial def pen

        // raw damage (simple; tune as needed)
        double raw = Math.Max(0.0, atk * 1.0 - effDef * 0.6 - effArmor * 0.4);

        // apply DamageBonus / TrueDamageBonus (0..1)
        var dmgBonus = Clamp01(a.Stats.DamageBonus);
        var trueBonus = Clamp01(a.Stats.TrueDamageBonus);

        // true part ignores reduction below
        double truePart = raw * trueBonus;
        double normalPart = raw * (1.0 - trueBonus);

        // apply crit to both parts
        if (didCrit)
        {
            truePart *= critMult;
            normalPart *= critMult;
        }

        // target damage reduction (0..1)
        var dr = Clamp01(t.Stats.DamageReduction);
        normalPart *= (1.0 - dr);

        // thorns reflect (0..1)
        var thorns = Clamp01(t.Stats.Thorns);

        // block reduces final damage
        if (didBlock)
        {
            // Block reduces normal more than true
            normalPart *= 0.55;
            truePart *= 0.80;
        }

        // attacker outgoing modifier (DamageBonus)
        normalPart *= (1.0 + dmgBonus);
        truePart *= (1.0 + dmgBonus * 0.5);

        // total
        double dealt = Math.Max(0.0, normalPart + truePart);

        // apply to HP
        t.Hp -= dealt;

        // lifesteal (0..1) * HealingBonus (0..1)
        var ls = Clamp01(a.Stats.LifeSteal);
        var healAmp = 1.0 + Clamp01(a.Stats.HealingBonus);
        var antiHeal = Clamp01(a.Stats.HealingReduction); // attacker self-anti-heal (tradeoff) if any
        var lifeStealHeal = dealt * ls * healAmp * (1.0 - antiHeal);
        if (lifeStealHeal > 0)
            a.Hp = Math.Min(a.MaxHp, a.Hp + lifeStealHeal);

        // thorns reflect to attacker
        var reflected = dealt * thorns;
        if (reflected > 0)
            a.Hp -= reflected;

        // Apply status procs:
        // Uses Bleed/Poison/Burn/Shock/Freeze chances, StatusPotency, StatusResistance, CleanseChance
        // Uses StatusDurationBonus and DotDamageBonus/DotDamageReduction later in ticks.
        TryApplyStatuses(rng, a, t);

        // Cleanse tick on target after being hit (small chance)
        TryCleanse(rng, t);

        // Clamp death
        if (t.Hp < 0) t.Hp = 0;
        if (a.Hp < 0) a.Hp = 0;

        return AttackOutcome.Hit(a, t, dealt, didCrit, didBlock, reflected, lifeStealHeal, isCounter, hitChance);
    }

    /// <summary>
    /// Zkusí aplikovat DOT statusy (Bleed, Poison, Burn, Shock, Freeze) na cíl <paramref name="t"/>
    /// po útoku útočníka <paramref name="a"/>. Šance je modulována Potency a StatusResistance.
    /// </summary>
    private static void TryApplyStatuses(Random rng, Combatant a, Combatant t)
    {
        // Potency pushes procs up, resistance pushes down
        var potency = Clamp01(a.Stats.StatusPotency);
        var resist = Clamp01(t.Stats.StatusResistance);

        double resistFactor = Clamp01(1.0 - resist + 0.35 * potency);

        // duration baseline = 2 turns; scaled by StatusDurationBonus
        int baseDur = 2;
        var durBonus = Clamp01(a.Stats.StatusDurationBonus);
        int dur = Math.Clamp(baseDur + (int)Math.Round(durBonus * 2.0), 1, 6);

        ApplyDotIf(rng, a.Stats.BleedChance, StatusKind.Bleed, t, dur, resistFactor);
        ApplyDotIf(rng, a.Stats.PoisonChance, StatusKind.Poison, t, dur, resistFactor);
        ApplyDotIf(rng, a.Stats.BurnChance, StatusKind.Burn, t, dur, resistFactor);
        ApplyDotIf(rng, a.Stats.ShockChance, StatusKind.Shock, t, dur, resistFactor);

        // Freeze: treat as “stun-like” (skip next action), simple
        var freezeChance = Clamp01(a.Stats.FreezeChance) * resistFactor;
        if (rng.NextDouble() < freezeChance)
        {
            t.FrozenTurns = Math.Max(t.FrozenTurns, 1);
            t.ActiveStatuses.Add(new ActiveStatus(StatusKind.Freeze, 1));
        }
    }

    /// <summary>
    /// Aplikuje status <paramref name="kind"/> na cíl s pravděpodobností <c>rawChance * resistFactor</c>.
    /// </summary>
    private static void ApplyDotIf(Random rng, double rawChance, StatusKind kind, Combatant t, int dur, double resistFactor)
    {
        var chance = Clamp01(rawChance) * resistFactor;
        if (chance <= 0) return;

        if (rng.NextDouble() < chance)
            t.ActiveStatuses.Add(new ActiveStatus(kind, dur));
    }

    /// <summary>
    /// Po obdržení zásahu má cíl šanci na CleanseChance odstranit jeden náhodný aktivní negativní status.
    /// </summary>
    private static void TryCleanse(Random rng, Combatant t)
    {
        var cleanse = Clamp01(t.Stats.CleanseChance);
        if (cleanse <= 0) return;

        if (rng.NextDouble() < cleanse && t.ActiveStatuses.Count > 0)
        {
            // remove one random negative
            t.ActiveStatuses.RemoveAt(rng.Next(0, t.ActiveStatuses.Count));
            t.FrozenTurns = 0;
        }
    }

    // ---------------------------
    // Loot (placeholder)
    // ---------------------------

    /// <summary>
    /// Sestaví řádky lootu po boji. Při prohře vrátí prázdný loot.
    /// Odměny jsou deterministicky odvozeny ze stage čísla.
    /// </summary>
    private static List<string> BuildLootLines(FightRequest req, bool didWin)
    {
        var list = new List<string>();
        if (!didWin)
        {
            list.Add("No loot.");
            return list;
        }

        // placeholder deterministic rewards
        var shards = 5 + Math.Max(1, req.Stage);
        var stones = 1 + (req.Stage / 3);

        list.Add($"+{shards} Beast shards (from dungeon)");
        list.Add($"+{stones} Upgrade stones (beast)");
        return list;
    }

    // ---------------------------
    // Scheduler helpers
    // ---------------------------

    /// <summary>
    /// Spočítá časový interval do příštího tahu jednotky.
    /// Vyšší Speed a TurnMeterGain/EnergyCostReduction znamenají kratší interval (jednotka jedná častěji).
    /// </summary>
    private static double SpeedInterval(float speed, double turnMeterGain, double energyCostReduction)
    {
        // Higher speed => smaller interval.
        // TurnMeterGain helps act sooner. EnergyCostReduction helps act sooner (tempo).
        var s = Math.Max(MinSpeed, (double)speed);

        var tempo = 1.0
            + Clamp01(turnMeterGain) * 0.40
            + Clamp01(energyCostReduction) * 0.25;

        // interval = 1/speed adjusted by tempo
        return (1.0 / s) / tempo;
    }

    /// <summary>Omezí hodnotu na rozsah [0, 1].</summary>
    private static double Clamp01(double x) => x < 0 ? 0 : (x > 1 ? 1 : x);

    // ---------------------------
    // Inner types
    // ---------------------------
    private enum Side { Player, Enemy }

    private enum StatusKind { Bleed, Poison, Burn, Shock, Freeze }

    private sealed record ActiveStatus(StatusKind Kind, int TurnsLeft);

    private sealed class Combatant
    {
        public Guid Id { get; }
        public string Name { get; }
        public Side Side { get; }
        public StatBlock Stats { get; }

        public double MaxHp { get; private set; }
        public double Hp { get; set; }

        public int FrozenTurns { get; set; } = 0;
        public List<ActiveStatus> ActiveStatuses { get; } = new();

        public bool IsAlive => Hp > EpsilonHp;

        public Combatant(Guid id, string name, Side side, StatBlock stats)
        {
            Id = id;
            Name = string.IsNullOrWhiteSpace(name) ? "Unknown" : name.Trim();
            Side = side;
            Stats = stats ?? throw new ArgumentNullException(nameof(stats));
        }

        public void Initialize()
        {
            // MaxHp is float; if 0 -> set safe baseline so fights aren't degenerate
            MaxHp = Math.Max(1.0, (double)Stats.MaxHp);
            Hp = MaxHp;

            // ensure crit mult sane if caller gives 0
            if (Stats.CriticalMultiplier <= 0)
                Stats.CriticalMultiplier = 1.5;
        }

        public void TickRegen(List<string> log)
        {
            if (!IsAlive) return;

            // HpRegen is described 0..1 but you might use it as absolute/turn; so keep it gentle:
            // regenAmount = MaxHp * HpRegen * (1 + HealingBonus) * (1 - HealingReduction)
            var regenRate = Math.Max(0.0, Stats.HpRegen);
            if (regenRate <= 0) return;

            var healAmp = 1.0 + Clamp01(Stats.HealingBonus);
            var antiHeal = Clamp01(Stats.HealingReduction);

            var heal = MaxHp * Clamp01(regenRate) * 0.08 * healAmp * (1.0 - antiHeal);
            if (heal <= 0) return;

            var before = Hp;
            Hp = Math.Min(MaxHp, Hp + heal);

            if (Hp > before + 0.001)
                log.Add($"{Name} regenerates {heal:0.0} HP.");
        }

        public void TickDots(List<string> log)
        {
            if (!IsAlive) return;

            // Freeze means skip this unit's next action; handled in scheduler by turning it into "no-op" action
            if (FrozenTurns > 0)
            {
                FrozenTurns--;
                log.Add($"{Name} is frozen and loses momentum.");
            }

            if (ActiveStatuses.Count == 0) return;

            var dotBonus = Clamp01(Stats.DotDamageBonus);
            var dotRed = Clamp01(Stats.DotDamageReduction);

            // base DOT damage: fraction of MaxHp (small), scaled by statuses count
            double totalDot = 0.0;

            for (int i = ActiveStatuses.Count - 1; i >= 0; i--)
            {
                var s = ActiveStatuses[i];
                // each tick per status: 2% MaxHp (small), can be tuned per kind
                var baseFrac = s.Kind switch
                {
                    StatusKind.Bleed => 0.020,
                    StatusKind.Poison => 0.018,
                    StatusKind.Burn => 0.022,
                    StatusKind.Shock => 0.016,
                    StatusKind.Freeze => 0.000,
                    _ => 0.0
                };

                var dmg = MaxHp * baseFrac;
                dmg *= (1.0 + dotBonus);
                dmg *= (1.0 - dotRed);

                totalDot += dmg;

                var newLeft = s.TurnsLeft - 1;
                if (newLeft <= 0)
                    ActiveStatuses.RemoveAt(i);
                else
                    ActiveStatuses[i] = s with { TurnsLeft = newLeft };
            }

            if (totalDot > 0.001)
            {
                Hp -= totalDot;
                if (Hp < 0) Hp = 0;
                log.Add($"{Name} suffers {totalDot:0.0} DOT damage.");
            }
        }
    }

    private sealed class TimelineSlot
    {
        public Combatant Unit { get; }
        public double NextActAt { get; set; }

        public TimelineSlot(Combatant unit)
        {
            Unit = unit;
            // start time based on speed
            NextActAt = SpeedInterval(unit.Stats.Speed, unit.Stats.TurnMeterGain, unit.Stats.EnergyCostReduction);
        }
    }

    private readonly record struct AttackOutcome(
        bool DidHit,
        string Text)
    {
        public static AttackOutcome Miss(Combatant a, Combatant t, bool isCounter, double hitChance)
            => new(false, $"{Prefix(isCounter)}{a.Name} misses {t.Name} (hit {hitChance:P0}).");

        public static AttackOutcome Hit(
            Combatant a, Combatant t,
            double dealt, bool didCrit, bool didBlock,
            double reflected, double lifeStealHeal,
            bool isCounter, double hitChance)
        {
            var parts = new List<string>(6)
            {
                $"{Prefix(isCounter)}{a.Name} hits {t.Name} for {dealt:0.0} (hit {hitChance:P0})."
            };

            if (didCrit) parts.Add("CRIT");
            if (didBlock) parts.Add("BLOCK");
            if (lifeStealHeal > 0.001) parts.Add($"+{lifeStealHeal:0.0} LS");
            if (reflected > 0.001) parts.Add($"-{reflected:0.0} thorns");

            parts.Add($"[{t.Name} HP {t.Hp:0.0}/{t.MaxHp:0.0}]");
            return new(true, string.Join(" ", parts));
        }

        public override string ToString() => Text;

        private static string Prefix(bool isCounter) => isCounter ? "COUNTER: " : "";
    }
}