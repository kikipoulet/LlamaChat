using System.IO;
using CommunityToolkit.Mvvm.ComponentModel;

namespace LlamaChat.Classes;

public partial class AiModel : ObservableObject
{
    public string Name { get; set; }

    public bool IsInstalled
    {
        get { return new FileInfo("models/" + Name + ".gguf").Exists; }
    }

    public string DownloadURl { get; set; }
    
    public string Description { get; set; }
    
    public string Size { get; set; }

    [ObservableProperty] private bool isDownloading = false;
    [ObservableProperty] private int downloadValue = 0;
}