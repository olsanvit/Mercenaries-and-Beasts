
namespace MercenariesAndBeasts.Domain;
public class Country
{
    public string Code { get; set; } = null!;   // ISO2, PK (CZ)
    public string Iso3 { get; set; } = null!;   // ISO3 (CZE)

    public string NameEn { get; set; } = null!;
    public string Continent { get; set; } = null!;

    public long Population { get; set; }
    public bool IsActive { get; set; } = true;
}