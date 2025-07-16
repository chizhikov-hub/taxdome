using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxDome.Application.DTOs;
using TaxDome.Application.Services;

namespace TaxDome.AvaloniaApp.Features.UploadReview;

public partial class UploadReviewViewModel : ObservableObject
{
    public UploadReviewViewModel(ClientService clientService)
    {
        _clientService = clientService;
        _ = InitializeAsync();
        
        // // Заполнить данные
        // DeleteFileCommand = new RelayCommand<FileItemViewModel>(file =>
        // {
        //     SelectedFiles.Remove(file);
        // });
        // UploadCommand = new RelayCommand(() => { /* Загрузить файлы */ });
    }
    
    #region Fields
    
    private readonly ClientService _clientService;
    
    #endregion

    #region Properties

    public ObservableCollection<FileItemViewModel> SelectedFiles { get; } = new();
    
    [ObservableProperty]
    private FileItemViewModel _selectedPreview;
    
    [ObservableProperty]
    private bool _isLoading;
    
    [ObservableProperty]
    private ObservableCollection<ClientDto> _clients = new();

    #endregion
    
    #region Commands

    [RelayCommand]
    private void Cancel(Window window)
    {
        window.Close();
    }
    
    // public ICommand UploadCommand { get; }
    // public ICommand DeleteFileCommand { get; }
    
    #endregion
    
    #region Methods
    
    private async Task InitializeAsync()
    {
        await LoadReferencesAsync();
    }

    private async Task LoadReferencesAsync()
    {
        try
        {
            IsLoading = true;

            var loadClientsTask = _clientService.GetAllClientsAsync(CancellationToken.None);

            await Task.WhenAll(loadClientsTask);

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Clients = new ObservableCollection<ClientDto>(loadClientsTask.Result);
            });
        }
        finally
        {
            IsLoading = false;
        }
    }

    #endregion
    
    // public string UploadButtonText => $"Upload {SelectedFiles.Count} Files";
    // public bool CanUpload => SelectedFiles.Any();
}