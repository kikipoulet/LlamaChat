using Avalonia;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Rendering.Composition;
using Avalonia.Styling;
using Avalonia.Threading;

namespace LLamaChatBackend;

public static class ControlExtensions
{
    public static void Animate<T>(this Animatable control, AvaloniaProperty Property, T from, T to, TimeSpan duration, ulong count = 1)
    {
        new Avalonia.Animation.Animation
        {
            Duration = duration,
            FillMode = FillMode.Forward,
            Easing = new CubicEaseInOut(),
            IterationCount = new IterationCount(count),
            PlaybackDirection = PlaybackDirection.Normal,
            Children =
            {
                new KeyFrame()
                {
                    Setters = { new Setter { Property = Property, Value = from } },
                    KeyTime = TimeSpan.FromSeconds(0)
                },
                new KeyFrame()
                {
                    Setters = { new Setter { Property = Property, Value = to } },
                    KeyTime = duration
                }
            }
        }.RunAsync(control);
    }
}

public static class AnimationExtensions
{

    static AnimationExtensions()
    {

    }
    
    
    public static readonly AttachedProperty<double> FadeInProperty = AvaloniaProperty.RegisterAttached<Control, double>("FadeIn", typeof(Control), defaultValue: 0);    
 

    public static double GetFadeIn(Control wrap)
    {
        return wrap.GetValue(FadeInProperty);
    }

    public static void SetFadeIn(Control interactElem, double value)
    {
        if (value > 0)
        {
            interactElem.Opacity = 0;
            interactElem.AttachedToVisualTree += (sender, args) =>
            {
                interactElem.Animate<double>(Control.OpacityProperty, 0,1, TimeSpan.FromMilliseconds(value));
               // interactElem.Animate<Transform>(Control.RenderTransformProperty, new ScaleTransform(2,1),new ScaleTransform(1,1), TimeSpan.FromMilliseconds(value));
            };
        }
        interactElem.SetValue(FadeInProperty, value);
    }

  
}