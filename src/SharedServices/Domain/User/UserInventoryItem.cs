
using MercenariesAndBeasts.Domain.Items;
using MercenariesAndBeasts.Domain.Utils;

namespace MercenariesAndBeasts.Domain.User;
public class UserInventoryItem: BaseGuid
{

    public string UserId { get; set; } = default!;

    public Guid ItemTemplateId { get; set; }
    public ItemTemplate ItemTemplate { get; set; } = default!;

    public int Quantity { get; set; } // stack
}