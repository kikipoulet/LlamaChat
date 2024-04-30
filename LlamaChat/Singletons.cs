using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MvvmStupidPocHelper;

public class Singleton<T> : ObservableObject where T : class, new()
{
    private static T _instance;

    public static T Instance 
    {
        get
        {
            if (_instance == null)
                _instance = new T();

            return _instance;
        }
    }
}

public class PersistentSingleton<T> : ObservableObject where T : class, new()
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                
                var path = "DB" + Path.DirectorySeparatorChar +   typeof(T).Name;

                var file = new FileInfo(path);
                if (file.Exists)
                {
                    _instance = JsonSerializer.Deserialize<T>(File.ReadAllText(path));
                }
                else
                {
                    _instance = new T(); 
                }
            }
            

            return _instance;
        }
    }

    public void Save()
    {
        SaveS();
    }
    
    private static void SaveS()
    {

        var dir = new DirectoryInfo("DB");

        if (!dir.Exists)
            dir.Create();

        var path = "DB" + Path.DirectorySeparatorChar +  typeof(T).Name;

        var file = new FileInfo(path);
        if(file.Exists)
            file.Delete();

        File.WriteAllText(path, JsonSerializer.Serialize(_instance));
    }
}