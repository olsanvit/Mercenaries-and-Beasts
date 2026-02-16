
namespace MercenariesAndBeasts.Domain;
public sealed class ShopProduct
{
    public string Code { get; set; } = default!;
    public string Name { get; set; } = default!;
    public int Soft { get; set; }
    public int Premium { get; set; }
    public bool IsActive { get; set; } = true;

    // map měna -> Stripe PriceId (Price je v konkrétní měně)
    public Dictionary<string, string> StripePriceIds { get; set; } = new();
}