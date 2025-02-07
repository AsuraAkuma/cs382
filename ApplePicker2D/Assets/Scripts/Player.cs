using UnityEngine;

public class Player : MonoBehaviour
{
    public string username;
    public int score;
    public int highScore;
    public int id;
    // Constructor
    public Player(string username, int score = 0)
    {
        this.username = username;
        this.score = score;
        highScore = 20;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

}
