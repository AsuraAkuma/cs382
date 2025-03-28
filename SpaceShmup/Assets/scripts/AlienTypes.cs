static class AlienType
{
    static class Scout
    {
        public const int Health = 50;
        public const float Speed = 2.0f;
        public const float AttackPower = 5.0f;
        public static GameObject Prefab = Resources.Load<GameObject>("Prefabs/ScoutAlien");
    }
    static class Warrior
    {
        public const int Health = 100;
        public const float Speed = 1.5f;
        public const float AttackPower = 10.0f;
        public static GameObject Prefab = Resources.Load<GameObject>("Prefabs/WarriorAlien");
    }
    static class Heavy
    {
        public const int Health = 200;
        public const float Speed = 0.5f;
        public const float AttackPower = 15.0f;
        public static GameObject Prefab = Resources.Load<GameObject>("Prefabs/HeavyAlien");
    }
    static class Sniper
    {
        public const int Health = 75;
        public const float Speed = 1.0f;
        public const float AttackPower = 8.0f;
        public static GameObject Prefab = Resources.Load<GameObject>("Prefabs/SniperAlien");
    }
    static class Boss
    {
        public const int Health = 300;
        public const float Speed = 1.0f;
        public const float AttackPower = 20.0f;
        public static GameObject Prefab = Resources.Load<GameObject>("Prefabs/BossAlien");
    }
}