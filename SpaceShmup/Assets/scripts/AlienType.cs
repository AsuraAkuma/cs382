public static class AlienType
{
    public static string name;
    public static int health;
    public static float speed;
    public static float attackPower;

    public static readonly AlienTypeData scout = new AlienTypeData("Scout", 25, 2.0f, 5.0f); // Scout alien type
    public static readonly AlienTypeData warrior = new AlienTypeData("Warrior", 75, 1.5f, 10.0f); // Warrior alien type
    public static readonly AlienTypeData heavy = new AlienTypeData("Heavy", 100, 0.5f, 15.0f); // Heavy alien type
    public static readonly AlienTypeData sniper = new AlienTypeData("Sniper", 50, 1.0f, 8.0f); // Sniper alien type
    public static readonly AlienTypeData boss = new AlienTypeData("Boss", 250, 1.0f, 20.0f); // Boss alien type

    public class AlienTypeData
    {
        public string name;
        public int health;
        public float speed;
        public float attackPower;

        public AlienTypeData(string name, int health, float speed, float attackPower) // Constructor to initialize the alien type with its properties
        {
            this.name = name;
            this.health = health;
            this.speed = speed;
            this.attackPower = attackPower;
        }
    }
}