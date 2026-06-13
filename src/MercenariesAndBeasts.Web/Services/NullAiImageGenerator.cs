using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Interface;

namespace MercenariesAndBeasts.Web.Services;

internal sealed class NullAiImageGenerator : IAiImageGenerator
{
    public Task<string?> GenerateLocationImageAsync(Location location, CancellationToken ct = default)
        => Task.FromResult<string?>(null);

    public Task<string?> GenerateDungeonImageAsync(Dungeon dungeon, CancellationToken ct = default)
        => Task.FromResult<string?>(null);

    public Task<string?> GenerateMonsterImageAsync(MonsterTemplate monster, CancellationToken ct = default)
        => Task.FromResult<string?>(null);

    public Task<string?> GenerateMercenaryImageAsync(MercenaryTemplate mercenary, CancellationToken ct = default)
        => Task.FromResult<string?>(null);
}
