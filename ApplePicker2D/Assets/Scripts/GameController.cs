using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
public class GameController : MonoBehaviour
{
    public bool isStarted;
    private Player player;
    private int playerIndex;
    public bool isPaused;
    public int scoreMultiplier = 1;
    public GameObject applePrefab;
    public GameObject bucketPrefab;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI timerText;
    private int timerDuration;
    private Bucket bucket;
    public float bucketSpeed = 0.60f;
    public float appleSpawnInterval = 3f; //time in seconds
    private System.Random rand = new System.Random();
    private float appleWidth;
    private float appleHeight;
    public float appleSpeed = 25;
    public float bottomLeftX;
    public float bottomRightX;
    private Settings settings = new Settings(new Settings.playerData());
    private static string settingsPath = "settings.json";

    public AppleType[] appleTypes = {
        new AppleType("HoneycrispX",true,5),
        new AppleType("Honeycrisp",false,5),
        new AppleType("FujiX",true,4),
        new AppleType("Fuji",false,4),
        new AppleType("Granny SmithX",true,3),
        new AppleType("Granny Smith",false,3),
        new AppleType("Pink LadyX",true,2),
        new AppleType("Pink Lady",false,2),
        new AppleType("GalaX",true,1),
        new AppleType("Gala ",false,1)
    };
    public List<GameType> gameTypes = new List<GameType> {
        new GameType("Normal",5 * 60 * 1000,10),
        new GameType("Medium",4 * 60 * 1000,20),
        new GameType("Hard",3 * 60 * 1000,40),
        new GameType("Extreme",2 * 60 * 1000,80),
        new GameType("Test",(int)(0.2f * 60 * 1000),80)
    };

    public struct AppleType
    {
        public string type;
        public bool special;
        public int value;
        public AppleType(string type, bool special, int value)
        {
            this.type = type;
            this.special = special;
            this.value = value;
        }
    }
    public struct GameType
    {
        public string name;
        public int duration;
        public int startSpeed;

        public GameType(string name, int duration, int startSpeed)
        {
            this.name = name;
            this.duration = duration;
            this.startSpeed = startSpeed;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Start on main menu
        // currentScreen = mainScreen;
        appleWidth = applePrefab.gameObject.transform.localScale.x;
        appleHeight = applePrefab.gameObject.transform.localScale.y;
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        bottomLeftX = bottomLeft.x;
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
        bottomRightX = bottomRight.x;
        // Create or load settings
        if (File.Exists(settingsPath))
        {
            settings.loadSettings();
        }
        else
        {
            settings.saveSettings();
        }
        // Create player and bucket
        player = new Player();
        player.username = settings.playerUsername;
        player.LoadPlayerData();
        player.score = 0;
        nameText.text = player.username;
        highScoreText.text = "Highscore: " + player.highScore;
        startGame(gameTypes.Find(g => g.name == settings.difficulty));
    }
    public void changeSpawnRate(float spawnRate)
    {
        CancelInvoke(nameof(SpawnApples));
        InvokeRepeating(nameof(SpawnApples), 0f, spawnRate);
    }
    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            // Listen for bucket inputs
            if (Input.GetKey(KeyCode.A))
            {
                float bucketX = bucket.gameObject.transform.localPosition.x;
                float bucketY = bucket.gameObject.transform.localPosition.y;
                float bucketZ = bucket.gameObject.transform.localPosition.z;
                float bucketWidth = bucket.gameObject.transform.localScale.x;
                Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
                float bottomLeftX = bottomLeft.x;
                if (isStarted && bucketX - (bucketWidth * 2) - 5 > bottomLeftX)
                {
                    bucket.gameObject.transform.localPosition = new Vector3(bucketX - bucketSpeed, bucketY, bucketZ);
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                float bucketX = bucket.gameObject.transform.localPosition.x;
                float bucketY = bucket.gameObject.transform.localPosition.y;
                float bucketZ = bucket.gameObject.transform.localPosition.z;
                float bucketWidth = bucket.gameObject.transform.localScale.x;
                Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
                float bottomRightX = bottomRight.x;
                if (isStarted && bucketX + (bucketWidth * 2) + 5 < bottomRightX)
                {
                    bucket.gameObject.transform.localPosition = new Vector3(bucketX + bucketSpeed, bucketY, bucketZ);
                }
            }
        }
        // Listen for pause input
        if (Input.GetKey(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Time.timeScale = 0;
                isPaused = true;
            }
            else
            {
                Time.timeScale = 1;
                isPaused = false;
            }
        }
    }
    // Game Management
    public void startGame(GameType gameType)
    {
        // Each "game" will have it's own box, each box acts as an instance for a single player.

        // Get the bottom middle of the screen in screen coordinates
        Vector3 screenPos = new Vector3(Screen.width / 2f, 60, Camera.main.nearClipPlane);

        // Convert screen coordinates to world coordinates
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        bucket = Instantiate(bucketPrefab, worldPos, Quaternion.identity).GetComponent<Bucket>();
        bucket.setPlayer(player.username);
        bucket.controller = this;
        isStarted = true;
        // Set interval for apple spawn
        InvokeRepeating("SpawnApples", 0f, appleSpawnInterval);
        // Set gamemode timer
        timerDuration = gameType.duration;
        appleSpeed = gameType.startSpeed;
        InvokeRepeating("countDownTimer", 0f, 1);
    }

    public void stopGame()
    {
        isStarted = false;
        // Destroy bucket
        Destroy(bucket.gameObject);
        // Stop timer
        CancelInvoke(nameof(countDownTimer));
        // Save player data
        player.SavePlayerData();
        // Change scene to after game scene

    }
    private void SpawnApples()
    {
        int multiplier = Math.Min(player.score / 100, 10);
        float z = 0f; // Z-axis position
        float y = (Screen.height / 2) + appleHeight; // Position above the screen
        if (player.score % 100 == 0 && player.score != 0)
        {
            for (int i = 0; i < multiplier; i++)
            {
                // Get random apple type
                AppleType appleType = appleTypes[rand.Next(0, appleTypes.Length - 1)];
                // Use Unity's main thread for the instantiation
                float x = rand.Next((int)(bottomLeftX + appleWidth), (int)(bottomRightX - appleWidth)); // Random x within range
                Vector3 position = new Vector3(x, y, z);
                // Quaternion rotation = Quaternion.Euler(0, rand.Next(-60, 60 + 1), 0);
                applePrefab.GetComponent<Rigidbody2D>().gravityScale = appleSpeed;
                GameObject appleObject = Instantiate(applePrefab, position, Quaternion.identity);
                appleObject.GetComponent<Apple>().value = appleType.value;
                appleObject.GetComponent<Apple>().type = appleType.type;
                appleObject.GetComponent<Apple>().special = appleType.special;
            }
        }
        else
        {
            // Get random apple type
            AppleType appleType = appleTypes[rand.Next(0, appleTypes.Length - 1)];
            // Use Unity's main thread for the instantiation
            float x = rand.Next((int)(bottomLeftX + appleWidth), (int)(bottomRightX - appleWidth)); // Random x within range
            Vector3 position = new Vector3(x, y, z);
            // Quaternion rotation = Quaternion.Euler(0, rand.Next(-60, 60 + 1), 0);
            applePrefab.GetComponent<Rigidbody2D>().gravityScale = appleSpeed;
            GameObject appleObject = Instantiate(applePrefab, position, Quaternion.identity);
            appleObject.GetComponent<Apple>().value = appleType.value;
            appleObject.GetComponent<Apple>().type = appleType.type;
            appleObject.GetComponent<Apple>().special = appleType.special;
        }
    }

    private void countDownTimer()
    {
        timerDuration -= 1000;
        // Get minutes
        int minutes = (int)Math.Floor((double)(timerDuration / 60_000));
        int seconds = timerDuration % 60_000 / 1000;
        string timeString = minutes.ToString("D2") + ":" + seconds.ToString("D2");
        timerText.text = timeString;
        // End game
        if (timerDuration == 0)
        {
            stopGame();
        }
        // Pre End game
        if (timerDuration == 2000)
        {
            print("pre end");
            // Stop apple spawning
            CancelInvoke(nameof(SpawnApples));
        }
    }
}
