using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
public class PlayerSelect : MonoBehaviour
{
    private UIDocument document;
    private ScrollView playerList;
    private DropdownField difficultySelector;
    public GameObject MainMenuUI;
    public GameObject GameOverUI;
    private Label errorText;
    private List<Label> labels = new List<Label>();
    private List<TempInfo.playerData> players = new List<TempInfo.playerData>();
    private Button addPlayerButton;
    private Button addPlayerSubmitButton;
    private Button backButton;
    private VisualElement newPlayerForm;
    private Button addPlayerCancelButton;
    private TextField addPlayerInput;
    private Button startButton;
    private static string settingsPath = "settings.json";
    private static string playerDataPath = "PlayerData";
    private Settings settings = new Settings();
    private void Start()
    {
        buildPlayerList();
        addPlayerButton = document.rootVisualElement.Q("addPlayerButton") as Button;
        startButton = document.rootVisualElement.Q("startButton") as Button;
        addPlayerSubmitButton = document.rootVisualElement.Q("addPlayerSubmitButton") as Button;
        addPlayerCancelButton = document.rootVisualElement.Q("addPlayerCancelButton") as Button;
        backButton = document.rootVisualElement.Q("backButton") as Button;
        addPlayerInput = document.rootVisualElement.Q("playerNameInput") as TextField;
        newPlayerForm = document.rootVisualElement.Q("newPlayerForm") as VisualElement;
        difficultySelector = document.rootVisualElement.Q("difficultySelector") as DropdownField;
        errorText = document.rootVisualElement.Q("errorText") as Label;
        addPlayerButton.RegisterCallback<ClickEvent>(OnAddPlayerClick);
        backButton.RegisterCallback<ClickEvent>(OnBackClick);
        addPlayerSubmitButton.RegisterCallback<ClickEvent>(OnAddPlayerSubmitClick);
        addPlayerCancelButton.RegisterCallback<ClickEvent>(OnAddPlayerCancelClick);
        startButton.RegisterCallback<ClickEvent>(OnStartGame);
    }
    private void OnEnable()
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
        buildPlayerList();
        addPlayerButton = document.rootVisualElement.Q("addPlayerButton") as Button;
        startButton = document.rootVisualElement.Q("startButton") as Button;
        addPlayerSubmitButton = document.rootVisualElement.Q("addPlayerSubmitButton") as Button;
        addPlayerCancelButton = document.rootVisualElement.Q("addPlayerCancelButton") as Button;
        backButton = document.rootVisualElement.Q("backButton") as Button;
        addPlayerInput = document.rootVisualElement.Q("playerNameInput") as TextField;
        newPlayerForm = document.rootVisualElement.Q("newPlayerForm") as VisualElement;
        difficultySelector = document.rootVisualElement.Q("difficultySelector") as DropdownField;
        errorText = document.rootVisualElement.Q("errorText") as Label;
        addPlayerButton.RegisterCallback<ClickEvent>(OnAddPlayerClick);
        backButton.RegisterCallback<ClickEvent>(OnBackClick);
        addPlayerSubmitButton.RegisterCallback<ClickEvent>(OnAddPlayerSubmitClick);
        addPlayerCancelButton.RegisterCallback<ClickEvent>(OnAddPlayerCancelClick);
        startButton.RegisterCallback<ClickEvent>(OnStartGame);
    }

    private void buildPlayerList()
    {
        document = GetComponent<UIDocument>();
        playerList = document.rootVisualElement.Q("playerList") as ScrollView;
        if (!Directory.Exists(playerDataPath))
        {
            Directory.CreateDirectory(playerDataPath);
        }
        if (Directory.Exists(playerDataPath))
        {
            // Load player files
            string[] files = Directory.GetFiles(playerDataPath);
            playerList.Clear();
            labels.Clear();
            players.Clear();
            // Iterate through the files and create labels
            for (int i = 0; i < files.Length; i++)
            {
                string filePath = files[i];
                // Extract just the file name from the full path
                string fileName = Path.GetFileName(filePath);

                // Create a new label element
                Label fileLabel = new Label(fileName);
                string jsonContent = File.ReadAllText(filePath);
                Player player = JsonUtility.FromJson<Player>(jsonContent);
                players.Add(new TempInfo.playerData(player.username, player.id));
                fileLabel.text = player.username + "\nHighscore: " + player.highScore;
                fileLabel.AddToClassList("selectmenu-player");
                fileLabel.name = player.username;
                if (TempInfo.playerId == player.id)
                {
                    fileLabel.AddToClassList("player-selected");
                }
                // Add the label to the ScrollView
                playerList.Add(fileLabel);
                labels.Add(fileLabel);
                // Listen for click
                fileLabel.RegisterCallback<ClickEvent>(evt => OnPlayerClick(player.id));
            }
            TempInfo.playerIndex = players.Count;
        }
        else
        {
            TempInfo.gameState = TempInfo.GameState.MainMenu;
            MainMenuUI.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    // private void OnDisable()
    // {
    //     for (int i = 0; i < labels.Count; i++)
    //     {
    //         labels[i].UnregisterCallback<ClickEvent>(OnCloseClick);
    //     }
    // }
    private void OnPlayerClick(int id)
    {
        // print("test: " + id);
        // Check if selected player is already selected
        if (TempInfo.playerId != id)
        {
            for (int i = 0; i < labels.Count; i++)
            {
                // Set all other labels to not selected and set new player to selected
                if (labels[i].name == players.Find(p => p.id == id).username)
                {
                    labels[i].AddToClassList("player-selected");
                    TempInfo.playerUsername = players[i].username;
                    TempInfo.playerId = players[i].id;
                }
                else
                {
                    labels[i].RemoveFromClassList("player-selected");
                }
            }
        }
    }
    private void OnAddPlayerClick(ClickEvent evt)
    {
        if (newPlayerForm.ClassListContains("hide"))
        {
            // Show addPlayerForm
            newPlayerForm.RemoveFromClassList("hide");
        }
    }
    private void OnAddPlayerCancelClick(ClickEvent evt)
    {
        if (!newPlayerForm.ClassListContains("hide"))
        {
            // Show addPlayerForm
            addPlayerInput.value = "";
            newPlayerForm.AddToClassList("hide");
        }
    }
    private void OnAddPlayerSubmitClick(ClickEvent evt)
    {
        if (!newPlayerForm.ClassListContains("hide"))
        {
            // Get input value
            string newName = addPlayerInput.value;
            if (newName != "")
            {
                // Create new player
                Player player = new Player(newName);
                player.SavePlayerData();
                settings.loadSettings();
                // Close player creation form and regenerate player list
                addPlayerInput.value = "";
                newPlayerForm.AddToClassList("hide");
                buildPlayerList();
            }
        }
    }
    private void OnBackClick(ClickEvent evt)
    {
        TempInfo.gameState = TempInfo.GameState.MainMenu;
        MainMenuUI.SetActive(true);
        gameObject.SetActive(false);
    }
    private void OnStartGame(ClickEvent evt)
    {
        if (difficultySelector.value == null)
        {
            errorText.text = "Pick a difficulty!";
            errorText.RemoveFromClassList("hide");
            return;
        }
        if (TempInfo.playerUsername == "")
        {
            errorText.text = "Pick a player!";
            errorText.RemoveFromClassList("hide");
            return;
        }
        TempInfo.difficulty = difficultySelector.value;
        settings.saveSettings();
        TempInfo.gameState = TempInfo.GameState.InGame;
        SceneManager.LoadScene("InGame");
    }
}
