public class State
{
    public static int NotPlaying = 0; // Game is not playing, waiting for user to start
    public static int Playing = 1;     // Game is currently playing
    public static int Paused = 2;      // Game is paused, waiting for user to resume or quit
    public static int GameOver = 3;    // Game is over, waiting for user to restart or quit
    public static int TimeFrozen = 4; // Game time is frozen, used for special events or cutscenes
}