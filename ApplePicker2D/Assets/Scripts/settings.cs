[System.Serializable] // Allows serialization
class Settings
{
    private static string settingsPath = "settings.json";
    public int volume;
    public string playerUsername;
    public int playerId;
    public int playerIndex;
    public string difficulty;
    public struct playerData
    {
        public string username;
        public int id;
        public playerData(string username = "", int id = -1)
        {
            this.username = username;
            this.id = id;
        }
    }
    public Settings(playerData player, int volume = 50, int playerIndex = 0, string difficulty = "")
    {
        this.volume = volume;
        playerUsername = player.username;
        playerId = player.id;
        this.playerIndex = playerIndex;
        this.difficulty = difficulty;
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
