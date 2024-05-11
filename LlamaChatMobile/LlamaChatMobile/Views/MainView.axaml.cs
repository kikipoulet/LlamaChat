using Avalonia.Controls;
using Avalonia.Interactivity;
using CherylUI.Controls;
using LlamaChatMobile.Views.ModelsList;

namespace LlamaChatMobile.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
    }

    private void GoToModels(object? sender, RoutedEventArgs e)
    {
        MobileNavigation.Push(new ModelsListView());
    }

    private void StartNewChatView(object? sender, RoutedEventArgs e)
    {
        MobileNavigation.Push(new StartNewChatView());
    }
}