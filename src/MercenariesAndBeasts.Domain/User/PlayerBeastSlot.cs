
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;
public class PlayerBeastSlot: BaseGuid
{
    public Guid PlayerProfileId { get; set; }

    public int SlotIndex { get; set; } // 1..5
    public Guid? BeastInstanceId { get; set; }
    public PlayerMonster? Beast { get; set; }
}