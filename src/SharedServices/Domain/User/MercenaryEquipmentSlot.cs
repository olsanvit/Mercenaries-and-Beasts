using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain.Players;

namespace MercenariesAndBeasts.Domain;

public class MercenaryEquipmentSlot
{
    public Guid Id { get; set; }

    public Guid MercenaryInstanceId { get; set; }

    public ItemEquipSlot Slot { get; set; }   // MainHand, OffHand, ...
    public Guid? PlayerItemId { get; set; }
    public PlayerItem? PlayerItem { get; set; }
}

public class MercenaryInstance
{
}