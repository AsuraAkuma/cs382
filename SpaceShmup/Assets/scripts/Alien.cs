using UnityEngine;

public class Alien : MonoBehaviour
{
    private int health; // Current health of the alien
    private float speed; // Speed of the alien
    private float attackPower; // Attack power of the alien
    private GameObject prefab;

    public Alien(AlienTypes)
    {
        health = type.Health;
        speed = type.Speed;
        attackPower = type.AttackPower;
        prefab = type.Prefab;
    }
}
