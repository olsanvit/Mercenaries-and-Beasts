using MercenariesAndBeasts.Domain.Enums;
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain.Players;
namespace MercenariesAndBeasts.Domain;

public class BeastEquipmentSlot
{
    public Guid Id { get; set; }

    public Guid BeastInstanceId { get; set; }

    public ItemEquipSlot Slot { get; set; }     // Fang, Claw, ...
    public Guid? PlayerItemId { get; set; }
    public PlayerItem? PlayerItem { get; set; }
}