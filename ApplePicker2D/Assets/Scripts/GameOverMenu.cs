using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class GamOverMenu : MonoBehaviour
{
    private UIDocument document;
    private Button mainMenuButton;
    private Button playAgainButton;
    private Label playerText;
    private Label scoreText;
    private Label highScoreText;
    public GameObject MainMenuUI;
    public GameObject PlayerSelectUI;
    private static string playerDataPath;
    private void Start()
    {
        playerDataPath = "PlayerData/" + TempInfo.playerUsername + ".json";
        document = GetComponent<UIDocument>();
        mainMenuButton = document.rootVisualElement.Q("mainMenuButton") as Button;
        playAgainButton = document.rootVisualElement.Q("playerSelectButton") as Button;
        playerText = document.rootVisualElement.Q("playerUsername") as Label;
        scoreText = document.rootVisualElement.Q("scoreStat") as Label;
        highScoreText = document.rootVisualElement.Q("highScoreStat") as Label;
        mainMenuButton.RegisterCallback<ClickEvent>(OnMainClick);
        playAgainButton.RegisterCallback<ClickEvent>(OnPlayAgainClick);
        // Display scores and player name
        if (File.Exists(playerDataPath))
        {
            // Create player and get data
            Player player = new Player(TempInfo.playerUsername);
            playerText.text = player.username;
            scoreText.text = player.score.ToString();
            highScoreText.text = player.highScore.ToString();
        }
        else
        {
            playerText.text = "????";
            scoreText.text = "????";
            highScoreText.text = "????";
        }
    }
    private void OnEnable()
    {
        document = GetComponent<UIDocument>();
        mainMenuButton = document.rootVisualElement.Q("mainMenuButton") as Button;
        playAgainButton = document.rootVisualElement.Q("playerSelectButton") as Button;
        mainMenuButton.RegisterCallback<ClickEvent>(OnMainClick);
        playAgainButton.RegisterCallback<ClickEvent>(OnPlayAgainClick);
    }

    private void OnDisable()
    {
        mainMenuButton.UnregisterCallback<ClickEvent>(OnMainClick);
        playAgainButton.UnregisterCallback<ClickEvent>(OnPlayAgainClick);
    }
    private void OnMainClick(ClickEvent evt)
    {
        // print("Settings button pressed");
        TempInfo.gameState = TempInfo.GameState.MainMenu;
        gameObject.SetActive(false);
        MainMenuUI.SetActive(true);
    }
    private void OnPlayAgainClick(ClickEvent evt)
    {
        TempInfo.gameState = TempInfo.GameState.PlayerSelect;
        gameObject.SetActive(false);
        PlayerSelectUI.SetActive(true);
    }
}
