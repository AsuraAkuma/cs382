using System.IO;

[System.Serializable] // Allows serialization
class Settings
{
    private static string settingsPath = "settings.json";
    public int volume = 50;

    public Settings()
    {
        if (File.Exists(settingsPath))
        {
            loadSettings();
        }
        else
        {
            saveSettings();
        }
    }
    public void saveSettings()
    {
        SaveSystem.Save(settingsPath, this);
    }

    public void loadSettings()
    {
        SaveSystem.Load(settingsPath, this);
    }
}
