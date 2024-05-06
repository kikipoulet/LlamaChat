using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using LlamaChat.Classes;
using MvvmStupidPocHelper;

namespace LlamaChat.Pages;

public partial class ResourcesVM : Singleton<ResourcesVM>
{

[ObservableProperty] private ObservableCollection<string> recentChats = GetChats();

public static ObservableCollection<string> GetChats()
{
    if(!new DirectoryInfo("chats").Exists)
        new DirectoryInfo("chats").Create();
            
    var collection = new ObservableCollection<string>(new DirectoryInfo("chats").GetFiles().Select(file => file.Name));
    return collection;
}


    [ObservableProperty] private ObservableCollection<string> models = GetModels();

    public static ObservableCollection<string> GetModels()
    {
        if(!new DirectoryInfo("models").Exists)
            new DirectoryInfo("models").Create();
            
        return new ObservableCollection<string>(new DirectoryInfo("models").GetFiles().Select(file => file.Name));
    }


    public List<AiModel> ModelsToDownload { get; } = new List<AiModel>()
    {
        new AiModel()
        {
            Name = "Phi 3 Mini 4k Instruct",
            DownloadURl = "https://huggingface.co/microsoft/Phi-3-mini-4k-instruct-gguf/resolve/main/Phi-3-mini-4k-instruct-q4.gguf?download=true",
            Size = "4B",
            Description = "The Phi-3-Mini-4K-Instruct is a 3.8B parameters, lightweight, state-of-the-art open model trained with the Phi-3 datasets."
        },
       
        new AiModel()
        {
            Name = "Llama 3 8B Instruct",
            DownloadURl = "https://huggingface.co/bartowski/Meta-Llama-3-8B-Instruct-GGUF/resolve/main/Meta-Llama-3-8B-Instruct-Q5_K_M.gguf?download=true",
            Size = "8B",
            Description = "The Llama 3 instruction tuned models are optimized for dialogue use cases and outperform many of the available open source chat models on common industry benchmarks."
        },
        
        new AiModel()
        {
            Name = "WizardLM 2 7B",
            DownloadURl = "https://huggingface.co/lmstudio-community/WizardLM-2-7B-GGUF/resolve/main/WizardLM-2-7B-Q6_K.gguf?download=true",
            Size = "7B",
            Description = "This model is trained to excel at multi-turn conversations, and does so very successfully, outclassing models more than twice its size."
        },
        
        new AiModel()
        {
            Name = "Wavecoder Ultra 6.7B",
            DownloadURl = "https://huggingface.co/lmstudio-community/wavecoder-ultra-6.7b-GGUF/resolve/main/wavecoder-ultra-6.7b-Q6_K.gguf?download=true",
            Size = "6.7B",
            Description = "WaveCoder ultra is a coding model. It has exceptional generalization ability across different code-related tasks and has a high efficiency in generation."
        },
        
     

        
    };

}