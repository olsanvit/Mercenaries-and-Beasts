
using MercenariesAndBeasts.Domain.AI;
using MercenariesAndBeasts.Domain.Dto;

namespace MercenariesAndBeasts.Domain.Interface;
public interface IUnitAiGenerator
{
    Task<DungeonGenerationResult> GenerateNextDungeonAsync(Dungeon previousDungeon, Dungeon currentDungeon, bool current, CancellationToken ct = default);
    Task<ExpeditionGenerationResult> GenerateNextLocationAsync(Location previousDungeon, Location currentDungeon, bool current, CancellationToken ct = default);
    Task<LocalizedNameResult?> GenerateLocalizedNamesAsync(
    string entityKind,            // "dungeon", "location", "monster", "mercenary"...
    string nameEn,
    string? descriptionEn,
    CancellationToken ct = default);
}