using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CherylUI.Controls;

namespace LlamaChatMobile.Views;

public partial class StartNewChatView : UserControl
{
    public StartNewChatView()
    {
        InitializeComponent();
    }
    
    private void GoBack(object? sender, RoutedEventArgs e)
    {
        MobileNavigation.Pop();
    }
}