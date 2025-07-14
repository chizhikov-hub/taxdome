using System.Globalization;

namespace TaxDome.Presentation.Localization;

public class LocalizationManager
{
    public static void SetCulture(string cultureCode)
    {
        var culture = new CultureInfo(cultureCode);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}