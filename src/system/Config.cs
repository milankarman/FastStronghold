using System.Configuration;

public static class Config
{
    public static bool WriteOutputToFile { private set; get; }
    public static bool ShowNetherTravelSuggestion { private set; get; }
    public static bool AlwaysOnTop { private set; get; }

    public static void Initialize()
    {
        WriteOutputToFile = bool.Parse(ConfigurationManager.AppSettings.Get("WriteOutputToFile"));
        ShowNetherTravelSuggestion = bool.Parse(ConfigurationManager.AppSettings.Get("ShowNetherTravelSuggestion"));
        AlwaysOnTop = bool.Parse(ConfigurationManager.AppSettings.Get("AlwaysOnTop"));
    }
}