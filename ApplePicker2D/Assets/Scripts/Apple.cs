using UnityEngine;

public class Apple : MonoBehaviour
{
    public int value;
    public bool special;
    public string type;
    public void Spawn(int x, int y, GameController controller)
    {
        System.Random rand = new System.Random();
        Instantiate(controller.applePrefab, new Vector3(x, y, 0), Quaternion.Euler(0, rand.Next(-60, 60 + 1), 0));
    }
}
