using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace LlamaChat.Pages.Chats;

public partial class ChatView : UserControl
{
    public ChatView()
    {
        InitializeComponent();
        
        ChatVM.Instance.MessagesScrollViewer = this.FindControl<ScrollViewer>("MonScrollViewer");
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Button image = sender as Button;
        ContextMenu contextMenu = image.ContextMenu;
        contextMenu.PlacementTarget = image;
        contextMenu.Open();
       
    }
}