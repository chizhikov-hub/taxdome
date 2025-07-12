using TaxDome.Presentation.Localization;

namespace TaxDome.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    public App()
    {
        LocalizationManager.SetCulture("ru-RU");
    }
}