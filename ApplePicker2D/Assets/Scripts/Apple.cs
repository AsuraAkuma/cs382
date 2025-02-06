using UnityEngine;

public class Apple : MonoBehaviour
{
    public int value;
    public bool special;
    public string type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Spawn(int x, int y, GameController controller)
    {
        System.Random rand = new System.Random();
        Instantiate(controller.applePrefab, new Vector3(x, y, 0), Quaternion.Euler(0, rand.Next(-60, 60 + 1), 0));
    }
}
