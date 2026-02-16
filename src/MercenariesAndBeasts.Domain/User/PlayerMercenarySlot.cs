using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;
public class PlayerMercenarySlot : BaseGuid
{
    public Guid PlayerProfileId { get; set; }
    public PlayerProfile Player { get; set; } = default!;

    public int SlotIndex { get; set; } // 1..5
public Guid? MercenaryInstanceId { get; set; }
public PlayerMercenary? Mercenary { get; set; }

}