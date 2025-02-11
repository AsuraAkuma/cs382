using UnityEngine;

public class AppleDestroyer : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collisionInfo)
    {
        Destroy(collisionInfo.gameObject);
    }
}
