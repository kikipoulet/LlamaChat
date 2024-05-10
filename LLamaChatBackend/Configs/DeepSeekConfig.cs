using MvvmStupidPocHelper;

namespace LLamaChatBackend.Configs;

public class DeepSeekConfig : PersistentSingleton<DeepSeekConfig>
{
    public string APIKEY { get; set; } = "";
}