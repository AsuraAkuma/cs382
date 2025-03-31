using UnityEngine;
using UnityEngine.UIElements;

public class Level : MonoBehaviour
{
    public int levelNumber; // The number of the level
    public int totalLives; // Total number of lives for the player in this level
    public int totalSeconds = 60; // Total time in seconds for the player to complete the level
    public Wave[] waves; // Array of waves for this level, each containing information about the aliens to spawn
    public Wave currentWave; // The current wave being processed
    public bool isWaveStarted = false;
    public GameController gameController; // Reference to the GameController for managing game state
    public Level(int totalLives, int totalSeconds, Wave[] waves)
    {
        this.totalLives = totalLives;
        this.totalSeconds = totalSeconds;
        this.waves = waves;
    }
    public void startWave()
    {
        if (isWaveStarted)
        {
            Debug.LogError("Wave already started!"); // Log an error if the wave has already started
            return; // Exit the method to prevent multiple starts
        }
        if (waves.Length > 0) // Check if there are any waves left to spawn
        {
            isWaveStarted = true; // Set the flag to true to prevent multiple starts
            Wave nextWave = waves[0]; // Get the next wave to spawn
            currentWave = nextWave; // Set the current wave to the next wave
            waves = waves[1..]; // Remove the first wave from the array
            nextWave.spawnWave(); // Call the method to spawn aliens for the next wave
            Label levelLabel = gameController.playerUI.rootVisualElement.Q<Label>("Level"); // Find the UI element by name
            if (levelLabel != null)
            {
                levelLabel.text = $"Level {levelNumber}"; // Update the UI label with the current wave number
            }
            else
            {
                Debug.LogWarning("levelLabel not found in playerUI."); // Log a warning if the label is not found
            }
            Label waveLabel = gameController.playerUI.rootVisualElement.Q<Label>("Wave"); // Find the UI element by name
            if (waveLabel != null)
            {
                waveLabel.text = $"Wave {nextWave.WaveNumber}"; // Update the UI label with the current wave number
            }
            else
            {
                Debug.LogWarning("WaveLabel not found in playerUI."); // Log a warning if the label is not found
            }
            Debug.Log($"Starting wave {nextWave.WaveNumber} with {nextWave.NumberOfAliens} aliens."); // Log the wave number and number of aliens
        }
        else
        {
            Debug.Log("All waves completed!"); // Log a message when all waves are completed
            gameController.levelCompleteUI.gameObject.SetActive(true); // Show the level complete UI
            StateController.gameState = State.LevelComplete; // Set the game state to level complete
            StateController.currentLevel = null; // Reset the current level
        }
    }
}
