

using MercenariesAndBeasts.Domain.Players;

namespace MercenariesAndBeasts.Web.Services;

/// <summary>
/// Provides business rules for player country selection and changes.
/// Enforces code format validation, cooldown periods, and escalating soft-currency costs.
/// </summary>
public static class CountryRules
{
    /// <summary>
    /// Determines whether the supplied country code has the correct format.
    /// A valid code is exactly two letters (ISO 3166-1 alpha-2 style).
    /// </summary>
    /// <param name="code">The country code to validate; may be <c>null</c> or empty.</param>
    /// <returns><c>true</c> if the code is non-empty and consists of exactly two letters; otherwise <c>false</c>.</returns>
    public static bool IsValidCode(string? code)
        => !string.IsNullOrWhiteSpace(code)
           && code.Length == 2
           && code.All(char.IsLetter);

    /// <summary>
    /// Determines whether the player is allowed to change their country at the given UTC time.
    /// The first choice is always free; subsequent changes require a 7-day cooldown.
    /// </summary>
    /// <param name="p">The player profile containing the current country code and change history.</param>
    /// <param name="nowUtc">The current UTC timestamp used to evaluate the cooldown.</param>
    /// <returns><c>true</c> if the player may change their country right now; otherwise <c>false</c>.</returns>
    public static bool CanChange(PlayerProfile p, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(p.CountryCode))
            return true; // první volba zdarma

        if (p.CountryChangedUtc is null)
            return true;

        return (nowUtc - p.CountryChangedUtc.Value).TotalDays >= 7;
    }

    /// <summary>
    /// Calculates the soft-currency cost for the player's next country change.
    /// The cost doubles with each previous change: 0 → 1 000 → 2 000 → 4 000 …
    /// </summary>
    /// <param name="p">The player profile whose <c>CountryChangeCount</c> is used to compute the cost.</param>
    /// <returns>The number of soft-currency units required; 0 for the first change.</returns>
    public static long ChangeCostSoft(PlayerProfile p)
    {
        if (p.CountryChangeCount == 0) return 0;
        return 1000 * (long)Math.Pow(2, p.CountryChangeCount - 1);
    }
}