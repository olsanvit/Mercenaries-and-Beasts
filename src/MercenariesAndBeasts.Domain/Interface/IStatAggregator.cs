
using MercenariesAndBeasts.Domain.Combat;
using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.Interface;
public interface IStatAggregator
{
    Task<UnitSnapshot> BuildMercenaryAsync(Guid playerMercenaryId, CancellationToken ct = default);
    Task<UnitSnapshot> BuildBeastAsync(Guid playerMonsterId, CancellationToken ct = default);
}
public sealed record UnitSnapshot(
    Guid UnitId,
    string Name,
    int Level,
    bool IsMercenary,
    StatBlock Stats,
    IReadOnlyList<EquippedItemSnapshot> EquippedItems
);

public sealed record EquippedItemSnapshot(
    Guid PlayerItemId,
    string Code,
    string Name,
    ItemEquipSlot Slot,
    QualityTier Quality,
    StatBlock Stats // už spočítané (template scaling + bonusy)
);