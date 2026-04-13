
using System.ComponentModel.DataAnnotations.Schema;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.Players;
public class PlayerBeastSlot: BaseGuid
{
    public Guid PlayerProfileId { get; set; }
    public PlayerProfile PlayerProfile { get; set; } = default!;

    public int SlotIndex { get; set; } // 1..5
    public Guid? BeastInstanceId { get; set; }

    [ForeignKey(nameof(BeastInstanceId))]
    public PlayerMonster? Beast { get; set; }
}