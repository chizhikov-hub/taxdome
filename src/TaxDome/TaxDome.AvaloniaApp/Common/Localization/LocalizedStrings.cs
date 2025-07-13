using System.ComponentModel;
using System.Globalization;
using TaxDome.AvaloniaApp.Resources;

namespace TaxDome.AvaloniaApp.Common.Localization;

public class LocalizedStrings : INotifyPropertyChanged
{
    public static LocalizedStrings Instance { get; } = new LocalizedStrings();

    public event PropertyChangedEventHandler PropertyChanged;

    public string this[string key] => Strings.ResourceManager.GetString(key, CultureInfo.CurrentUICulture);

    public void Reload()
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }
}
