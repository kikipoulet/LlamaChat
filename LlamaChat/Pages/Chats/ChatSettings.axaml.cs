using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using SukiUI;

namespace LlamaChat.Pages.Chats;

public partial class ChatSettings : UserControl
{
    public ChatSettings()
    {
        InitializeComponent();
    }

    private object lockboject = new object();
    private void InputElement_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        lock (lockboject)
        {
            this.Get<TextBlock>("TB").Animate<double>(WidthProperty, 0,78, TimeSpan.FromMilliseconds(200));
        }
    }

    private void InputElement_OnPointerExited(object? sender, PointerEventArgs e)
    {
        lock (lockboject)
        {
            this.Get<TextBlock>("TB").Animate<double>(WidthProperty, 78, 0, TimeSpan.FromMilliseconds(200));
        }
    }
}