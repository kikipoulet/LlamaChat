using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using LlamaChat.Classes;
using SukiUI.Controls;

namespace LlamaChat.Pages.Models;

public partial class ModelsView : UserControl
{
    public ModelsView()
    {
        InitializeComponent();
    }

    private void DownloadModel(object? sender, RoutedEventArgs e)
    {
        AiModel model = (AiModel)((Button)sender).Tag;

        string name = model.Name + ".gguf";
        string url = model.DownloadURl;
        string destinationFolder = @"models/"; // Dossier de destination où le fichier sera enregistré

        model.IsDownloading = true;

        Task.Run(() =>
        {
            WebClient client = new WebClient();
            
            client.DownloadProgressChanged += (sender, e) =>
            {
                double bytesIn = double.Parse(e.BytesReceived.ToString());
                double totalBytes = double.Parse(e.TotalBytesToReceive.ToString());
                double percentage = bytesIn / totalBytes * 100;
                model.DownloadValue = (int)percentage;
            };


            client.DownloadFileCompleted += (sender, e) =>
            {
                model.IsDownloading = false;
               
                ResourcesVM.Instance.Models = new ObservableCollection<string>(new DirectoryInfo("models").GetFiles().Select(file => file.Name));
            };

            try
            {
                client.DownloadFileAsync(new Uri(url), destinationFolder + name);
            }
            catch (Exception ex)
            {
                Dispatcher.UIThread.Invoke(() =>
                {
                    SukiHost.ShowToast("Download Error", ex.Message);
                });
            }

           

        });
    }
}