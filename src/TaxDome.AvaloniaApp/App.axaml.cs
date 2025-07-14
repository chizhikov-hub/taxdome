using System;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using TaxDome.AvaloniaApp.Common.Localization;
using TaxDome.AvaloniaApp.Features.DocumentHistory;

namespace TaxDome.AvaloniaApp;

public partial class App : Avalonia.Application
{
    public static IServiceProvider Services { get; set; }
    
    public override void Initialize()
    {
        LocalizationManager.SetCulture("ru-RU");
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new DocumentHistoryView
            {
                DataContext = Services.GetService(typeof(DocumentHistoryViewModel))
            };
            desktop.ShutdownRequested += DesktopOnShutdownRequested;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private bool _canClose; // This flag is used to check if window is allowed to close

    private async void DesktopOnShutdownRequested(object? sender, ShutdownRequestedEventArgs e)
    {
        e.Cancel = !_canClose; // cancel closing event first time

        if (!_canClose)
        {
            // // To save the items, we map them to the ToDoItem-Model which is better suited for I/O operations
            // var itemsToSave = _mainViewModel.ToDoItems.Select(item => item.GetToDoItem());
            //
            // await ToDoListFileService.SaveToFileAsync(itemsToSave);

            // Set _canClose to true and Close this Window again
            _canClose = true;
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.Shutdown();
            }
        }
    }
}