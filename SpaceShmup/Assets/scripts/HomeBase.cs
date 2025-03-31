using UnityEngine;

public class HomeBase : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Scout") || collision.gameObject.name.Contains("Warrior") || collision.gameObject.name.Contains("Heavy") || collision.gameObject.name.Contains("Sniper") || collision.gameObject.name.Contains("Boss"))
        {
            if (StateController.playerHealth > 0)
            {
                StateController.playerHealth -= 1; // Reduce player health by 1
            }
            if (StateController.playerHealth == 0)
            {
                StateController.gameState = State.GameOver; // Set the game state to GameOver if player health is 0
                return;
            }
            collision.gameObject.GetComponent<Alien>().Die(); // Call the Die method to handle the alien's death
        }
    }
}
