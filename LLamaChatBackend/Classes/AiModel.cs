using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using LlamaChat.Pages;

namespace LlamaChat.Classes;

public partial class AiModel : ObservableObject
{
    public string Name { get; set; }

    public bool IsInstalled
    {
        get {


            return  new FileInfo(ResourcesVM.GetRootPath() + "models/" + Name + ".gguf").Exists;
        
        }
    }

    public string DownloadURl { get; set; }
    
    public string Description { get; set; }
    public string LightDescription { get; set; }
    
    public string Size { get; set; }

    [ObservableProperty] private bool isDownloading = false;
    [ObservableProperty] private int downloadValue = 0;
    
 
}