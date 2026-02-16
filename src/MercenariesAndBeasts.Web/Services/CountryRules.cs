

using MercenariesAndBeasts.Domain.Players;

namespace MercenariesAndBeasts.Web.Services;
public static class CountryRules
{
    public static bool IsValidCode(string? code)
        => !string.IsNullOrWhiteSpace(code)
           && code.Length == 2
           && code.All(char.IsLetter);

    public static bool CanChange(PlayerProfile p, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(p.CountryCode))
            return true; // první volba zdarma

        if (p.CountryChangedUtc is null)
            return true;

        return (nowUtc - p.CountryChangedUtc.Value).TotalDays >= 7;
    }

    public static long ChangeCostSoft(PlayerProfile p)
    {
        if (p.CountryChangeCount == 0) return 0;
        return 1000 * (long)Math.Pow(2, p.CountryChangeCount - 1);
    }
}