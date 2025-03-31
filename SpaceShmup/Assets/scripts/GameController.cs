using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public Level[] levels; // Array of levels in the game
    public GameObject[] spawnPoints; // Array of spawn points for the aliens
    public GameObject[] alienPrefabs; // Array of alien prefabs to be spawned
    public UIDocument playerUI; // Reference to the UI document for displaying game information
    public UIDocument gameOverUI; // Reference to the UI document for displaying game over information
    public UIDocument levelCompleteUI; // Reference to the UI document for displaying level complete information
    public Button gameOverButton; // Reference to the Game Over button
    void Awake()
    {
        levels = new Level[5]; // Initialize the levels array with 5 levels

        Wave[] wavesLvl1 = {
            CreateWave(1, 5, 1.0f, 0.5f, AlienType.scout),
            CreateWave(2, 10, 1.0f, 0.5f, AlienType.scout),
            CreateWave(3, 15, 1.0f, 0.5f, AlienType.scout)
        };
        Wave[] wavesLvl2 = {
            CreateWave(1, 5, 0.5f, 0.5f, AlienType.warrior),
            CreateWave(2, 10, 0.5f, 0.5f, AlienType.warrior),
            CreateWave(3, 15, 0.5f, 0.5f, AlienType.warrior)
        };
        Wave[] wavesLvl3 = {
            CreateWave(1, 5, 0.5f, 0.5f, AlienType.heavy),
            CreateWave(2, 10, 0.5f, 0.5f, AlienType.heavy),
            CreateWave(3, 15, 0.5f, 0.5f, AlienType.heavy)
        };
        Wave[] wavesLvl4 = {
            CreateWave(1, 5, 0.5f, 0.5f, AlienType.sniper),
            CreateWave(2, 10, 0.5f, 0.5f, AlienType.sniper),
            CreateWave(3, 15, 0.5f, 0.5f, AlienType.sniper)
        };
        Wave[] wavesLvl5 = {
            CreateWave(1, 15, 1.0f, 0.5f, AlienType.scout),
            CreateWave(2, 15, 0.5f, 0.5f, AlienType.warrior),
            CreateWave(3, 15, 0.5f, 0.5f, AlienType.heavy),
            CreateWave(4, 15, 0.5f, 0.5f, AlienType.sniper),
            CreateWave(5, 1, 0.5f, 0.5f, AlienType.boss)
        };

        levels[0] = CreateLevel(1, 3, 60, wavesLvl1);
        levels[1] = CreateLevel(2, 3, 60, wavesLvl2);
        levels[2] = CreateLevel(3, 3, 60, wavesLvl3);
        levels[3] = CreateLevel(4, 3, 60, wavesLvl4);
        levels[4] = CreateLevel(5, 3, 60, wavesLvl5);

        if (spawnPoints[0] == null || spawnPoints[1] == null || spawnPoints[2] == null || spawnPoints[3] == null)
        {
            Debug.LogError("Spawn points not found! Please ensure they are set up in the scene.");
        }
        else
        {
            Debug.Log("Spawn points initialized successfully!"); // Log a message indicating successful initialization of spawn points
        }

        if (gameOverUI == null)
        {
            Debug.LogError("GameOverUI is not assigned in the Inspector!");
            return;
        }

        gameOverButton = gameOverUI.rootVisualElement.Q<Button>("StartOverButton");
        if (gameOverButton == null)
        {
            Debug.LogError("StartOverButton not found in GameOverUI!");
        }
        else
        {
            gameOverUI.rootVisualElement.style.display = DisplayStyle.None; // Hide the game over UI initially
        }
    }

    void OnEnable()
    {
        gameOverButton.RegisterCallback<ClickEvent>(ev => OnGameOverButtonClicked()); // Register the button click event
    }

    void OnDisable()
    {
        gameOverButton.UnregisterCallback<ClickEvent>(ev => OnGameOverButtonClicked()); // Unregister the button click event
    }
    private Wave CreateWave(int waveNumber, int numberOfAliens, float spawnInterval, float spawnDelay, AlienType.AlienTypeData alienType)
    {
        // Create a new GameObject for the wave
        GameObject waveObject = new GameObject($"Wave_{waveNumber}");
        Wave wave = waveObject.AddComponent<Wave>();

        // Initialize the wave properties
        wave.WaveNumber = waveNumber;
        wave.NumberOfAliens = numberOfAliens;
        wave.SpawnInterval = spawnInterval;
        wave.SpawnDelay = spawnDelay;
        wave.AlienType = alienType;
        wave.gameController = this; // Set the reference to the GameController
        wave.spawnPoints = spawnPoints; // Set the spawn points for the wave
        wave.TotalAliens = numberOfAliens; // Set the total number of aliens for this wave
        Debug.Log($"Wave {waveNumber} created with {numberOfAliens} aliens of type {alienType}.");
        return wave;
    }

    private Level CreateLevel(int levelNumber, int totalLives, int totalSeconds, Wave[] waves)
    {
        // Create a new GameObject for the level
        GameObject levelObject = new GameObject($"Level_{levelNumber}");
        Level level = levelObject.AddComponent<Level>();

        // Initialize the level properties
        level.totalLives = totalLives;
        level.totalSeconds = totalSeconds;
        level.waves = waves;
        level.gameController = this;
        level.levelNumber = levelNumber; // Set the level number based on the current length of the levels array
        Debug.Log($"Level {levelNumber} created with {totalLives} lives and {totalSeconds} seconds.");
        return level;
    }

    void Update()
    {
        if (StateController.gameState != State.transitioning)
        {
            if (StateController.gameState == State.NotPlaying) // Check if the game is not playing
            {
                if (Input.GetKeyDown(KeyCode.Space)) // Check if the space key is pressed
                {
                    StartGame(); // Call the method to start the game
                    playerUI.rootVisualElement.Q<Label>("Start").style.display = DisplayStyle.None; // Hide the start label in the UI
                }
            }
            if (StateController.gameState == State.Playing) // Check if the game is currently playing
            {
                if (Input.GetKeyDown(KeyCode.Escape)) // Check if the escape key is pressed
                {
                    StateController.gameState = State.Paused; // Set the game state to not playing
                    Time.timeScale = 0; // Pause the game by setting the time scale to 0
                    Debug.Log("Game paused!"); // Log a message indicating the game is paused
                }
            }
            if (StateController.gameState == State.Paused) // Check if the game is paused
            {
                if (Input.GetKeyDown(KeyCode.Escape)) // Check if the space key is pressed
                {
                    StateController.gameState = State.Playing; // Set the game state to playing
                    Time.timeScale = 1; // Resume the game by setting the time scale to 1
                    Debug.Log("Game resumed!"); // Log a message indicating the game is resumed
                }
            }
        }
        else if (StateController.gameState == State.LevelComplete) // Check if the level is complete
        {
            if (playerUI.rootVisualElement.Q<Label>("Start").style.display == DisplayStyle.None)
            {
                playerUI.rootVisualElement.Q<Label>("Start").style.display = DisplayStyle.Flex; // Show the start label in the UI
            }
            if (Input.GetKeyDown(KeyCode.Space)) // Check if the space key is pressed
            {
                if (levels.Length == 0) // Check if there are no more levels left
                {
                    StateController.gameState = State.GameOver; // Set the game state to game over
                    Debug.Log("No more levels! Game over!"); // Log a message indicating no more levels
                }
                else
                {
                    StateController.gameState = State.transitioning; // Set the game state to transitioning
                    levels = levels[1..]; // Remove the first level from the array
                    StateController.currentLevel = levels[0]; // Set the current level to the next level
                    StateController.currentLevel.startWave(); // Start the first wave of the current level
                }
            }
        }
        else if (StateController.gameState == State.GameOver) // Check if the game is over
        {
            if (Input.GetKeyDown(KeyCode.Space)) // Check if the space key is pressed
            {
                StateController.gameState = State.NotPlaying; // Set the game state to not playing
                Debug.Log("Game over! Press Space to start a new game."); // Log a message indicating the game is over
            }
        }


    }

    public void StartGame()
    {
        // Start the first level when the game starts
        StateController.currentLevel = levels[0]; // Set the current level to the first level
        StateController.currentLevel.startWave(); // Start the first wave of the current level
        Debug.Log($"Starting level with {StateController.currentLevel.totalLives} lives and {StateController.currentLevel.totalSeconds} seconds."); // Log the level information
    }

    public void OnGameOverButtonClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}