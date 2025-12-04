using System.Globalization;

namespace MercenariesAndBeasts.Domain.Utils;

public static class LocalizationExtensions
{
    public static string NameForCurrentCulture(BaseGuid entity)
    {
        var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        return lang switch
        {
            "cs" => string.IsNullOrWhiteSpace(entity.NameCs) ? entity.NameEn : entity.NameCs!,
            "de" => string.IsNullOrWhiteSpace(entity.NameDe) ? entity.NameEn : entity.NameDe!,
            _    => entity.NameEn
        };
    }

    public static string DescriptionForCurrentCulture(BaseGuid entity)
    {
        var lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

        return lang switch
        {
            "cs" => string.IsNullOrWhiteSpace(entity.DescriptionCs) ? entity.DescriptionEn : entity.DescriptionCs!,
            "de" => string.IsNullOrWhiteSpace(entity.DescriptionDe) ? entity.DescriptionEn : entity.DescriptionDe!,
            _    => entity.DescriptionEn
        };
    }

}