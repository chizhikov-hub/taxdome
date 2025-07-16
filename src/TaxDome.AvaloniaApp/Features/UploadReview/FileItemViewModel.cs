using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TaxDome.AvaloniaApp.Features.UploadReview;

public class FileItemViewModel : ObservableObject
{
    public string FileName { get; set; }
    public ulong Size { get; set; }
    public string MimeType { get; set; }
    public Bitmap Icon { get; set; }
}