using Android.App;
using Android.Content.PM;
using Avalonia.Android;

namespace LlamaChatMobile.Android;

[Activity(Label = "LlamaChatMobile.Android", Theme = "@style/MyTheme.NoActionBar", Icon = "@drawable/icon",
     MainLauncher = true, ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
public class MainActivity : AvaloniaMainActivity<App>
{ 
}