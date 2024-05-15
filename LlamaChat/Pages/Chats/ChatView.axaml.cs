using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Animation;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Rendering.Composition;
using Avalonia.Rendering.Composition.Animations;
using LlamaChatBackend;
using SukiUI;
using SukiUI.Controls;

namespace LlamaChat.Pages.Chats;

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
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(500);

            ImplicitAnimationCollection implicitAnimationCollection = compositor.CreateImplicitAnimationCollection();
            animationGroup.Add(offsetAnimation);
            implicitAnimationCollection["Offset"] = animationGroup;
            compositionVisual.ImplicitAnimations = implicitAnimationCollection;
        };

       

    }
    
    

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        Button image = sender as Button;
        ContextMenu contextMenu = image.ContextMenu;
        contextMenu.PlacementTarget = image;
        contextMenu.Open();
       
    }

    private void newChatClick(object? sender, RoutedEventArgs e)
    {
        this.StackNavigationPop();
    }

    private void SaveChat(object? sender, RoutedEventArgs e)
    {
            ((ChatProvider)DataContext).SaveChat();
            ResourcesVM.Instance.RecentChats = ResourcesVM.GetChats();
        SukiHost.ShowToast("Success !", "Your chat has been saved.");
    }

    private void stopgenclick(object? sender, RoutedEventArgs e)
    { 
        ((Button)sender).Animate<double>(OpacityProperty,1,0);
    }

    private void showcopied(object? sender, RoutedEventArgs e)
    {
        var ctl = sender as Control;
        if (ctl != null)
        {
            FlyoutBase.ShowAttachedFlyout(ctl);
        }
    }

  
}

