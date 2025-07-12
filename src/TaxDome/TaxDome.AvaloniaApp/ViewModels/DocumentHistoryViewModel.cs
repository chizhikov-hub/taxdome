using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using TaxDome.Application.DTOs;

namespace TaxDome.AvaloniaApp.ViewModels;

public class DocumentHistoryViewModel : ObservableObject
{
    public DocumentHistoryViewModel()
    {
        var items = new ObservableCollection<DocumentDto>();
        // Заполняем данными
        // _groupedItems = new CollectionViewSource
        // {
        //     Source = items,
        //     IsSourceGrouped = true
        // };
        //
        // _groupedItems.GroupDescriptions.Add(new PropertyGroupDescription("DateGroup"));

    }

    // private CollectionViewSource _groupedItems;
    //
    // public ICollectionView GroupedItems => _groupedItems?.View;
}