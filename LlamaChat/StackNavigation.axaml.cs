using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Controls.Templates;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Avalonia.VisualTree;
using SukiUI;

namespace LlamaChat.Pages.Chats;

public partial class StackNavigation : UserControl
{
    
        public StackNavigation()
        {
            InitializeComponent();
        }

     
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void PopPage()
        {    
            if (Pages.Count == 0)
                return;

            var page = Pages.Pop();

            Content = page;
            CurrentPage = page;
        }
     
        public Control CurrentPage;




        public Stack<Control> Pages = new Stack<Control>();
        

}

public static class StackNavigationExtensions{
    public static void StackNavigationPop(this UserControl u)
    {
        var instance = u.FindAncestorOfType<StackNavigation>();
            
        if (instance.Pages.Count == 0)
            return;

        var page = instance.Pages.Pop();
           
            
        instance.CurrentPage = page;
        var x = instance.GetTemplateChildren();
        TransitioningContentControl baseCT = (TransitioningContentControl) x.First(f => f.Name == "base");
        baseCT.Content = page;
    }
    
    public static void StackNavigationPush(this UserControl u, UserControl content,bool DisableComeBack = false)
    {
       var instance = u.FindAncestorOfType<StackNavigation>();
       
       if(instance.CurrentPage == null)
           instance.CurrentPage =(Control)instance.Content  ;

       instance.Pages.Push(instance.CurrentPage);

       if (DisableComeBack)
           instance.Pages.Clear();

       var m =  content ;
       instance.CurrentPage = m;
            
       var x = instance.GetTemplateChildren();
       TransitioningContentControl baseCT = (TransitioningContentControl) x.First(f => f.Name == "base");
       baseCT.Content = m;
    }
}