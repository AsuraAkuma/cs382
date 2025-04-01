using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject primaryPrefab; // The bullet prefab to be instantiated when the player shoots
    public GameObject secondaryPrefab; // The bullet prefab to be instantiated when the player shoots
    public float bulletSpeed = 10; // Speed of the bullet, can be adjusted for bullet speed
    private float fireRate = 0.0f; // Rate of fire for the player's bullets
    public int bulletDamage = 10; // The damage dealt by the player's bullets
    public int primaryAmmoCount = 100; // The player's primary ammo count
    public int primaryAmmoReserve = 1000; // The player's primary ammo reserve
    public int secondaryAmmoCount = 10; // The player's secondary ammo count
    public int secondaryAmmoReserve = 100; // The player's secondary ammo reserve
    public int playerHealth = 100; // The player's health
    public bool canShoot = true; // Flag to determine if the player can shoot or not, useful for controlling shooting in certain conditions
    private int playerSpeed = 7; // Speed of the player character, can be adjusted for movement speed
    private float leftCameraBorder;
    private float rightCameraBorder;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // Initialize camera borders here
        leftCameraBorder = -Camera.main.aspect * Camera.main.orthographicSize;
        rightCameraBorder = Camera.main.aspect * Camera.main.orthographicSize;
        StateController.playerHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (StateController.gameState != State.Playing) return; // Check if the game is in the playing state, if not, exit the method
        // Check for input to control the player character, e.g., moving left or right
        if (Input.GetKey(KeyCode.A))
        {
            // Check if player object is at edge of the screen to prevent moving off-screen
            if (transform.position.x - (transform.localScale.x / 2) <= leftCameraBorder) // Assuming the left edge of the screen is at x = -8, adjust as necessary for your game
            {
                return; // Prevent moving left if at the edge of the screen
            }
            // Move left
            transform.position = new Vector3(transform.position.x - playerSpeed * Time.deltaTime, transform.position.y, transform.position.z); // Move the player left by speed units per second
        }
        else if (Input.GetKey(KeyCode.D))
        {
            // Check if player object is at edge of the screen to prevent moving off-screen
            if (transform.position.x + (transform.localScale.x / 2) >= rightCameraBorder) // Assuming the left edge of the screen is at x = -8, adjust as necessary for your game
            {
                return; // Prevent moving left if at the edge of the screen
            }
            // Move right
            transform.position = new Vector3(transform.position.x + playerSpeed * Time.deltaTime, transform.position.y, transform.position.z); // Move the player right by speed units per second
        }
        // Check for if the player is shooting
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (StateController.gameState != State.Playing) return; // Check if the game is in the playing state, if not, exit the method
            primaryShoot(); // Call the Shoot method to handle player shooting
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (StateController.gameState != State.Playing) return; // Check if the game is in the playing state, if not, exit the method
            secondaryShoot(); // Call the secondaryShoot method to handle player shooting
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            reload(); // Call the reload method to handle player reloading
        }
    }

    void primaryShoot()
    {
        if (!canShoot || primaryAmmoCount <= 0) return; // Prevent shooting if the player is not allowed to shoot
        canShoot = false; // Prevent the player from shooting again until after this bullet has been shot
        // Wait 1 second before allowing the player to shoot again
        // Create a bullet object at the player's position
        GameObject bullet = Instantiate(primaryPrefab, transform.position, Quaternion.identity);
        // Get the bullet's rigidbody component
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // Apply a force to the bullet to make it move upwards
        rb.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
        primaryAmmoCount--; // Reduce the player's primary ammo count
        Invoke("allowShoot", fireRate);
    }
    void secondaryShoot()
    {
        if (!canShoot || secondaryAmmoCount <= 0) return; // Prevent shooting if the player is not allowed to shoot or has no ammo
        canShoot = false; // Prevent the player from shooting again until after this bullet has been shot
        // Create a bullet object at the player's position
        GameObject bullet = Instantiate(secondaryPrefab, transform.position, Quaternion.identity);
        // Get the bullet's rigidbody component
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        // Apply a force to the bullet to make it move upwards
        rb.AddForce(Vector2.up * bulletSpeed, ForceMode2D.Impulse);
        secondaryAmmoCount--; // Reduce the player's secondary ammo count
        Invoke("allowShoot", fireRate);
    }
    void reload()
    {
        if (!canShoot) return; // Prevent reloading if the player is not allowed to shoot
        canShoot = false; // Prevent the player from shooting while reloading
        if (primaryAmmoReserve > 0)
        {
            int ammoNeeded = 100 - primaryAmmoCount; // Calculate the amount of ammo needed to fill the primary ammo count
            if (primaryAmmoReserve >= ammoNeeded)
            {
                primaryAmmoCount += ammoNeeded; // Fill the primary ammo count
                primaryAmmoReserve -= ammoNeeded; // Deduct the ammo used from the primary ammo reserve
            }
            else
            {
                primaryAmmoCount += primaryAmmoReserve; // Fill the primary ammo count with the remaining ammo in the reserve
                primaryAmmoReserve = 0; // Set the primary ammo reserve to zero
            }
        }
        if (secondaryAmmoReserve > 0)
        {
            int ammoNeeded = 10 - secondaryAmmoCount; // Calculate the amount of ammo needed to fill the secondary ammo count
            if (secondaryAmmoReserve >= ammoNeeded)
            {
                secondaryAmmoCount += ammoNeeded; // Fill the secondary ammo count
                secondaryAmmoReserve -= ammoNeeded; // Deduct the ammo used from the secondary ammo reserve
            }
            else
            {
                secondaryAmmoCount += secondaryAmmoReserve; // Fill the secondary ammo count with the remaining ammo in the reserve
                secondaryAmmoReserve = 0; // Set the secondary ammo reserve to zero
            }
        }
        Invoke("allowShoot", fireRate);
    }
    void allowShoot()
    {
        canShoot = true; // Allow the player to shoot again
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Alien"))
        {
            Destroy(gameObject); // Destroy the player if it collides with an alien
        }
    }
}
