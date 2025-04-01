using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class Wave : MonoBehaviour
{
    public int WaveNumber; // The number of the wave
    public int TotalAliens; // Total number of aliens in this wave
    public int NumberOfAliens; // Total number of aliens in this wave
    public float SpawnInterval; // Time interval between spawns in seconds
    public float SpawnDelay; // Delay before the first alien spawns in seconds
    public AlienType.AlienTypeData AlienType; // Type of alien to spawn (e.g., "Scout", "Warrior", etc.)
    public GameObject[] spawnPoints;
    public GameController gameController; // Reference to the GameController for managing game state
    private bool isSpawning = false; // Flag to prevent multiple coroutines from running simultaneously

    public Wave() { } // Default constructor for the Wave class

    public Wave(int waveNumber, int numberOfAliens, float spawnInterval, float spawnDelay, AlienType.AlienTypeData alienType, GameController gameController) // Constructor to initialize the wave with its properties
    {
        WaveNumber = waveNumber;
        NumberOfAliens = numberOfAliens;
        TotalAliens = numberOfAliens; // Set the total number of aliens for this wave
        SpawnInterval = spawnInterval;
        SpawnDelay = spawnDelay;
        AlienType = alienType;
        spawnPoints = gameController.spawnPoints; // Get the spawn points from the GameController
    }

    // Coroutine to spawn aliens based on the wave configuration
    IEnumerator SpawnAliens()
    {
        if (isSpawning) yield break; // Prevent multiple coroutines from running simultaneously
        isSpawning = true;
        StateController.gameState = State.spawning; // Set the game state to spawning
        // Wait for the initial spawn delay
        yield return new WaitForSeconds(SpawnDelay);

        for (int i = 0; i < TotalAliens; i++)
        {
            GameObject alienPrefab = null;
            switch (AlienType.name)
            {
                case "Scout":
                    alienPrefab = gameController.alienPrefabs[0]; // Load the scout prefab
                    break;
                case "Warrior":
                    alienPrefab = gameController.alienPrefabs[1]; // Load the warrior prefab
                    break;
                case "Heavy":
                    alienPrefab = gameController.alienPrefabs[2]; // Load the heavy prefab
                    break;
                case "Sniper":
                    alienPrefab = gameController.alienPrefabs[3]; // Load the sniper prefab
                    break;
                case "Boss":
                    alienPrefab = gameController.alienPrefabs[4]; // Load the boss prefab
                    break;
                default:
                    Debug.LogError($"Unknown alien type: {AlienType.name}"); // Log an error if the alien type is unknown
                    yield break; // Exit the coroutine if the alien type is unknown
            }

            if (alienPrefab != null)
            {
                GameObject alienObject = Instantiate(alienPrefab, spawnPoints[GenerateRandomNumber(0, 3)].transform.position, Quaternion.identity);
                Alien alien = alienObject.GetComponent<Alien>();
                alien.health = AlienType.health; // Set the alien's health based on the type
                alien.speed = AlienType.speed; // Set the alien's speed based on the type
                alien.attackPower = AlienType.attackPower; // Set the alien's attack power based on the type
                alien.name = AlienType.name; // Set the alien's name based on the type
                alien.gameController = gameController; // Set the reference to the GameController
            }
            else
            {
                Debug.LogError($"Alien prefab for type {AlienType.name} not found.");
            }

            // Wait for the specified spawn interval before spawning the next alien
            yield return new WaitForSeconds(SpawnInterval);
        }
        StateController.gameState = State.Playing; // Set the game state to spawning
        isSpawning = false; // Mark the coroutine as finished
    }

    public void spawnWave()
    {
        gameController.StartCoroutine(SpawnAliens());
    }

    private int GenerateRandomNumber(int min, int max)
    {
        return Random.Range(min, max + 1);
    }
}

