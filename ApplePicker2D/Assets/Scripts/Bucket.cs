using TMPro;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    public Player player;
    public GameController controller;

    // Constructor
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Listen for collision
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        if (collisionInfo.gameObject.name.Contains("apple"))
        {
            // Get apple object
            Apple apple = collisionInfo.gameObject.GetComponent<Apple>();
            // Give player score per apple
            if (apple.special)
            {
                player.score += apple.value * 2;
            }
            else
            {
                player.score += apple.value;
            }
            if (player.score > player.highScore)
            {
                player.highScore = player.score;
            }
            controller.scoreText.text = "Score: " + player.score;
            controller.highScoreText.text = "Highscore: " + player.highScore;
            // Change apple and bucket speed
            if (controller.appleSpeed < 800f)
            {
                controller.appleSpeed += 5f;
            }
            controller.bucketSpeed += 0.01f;
            // Change apple spawn rate
            if (player.score > 50 && player.score <= 100 && controller.appleSpawnInterval != 2)
            {
                controller.appleSpawnInterval = 2;
                controller.changeSpawnRate(2);
            }
            else if (player.score > 100 && player.score <= 200 && controller.appleSpawnInterval != 1)
            {
                controller.appleSpawnInterval = 1;
                controller.changeSpawnRate(1);
            }
            else if (player.score > 200 && player.score <= 300 && controller.appleSpawnInterval != 0.5f)
            {
                controller.appleSpawnInterval = 0.5f;
                controller.changeSpawnRate(0.5f);
            }
            else if (player.score > 300 && player.score <= 400 && controller.appleSpawnInterval != 0.25f)
            {
                controller.appleSpawnInterval = 0.25f;
                controller.changeSpawnRate(0.25f);
            }
            else if (player.score > 400 && controller.appleSpawnInterval != 0.1f)
            {
                controller.appleSpawnInterval = 0.1f;
                controller.changeSpawnRate(0.1f);
            }
            // Destroy apple
            Destroy(collisionInfo.gameObject);
        }
    }

}
