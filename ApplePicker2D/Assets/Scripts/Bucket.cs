using System;
using TMPro;
using UnityEngine;

public class Bucket : MonoBehaviour
{
    private Player player;
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
                player.score += (int)Math.Ceiling(apple.value * 2 * controller.scoreMultiplier);
            }
            else
            {
                player.score += (int)Math.Ceiling(apple.value * controller.scoreMultiplier);
            }
            if (player.score > player.highScore)
            {
                player.highScore = player.score;
            }
            controller.scoreText.text = "Score: " + player.score;
            controller.highScoreText.text = "Highscore: " + player.highScore;
            // Change apple and bucket speed
            if (controller.appleSpeed < 600f)
            {
                controller.appleSpeed += 2.5f;
            }
            if (controller.bucketSpeed < 500f)
            {
                controller.bucketSpeed += 2.5f;
            }
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
            // Make sound
            controller.PingSound.GetComponent<AudioSource>().Play();
            // Destroy apple
            Destroy(collisionInfo.gameObject);
            // Save player data
            player.SavePlayerData();
        }
    }

    public void setPlayer(string username)
    {
        player = new Player(username);
    }
}
