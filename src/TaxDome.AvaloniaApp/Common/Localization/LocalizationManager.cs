using System.Globalization;
using System.Threading;

namespace TaxDome.AvaloniaApp.Common.Localization;

public class LocalizationManager
{
    public static void SetCulture(string cultureCode)
    {
        // Устанавливаем культуру
        var culture = new CultureInfo(cultureCode);
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }
}