using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxDome.Application.DTOs;
using TaxDome.Application.Services;
using TaxDome.AvaloniaApp.Common;
using TaxDome.AvaloniaApp.Common.Localization;

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
    }

    #region Fields

    private readonly ClientService _clientService;

    #endregion

    #region Properties

    public ObservableCollection<FileItemViewModel> SelectedFiles { get; } = new();

    [ObservableProperty] private bool _hasFiles;

    [ObservableProperty] private FileItemViewModel _selectedPreview;

    [ObservableProperty] private bool _isLoading;

    [ObservableProperty] private ObservableCollection<ClientDto> _clients = new();

    #endregion

    #region Commands

    [RelayCommand]
    private void Cancel(Window window)
    {
        window.Close(false);
    }

    [RelayCommand]
    private void CloseAndUpload(Window window)
    {
        window.Close(true);
    }

    [RelayCommand]
    private async Task UploadFiles(Window window)
    {
        var topLevel = TopLevel.GetTopLevel(window);
        var sp = topLevel?.StorageProvider;
        if (sp is null) return;
        var result = await sp.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = LocalizedStrings.Instance["UploadFiles_OpenFiles"],
            FileTypeFilter = GetFileTypes(),
            AllowMultiple = true,
        });

        foreach (IStorageFile storageFile in result)
        {
            var properties = await storageFile.GetBasicPropertiesAsync();
            var fileItemViewModel = new FileItemViewModel
            {
                FileName = storageFile.Name,
                Size = properties.Size ?? 0,
                MimeType = MimeTypeHelper.GetMimeType(storageFile.Name)
            };
            SelectedFiles.Add(fileItemViewModel);
        }
    }

    // public ICommand DeleteFileCommand { get; }

    #endregion

    #region Methods

    private async Task InitializeAsync()
    {
        SelectedFiles.CollectionChanged += (sender, args) => HasFiles = SelectedFiles.Any();
        // SelectedFiles.Add(new FileItemViewModel { FileName = "file1.pdf", Size = 10000000 });
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

    List<FilePickerFileType> GetFileTypes()
    {
        return
        [
            FilePickerFileTypes.ImageAll,
            FilePickerFileTypes.TextPlain,
            FilePickerFileTypes.Pdf,
            new FilePickerFileType("Word Documents")
            {
                Patterns = ["*.doc", "*.docx"],
                MimeTypes =
                [
                    "application/msword", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
                ]
            },
            new FilePickerFileType("Excel Spreadsheets")
            {
                Patterns = ["*.xls", "*.xlsx"],
                MimeTypes =
                [
                    "application/vnd.ms-excel", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                ]
            },
            new FilePickerFileType("PowerPoint Presentations")
            {
                Patterns = ["*.ppt", "*.pptx"],
                MimeTypes =
                [
                    "application/vnd.ms-powerpoint",
                    "application/vnd.openxmlformats-officedocument.presentationml.presentation"
                ]
            }
        ];
    }

    #endregion

    // public string UploadButtonText => $"Upload {SelectedFiles.Count} Files";
    // public bool CanUpload => SelectedFiles.Any();
}