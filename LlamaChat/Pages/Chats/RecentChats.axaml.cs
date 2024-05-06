using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using LlamaChatBackend;
using LlamaChatBackend.Classes;
using Newtonsoft.Json;

namespace LlamaChat.Pages.Chats;

public partial class RecentChats : UserControl
{
    public RecentChats()
    {
        InitializeComponent();
    }

    private void NewChatFromHistory(object? sender, RoutedEventArgs e)
    {
        Chat chat = JsonConvert.DeserializeObject<Chat>(File.ReadAllText(@"chats\\" + ((Button)sender).Tag));
        ChatVM.Instance.InitChat("",chat);
    }
}

public class FileNameConverter : IValueConverter
{
   

    public object? Convert( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        if (value is null)
            return "";
        
        return ((string)value).Split("####").First() ;
    }

    public object ConvertBack( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        return value;
    }
}