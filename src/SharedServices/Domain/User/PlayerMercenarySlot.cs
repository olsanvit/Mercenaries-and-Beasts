using System.ComponentModel.DataAnnotations.Schema;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;
public class PlayerMercenarySlot : BaseGuid
{
    public Guid PlayerProfileId { get; set; }
    public PlayerProfile PlayerProfile { get; set; } = default!;

    public int SlotIndex { get; set; } // 1..5
    public Guid? MercenaryInstanceId { get; set; }

[ForeignKey(nameof(MercenaryInstanceId))]
public PlayerMercenary? Mercenary { get; set; }

}