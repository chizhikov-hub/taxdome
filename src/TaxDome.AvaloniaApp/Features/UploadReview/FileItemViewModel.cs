using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;

namespace TaxDome.AvaloniaApp.Features.UploadReview;

public class FileItemViewModel : ObservableObject
{
    public string FileName { get; set; } = "";
    public ulong Size { get; set; }
    public string MimeType { get; set; } = "application/octet-stream";

    public Bitmap Icon => GetIconForMime(MimeType);

    private Bitmap GetIconForMime(string mime)
    {
        if (mime.Contains("pdf"))
            return new Bitmap("avares://YourApp/Assets/pdf_icon.png");
        if (mime.Contains("image"))
            return new Bitmap("avares://YourApp/Assets/image_icon.png");
        return new Bitmap("avares://YourApp/Assets/file_icon.png");
    }
}