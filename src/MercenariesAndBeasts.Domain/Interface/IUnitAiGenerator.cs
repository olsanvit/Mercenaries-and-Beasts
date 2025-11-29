
using MercenariesAndBeasts.Domain.AI;
using MercenariesAndBeasts.Domain.Dto;

namespace MercenariesAndBeasts.Domain.Interface;
public interface IUnitAiGenerator
{
    Task<DungeonGenerationResult> GenerateNextDungeonAsync(Dungeon previousDungeon, bool current, CancellationToken ct = default);
    Task<ExpeditionGenerationResult> GenerateNextLocationAsync(Location previousDungeon, bool current, CancellationToken ct = default);
}