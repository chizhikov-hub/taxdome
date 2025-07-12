using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;
using TaxDome.Application.DTOs;
using TaxDome.Application.Services;

namespace TaxDome.Presentation.ViewModels;

public class DocumentHistoryViewModel : INotifyPropertyChanged
{
    private readonly DocumentService _documentService;
    private readonly ObservableCollection<DocumentDto> _documents;
    private string _searchText = string.Empty;
    private bool _isLoading;

    public event PropertyChangedEventHandler? PropertyChanged;

    public ICollectionView Documents { get; }
    
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

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public DocumentHistoryViewModel(DocumentService documentService)
    {
        _documentService = documentService;
        _documents = new ObservableCollection<DocumentDto>();
        Documents = CollectionViewSource.GetDefaultView(_documents);
        Documents.Filter = FilterDocuments;
        
        LoadDocumentsAsync().ConfigureAwait(false);
    }

    private bool FilterDocuments(object obj)
    {
        if (string.IsNullOrWhiteSpace(SearchText)) return true;
        
        if (obj is DocumentDto doc)
        {
            return doc.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   doc.Type.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                   doc.CreatedBy.Contains(SearchText, StringComparison.OrdinalIgnoreCase);
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