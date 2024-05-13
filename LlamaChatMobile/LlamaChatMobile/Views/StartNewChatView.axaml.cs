using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using CherylUI.Controls;
using LlamaChat.Pages;
using LlamaChatBackend;
using LLamaChatBackend.Configs;
using LlamaChatMobile.Views.ChatViewModel;

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

    private void StartChat(object? sender, RoutedEventArgs e)
    {
        var vm= new OpenAIChatVM()
        {
            Key = ""
        };
        vm.InitChat("DeepSeek Chat");
        MobileNavigation.Push(new ChatView(){DataContext = vm});
    }
}