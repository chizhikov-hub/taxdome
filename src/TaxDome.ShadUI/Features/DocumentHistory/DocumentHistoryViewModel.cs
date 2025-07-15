using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Collections;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ShadUI;
using ShadUI.Demo;
using ShadUI.Demo.ViewModels;
using TaxDome.Application.DTOs;
using TaxDome.Application.Services;

namespace TaxDome.ShadUI.Features.DocumentHistory;

[Page("document-history")]
public sealed partial class DocumentHistoryViewModel : ObservableObject, INavigable
{
    public DocumentHistoryViewModel(PageManager pageManager, DocumentService documentService, 
        ClientService clientService, FolderService folderService, DocumentActionService documentActionService)
    {
        _pageManager = pageManager;
        _documentService = documentService;
        _clientService = clientService;
        _folderService = folderService;
        _documentActionService = documentActionService;
        _ = InitializeAsync();
    }
    
    #region Fields
    
    private readonly PageManager _pageManager;
    private readonly DocumentService _documentService;
    private readonly ClientService _clientService;
    private readonly FolderService _folderService;
    private readonly DocumentActionService _documentActionService;
    private readonly List<DocumentViewModel> _originalItems = [];
    
    #endregion
    
    #region Properties
    
    [ObservableProperty]
    private bool _isLoading;
    
    [ObservableProperty]
    private bool _allowSelectDocuments;
    
     [ObservableProperty]
     private bool? _selectAll = false;
    
     [ObservableProperty]
     private ObservableCollection<ClientDto> _clients = new();
    
     [ObservableProperty]
     private ObservableCollection<FolderDto> _folders = new();
    
     [ObservableProperty]
     private ObservableCollection<DocumentActionDto> _documentActions = new();
    
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

    #endregion
    
    #region Commands
    
    [RelayCommand]
    private async Task OpenUploadPreview(Window window)
    {
        // var view = new UploadReviewView();
        // await view.ShowDialog(window);
    }
    
    [RelayCommand]
    private void ChangeSelectionMode()
    {
        AllowSelectDocuments = !AllowSelectDocuments;
        if (!AllowSelectDocuments)
        {
            foreach (var item in _originalItems)
            {
                item.IsSelected = false;
            }
        }
    }

    [RelayCommand]
    private void NextPage()
    {
        _pageManager.Navigate<DashboardViewModel>();
    }
    
    #endregion

    #region Methods

    private async Task InitializeAsync()
    {
        await LoadReferencesAsync();
        await LoadDocumentsAsync();
    }
    
    private async Task LoadReferencesAsync()
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
                _originalItems.Clear();
                foreach (var doc in documents)
                {
                    _originalItems.Add(doc);
                }
                foreach (var i in _originalItems) i.PropertyChanged += OnItemsChanged;
            });
        }
        finally
        {
            IsLoading = false;
        }
    
        UpdateFilter();
    }
    
    private void OnItemsChanged(object sender, PropertyChangedEventArgs e)
    {
        UpdateSelectAllStatus();
    }
    
    private void UpdateSelectAllStatus()
    {
        var items = FilteredItems.OfType<DocumentViewModel>().ToArray();
        var selectedAll = items.All(item => item.IsSelected);
        var notSelectedCount = items.Count(item => !item.IsSelected);
        
        if (selectedAll)
        {
            SelectAll = true;
        }
        else if (notSelectedCount == items.Length)
        {
            SelectAll = false;
        }
        else
        {
            SelectAll = null;
        }
    }
    
    private void UpdateFilter()
    {
        IEnumerable<DocumentViewModel> filtered = _originalItems;
        
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
        UpdateSelectAllStatus();
        OnPropertyChanged(nameof(FilteredItems));
    }

    #endregion
}