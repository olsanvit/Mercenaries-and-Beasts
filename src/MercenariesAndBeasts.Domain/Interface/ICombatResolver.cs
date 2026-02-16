using MercenariesAndBeasts.Domain.Interface;

namespace MercenariesAndBeasts.Domain.Interface;
public interface ICombatResolver
{
    CombatResult Resolve(CombatRequest req);
}

public sealed record CombatRequest(
    int Seed,
    IReadOnlyList<UnitSnapshot> PlayerParty, // 10 jednotek
    UnitSnapshot Enemy,
    CombatMode Mode,
    int Stage // dungeon stage 1..11, expedition stage “virtual”
);

public enum CombatMode { Dungeon, Expedition }

public sealed record CombatResult(
    bool PlayerWon,
    int Rounds,
    IReadOnlyList<CombatLogLine> Log,
    LootRoll Loot
);

public sealed record CombatLogLine(
    int Round,
    string Text
);

public sealed record LootRoll(
    int Gold,
    int Shards,
    int Stones,
    IReadOnlyList<Guid> ItemTemplateDrops // nebo rovnou PlayerItem create list
);