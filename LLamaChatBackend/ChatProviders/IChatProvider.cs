using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LlamaChat.Pages;
using LlamaChatBackend.Classes;
using MvvmStupidPocHelper;
using Newtonsoft.Json;

namespace LlamaChatBackend;

public partial class ChatProvider : ObservableObject
{
    [ObservableProperty] private Chat currentChat = null;
    
    [ObservableProperty] private ChatAdvancedSettings advancedSettings = new ChatAdvancedSettings();

    [
        ObservableProperty] private bool sending = false;

    [ObservableProperty] private bool settingsOpen = false;

    [ObservableProperty] private bool isInit = false;

    [ObservableProperty] private string currentMessage = "";
    
    
    public void ChangePane() => SettingsOpen = !SettingsOpen;
    
    public void SaveChat()
    {
        File.WriteAllText("chats\\" +CurrentChat.Title + "####" + CurrentChat.CreationDate.ToString().Replace(':','_'),JsonConvert.SerializeObject(CurrentChat));
    }
    
    public void ScrollToBottom()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
            ResourcesVM.Instance.ChatScroll?.ScrollToEnd();
        });
    }

    public virtual void StopAnswerGeneration(){}
    public virtual void InitChat(string modelPath, Chat? previouschat = null){}

    public virtual void Retry(){}
    public virtual void Continue(){}
    public virtual void SendMessage(){}

}