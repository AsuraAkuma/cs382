using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour
{
    public bool isStarted;
    private Player player;
    public bool isPaused;
    public float scoreMultiplier = 1f;
    public GameObject applePrefab;
    public GameObject bucketPrefab;
    public GameObject Music;
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
    public GameObject leftBarrier;
    public GameObject rightBarrier;
    public GameObject appleDestroyer;
    public GameObject PauseMenuUI;
    public GameObject PingSound;
    public Texture2D cursorTexture;
    private Settings settings = new Settings();
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
        new GameType("Tutorial",1 * 60 * 1000,5, 50,1f),
        new GameType("Normal",5 * 60 * 1000,10, 50,1f),
        new GameType("Medium",4 * 60 * 1000,20, 70, 1.33f),
        new GameType("Hard",3 * 60 * 1000,40,90,1.66f),
        new GameType("Extreme",2 * 60 * 1000,80,110,2f),
        new GameType("Test",(int)(0.2f * 60 * 1000),80,200, 3f)
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
        public int startBucketSpeed;
        public float multiplier;
        public GameType(string name, int duration, int startSpeed, int startBucketSpeed, float multiplier)
        {
            this.name = name;
            this.duration = duration;
            this.startSpeed = startSpeed;
            this.startBucketSpeed = startBucketSpeed;
            this.multiplier = multiplier;
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        Music.GetComponent<AudioSource>().volume = 0.2f * ((float)settings.volume / 100f);
        PingSound.GetComponent<AudioSource>().volume = 0.1f * ((float)settings.volume / 100f);
        PingSound.GetComponent<AudioSource>().timeSamples = (int)(PingSound.GetComponent<AudioSource>().clip.frequency * 0.05f);
        // Start on main menu
        // currentScreen = mainScreen;
        appleWidth = applePrefab.gameObject.transform.localScale.x;
        appleHeight = applePrefab.gameObject.transform.localScale.y;
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        bottomLeftX = bottomLeft.x;
        Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
        bottomRightX = bottomRight.x;
        leftBarrier.transform.position = new Vector3(bottomLeftX - 10, leftBarrier.transform.position.y, leftBarrier.transform.position.z);
        rightBarrier.transform.position = new Vector3(bottomRightX + 10, leftBarrier.transform.position.y, leftBarrier.transform.position.z);
        appleDestroyer.transform.localScale = new Vector3(bottomRightX - bottomLeftX, 10);
        // Create player and bucket
        player = new Player(TempInfo.playerUsername);
        player.score = 0;
        player.SavePlayerData();
        nameText.text = player.username;
        highScoreText.text = "Highscore: " + player.highScore;
        startGame(gameTypes.Find(g => g.name == TempInfo.difficulty));
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
                if (!isPaused)
                {
                    float bucketX = bucket.gameObject.transform.localPosition.x;
                    float bucketY = bucket.gameObject.transform.localPosition.y;
                    float bucketZ = bucket.gameObject.transform.localPosition.z;
                    float bucketWidth = bucket.gameObject.transform.localScale.x;
                    Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
                    float bottomLeftX = bottomLeft.x;
                    float screenWidth = bottomRightX - bottomLeftX;
                    float normalizedSpeed = bucketSpeed * (screenWidth / 180) * Time.deltaTime;

                    if (isStarted && bucketX - (bucketWidth * 2) - 5 > bottomLeftX)
                    {
                        bucket.gameObject.transform.localPosition = new Vector3(bucketX - normalizedSpeed, bucketY, bucketZ);
                    }
                }
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (!isPaused)
                {
                    float bucketX = bucket.gameObject.transform.localPosition.x;
                    float bucketY = bucket.gameObject.transform.localPosition.y;
                    float bucketZ = bucket.gameObject.transform.localPosition.z;
                    float bucketWidth = bucket.gameObject.transform.localScale.x;
                    Vector3 bottomRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, Camera.main.nearClipPlane));
                    float bottomRightX = bottomRight.x;
                    float screenWidth = bottomRightX - bottomLeftX;
                    float normalizedSpeed = bucketSpeed * (screenWidth / 180) * Time.deltaTime;
                    if (isStarted && bucketX + (bucketWidth * 2) + 5 < bottomRightX)
                    {
                        bucket.gameObject.transform.localPosition = new Vector3(bucketX + normalizedSpeed, bucketY, bucketZ);
                    }
                }
            }
        }
        // Listen for pause input
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused)
            {
                Time.timeScale = 0;
                isPaused = true;
                PauseMenuUI.SetActive(true);
                Cursor.visible = true;
            }
            else
            {
                Time.timeScale = 1;
                isPaused = false;
                PauseMenuUI.SetActive(false);
                Cursor.visible = false;
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
        bucketSpeed = gameType.startBucketSpeed;
        scoreMultiplier = gameType.multiplier;
        InvokeRepeating("countDownTimer", 0f, 1);
        // Make cursor dissapear
        Cursor.visible = false;
    }

    public void stopGame()
    {
        Cursor.visible = true;
        isStarted = false;
        // Destroy bucket
        Destroy(bucket.gameObject);
        // Stop timer
        CancelInvoke(nameof(countDownTimer));
        // Change scene to after game scene
        TempInfo.gameState = TempInfo.GameState.GameOver;
        SceneManager.LoadScene("StartMenu");

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
            // print("pre end");
            // Stop apple spawning
            CancelInvoke(nameof(SpawnApples));
        }
    }
}
