using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LlamaChatBackend;
using SukiUI;
using SukiUI.Controls;

namespace LlamaChat.Pages.Chats;

public partial class ChatView : UserControl
{
    public ChatView()
    {
        InitializeComponent();
        
        ResourcesVM.Instance.ChatScroll= this.FindControl<ScrollViewer>("MonScrollViewer");
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Button image = sender as Button;
        ContextMenu contextMenu = image.ContextMenu;
        contextMenu.PlacementTarget = image;
        contextMenu.Open();
       
    }

    private void newChatClick(object? sender, RoutedEventArgs e)
    {
        this.StackNavigationPop();
    }

    private void SaveChat(object? sender, RoutedEventArgs e)
    {
            ((ChatProvider)DataContext).SaveChat();
            ResourcesVM.Instance.RecentChats = ResourcesVM.GetChats();
        SukiHost.ShowToast("Success !", "Your chat has been saved.");
    }

    private void stopgenclick(object? sender, RoutedEventArgs e)
    { 
        ((Button)sender).Animate<double>(OpacityProperty,1,0);
    }
}