using Avalonia.Controls;
using Avalonia.Input;

namespace TaxDome.AvaloniaApp.Features.UploadReview;

public partial class UploadReviewView : Window
{
    public UploadReviewView()
    {
        InitializeComponent();
    }
    
    private void Border_OnDrop(object? sender, DragEventArgs e)
    {
        if (DataContext is UploadReviewViewModel vm)
        {
            vm.DropCommand.Execute(e);
        }
    }

    private void Border_OnDragEnter(object? sender, DragEventArgs e)
    {
        if (DataContext is UploadReviewViewModel vm)
        {
            vm.DragEnterCommand.Execute(e);
        }
    }
}