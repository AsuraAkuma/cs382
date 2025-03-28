static class GameController
{
    static public int gameState = State.NotPlaying; // The current state of the game. Possible values: NotPlaying, Playing, GameOver, LevelComplete
    static public Level currentLevel; // The current level being played. This will be set when a level is loaded.
}