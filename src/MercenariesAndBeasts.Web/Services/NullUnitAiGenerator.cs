using MercenariesAndBeasts.Domain;
using MercenariesAndBeasts.Domain.Interface;
using MercenariesAndBeasts.Domain.Items;

namespace MercenariesAndBeasts.Web.Services;

internal sealed class NullUnitAiGenerator : IUnitAiGenerator
{
    private static Exception Disabled() =>
        new InvalidOperationException("AI generation is disabled — OpenAI:ApiKey is not configured.");

    // AUDIT:PENDING|Střední|Hází InvalidOperationException místo null – crash aplikace
    public Task<Dungeon> GenerateNextDungeonAsync(Dungeon previousDungeon, Dungeon currentDungeon, bool current,
        string? predefinedBeastsCsv = "", CancellationToken ct = default) => throw Disabled();

    // AUDIT:PENDING|Střední|Hází InvalidOperationException místo null – crash aplikace
    public Task<Location> GenerateNextLocationAsync(Location previousDungeon, Location currentDungeon, bool current,
        string? predefinedOrdersCsv = "", CancellationToken ct = default) => throw Disabled();

    // AUDIT:PENDING|Střední|Hází InvalidOperationException místo null – crash aplikace
    public Task<LocalizedNameResult?> GenerateLocalizedNamesAsync(
        string entityKind, string nameEn, string? descriptionEn,
        List<string?> missing, CancellationToken ct = default) => throw Disabled();

    // AUDIT:PENDING|Střední|Hází InvalidOperationException místo null – crash aplikace
    public Task<ItemTemplate?> GenerateItemTemplateAsync(ItemTemplate tpl, string? groupHint,
        CancellationToken ct = default) => throw Disabled();

    // AUDIT:PENDING|Střední|Hází InvalidOperationException místo null – crash aplikace
    public Task<ItemUpgradeResourceNamesResult?> GenerateUpgradeResourceNamesAsync(
        ItemTemplate tpl, CancellationToken ct = default) => throw Disabled();
}
