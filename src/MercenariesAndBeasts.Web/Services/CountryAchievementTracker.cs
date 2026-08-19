namespace MercenariesAndBeasts.Web.Services;

public static class CountryAchievementTracker
{
    public static readonly string[] SupportedCountries =
        ["CZ", "SK", "DE", "GB", "FI", "PL", "HU", "AT", "FR", "ES", "IT", "PT", "NL", "SE", "NO", "DK"];

    public static readonly int[] DefeatThresholds  = [10, 25, 50];
    public static readonly int[] DamageThresholds  = [50_000, 250_000, 1_000_000];
    public static readonly int[] BlockThresholds   = [25, 100, 500];
    public static readonly int[] PassiveThresholds = [10, 50, 200];

    public static string DefeatKey(string cc, int t)  => $"MB_DEFEAT_{cc.ToUpper()}_{t}";
    public static string DamageKey(string cc, int t)  => $"MB_DMG_{cc.ToUpper()}_{t}";
    public static string BlockKey(string cc, int t)   => $"MB_BLOCK_{cc.ToUpper()}_{t}";
    public static string PassiveKey(string cc, int t) => $"MB_PASSIVE_{cc.ToUpper()}_{t}";
}
