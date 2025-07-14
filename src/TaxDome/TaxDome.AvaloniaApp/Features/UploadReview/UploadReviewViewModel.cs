using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace TaxDome.AvaloniaApp.Features.UploadReview;

public class UploadReviewViewModel : ObservableObject
{
    public ObservableCollection<FileItemViewModel> SelectedFiles { get; } = new();
    
    private FileItemViewModel? _selectedPreview;
    public FileItemViewModel? SelectedPreview
    {
        get => _selectedPreview;
        set => SetProperty(ref _selectedPreview, value);
    }

    public string UploadButtonText => $"Upload {SelectedFiles.Count} Files";
    public bool CanUpload => SelectedFiles.Any();

    public ICommand CancelCommand { get; }
    public ICommand UploadCommand { get; }
    public ICommand DeleteFileCommand { get; }

    public UploadReviewViewModel()
    {
        // Заполнить данные
        DeleteFileCommand = new RelayCommand<FileItemViewModel>(file =>
        {
            SelectedFiles.Remove(file);
        });

        CancelCommand = new RelayCommand(() => { /* Закрыть окно */ });
        UploadCommand = new RelayCommand(() => { /* Загрузить файлы */ });
    }
}