using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LLamaChatBackend.Classes;
using Newtonsoft.Json;
using SukiUI.Controls;

namespace LlamaChat.Pages.Chats;

public partial class CreatepromptView : UserControl
{
    public CreatepromptView()
    {
        InitializeComponent();
    }

    private void Create(object? sender, RoutedEventArgs e)
    {
        var prompt = new Prompt()
        {
            Name = this.Get<TextBox>("PromptName").Text,
            Content = this.Get<TextBox>("PromptContent").Text,
        };
        
        File.WriteAllText(ResourcesVM.GetRootPath() + @"Prompts\" + prompt.Name, JsonConvert.SerializeObject(prompt));

        ResourcesVM.Instance.Prompts = ResourcesVM.GetPrompts();
        SukiHost.CloseDialog();
    }
}