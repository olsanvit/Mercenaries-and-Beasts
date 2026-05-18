using System.Text;
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Interface;

/// <summary>
/// Executes turn-based combat simulations for both Expedition and Dungeon modes.
/// Processes initiative, attacks, status effects, and loot rolls, returning a full
/// <see cref="CombatResult"/> including a per-round combat log.
/// </summary>
public sealed class CombatResolver : ICombatResolver
{
    /// <summary>
    /// Runs a complete combat simulation based on the supplied <paramref name="req"/>.
    /// Combat proceeds round-by-round until the enemy is defeated, all player units
    /// are dead, or the maximum round limit is reached.
    /// </summary>
    /// <param name="req">The combat request containing the player party, enemy, combat mode, stage, and RNG seed.</param>
    /// <returns>
    /// A <see cref="CombatResult"/> indicating whether the player won, the number of rounds played,
    /// the full combat log, and any loot rolled on victory.
    /// </returns>
    public CombatResult Resolve(CombatRequest req)
    {
        var rng = new Random(req.Seed);
        var log = new List<CombatLogLine>();

        // runtime HP
        var player = req.PlayerParty
            .Select(u => new RuntimeUnit(u))
            .ToList();

        var enemy = new RuntimeUnit(req.Enemy);

        int round = 0;
        const int maxRounds = 30;

        // init turn meters
        foreach (var u in player) u.TurnMeter = 0;
        enemy.TurnMeter = 0;

        while (round < maxRounds && enemy.Hp > 0 && player.Any(x => x.Hp > 0))
        {
            round++;

            // 1) tick DOT/status (MVP)
            foreach (var u in player.Where(x => x.Hp > 0))
                TickStatuses(u, log, round);

            TickStatuses(enemy, log, round);

            if (enemy.Hp <= 0) break;
            if (!player.Any(x => x.Hp > 0)) break;

            // 2) build initiative: add speed to turn meter until someone acts
            for (int step = 0; step < 50; step++)
            {
                foreach (var u in player.Where(x => x.Hp > 0))
                    u.TurnMeter += u.S. Speed * (1.0 + u.S.TurnMeterGain);

                if (enemy.Hp > 0)
                    enemy.TurnMeter += enemy.S.Speed * (1.0 + enemy.S.TurnMeterGain);

                var next = PickNextActor(player, enemy);
                if (next is null) continue;

                // act
                if (next.IsEnemy)
                {
                    var target = PickPlayerTarget(player, rng);
                    if (target is null) break;

                    DoAttack(attacker: enemy, defender: target, rng, log, round);
                }
                else
                {
                    var attacker = next.PlayerActor!;
                    if (enemy.Hp <= 0) break;

                    DoAttack(attacker, enemy, rng, log, round);
                }

                next.ResetTurnMeter();
                break; // 1 action per round in MVP (později víc)
            }
        }

        var playerWon = enemy.Hp <= 0 && player.Any(x => x.Hp > 0);
        var loot = RollLoot(req, rng, playerWon);

        log.Add(new CombatLogLine(round, playerWon ? "✅ Victory" : "❌ Defeat"));

        return new CombatResult(playerWon, round, log, loot);
    }

    // ----------------- internals -----------------

    private static void DoAttack(RuntimeUnit attacker, RuntimeUnit defender, Random rng, List<CombatLogLine> log, int round)
    {
        if (attacker.Hp <= 0 || defender.Hp <= 0) return;

        // hit check: Accuracy vs Evasion
        var hitChance = Clamp01(0.85 + (attacker.S.Accuracy - defender.S.Evasion) * 0.05);
        if (rng.NextDouble() > hitChance)
        {
            log.Add(new CombatLogLine(round, $"{attacker.Name} missed {defender.Name}."));
            return;
        }

        // block check
        var blocked = rng.NextDouble() < Clamp01(defender.S.BlockChance);
        var blockMult = blocked ? 0.5 : 1.0;

        // crit check
        var isCrit = rng.NextDouble() < Clamp01(attacker.S.CriticalChance);
        var critMult = isCrit ? attacker.S.CriticalMultiplier : 1.0;

        // base dmg
        var atk = attacker.S.Attack;
        var def = defender.S.Defense;

        // armor penetration reduces effective defense
        var effDef = def * (float)(1.0 - Clamp01(attacker.S.ArmorPenetration));

        var raw = Math.Max(1f, atk - effDef);
        var dmg = raw;

        // dmg bonus/reduction
        dmg *= (float)(1.0 + attacker.S.DamageBonus);
        dmg *= (float)(1.0 - Clamp01(defender.S.DamageReduction));

        // true dmg bonus (flat % of raw)
        dmg += raw * (float)Math.Max(0, attacker.S.TrueDamageBonus);

        dmg = (float)(dmg * critMult * blockMult);

        defender.Hp -= dmg;

        var sb = new StringBuilder();
        sb.Append($"{attacker.Name} hits {defender.Name} for {Math.Round(dmg)}");
        if (isCrit) sb.Append(" (CRIT)");
        if (blocked) sb.Append(" (BLOCK)");
        sb.Append($". HP: {Math.Max(0, Math.Round(defender.Hp))}");
        log.Add(new CombatLogLine(round, sb.ToString()));

        // apply one status (MVP) – třeba Bleed
        TryApplyStatuses(attacker, defender, rng, log, round);
    }

    private static void TryApplyStatuses(RuntimeUnit attacker, RuntimeUnit defender, Random rng, List<CombatLogLine> log, int round)
    {
        // resistance reduces chance
        double Resisted(double chance) => chance * (1.0 - Clamp01(defender.S.StatusResistance));

        if (rng.NextDouble() < Clamp01(Resisted(attacker.S.BleedChance)))
        {
            defender.AddStatus(new RuntimeStatus(RuntimeStatusType.Bleed, turns: 3));
            log.Add(new CombatLogLine(round, $"{defender.Name} is bleeding."));
        }
        if (rng.NextDouble() < Clamp01(Resisted(attacker.S.PoisonChance)))
        {
            defender.AddStatus(new RuntimeStatus(RuntimeStatusType.Poison, turns: 3));
            log.Add(new CombatLogLine(round, $"{defender.Name} is poisoned."));
        }
    }

    private static void TickStatuses(RuntimeUnit u, List<CombatLogLine> log, int round)
    {
        if (u.Hp <= 0) return;

        foreach (var st in u.Statuses.ToList())
        {
            float tick = 0;

            switch (st.Type)
            {
                case RuntimeStatusType.Bleed:
                    tick = Math.Max(1, u.MaxHp * 0.03f);
                    break;
                case RuntimeStatusType.Poison:
                    tick = Math.Max(1, u.MaxHp * 0.02f);
                    break;
            }

            // DOT modifiers
            tick *= (float)(1.0 + u.S.DotDamageBonus);
            tick *= (float)(1.0 - Clamp01(u.S.DotDamageReduction));

            if (tick > 0)
            {
                u.Hp -= tick;
                log.Add(new CombatLogLine(round, $"{u.Name} takes {Math.Round(tick)} DOT ({st.Type})."));
            }

            st.TurnsLeft--;
            if (st.TurnsLeft <= 0)
                u.Statuses.Remove(st);
        }

        // sustain regen (MVP)
        var regen = (float)(u.MaxHp * Math.Max(0, u.S.HpRegen));
        if (regen > 0 && u.Hp > 0)
            u.Hp = Math.Min(u.MaxHp, u.Hp + regen);
    }

    private static NextActor? PickNextActor(List<RuntimeUnit> player, RuntimeUnit enemy)
    {
        var bestPlayer = player.Where(x => x.Hp > 0).OrderByDescending(x => x.TurnMeter).FirstOrDefault();
        var bestEnemy = enemy.Hp > 0 ? enemy : null;

        if (bestPlayer is null && bestEnemy is null) return null;

        if (bestEnemy is null) return NextActor.Player(bestPlayer!);
        if (bestPlayer is null) return NextActor.Enemy();

        return (bestEnemy.TurnMeter >= bestPlayer.TurnMeter)
            ? NextActor.Enemy()
            : NextActor.Player(bestPlayer);
    }

    private static RuntimeUnit? PickPlayerTarget(List<RuntimeUnit> player, Random rng)
    {
        var alive = player.Where(x => x.Hp > 0).ToList();
        if (alive.Count == 0) return null;
        return alive[rng.Next(alive.Count)];
    }

    private static LootRoll RollLoot(CombatRequest req, Random rng, bool win)
    {
        if (!win) return new LootRoll(0, 0, 0, Array.Empty<Guid>());

        // jednoduché: dungeon dává shards+stones, location dává shards+gold
        var baseGold = req.Mode == CombatMode.Expedition ? 50 : 30;
        var baseShards = req.Mode == CombatMode.Dungeon ? 10 : 6;
        var baseStones = req.Mode == CombatMode.Dungeon ? 2 : 1;

        var stageMult = 1.0 + (req.Stage - 1) * 0.2;

        return new LootRoll(
            Gold: (int)Math.Round(baseGold * stageMult * (0.8 + rng.NextDouble() * 0.4)),
            Shards: (int)Math.Round(baseShards * stageMult * (0.8 + rng.NextDouble() * 0.4)),
            Stones: (int)Math.Round(baseStones * stageMult * (0.8 + rng.NextDouble() * 0.4)),
            ItemTemplateDrops: Array.Empty<Guid>() // zatím
        );
    }

    private static double Clamp01(double v) => v < 0 ? 0 : (v > 1 ? 1 : v);

    // ---------- runtime models ----------
    private sealed class RuntimeUnit
    {
        public Guid Id { get; }
        public string Name { get; }
        public StatBlock S { get; }
        public float MaxHp { get; }
        public float Hp { get; set; }
        public double TurnMeter { get; set; }
        public List<RuntimeStatus> Statuses { get; } = new();

        public RuntimeUnit(UnitSnapshot u)
        {
            Id = u.UnitId;
            Name = u.Name;
            S = u.Stats;
            MaxHp = u.Stats.MaxHp;
            Hp = MaxHp;
        }

        public void AddStatus(RuntimeStatus st)
        {
            // refresh = prodloužení, jednoduché
            var existing = Statuses.FirstOrDefault(x => x.Type == st.Type);
            if (existing is not null) existing.TurnsLeft = Math.Max(existing.TurnsLeft, st.TurnsLeft);
            else Statuses.Add(st);
        }
    }

    private enum RuntimeStatusType { Bleed, Poison }
    private sealed class RuntimeStatus
    {
        public RuntimeStatusType Type { get; }
        public int TurnsLeft { get; set; }
        public RuntimeStatus(RuntimeStatusType type, int turns) { Type = type; TurnsLeft = turns; }
    }

    private sealed class NextActor
    {
        public bool IsEnemy { get; }
        public RuntimeUnit? PlayerActor { get; private set; }

        private NextActor(bool enemy, RuntimeUnit? actor) { IsEnemy = enemy; PlayerActor = actor; }

        public static NextActor Enemy() => new(true, null);
        public static NextActor Player(RuntimeUnit u) => new(false, u);

        public void ResetTurnMeter()
        {
            if (IsEnemy) return;
            if (PlayerActor is not null) PlayerActor.TurnMeter = 0;
        }
    }
}