using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using MvvmStupidPocHelper;

namespace LlamaChatMobile.Views;

public partial class SettingsVM : Singleton<SettingsVM>
{
    [ObservableProperty] private bool darkMode = Application.Current.RequestedThemeVariant != ThemeVariant.Dark;
    
    partial void OnDarkModeChanged(bool value)
    {
            Application.Current.RequestedThemeVariant = value ? ThemeVariant.Dark : ThemeVariant.Light;
    }
}