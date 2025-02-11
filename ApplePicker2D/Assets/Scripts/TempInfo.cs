
public static class TempInfo
{
    public static int gameState { get; set; } = 0;
    public static string playerUsername { get; set; } = "";
    public static int playerId { get; set; } = -1;
    public static int playerIndex { get; set; } = 0;
    public static string difficulty { get; set; } = "";
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
    public struct GameState
    {
        public static readonly int MainMenu = 0;
        public static readonly int PlayerSelect = 1;
        public static readonly int InGame = 2;
        public static readonly int GameOver = 3;
        public static readonly int Settings = 4;
    }
}