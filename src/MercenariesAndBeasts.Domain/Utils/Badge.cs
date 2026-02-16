using MercenariesAndBeasts.Domain.Enums;

namespace MercenariesAndBeasts.Domain.Utils;

public static class ItemBadgeRules
{
    public const int MaxTier = 111;

    // Thresholds[t] = minimal wins to reach tier t (0..MaxTier)
    public static readonly int[] Thresholds = BuildThresholds();

    // =========================================
    // HLAVNÍ API
    // =========================================

    /// <summary>
    /// Vrátí badge tier podle počtu winů.
    /// </summary>
    public static ItemBadgeTier ComputeTier(int wins)
    {
        if (wins <= 0) return ItemBadgeTier.None;

        var idx = ComputeTierIndex(wins);        // 0..MaxTier
        idx = Math.Clamp(idx, 0, MaxTier);

        // Pozn: tohle předpokládá, že enum hodnoty odpovídají indexům (0..111).
        // Pokud máš enum "děravý", tak to musíš mapovat jinak.
        return (ItemBadgeTier)idx;
    }

    /// <summary>
    /// Kolik winů chybí do dalšího tieru (podle aktuálního tieru a currentWins).
    /// </summary>
    public static int WinsToNextTier(ItemBadgeTier tier, int currentWins)
    {
        var t = (int)tier;
        if (t < 0) t = 0;
        if (t >= MaxTier) return 0;

        var nextThreshold = Thresholds[t + 1];
        return Math.Max(0, nextThreshold - currentWins);
    }

    /// <summary>
    /// Vrátí index tieru (0..MaxTier) pro daný počet winů.
    /// </summary>
    public static int ComputeTierIndex(int wins)
    {
        if (wins <= 0) return 0;

        int lo = 0, hi = MaxTier;
        while (lo < hi)
        {
            int mid = (lo + hi + 1) / 2;
            if (Thresholds[mid] <= wins) lo = mid;
            else hi = mid - 1;
        }
        return lo;
    }

    // =========================================
    // KŘIVKA
    // =========================================

    /// <summary>
    /// Kolik winů stojí přechod z tieru "tier" do "tier+1".
    /// tier je 0..MaxTier-1
    /// </summary>
    public static int WinsForNextTier(int tier)
    {
        if (tier < 0) tier = 0;
        if (tier >= MaxTier) return 0;

        // rychlý start, plynulý růst, tvrdý endgame
        // ~3..280 wins / tier
        var w = 3
              + 0.55 * tier
              + 0.018 * tier * tier;

        return (int)Math.Ceiling(w);
    }

    private static int[] BuildThresholds()
    {
        var arr = new int[MaxTier + 1];
        arr[0] = 0;

        int sum = 0;

        // tier = 0..MaxTier-1; arr[tier+1] = arr[tier] + winsNeededForNextTier
        for (int tier = 0; tier < MaxTier; tier++)
        {
            sum += WinsForNextTier(tier);
            arr[tier + 1] = sum;
        }

        return arr;
    }
}