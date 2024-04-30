using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using LLama;
using LLama.Common;
using LlamaChat.Classes;
using SukiUI.Controls;

namespace LlamaChat.Pages.Chats;

public partial class EmptyChatView : UserControl
{
    public EmptyChatView()
    {
        InitializeComponent();
    }

    private void newChat(object? sender, RoutedEventArgs e)
    {
  
        var modelName = this.Get<ListBox>("ListModels").SelectedItem.ToString();

        ChatVM.Instance.InitChat(@"models\" + modelName);
     
        
    }
}


public class BoolToDoubleConverter : IValueConverter
{
    public static readonly BoolToDoubleConverter Instance = new();

    public object? Convert( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        return ((bool)value) ? 1 : 0;
    }

    public object ConvertBack( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        return value;
    }
}

public class NullToDoubleConverter : IValueConverter
{
    public static readonly BoolToDoubleConverter Instance = new();

    public object? Convert( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        return value is null ? 0 : 1;
    }

    public object ConvertBack( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        return value;
    }
}

public class NotNullToDoubleConverter : IValueConverter
{
    public static readonly BoolToDoubleConverter Instance = new();

    public object? Convert( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        return value is null ? 1 : 0;
    }

    public object ConvertBack( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        return value;
    }
}


public class FilenameToStringConverter : IValueConverter
{

    public object? Convert( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        return value is null ? "" : ((string)value).Split('.').First().Replace('-', ' ');
    }

    public object ConvertBack( object? value, Type targetType, object? parameter, CultureInfo culture )
    {
        return value;
    }
}