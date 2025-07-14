using System;
using System.Collections.Generic;
using System.Windows.Input;
using Avalonia.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using TaxDome.Application.DTOs;

namespace TaxDome.AvaloniaApp.Features.DocumentHistory;

public partial class DocumentViewModel : ObservableObject
{
    public DocumentViewModel()
    {
        ToggleSelectionCommand = new RelayCommand<TappedEventArgs>(args =>
        {
            IsSelected = !IsSelected;
            args.Handled = true;
        });
    }

    public Guid Id { get; set; }
    public DateTime Date { get; init; }
    public string Document { get; init; }
    public long FileSize { get; init; }
    public ClientDto Client { get; init; }
    public FolderDto Folder { get; init; }
    public List<DocumentActionDto> AppliedActions { get; init; }
    public List<DocumentActionDto> AvailableActions { get; init; }
    
    private string group;
    public string Group
    {
        get => group;
        set
        {
            if (value == group) return;
            group = value;
            OnPropertyChanged();
        }
    }
    
    private bool isSelected;
    public bool IsSelected
    {
        get => isSelected;
        set
        {
            if (value == isSelected) return;
            isSelected = value;
            OnPropertyChanged();
        }
    }

    public ICommand ToggleSelectionCommand { get; } 
    
    public static DocumentViewModel FromDto(DocumentDto dto)
    {
        var today = DateTime.Today;
        var yesterday = today.AddDays(-1);        
        
        return new DocumentViewModel
        {
            Id = dto.Id,
            Date = dto.Date,
            Document = dto.Document,
            FileSize = dto.FileSize,
            Client = dto.Client,
            Folder = dto.Folder,
            AppliedActions = dto.AppliedActions,
            AvailableActions = dto.AvailableActions,
            Group = dto.Date switch
            {
                var d when d.Date == today => "Today",
                var d when d.Date == yesterday => "Yesterday",
                var d => d.ToString("yyyy-MM-dd")
            }
        };
    }

    public DocumentDto ToDto() => new()
    {
        Id = Id,
        Date = Date,
        Document = Document,
        FileSize = FileSize,
        Client = Client,
        Folder = Folder,
        AppliedActions = AppliedActions,
        AvailableActions = AvailableActions
    };
}