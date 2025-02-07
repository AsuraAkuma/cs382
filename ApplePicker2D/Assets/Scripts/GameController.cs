using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

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
    private Bucket bucket;
    public float bucketSpeed = 0.60f;
    public float appleSpawnInterval = 3f; //time in seconds
    private System.Random rand = new System.Random();
    private float appleWidth;
    private float appleHeight;
    public float appleSpeed = 25;
    private AppleType[] appleTypes = {
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
    struct AppleType
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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Start on main menu
        // currentScreen = mainScreen;
        appleWidth = applePrefab.gameObject.transform.localScale.x;
        appleHeight = applePrefab.gameObject.transform.localScale.y;
        // Create player and bucket
        player = gameObject.AddComponent<Player>();
        player.username = "AsuraAkuma";
        nameText.text = player.username;
        player.highScore = 20;
        highScoreText.text = "Highscore: " + player.highScore;
        startGame();
    }
    public void changeSpawnRate(float spawnRate)
    {
        CancelInvoke(nameof(SpawnApples));
        InvokeRepeating(nameof(SpawnApples), 0f, spawnRate);
    }
    private void SpawnApples()
    {
        int multiplier = Math.Min(player.score / 100, 10);
        float z = 0f; // Z-axis position
        float y = Screen.height + (appleHeight * 2); // Position above the screen
        if (player.score % 100 == 0 && player.score != 0)
        {
            for (int i = 0; i < multiplier; i++)
            {
                // Get random apple type
                AppleType appleType = appleTypes[rand.Next(0, appleTypes.Length - 1)];
                // Use Unity's main thread for the instantiation
                float x = rand.Next((int)appleWidth, (int)(Screen.width - appleWidth)); // Random x within range
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
            float x = rand.Next((int)appleWidth, (int)(Screen.width - appleWidth)); // Random x within range
            Vector3 position = new Vector3(x, y, z);
            // Quaternion rotation = Quaternion.Euler(0, rand.Next(-60, 60 + 1), 0);
            applePrefab.GetComponent<Rigidbody2D>().gravityScale = appleSpeed;
            GameObject appleObject = Instantiate(applePrefab, position, Quaternion.identity);
            appleObject.GetComponent<Apple>().value = appleType.value;
            appleObject.GetComponent<Apple>().type = appleType.type;
            appleObject.GetComponent<Apple>().special = appleType.special;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // Listen for bucket inputs
        if (Input.GetKey(KeyCode.A))
        {
            float bucketX = bucket.gameObject.transform.localPosition.x;
            float bucketY = bucket.gameObject.transform.localPosition.y;
            float bucketZ = bucket.gameObject.transform.localPosition.z;
            float bucketWidth = bucket.gameObject.transform.localScale.x;
            if (isStarted && bucketX - (bucketWidth * 2) - 5 > 0)
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
            if (isStarted && bucketX + (bucketWidth * 2) + 5 < Screen.width - 1)
            {
                bucket.gameObject.transform.localPosition = new Vector3(bucketX + bucketSpeed, bucketY, bucketZ);
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
    public void startGame()
    {
        // Each "game" will have it's own box, each box acts as an instance for a single player.

        // Get the bottom middle of the screen in screen coordinates
        Vector3 screenPos = new Vector3(Screen.width / 2f, 60, Camera.main.nearClipPlane);

        // Convert screen coordinates to world coordinates
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        bucket = Instantiate(bucketPrefab, worldPos, Quaternion.identity).GetComponent<Bucket>();
        bucket.player = player;
        bucket.controller = this;
        isStarted = true;
        // Set interval for apple spawn
        print(appleWidth + " " + appleHeight);
        InvokeRepeating("SpawnApples", 0f, appleSpawnInterval);
    }

    public void stopGame()
    {
        // Destroy bucket
        Destroy(bucket.gameObject);

    }

    private void savePlayerData()
    {
        // Get current saved data
        string playerDataFilePath = "/PlayerData/" + player.id;
        // Player oldPlayerData = File.ReadAllText(playerDataFilePath);

        var playerData = new
        {
            name = player.name,
            high = 30,
            em = "johndoe@example.com"
        };
    }
}
