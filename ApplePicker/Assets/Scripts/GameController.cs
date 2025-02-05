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
            mainScreen.SetActive(true);
            playerSelect.SetActive(false);
            pauseScreen.SetActive(false);
            endScreen.SetActive(false);
        }
        else if (screenName == playerSelect.name)
        {
            mainScreen.SetActive(false);
            playerSelect.SetActive(true);
            pauseScreen.SetActive(false);
            endScreen.SetActive(false);
        }
        else if (screenName == pauseScreen.name)
        {
            mainScreen.SetActive(false);
            playerSelect.SetActive(false);
            pauseScreen.SetActive(true);
            endScreen.SetActive(false);
        }
        else if (screenName == endScreen.name)
        {
            mainScreen.SetActive(false);
            playerSelect.SetActive(false);
            pauseScreen.SetActive(false);
            endScreen.SetActive(true);
        }
    }

    public void startGame()
    {
        // Each "game" will have it's own box, each box acts as an instance for a single player.
        // Generate boxs
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
