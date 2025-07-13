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
        var clients = new []
        {
            new ClientDto(new Guid("A3CAC9C5-C924-444F-A4C0-38823E3C9BD6"), "Jane Cooper"), 
            new ClientDto(new Guid("9BAAB71D-8A1C-413B-93FE-B996EBA47C6D"), "Esther Howard"), 
            new ClientDto(new Guid("0888D441-F5FD-403F-A43A-FD7FA5D6BCA2"), "Leslie Alexander")
        };
        var folders = new []
        {
            new FolderDto(new Guid("C5DAC252-7961-4638-BFBE-8291EF99D3C7"), "Shared with Client"), 
            new FolderDto(new Guid("F796EF33-5B93-40FD-9EB5-D34668843F45"), "Private")
        };
        var actions = new[]
        {
            new DocumentActionDto(new Guid("7AC3BF0E-5B3D-47DE-B0A0-68AC68DFBBA2"), "Pending Signature"),
            new DocumentActionDto(new Guid("9FB03BCF-D42E-497D-A756-2AB2CF13C6B7"), "Approved"),
            new DocumentActionDto(new Guid("7D3B3075-E252-473C-9FD1-A1E7213DC75D"), "Retry"),
            new DocumentActionDto(new Guid("3BE186F1-17A4-42D9-AA8B-CFEBA27EECD6"), "Pending Approval"),
            new DocumentActionDto(new Guid("7D76BE70-9B0A-4942-9A89-3A05C99CD907"), "Job Processing"),
            new DocumentActionDto(new Guid("5C0E250A-FD43-4FD4-A5EC-00575B08EDF6"), "Invoice Linked"),
            new DocumentActionDto(new Guid("FF7B2E8B-93DD-471D-8DAF-0EE8C35F4371"), "Job Linked")
        };
        var random = new Random();

        //for (var i = 0; i < 1000; i++)
        {
            var startDate = DateTime.Now.AddDays(-10); 
            var endDate = DateTime.Now;

            var shuffledActions = actions.OrderBy(_ => random.Next()).ToList();
            var count = random.Next(0, actions.Length + 1);
            var appliedActions = shuffledActions.Take(count).ToList();
            var availableActions = shuffledActions.Skip(count).ToList();

            var document = new DocumentDto
            {
                Document = Path.ChangeExtension(Path.GetRandomFileName(), ".pdf"), 
                FileSize = random.NextInt64(0, 10 * 1024 * 1024),  
                Client = clients[random.Next(clients.Length)],
                Folder = folders[random.Next(folders.Length)],
                Date = startDate.AddDays(random.Next((endDate - startDate).Days + 1)),
                AppliedActions = appliedActions,
                AvailableActions = availableActions
            };
            await _documentService.AddAsync(document, CancellationToken.None);
        }        

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