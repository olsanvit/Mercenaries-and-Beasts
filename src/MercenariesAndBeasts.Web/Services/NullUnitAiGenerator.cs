using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Interface;
using MercenariesAndBeasts.Domain.Items;

namespace MercenariesAndBeasts.Web.Services;

internal sealed class NullUnitAiGenerator : IUnitAiGenerator
{
    public Task<Dungeon> GenerateNextDungeonAsync(Dungeon previousDungeon, Dungeon currentDungeon, bool current,
        string? predefinedBeastsCsv = "", CancellationToken ct = default) => Task.FromResult<Dungeon>(null!);

    public Task<Location> GenerateNextLocationAsync(Location previousDungeon, Location currentDungeon, bool current,
        string? predefinedOrdersCsv = "", CancellationToken ct = default) => Task.FromResult<Location>(null!);

    public Task<LocalizedNameResult?> GenerateLocalizedNamesAsync(
        string entityKind, string nameEn, string? descriptionEn,
        List<string?> missing, CancellationToken ct = default) => Task.FromResult<LocalizedNameResult?>(null);

    public Task<ItemTemplate?> GenerateItemTemplateAsync(ItemTemplate tpl, string? groupHint,
        CancellationToken ct = default) => Task.FromResult<ItemTemplate?>(null);

    public Task<ItemUpgradeResourceNamesResult?> GenerateUpgradeResourceNamesAsync(
        ItemTemplate tpl, CancellationToken ct = default) => Task.FromResult<ItemUpgradeResourceNamesResult?>(null);
}
