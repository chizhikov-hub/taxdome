using System.Windows;
using TaxDome.Application.Services;
using TaxDome.Presentation.ViewModels;

namespace TaxDome.Presentation.Views;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class DocumentHistoryView : Window
{
    public DocumentHistoryView(DocumentHistoryViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}