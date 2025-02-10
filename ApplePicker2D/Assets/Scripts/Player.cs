using System.IO;
using UnityEngine;

[System.Serializable] // Allows serialization
class Player
{
    public string username;
    public int score;
    public int highScore = 0;
    public int id = -1;
    private Settings settings = new Settings(new Settings.playerData());
    private static string settingsPath = "settings.json";
    public void SavePlayerData()
    {
        if (id == -1)
        {
            // Create or load settings
            if (File.Exists(settingsPath))
            {
                settings.loadSettings();
            }
            else
            {
                settings.saveSettings();
            }
            id = settings.playerIndex;
            settings.playerIndex++;
            settings.saveSettings();
        }
        string path = "PlayerData/" + username + ".json";
        SaveSystem.Save(path, this);
    }

    public void LoadPlayerData()
    {
        string path = "PlayerData/" + username + ".json";
        SaveSystem.Load(path, this);
    }

    public Player()
    {

    }
}
