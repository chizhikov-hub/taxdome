using System.ComponentModel;
using System.Globalization;
using TaxDome.Presentation.Resources;

namespace TaxDome.Presentation.Localization;

public class LocalizedStrings : INotifyPropertyChanged
{
    private static LocalizedStrings _instance = new LocalizedStrings();
    public static LocalizedStrings Instance => _instance;

    public event PropertyChangedEventHandler PropertyChanged;

    public string this[string key] => Strings.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);

    public void Reload()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }
}
