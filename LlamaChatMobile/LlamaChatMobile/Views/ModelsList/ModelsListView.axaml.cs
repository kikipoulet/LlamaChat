using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CherylUI.Controls;
using LlamaChat.Classes;

namespace LlamaChatMobile.Views.ModelsList;

public partial class ModelsListView : UserControl
{
    public ModelsListView()
    {
        InitializeComponent();
    }

    private void GoBack(object? sender, RoutedEventArgs e)
    {
        MobileNavigation.Pop();
    }

    private void DownloadModel(object? sender, RoutedEventArgs e)
    {
        InteractiveContainer.ShowDialog(new DownloadModelDialog((AiModel)((Button)sender).Tag));
    }
}