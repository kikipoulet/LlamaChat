using System;
using System.Collections.ObjectModel;

using CommunityToolkit.Mvvm.ComponentModel;

namespace LlamaChatBackend.Classes;

public partial class Chat : ObservableObject
{
    [ObservableProperty] private string title;
    [ObservableProperty] private string originalModel;
    [ObservableProperty] private ObservableCollection<Message> messages;
    public DateTime CreationDate { get; } = DateTime.Now;
}

public partial class ChatAdvancedSettings : ObservableObject
{
    [ObservableProperty] private int maxTokens = 256;
    [ObservableProperty] private float temperature = 0.8f;
    [ObservableProperty] private float frequencePenalty= 0;
    [ObservableProperty] private float repeatpenalty = 1.1f;
}

public partial class Message : ObservableObject
{
    [ObservableProperty] private bool isUser;
    [ObservableProperty] private bool isWriting;
    [ObservableProperty] private string content;
    
    public void Copy(string message)
    {
       // TopLevel.GetTopLevel(((ClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow).Clipboard.SetTextAsync(message);
    }
}