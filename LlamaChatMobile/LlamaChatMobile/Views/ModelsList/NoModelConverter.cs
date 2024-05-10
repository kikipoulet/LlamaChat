using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Avalonia.Data;
using Avalonia.Data.Converters;
using LlamaChat.Classes;

namespace LlamaChatMobile.Views.ModelsList;

public class NoModelConverter : IValueConverter
{
    

    public object? Convert(object? value, Type targetType, object? parameter, 
        CultureInfo culture)
    {
       return !((List<AiModel>)value).Any(model => model.IsInstalled);
       
    }

    public object ConvertBack(object? value, Type targetType, 
        object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}