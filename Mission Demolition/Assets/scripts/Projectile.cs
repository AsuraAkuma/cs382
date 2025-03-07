using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageMultiplier = 10f; // Adjust based on balance needs
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.name.Contains("Pillar")) return; // Prevent self-collision
        float velocity = rb.linearVelocity.magnitude; // Get speed at impact
        float damage = velocity * damageMultiplier; // Calculate damage

        Debug.Log($"Hit {collision.gameObject.name} with {damage} damage!");

        // Apply damage if the object has a health component
        Health targetHealth = collision.gameObject.GetComponent<Health>();
        if (targetHealth != null)
        {
            targetHealth.TakeDamage(damage);
        }

        // Destroy the projectile upon impact (optional)
        // Destroy(gameObject);
    }
}
