using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Collections;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxDome.Application.DTOs;
using TaxDome.Application.Services;
using TaxDome.AvaloniaApp.Features.UploadReview;

namespace TaxDome.AvaloniaApp.Features.DocumentHistory;

public class DocumentHistoryViewModel : ObservableObject
{
    public DocumentHistoryViewModel(DocumentService documentService, ClientService clientService, FolderService folderService, DocumentActionService documentActionService)
    {
        _documentService = documentService;
        _clientService = clientService;
        _folderService = folderService;
        _documentActionService = documentActionService;
        InitializeCommands();
        LoadReferencesAsync().ConfigureAwait(false);
        LoadDocumentsAsync().ConfigureAwait(false);
    }

    #region Fields

    private readonly DocumentService _documentService;
    private readonly ClientService _clientService;
    private readonly FolderService _folderService;
    private readonly DocumentActionService _documentActionService;
    private readonly ObservableCollection<DocumentViewModel> _allItems = new ObservableCollection<DocumentViewModel>();

    #endregion

    #region Properties

    public DataGridCollectionView FilteredItems { get; set; }    

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
    
    private ObservableCollection<ClientDto> _clients = new();
    public ObservableCollection<ClientDto> Clients
    {
        get => _clients;
        set => SetProperty(ref _clients, value);
    }

    private ObservableCollection<FolderDto> _folders = new();
    public ObservableCollection<FolderDto> Folders
    {
        get => _folders;
        set => SetProperty(ref _folders, value);
    }
    
    private ObservableCollection<DocumentActionDto> _documentActions = new();
    public ObservableCollection<DocumentActionDto> DocumentActions
    {
        get => _documentActions;
        set => SetProperty(ref _documentActions, value);
    }
    
    private ClientDto _selectedClient;
    public ClientDto SelectedClient
    {
        get => _selectedClient;
        set
        {
            if (_selectedClient != value)
            {
                _selectedClient = value;
                OnPropertyChanged();
                UpdateFilter();
            }
        }
    }

    private FolderDto _selectedFolder;
    public FolderDto SelectedFolder
    {
        get => _selectedFolder;
        set
        {
            if (_selectedFolder != value)
            {
                _selectedFolder = value;
                OnPropertyChanged();
                UpdateFilter();
            }
        }
    }
    
    private DocumentActionDto _selectedAppliedAction;
    public DocumentActionDto SelectedAppliedAction
    {
        get => _selectedAppliedAction;
        set
        {
            if (_selectedAppliedAction != value)
            {
                _selectedAppliedAction = value;
                OnPropertyChanged();
                UpdateFilter();
            }
        }
    }
    
    private bool _allowSelectDocuments;
    public bool AllowSelectDocuments
    {
        get => _allowSelectDocuments;
        set => SetProperty(ref _allowSelectDocuments, value);
    }

    #endregion

    #region Commands

    public IAsyncRelayCommand AddCommand { get; private set; }
    public IAsyncRelayCommand OpenUploadPreviewCommand { get; private set; }
    public IRelayCommand ChangeSelectModeCommand { get; private set; }

    private void InitializeCommands()
    {
        AddCommand = new AsyncRelayCommand(ExecuteAddCommand);
        OpenUploadPreviewCommand = new AsyncRelayCommand<Window>(OpenUploadPreview);
        ChangeSelectModeCommand = new RelayCommand(() =>
        {
            AllowSelectDocuments = !AllowSelectDocuments;
            if (!AllowSelectDocuments)
            {
                foreach (var item in _allItems)
                {
                    item.IsSelected = false;
                }
            }            
        });
    }

    #endregion

    #region Methods
    
    private async Task OpenUploadPreview(Window window)
    {
        var view = new UploadReviewView();
        await view.ShowDialog(window);
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

    private void UpdateFilter()
    {
        IEnumerable<DocumentViewModel> filtered = _allItems;
        
        if (!string.IsNullOrWhiteSpace(_searchText))
        {
            var searchText = _searchText.ToLower();
            filtered = filtered.Where(doc =>
            {
                var docSearchText = (doc.Document + doc.Client.Name).ToLower();
                return docSearchText.Contains(searchText);
            });
        }
        
        if (SelectedClient != null)
        {
            filtered = filtered.Where(doc => doc.Client.Id == SelectedClient.Id);
        }

        if (SelectedFolder != null)
        {
            filtered = filtered.Where(doc => doc.Folder.Id == SelectedFolder.Id);
        }

        if (SelectedAppliedAction != null)
        {
            filtered = filtered.Where(doc => 
                doc.AppliedActions.Any(action => action.Id == SelectedAppliedAction.Id));
        }

        FilteredItems = new DataGridCollectionView(filtered);
        FilteredItems.GroupDescriptions.Add(new DataGridPathGroupDescription("Group"));
        OnPropertyChanged(nameof(FilteredItems));
    }

    private async Task LoadDocumentsAsync()
    {
        try
        {
            IsLoading = true;
            var dtos = await _documentService.GetAllDocumentsAsync(CancellationToken.None);
            var documents = new ObservableCollection<DocumentViewModel>(
                dtos.Select(DocumentViewModel.FromDto).OrderByDescending(x => x.Date)
            );

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
    
    public async Task LoadReferencesAsync()
    {
        try
        {
            IsLoading = true;

            var loadClientsTask = _clientService.GetAllClientsAsync(CancellationToken.None);
            var loadFoldersTask = _folderService.GetAllFoldersAsync(CancellationToken.None);
            var loadDocumentActionsTask = _documentActionService.GetAllDocumentActionsAsync(CancellationToken.None);

            await Task.WhenAll(loadClientsTask, loadFoldersTask, loadDocumentActionsTask);

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
            {
                Clients = new ObservableCollection<ClientDto>(loadClientsTask.Result);
                Folders = new ObservableCollection<FolderDto>(loadFoldersTask.Result);
                DocumentActions = new ObservableCollection<DocumentActionDto>(loadDocumentActionsTask.Result);
            });
        }
        finally
        {
            IsLoading = false;
        }
    }

    #endregion
}