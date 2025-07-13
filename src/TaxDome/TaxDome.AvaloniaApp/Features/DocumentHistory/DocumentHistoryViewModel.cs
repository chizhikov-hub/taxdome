using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TaxDome.Application.DTOs;

namespace TaxDome.AvaloniaApp.Features.DocumentHistory;

public class DocumentHistoryViewModel : ObservableObject
{
    public DocumentHistoryViewModel()
    {
        _items = new ObservableCollection<DocumentDto>();
        Initialize();
    }

    private void Initialize()
    {
        Items.Add(new DocumentDto{Document = "Tax_Return_2024.pdf",     Client = "Jane Cooper",      Folder = "Shared with Client", AppliedActions = ["Pending Signature", "Job Linked"],                      AvailableActions = ["Approved", "Invoice Linked"],    Date = DateTime.Today,             FileSize = 2411725 /*2.3m * 1024 * 1024*/});
        Items.Add(new DocumentDto{Document = "Invoice_March_2024.pdf",  Client = "Jane Cooper",      Folder = "Shared with Client", AppliedActions = ["Approved", "Job Linked", "Invoice Linked"],             AvailableActions = ["Pending Signature"],             Date = DateTime.Today,             FileSize = 345703 /*337.6m * 1024*/});
        Items.Add(new DocumentDto{Document = "Contract_Amendment.pdf",  Client = "Esther Howard",    Folder = "Shared with Client", AppliedActions = [],                                                       AvailableActions = ["Retry"],                         Date = DateTime.Today,             FileSize = 1258292 /*1.2m * 1024 * 1024*/});
        Items.Add(new DocumentDto{Document = "Financial_Summary.xslx",  Client = "Esther Howard",    Folder = "Private",            AppliedActions = ["Pending Approval", "Job Processing", "Invoice Linked"], AvailableActions = ["Pending Signature"],             Date = DateTime.Today.AddDays(-1), FileSize = 1258292 /*1.2m * 1024 * 1024*/});
        Items.Add(new DocumentDto{Document = "Bank_Statements_Q1.xslx", Client = "Leslie Alexander", Folder = "Private",            AppliedActions = [],                                                       AvailableActions = ["Pending Signature", "Approved"], Date = DateTime.Today.AddDays(-1), FileSize = 363111 /*354.6m * 1024*/});
    }

    private readonly ObservableCollection<DocumentDto> _items;
    public ObservableCollection<DocumentDto> Items => _items;

    // public IEnumerable<IGrouping<string, DocumentDto>> GroupedItems => 
    //     _items.GroupBy(x => x.DateGroup);
}