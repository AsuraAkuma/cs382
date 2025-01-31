using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool isStarted;
    private Player[] players;
    private int playerIndex;
    public bool isPaused;
    public int scoreMultiplier = 1;
    public GameObject mainScreen;
    public GameObject playerSelect;
    public GameObject pauseScreen;
    public GameObject endScreen;
    private GameObject currentScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentScreen = mainScreen;
    }

    // Update is called once per frame
    void Update()
    {

    }
    // Game Management
    public void showScreen(string screenName)
    {
        if (screenName == mainScreen.name)
        {
        }
        else if (screenName == playerSelect.name)
        {

        }
        else if (screenName == pauseScreen.name)
        {

        }
        else if (screenName == endScreen.name)
        {

        }
    }
    // Player Management
    public void addPlayer(Player player)
    {
        playerIndex++;
        player.id = playerIndex;
        players[players.Length] = player;
    }

    public void removePlayer(Player player)
    {
        Player[] newList = new Player[players.Length - 1];
        for (int i = 0; i < players.Length; i++)
        {
            Player currPlayer = players[i];
            if (currPlayer.id != player.id)
            {
                newList[newList.Length] = currPlayer;
            }
        }
    }
}
