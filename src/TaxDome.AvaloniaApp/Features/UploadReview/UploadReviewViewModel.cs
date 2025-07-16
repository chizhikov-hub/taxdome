using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxDome.Application.DTOs;
using TaxDome.Application.Services;
using TaxDome.AvaloniaApp.Common.Helpers;
using TaxDome.AvaloniaApp.Common.Localization;

namespace TaxDome.AvaloniaApp.Features.UploadReview;

public partial class UploadReviewViewModel : ObservableObject
{
    public UploadReviewViewModel(DocumentService documentService, ClientService clientService)
    {
        _documentService = documentService;
        _clientService = clientService;
        _ = InitializeAsync();
    }

    #region Fields

    private readonly DocumentService _documentService;
    private readonly ClientService _clientService;

    #endregion

    #region Properties

    public ObservableCollection<FileItemViewModel> SelectedFiles { get; } = new();

    [ObservableProperty] 
    private bool _hasFiles;

    [ObservableProperty] 
    private FileItemViewModel _selectedPreview;

    [ObservableProperty] 
    private bool _isLoading;
    
    public bool CanUpload => SelectedClient != default && SelectedFiles.Any();

    [ObservableProperty] 
    private ObservableCollection<ClientDto> _clients = new();
    
    [ObservableProperty] 
    private ClientDto _selectedClient;

    #endregion

    #region Commands

    [RelayCommand]
    private void Cancel(Window window)
    {
        window.Close(false);
    }

    [RelayCommand]
    private async Task CloseAndUpload(Window window)
    {
        foreach (var selectedFile in SelectedFiles)
        {
            var document = new DocumentDto
            {
                Id = Guid.NewGuid(),
                Client = SelectedClient,
                Folder = new FolderDto(new Guid("C5DAC252-7961-4638-BFBE-8291EF99D3C7"), "Shared with Client"),
                Date = DateTime.Today,
                Document = selectedFile.FileName,
                FileSize = (long)selectedFile.Size,
                AppliedActions = [],
                AvailableActions = []
            };
            await _documentService.AddAsync(document, CancellationToken.None);
        }        
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
                MimeType = MimeTypeHelper.GetMimeType(storageFile.Name),
                Icon = IconHelper.GetFileIcon(storageFile.Name)
            };
            SelectedFiles.Add(fileItemViewModel);
        }
    }
    
    [RelayCommand]
    private void DragEnter(DragEventArgs e)
    {
        e.DragEffects = e.DragEffects & (DragDropEffects.Copy | DragDropEffects.Link);
        e.Handled = true;
    }

    [RelayCommand]
    private async Task Drop(DragEventArgs e)
    {
        if (e.Data.Contains(DataFormats.Files))
        {
            var files = e.Data.GetFiles();
            if (files is null) return;

            foreach (var file in files)
            {
                if (file is not IStorageFile storageFile) continue;
            
                var properties = await storageFile.GetBasicPropertiesAsync();
                var fileItemViewModel = new FileItemViewModel
                {
                    FileName = storageFile.Name,
                    Size = properties.Size ?? 0,
                    MimeType = MimeTypeHelper.GetMimeType(storageFile.Name),
                    Icon = IconHelper.GetFileIcon(storageFile.Name)
                };
                SelectedFiles.Add(fileItemViewModel);
            }
        }
    }

    #endregion

    #region Methods

    private async Task InitializeAsync()
    {
        SelectedFiles.CollectionChanged += (sender, args) =>
        {
            HasFiles = SelectedFiles.Any();
            OnPropertyChanged(nameof(CanUpload));
        };
        await LoadReferencesAsync();
    }
    
    partial void OnSelectedClientChanged(ClientDto value)
    {
        OnPropertyChanged(nameof(CanUpload));
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
}