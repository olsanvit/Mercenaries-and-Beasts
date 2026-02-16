using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MercenariesAndBeasts.Domain.Combat;

namespace MercenariesAndBeasts.Infrastructure.Fights;

public interface IFightService
{
    Task<FightResult> ResolveDungeonFightAsync(FightRequest req, CancellationToken ct);
}

// DTOs (můžeš dát do Shared projektu)
public sealed class FightRequest
{
    public Guid PlayerId { get; set; }
    public Guid DungeonId { get; set; }
    public int Stage { get; set; }
    public int Seed { get; set; }

    public List<FightUnitInput> PlayerUnits { get; set; } = new();
    public FightUnitInput Enemy { get; set; } = default!;
}

public sealed record FightUnitInput(Guid Id, string Name, StatBlock Stats);

public sealed class FightResult
{
    public bool DidWin { get; set; }
    public string? Summary { get; set; }
    public List<string> LogLines { get; set; } = new();
    public List<string> LootLines { get; set; } = new();
}