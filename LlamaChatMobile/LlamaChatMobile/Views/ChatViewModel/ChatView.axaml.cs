using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using LlamaChat.Pages;

namespace LlamaChatMobile.Views.ChatViewModel;

public partial class ChatView : UserControl
{
    public ChatView()
    {
        InitializeComponent();
        ResourcesVM.Instance.ChatScroll= this.FindControl<ScrollViewer>("MonScrollViewer");
    }
}