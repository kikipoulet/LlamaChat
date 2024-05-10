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
using Avalonia.Media;
using Avalonia.Threading;
using CherylUI.Controls;
using LlamaChat.Classes;
using LlamaChat.Pages;

namespace LlamaChatMobile.Views.ModelsList;

public partial class DownloadModelDialog : UserControl
{
    public DownloadModelDialog(AiModel odeltodl)
    {
        InitializeComponent();
        this.DataContext = odeltodl;
        
        DownloadModel(odeltodl);
    }
    
    private WebClient client = new WebClient();
    
    public void DownloadModel(AiModel model)
    {
        
        string name = model.Name + ".gguf";
        string url = model.DownloadURl;
        string destinationFolder = ResourcesVM.GetRootPath() +  @"models/"; // Dossier de destination où le fichier sera enregistré

        model.IsDownloading = true;

        Task.Run(() =>
        {
            
            
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

                ResourcesVM.Instance.Models = ResourcesVM.GetModels();

               
                    Dispatcher.UIThread.Post(() =>
                    {
                    try
                    {
                            InteractiveContainer.CloseDialog();
                        }
                        catch { }

                        InteractiveContainer.ShowToast(new TextBlock() { Text = "Download Succeeded !", FontWeight = FontWeight.DemiBold }, 5);
                    });
                   
              
            };

            try
            {
                client.DownloadFileAsync(new Uri(url), destinationFolder + name);
            }
            catch (Exception ex)
            {
               
            }

           

        });
    }

    private void Cancel(object? sender, RoutedEventArgs e)
    {
        client.CancelAsync();
        InteractiveContainer.CloseDialog();
    }
}