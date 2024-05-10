using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Styling;

namespace LlamaChatMobile;


public partial class WaveProgress : UserControl
{
    public WaveProgress()
    {
        InitializeComponent();
     
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private double _value = 50;

    public double Value
    {
        get => _value;
        set
        {
            if (value is < 0 or > 100)
                return;

            SetValue(ValueProperty,value);
        }
    }

    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<WaveProgress, double>(nameof(Value), defaultValue: 50);
    
   

    public static readonly StyledProperty<bool> IsTextVisibleProperty = AvaloniaProperty.Register<WaveProgress, bool>(nameof(IsTextVisible), defaultValue: true);

    public bool IsTextVisible
    {
        get { return GetValue(IsTextVisibleProperty); }
        set
        {
            SetValue(IsTextVisibleProperty, value);
        }
    }
}


public class WaveProgressValueConverter : IValueConverter
{
    public static readonly WaveProgressValueConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double i) return 0;
        return 155 - i * 2.1;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class WaveProgressValueColorConverter : IValueConverter
{
    public static readonly WaveProgressValueColorConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double d) return Brushes.Black;
        if (d > 50) return Brushes.GhostWhite;
        return Application.Current?.ActualThemeVariant == ThemeVariant.Dark ? Brushes.GhostWhite : Brushes.Black;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class WaveProgressValueTextConverter : IValueConverter
{
    public static readonly WaveProgressValueTextConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value is not double d ? "0%" : $"{d:#0}%";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public class WaveProgressGradientOffsetConverter : IValueConverter
{
    public static readonly WaveProgressGradientOffsetConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not double v)
            return Brushes.Blue;

        Color PrimaryColor = Colors.DodgerBlue;
        Color AccentColor = Colors.DimGray;
        

        return new LinearGradientBrush()
        {
            EndPoint = new RelativePoint(0.5, 1, RelativeUnit.Relative),
            StartPoint = new RelativePoint(0.5, 0, RelativeUnit.Relative),
            GradientStops = new GradientStops()
            {
                new GradientStop() { Color = PrimaryColor, Offset = 0 },
                new GradientStop() { Color = AccentColor, Offset = 0.5 }
            }
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}