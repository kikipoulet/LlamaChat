using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using CherylUI;
using LlamaChat.Pages;

namespace LlamaChatMobile.Views.ChatViewModel;

public partial class ChatView : UserControl
{
    public ChatView()
    {
        InitializeComponent();
        ResourcesVM.Instance.ChatScroll= this.FindControl<ScrollViewer>("MonScrollViewer");
    }
    
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);

        var itemscontrol = this.Get<ItemsControl>("IC");
        
        itemscontrol.AttachedToVisualTree += (sender, args) =>
        {
            var compositionVisual = ElementComposition.GetElementVisual(itemscontrol);

            if (compositionVisual == null)
                return;

            Compositor compositor = compositionVisual.Compositor;

            var animationGroup = compositor.CreateAnimationGroup();
            Vector3KeyFrameAnimation offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.Target = "Offset";

            offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(400);

            ImplicitAnimationCollection implicitAnimationCollection = compositor.CreateImplicitAnimationCollection();
            animationGroup.Add(offsetAnimation);
            implicitAnimationCollection["Offset"] = animationGroup;
            compositionVisual.ImplicitAnimations = implicitAnimationCollection;
        };

       

    }
    
    private void Animate(object? sender, RoutedEventArgs e)
    { 
        ((Grid)sender).Animate<double>(OpacityProperty, 0 ,1 );
    }

}