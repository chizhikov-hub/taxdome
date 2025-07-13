using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxDome.Application.DTOs;
using TaxDome.Application.Services;

namespace TaxDome.AvaloniaApp.Features.DocumentHistory;

public class DocumentHistoryViewModel : ObservableObject
{
    public DocumentHistoryViewModel(DocumentService documentService)
    {
        _documentService = documentService;
        InitializeCommands();
        LoadDocumentsAsync().ConfigureAwait(false);
    }

    public IAsyncRelayCommand AddCommand { get; private set; }

    private void InitializeCommands()
    {
        AddCommand = new AsyncRelayCommand(ExecuteAddCommand);
    }

    private async Task ExecuteAddCommand()
    {
        var clients = new [] { new ClientDto(Guid.NewGuid(), "Jane Cooper"), new ClientDto(Guid.NewGuid(), "Esther Howard"), new ClientDto(Guid.NewGuid(), "Leslie Alexander")};
        var folders = new [] { new FolderDto(Guid.NewGuid(), "Shared with Client"), new FolderDto(Guid.NewGuid(), "Private")};
        
        var startDate = DateTime.Now.AddDays(-10); 
        var endDate = DateTime.Now;

        var document = new DocumentDto
        {
            Document = Path.ChangeExtension(Path.GetRandomFileName(), ".pdf"), 
            FileSize = new Random().NextInt64(0, 1024 * 1024 * 1024),  
            Client = clients[new Random().Next(clients.Length)],
            Folder = folders[new Random().Next(folders.Length)],
            Date = startDate.AddDays(new Random().Next((endDate - startDate).Days + 1)),
            AppliedActions = [],
            AvailableActions = []
        };
        await _documentService.AddAsync(document, CancellationToken.None);

        await LoadDocumentsAsync();
    }

    private readonly DocumentService _documentService;
    private readonly ObservableCollection<DocumentDto> _allItems = new ObservableCollection<DocumentDto>();
    private ObservableCollection<DocumentDto> _filteredItems = new ObservableCollection<DocumentDto>();
    public ObservableCollection<DocumentDto> FilteredItems => _filteredItems;

    private string _searchText = string.Empty;

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (SetProperty(ref _searchText, value))
            {
                UpdateFilter();
            }
        }
    }

    private bool _isLoading;

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    private void UpdateFilter()
    {
        if (string.IsNullOrWhiteSpace(_searchText))
        {
            _filteredItems = new ObservableCollection<DocumentDto>(_allItems);
            OnPropertyChanged(nameof(FilteredItems));
            return;
        }

        var searchText = _searchText.ToLower();
        var filtered = _allItems.Where(doc =>
        {
            var docSearchText = (doc.Document + doc.Client).ToLower();
            return docSearchText.Contains(searchText);
        });

        _filteredItems = new ObservableCollection<DocumentDto>(filtered);
        OnPropertyChanged(nameof(FilteredItems));
    }

    private async Task LoadDocumentsAsync()
    {
        try
        {
            IsLoading = true;
            var documents = await _documentService.GetAllDocumentsAsync(CancellationToken.None);

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                _allItems.Clear();
                foreach (var doc in documents)
                {
                    _allItems.Add(doc);
                }
            });
        }
        finally
        {
            IsLoading = false;
        }

        UpdateFilter();
    }
}