using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LlamaChat.Classes;

public partial class Chat : ObservableObject
{
    [ObservableProperty] private string title;
    [ObservableProperty] private string originalModel;
    [ObservableProperty] private ObservableCollection<Message> messages;


}

public partial class Message : ObservableObject
{
    [ObservableProperty] private bool isUser;
    [ObservableProperty] private bool isWriting;
    [ObservableProperty] private string content;
    
    public void Copy(string message)
    {
        TopLevel.GetTopLevel(((ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow).Clipboard.SetTextAsync(message);
    }
}