using UnityEngine;

public class Pig : MonoBehaviour
{
    private Rigidbody2D rb;
    private Health health;
    void Awake()
    {
        health = GetComponent<Health>();
        rb = GetComponent<Rigidbody2D>();
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        // print(collision.gameObject.name);
        if (collision.gameObject.name == "Ground")
        {
            health.TakeDamage(health.currentHealth);
            return;
        }
        float impactVelocity = Mathf.Abs(rb.linearVelocity.y); // Get downward velocity
        // print("Impact velocity: " + impactVelocity);
        if (impactVelocity > health.fallDamageThreshold) // Check if velocity exceeds threshold
        {
            float damage = (impactVelocity - health.fallDamageThreshold) * health.fallDamageMultiplier; // Calculate damage
            health.TakeDamage(damage);
        }
    }

}
