using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json.Nodes;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using LlamaChat.Classes;
using LlamaChatBackend;
using LLamaChatBackend.Classes;
using MvvmStupidPocHelper;
using Newtonsoft.Json;
using Xamarin.Essentials;

namespace LlamaChat.Pages;

public partial class ResourcesVM : Singleton<ResourcesVM>
{
    public ScrollViewer ChatScroll = null;
    
    [ObservableProperty] private ChatProvider chatVM = new LlamaChatVM();  
    
    [ObservableProperty] private ObservableCollection<Prompt> prompts = GetPrompts();
    [ObservableProperty] private int selectedPrompt = 0;

    public static ObservableCollection<Prompt> GetPrompts()
    {
        var collection =  new ObservableCollection<Prompt>()
        {
            new Prompt()
            {
                Name = "Assistant",
                Content =
                    "Transcript of a dialog, where the User interacts with you, its Assistant. You are helpful, kind, honest, good at writing, and never fails to answer the User's requests immediately and with precision. You are an expert in various scientific disciplines, including physics, chemistry, and biology. Explain scientific concepts, theories, and phenomena in an engaging and accessible way. Use lots of emojis in your answers."
            }
        };

        var d = new DirectoryInfo(GetRootPath() + "Prompts");
        if(!d.Exists)
            d.Create();
        
        foreach (var fileInfo in d.GetFiles())
            try
            {
                collection.Add(JsonConvert.DeserializeObject<Prompt>(File.ReadAllText(fileInfo.FullName)));
            }catch{}
            

        return collection;
    }

    [ObservableProperty] private ObservableCollection<string> recentChats = GetChats();

public static string GetRootPath()
{
    if (OperatingSystem.IsAndroid())
        return FileSystem.AppDataDirectory + @"\";

    return "";
}

public static ObservableCollection<string> GetChats()
{
    if(!new DirectoryInfo(GetRootPath() +"chats").Exists)
        new DirectoryInfo(GetRootPath() + "chats").Create();
            
    var collection = new ObservableCollection<string>(new DirectoryInfo(GetRootPath() + "chats").GetFiles().Select(file => file.Name));
    return collection;
}


    [ObservableProperty] private ObservableCollection<string> models = GetModels();

    public static ObservableCollection<string> GetModels()
    {
        if(!new DirectoryInfo(GetRootPath() + "models").Exists)
            new DirectoryInfo(GetRootPath() +"models").Create();
            
        return new ObservableCollection<string>(new DirectoryInfo(GetRootPath() +"models").GetFiles().Select(file => file.Name));
    }


  
    public List<AiModel> ModelsToDownload { get; } = GetModelsToDownload();

    private static List<AiModel> GetModelsToDownload()
    {
        
        return new List<AiModel>()
        {
            new AiModel()
            {
                Name = "Phi 3 Mini 4k Instruct",
                DownloadURl = "https://huggingface.co/microsoft/Phi-3-mini-4k-instruct-gguf/resolve/main/Phi-3-mini-4k-instruct-q4.gguf?download=true",
                Size = "4B",
                Description = "The Phi-3-Mini-4K-Instruct is a 3.8B parameters, lightweight, state-of-the-art open model trained with the Phi-3 datasets.",
                LightDescription = "Phi-3-Mini-4K-Instruct is a 3.8B parameters, lightweight model."
            },
       
            new AiModel()
            {
                Name = "Llama 3 8B Instruct",
                DownloadURl = "https://huggingface.co/bartowski/Meta-Llama-3-8B-Instruct-GGUF/resolve/main/Meta-Llama-3-8B-Instruct-Q5_K_M.gguf?download=true",
                Size = "8B",
                Description = "The Llama 3 instruction tuned models are optimized for dialogue use cases and outperform many of the available open source chat models on common industry benchmarks."
                ,  LightDescription = "Llama 3 instruction is last model by Meta optimized for dialogue."
            },

        };

    }


   

}