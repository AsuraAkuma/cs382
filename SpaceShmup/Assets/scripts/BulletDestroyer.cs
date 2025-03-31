using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Ammo"))
        {
            Destroy(collision.gameObject); // Destroy the bullet
        }
    }
}
