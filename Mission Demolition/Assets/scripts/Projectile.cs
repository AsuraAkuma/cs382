using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageMultiplier = 5f; // Adjust based on balance needs
    private Rigidbody2D rb;
    private string[] damageable = { "Pillar", "Pig", "Ground" };

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (rb == null) return; // Return if Rigidbody2D is missing
        if (Array.Exists<string>(damageable, element => element == collision.gameObject.name)) return; // Prevent self-collision
        float velocity = rb.linearVelocity.magnitude; // Get speed at impact
        float damage = velocity * damageMultiplier; // Calculate damage

        // Debug.Log($"Hit {collision.gameObject.name} with {damage} damage!");

        // Apply damage if the object has a health component
        Health targetHealth = collision.gameObject.GetComponent<Health>();
        if (targetHealth != null)
        {
            if (collision.gameObject.name == "Pig")
            {
                targetHealth.TakeDamage(targetHealth.currentHealth);
                return;
            }
            targetHealth.TakeDamage(damage);
        }

        // Destroy the projectile upon impact (optional)
        // Destroy(gameObject);
    }
}
