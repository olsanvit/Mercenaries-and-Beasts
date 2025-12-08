using MercenariesAndBeasts.Domain.Units;

namespace MercenariesAndBeasts.Domain.Interface;

public interface IAiImageGenerator
{
    Task<string?> GenerateLocationImageAsync(Location location, CancellationToken ct = default);
    Task<string?> GenerateDungeonImageAsync(Dungeon dungeon, CancellationToken ct = default);
    Task<string?> GenerateMonsterImageAsync(MonsterTemplate monster, CancellationToken ct = default);
    Task<string?> GenerateMercenaryImageAsync(MercenaryTemplate mercenary, CancellationToken ct = default);
}