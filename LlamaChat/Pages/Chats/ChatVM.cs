using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LLama;
using LLama.Common;
using LLama.Native;
using LlamaChat.Classes;
using MvvmStupidPocHelper;

namespace LlamaChat.Pages.Chats;

public partial class ChatVM : Singleton<ChatVM>
{
    [ObservableProperty] private Chat currentChat = null;

    [ObservableProperty] private ChatAdvancedSettings advancedSettings = new ChatAdvancedSettings();
    [
        ObservableProperty] private bool sending = false;
    [ObservableProperty] private bool settingsOpen = false;
    [ObservableProperty] private bool isInit = false;

    [ObservableProperty] private string currentMessage = "";

    public List<CancellationTokenSource> CurrentBackends = new List<CancellationTokenSource>();

    public ChatSession Session = null;
    
    private ManualResetEventSlim signalEvent = new ManualResetEventSlim(false);
    
    public ScrollViewer MessagesScrollViewer { get; set; } = null;

    public void ChangePane() => SettingsOpen = !SettingsOpen;

    public void InitChat(string modelPath)
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        
        CurrentBackends.ForEach(t => t.Cancel());
        
        CurrentBackends.Add(cts);
        
        CurrentChat = new Chat()
        {
            OriginalModel = modelPath.Replace("models\\","").Replace(".gguf", "").Replace("-"," ") , Messages = new ObservableCollection<Message>(), Title = ""
        };
        
        
        Task.Run((async () =>
        {
            var parameters = new ModelParams(modelPath)
            {
                ContextSize = 1024, // The longest length of chat as memory.
                GpuLayerCount = 8 // How many layers to offload to GPU. Please adjust it according to your GPU memory.
            };
            using var model = LLamaWeights.LoadFromFile(parameters);
            using var context = model.CreateContext(parameters);
            var executor = new InteractiveExecutor(context);

// Add chat histories as prompt to tell AI how to act.
            var chatHistory = new ChatHistory(); 
            
            chatHistory.AddMessage(AuthorRole.System, "Transcript of a dialog, where the User interacts with an Assistant named Bob. Bob is helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision.");
             chatHistory.AddMessage(AuthorRole.User, "Hello, Bob.");
             chatHistory.AddMessage(AuthorRole.Assistant, "Hello. How may I help you today?"); 

            ChatSession session = new(executor, chatHistory);

         
            var bannedwords = new List<string>() { "User:", "Assistant:", "<|assistant|>" };
            

            while (!token.IsCancellationRequested)
            {
                Sending = false;
                signalEvent.Wait(token);
                Sending = true;
                
                CurrentChat.Messages.Add(new Message()
                {
                    IsUser = true, IsWriting = false, Content = currentMessage
                });
                
                ScrollToBottom();

                var aimessage = new Message()
                {
                    IsUser = false, IsWriting = true, Content = "" };
                
                CurrentChat.Messages.Add(aimessage);

                string buffer = "";
                
                InferenceParams inferenceParams = new InferenceParams()
                {
                    MaxTokens = AdvancedSettings.MaxTokens,
                    Temperature = AdvancedSettings.Temperature,
                    FrequencyPenalty = AdvancedSettings.FrequencePenalty,
                    RepeatPenalty = AdvancedSettings.Repeatpenalty,
                    AntiPrompts = new List<string> { "User:" } // Stop generation once antiprompts appear.
                    , 
                
                };

                
                await foreach (var text in session.ChatAsync(new ChatHistory.Message(AuthorRole.User, currentMessage), inferenceParams))
                {
                    if (bannedwords.Any(s => s.Contains(buffer + text)))
                    {
                        buffer += text;
                        if (bannedwords.Any(s => buffer.Contains(s)))
                            buffer = "";
                    }
                    else
                    {
                        aimessage.Content += buffer + text;
                        buffer = "";
                        ScrollToBottom();
                    }
                    
                }
                
                ScrollToBottom();

                aimessage.IsWriting = false;
                signalEvent.Reset();
                CurrentMessage = "";
                
                ScrollToBottom();
            }
        }));
    }

    private void ScrollToBottom()
    {
        Dispatcher.UIThread.Invoke(() =>
        {
    
            MessagesScrollViewer?.ScrollToEnd();
            
        });
        

    }
    
    
    
    public async void SendMessage()
    {
        if (CurrentChat?.Title?.Length == 0)
            CurrentChat.Title = CurrentMessage.Length > 30 ? CurrentMessage.Substring(0, 30) + " .." : CurrentMessage;
        
        signalEvent.Set();
    }
}