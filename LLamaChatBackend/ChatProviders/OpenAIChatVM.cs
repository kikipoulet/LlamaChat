using System.Collections.ObjectModel;
using Avalonia.Controls;
using LLama;
using LLama.Common;
using LlamaChatBackend.Classes;
using OpenAI;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OpenAI.Assistants;
using OpenAI.Chat;
using Message = LlamaChatBackend.Classes.Message;

namespace LlamaChatBackend;

public class OpenAIChatVM:  ChatProvider
{
  

    private ManualResetEventSlim signalEvent = new ManualResetEventSlim(false);
    
    public ScrollViewer MessagesScrollViewer { get; set; } = null;

    public string Domain { get; set; } = "https://api.deepseek.com";

    public string Key { get; set; } = "";
    public string Model { get; set; } = "deepseek-chat";

    private CancellationTokenSource GenerationAnswerToken = new CancellationTokenSource();

    public override void StopAnswerGeneration()
    {
        GenerationAnswerToken.Cancel();
    }
    
    public override void SaveChat()
    {
        File.WriteAllText("chats\\" +CurrentChat.Title + "####" + CurrentChat.CreationDate.ToString().Replace(':','_'),JsonConvert.SerializeObject(CurrentChat));
    }
    
    public override void InitChat(string modelPath, Chat? previouschat = null)
    {
        
        CurrentChat = previouschat ?? new Chat()
        {
            OriginalModel = modelPath , Messages = new ObservableCollection<Message>(), Title = ""
        };
        
        
        Task.Run((async () =>
        {
            var cts = new CancellationTokenSource();
            var token = cts.Token;
            
            using var api = new OpenAIClient( new OpenAIAuthentication(Key), new OpenAIClientSettings(Domain));
            
            
            var messages = new List<OpenAI.Chat.Message>
            {
                new(Role.System, "Transcript of a dialog, where the User interacts with you, its Assistant. You are helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision. You are an expert in various scientific disciplines, including physics, chemistry, and biology. Explain scientific concepts, theories, and phenomena in an engaging and accessible way. Use lots of emojis in your answers."),
            };
            
          
            foreach (var currentChatMessage in CurrentChat.Messages)
                messages.Add(new OpenAI.Chat.Message(currentChatMessage.IsUser ? Role.User: Role.Assistant, currentChatMessage.Content));

           
          

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
                
                messages.Add(new OpenAI.Chat.Message(Role.User, CurrentMessage));
                
                ScrollToBottom();

                var aimessage = new Message() { IsUser = false, IsWriting = true, Content = "" };
                
                CurrentChat.Messages.Add(aimessage);

                string buffer = "";
                
                if (CurrentMessage == "")
                    CurrentMessage = "Continue";
                
                var chatRequest = new ChatRequest(messages, Model);

                string bufferstring = "";

                Task.Run(() =>
                {
                    while (bufferstring.Length > 0 || aimessage.IsWriting)
                    {
                        try
                        {
                            if (bufferstring.Length > 0)
                            {
                                aimessage.Content += bufferstring.Substring(0, 3);
                                bufferstring = bufferstring.Remove(0, 3);
                            }
                        }catch{}
                        Thread.Sleep(50);
                    }
                    
                    ScrollToBottom();
                });
                
                GenerationAnswerToken = new CancellationTokenSource();
                
                var response = await api.ChatEndpoint.StreamCompletionAsync(chatRequest, partialResponse =>
                {
                    if(!GenerationAnswerToken.IsCancellationRequested)
                         bufferstring += partialResponse.FirstChoice.Delta.ToString();
                  
                    ScrollToBottom();
                });
                
              
                
                messages.Add(new OpenAI.Chat.Message(Role.Assistant, aimessage.Content));
                
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
       
    }
    
    public override async void Continue()
    {
       
    }
    
    public override async void SendMessage()
    {
        if (CurrentChat?.Title?.Length == 0)
            CurrentChat.Title = CurrentMessage.Length > 30 ? CurrentMessage.Substring(0, 30) + " .." : CurrentMessage;
        
        signalEvent.Set();
    }
}