using UnityEngine;

public class Basket : MonoBehaviour
{
    private Player player;
    // Constructor
    public Basket(Player player)
    {
        this.player = player;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        print("started");
        // 
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Listen for collision
    void OnCollisionEnter(Collision collisionInfo)
    {
        print("collision");
        if (collisionInfo.gameObject.name == "Apple")
        {
            print("You won");
        }
        Destroy(collisionInfo.gameObject);
    }

}
