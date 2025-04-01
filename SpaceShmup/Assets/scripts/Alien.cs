using UnityEngine;
using UnityEngine.UIElements;

public class Alien : MonoBehaviour
{
    public int health; // Current health of the alien
    public float speed; // Speed of the alien
    public float attackPower; // Attack power of the alien
    private bool canMove = true; // Flag to determine if the alien can move or not
    private float leftCameraBorder;
    private float rightCameraBorder;
    private Player player;
    public GameController gameController;
    private int direction; // Direction of movement for the alien
    public Alien() { }

    public Alien(AlienType.AlienTypeData type)
    {
        health = type.health;
        speed = type.speed;
        attackPower = type.attackPower;
    }

    void Start()
    {
        direction = 1; // Set the initial direction of the alien
        leftCameraBorder = -Camera.main.aspect * Camera.main.orthographicSize;
        rightCameraBorder = Camera.main.aspect * Camera.main.orthographicSize;
        player = GameObject.Find("Player").GetComponent<Player>(); // Get the player component
    }

    void Update()
    {
        if (canMove)
        {
            transform.position = new Vector3(transform.position.x + (speed * direction * Time.deltaTime), transform.position.y - (speed / 10 * Time.deltaTime), transform.position.z); // Move the alien based on speed and direction
            if ((transform.position.x - transform.localScale.x * 2.5f <= leftCameraBorder && direction == -1) || (transform.position.x + transform.localScale.x * 2.5f >= rightCameraBorder && direction == 1))
            {
                direction *= -1; // Reverse direction
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("PrimaryAmmo"))
        {
            health -= player.bulletDamage / 2; // Reduce the alien's health by the bullet's damage
            Destroy(collision.gameObject); // Destroy the bullet
            print(health + " : " + player.bulletDamage / 2);
            if (health <= 0)
            {
                Die(); // Call the Die method instead of directly destroying the alien
            }
        }
        if (collision.gameObject.name.Contains("SecondaryAmmo"))
        {
            health -= player.bulletDamage; // Reduce the alien's health by the bullet's damage
            Destroy(collision.gameObject); // Destroy the bullet
            if (health <= 0)
            {
                Die(); // Call the Die method instead of directly destroying the alien
            }
        }
    }

    public void Die()
    {
        // Destroy the alien object
        Destroy(gameObject);

        StateController.currentLevel.currentWave.NumberOfAliens--; // Decrease the number of aliens in the current wave
        Debug.Log($"Alien died. Remaining aliens: {StateController.currentLevel.currentWave.NumberOfAliens}");

        // Optional: Check if all aliens are dead
        if (StateController.currentLevel.currentWave.NumberOfAliens <= 0)
        {
            Debug.Log("Wave completed!");
            StateController.currentLevel.isWaveStarted = false; // Reset the wave started flag
            StateController.currentLevel.startWave(); // Start the next wave
        }

    }
}
