
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

static class GameController
{

    public static int gameState = State.NotStarted;
    public static int score = 0;
    public static int shotsTaken = 0;
    public static int maxShots = 3;
    public static int starCount = 0;
    private static GameLoop gameLoop = GameObject.Find("GameLoop").GetComponent<GameLoop>();
    public static Level[] levels = {
        new Level("Level 1",gameLoop.level1prefab),
        new Level("Level 2",gameLoop.level2prefab),
        new Level("Level 3",gameLoop.level3prefab)
    };
    public static Level currentLevel = levels[0];
    // Check win conditions
    public static void CheckWin()
    {
        if (maxShots == shotsTaken)
        {
            if (score / currentLevel.maxPoints >= 0.30f)
            {
                // Get star count
                if (score / currentLevel.maxPoints >= 0.90f)
                {
                    starCount++;
                }
                gameState = State.Won;
            }
            else
            {
                gameState = State.Lost;
            }
            NextLevel();
        }
    }

    // Start the game when play button is clicked
    public static void StartGame()
    {
        gameState = State.Playing;
        score = 0;
        shotsTaken = 0;
    }

    // End the game when the player runs out of shots
    public static void EndGame()
    {
        Debug.Log("Game Over");
        gameState = State.GameOver;
        if (gameLoop.GameOverUI != null)
        {
            gameLoop.GameOverUI.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene("Menus");
        }
    }

    // Go to next level
    public static void NextLevel()
    {
        int index = System.Array.IndexOf(levels, currentLevel);
        if (index < levels.Length - 1)
        {
            // Unload current level
            // currentLevel.UnloadLevel();
            SceneManager.UnloadSceneAsync("Game");
            shotsTaken = 0;
            currentLevel = levels[index + 1];
            SceneManager.LoadScene("Game");
            gameLoop.StartCoroutine(gameLoop.InitializeLevel());
        }
        else
        {
            // Last level
            EndGame();
        }
    }
}