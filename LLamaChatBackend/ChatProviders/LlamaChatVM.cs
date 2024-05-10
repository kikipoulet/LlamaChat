using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LLama;
using LLama.Abstractions;
using LLama.Common;
using LLama.Native;
using LlamaChatBackend.Classes;
using MvvmStupidPocHelper;
using Newtonsoft.Json;
using SharpToken;


namespace LlamaChatBackend;

public partial class LlamaChatVM :  ChatProvider
{
  


    public List<CancellationTokenSource> CurrentBackends = new List<CancellationTokenSource>();

    public ChatSession session = null;
    
    private ManualResetEventSlim signalEvent = new ManualResetEventSlim(false);
    
    public ScrollViewer MessagesScrollViewer { get; set; } = null;


    
    private static uint contextSize = 24048;

    private CancellationTokenSource GenerationAnswerToken = new CancellationTokenSource();

    public override void StopAnswerGeneration()
    {
        GenerationAnswerToken.Cancel();
    }
    
    public override void InitChat(string modelPath, Chat? previouschat = null)
    {
        var cts = new CancellationTokenSource();
        var token = cts.Token;
        
        CurrentBackends.ForEach(t => t.Cancel());
        
        CurrentBackends.Add(cts);
        
        CurrentChat = previouschat ?? new Chat()
        {
            OriginalModel = modelPath , Messages = new ObservableCollection<Message>(), Title = ""
        };
        
        
        Task.Run((async () =>
        {
            var parameters = new ModelParams(CurrentChat.OriginalModel)
            {
                ContextSize = contextSize, // The longest length of chat as memory.
                
                GpuLayerCount = 8 // How many layers to offload to GPU. Please adjust it according to your GPU memory.
            };
            using var model = LLamaWeights.LoadFromFile(parameters);
            using var context = model.CreateContext(parameters);
            
            var executor = new InteractiveExecutor(context);

// Add chat histories as prompt to tell AI how to act.
            var chatHistory = new ChatHistory(); 
            
            chatHistory.AddMessage(AuthorRole.System, "Transcript of a dialog, where the User interacts with you, its Assistant. You are helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision. You are an expert in various scientific disciplines, including physics, chemistry, and biology. Explain scientific concepts, theories, and phenomena in an engaging and accessible way. Use lots of emojis in your answers.");
          
            foreach (var currentChatMessage in CurrentChat.Messages)
                chatHistory.AddMessage( currentChatMessage.IsUser ? AuthorRole.User : AuthorRole.Assistant, currentChatMessage.Content);
            
            session = new(executor, chatHistory);



            var bannedwords = new List<string>() {"User:", "Assistant:", "<|assistant|>" };
            

            while (!token.IsCancellationRequested)
            {
                Sending = false;
                signalEvent.Wait(token);
                Sending = true;
                
                if(CurrentMessage.Length > 0)
                     CurrentChat.Messages.Add(new Message()
                     {
                        IsUser = true, IsWriting = false, Content = CurrentMessage
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
                
                
                

                var x = new ChatHistory.Message(AuthorRole.User, CurrentMessage).ToString();

                while (context.Tokenize(session.HistoryTransform.HistoryToText(session.History))
                           .Length +
                       AdvancedSettings.MaxTokens
                       + context.Tokenize(CurrentMessage).Length
                       + 20 > contextSize){
                    
                    session.History.Messages.Remove(session.History.Messages.FirstOrDefault(m => m.AuthorRole != AuthorRole.System));

                }

                GenerationAnswerToken = new CancellationTokenSource();

                if (CurrentMessage == "")
                    CurrentMessage = "Continue";
                
                await foreach (var text in session.ChatAsync(new ChatHistory.Message(AuthorRole.User, CurrentMessage), inferenceParams, GenerationAnswerToken.Token))
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

  
    
    
    public override async void Retry()
    {
        CurrentChat.Messages.RemoveAt(CurrentChat.Messages.Count -1);
        CurrentMessage = CurrentChat.Messages.Last().Content;
        CurrentChat.Messages.RemoveAt(CurrentChat.Messages.Count -1);
        
        session.History.Messages.RemoveAt(session.History.Messages.Count -1);
        session.History.Messages.RemoveAt(session.History.Messages.Count -1);
        
        signalEvent.Set();
    }
    
    public override async void Continue()
    {
        CurrentMessage = "";
        
        signalEvent.Set();
    }
    
    public override async void SendMessage()
    {
        if (CurrentChat?.Title?.Length == 0)
            CurrentChat.Title = CurrentMessage.Length > 30 ? CurrentMessage.Substring(0, 30) + " .." : CurrentMessage;
        
        signalEvent.Set();
    }
}