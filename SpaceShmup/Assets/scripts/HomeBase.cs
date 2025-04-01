using UnityEngine;
using UnityEngine.UIElements;

public class HomeBase : MonoBehaviour
{
    public GameController gameController; // Reference to the GameController script
    void OnCollisionEnter2D(Collision2D collision)
    {
        print("1 Collision detected with " + collision.gameObject.name); // Print the name of the object that collided with the home base
        if (collision.gameObject.name.Contains("Scout") || collision.gameObject.name.Contains("Warrior") || collision.gameObject.name.Contains("Heavy") || collision.gameObject.name.Contains("Sniper") || collision.gameObject.name.Contains("Boss"))
        {
            if (StateController.playerHealth > 0)
            {
                StateController.playerHealth -= (int)collision.gameObject.GetComponent<Alien>().attackPower; // Reduce player health by 1
            }
            gameController.playerUI.rootVisualElement.Q<Label>("HP").text = $"Health: {StateController.playerHealth}/100"; // Update the health value in the UI
            collision.gameObject.GetComponent<Alien>().Die(); // Call the Die method to handle the alien's death
            if (StateController.playerHealth <= 0)
            {
                StateController.gameState = State.GameOver; // Set the game state to GameOver if player health is 0
                return;
            }
        }
    }
}
