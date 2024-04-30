using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace LlamaChat.Pages.Chats;

public partial class ChatView : UserControl
{
    public ChatView()
    {
        InitializeComponent();
        
        ChatVM.Instance.MessagesScrollViewer = this.FindControl<ScrollViewer>("MonScrollViewer");
    }

}