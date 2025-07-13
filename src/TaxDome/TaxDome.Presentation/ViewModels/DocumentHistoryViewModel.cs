using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using TaxDome.Application.DTOs;
using TaxDome.Application.Services;
using TaxDome.Domain.Entities;

namespace TaxDome.Presentation.ViewModels;

public class DocumentHistoryViewModel : INotifyPropertyChanged
{
    private readonly DocumentService _documentService;
    private readonly ObservableCollection<DocumentDto> _documents;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICollectionView Documents { get; }
    
    private string _searchText = string.Empty;
    public string SearchText
    {
        get => _searchText;
        set
        {
            _searchText = value;
            OnPropertyChanged();
            Documents.Refresh();
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
    
    public IAsyncRelayCommand AddCommand { get; private set; }

    public DocumentHistoryViewModel(DocumentService documentService)
    {
        _documentService = documentService;
        _documents = new ObservableCollection<DocumentDto>();
        Documents = CollectionViewSource.GetDefaultView(_documents);
        Documents.Filter = FilterDocuments;
        InitializeCommands();
        
        LoadDocumentsAsync().ConfigureAwait(false);
    }

    private void InitializeCommands()
    {
        AddCommand = new AsyncRelayCommand(ExecuteAddCommand);
    }
    
    private async Task ExecuteAddCommand()
    {
        // var document = new Document(Path.ChangeExtension(Path.GetRandomFileName(), ".pdf"), "PDF", "chizhikov");
        // await _documentService.AddAsync(document, CancellationToken.None);
        //
        //
        //
        // await LoadDocumentsAsync();
    }

    private async Task GenerateDocumentAsync()
    {
        try
        {
            IsLoading = true;
            var documents = await _documentService.GetAllDocumentsAsync(CancellationToken.None);
            
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _documents.Clear();
                foreach (var doc in documents)
                {
                    _documents.Add(doc);
                }
            });
        }
        finally
        {
            IsLoading = false;
        }
    }

    private bool FilterDocuments(object obj)
    {
        if (string.IsNullOrWhiteSpace(SearchText)) return true;
        
        if (obj is DocumentDto doc)
        {
            return doc.Document.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   doc.Folder.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   doc.Client.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
        }
        return false;
    }

    private async Task LoadDocumentsAsync()
    {
        try
        {
            IsLoading = true;
            var documents = await _documentService.GetAllDocumentsAsync(CancellationToken.None);
            
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                _documents.Clear();
                foreach (var doc in documents)
                {
                    _documents.Add(doc);
                }
            });
        }
        finally
        {
            IsLoading = false;
        }
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}