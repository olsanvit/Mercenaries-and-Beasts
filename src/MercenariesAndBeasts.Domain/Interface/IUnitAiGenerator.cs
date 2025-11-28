
using MercenariesAndBeasts.Domain.AI;
using MercenariesAndBeasts.Domain.Dto;

namespace MercenariesAndBeasts.Domain.Interface;
public interface IUnitAiGenerator
{
     Task<ExpeditionGenerationResult> GenerateAsync(ExpeditionGenerationOptions options, CancellationToken ct = default);

    Task<DungeonGenerationResult> GenerateAsync(DungeonGenerationOptions options, CancellationToken ct = default);

}