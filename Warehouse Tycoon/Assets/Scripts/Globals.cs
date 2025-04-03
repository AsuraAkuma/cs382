public class Globals
{
    public static int gameState = State.NotPlaying; // Current state of the game, initialized to NotPlaying
    // Warehouse data
    public static string warehouseName;
    public static int warehouseId; // Unique identifier for the warehouse
    public static int warehouselevel;
    public static int warehouseValue;
    public static int warehouseExp;
    public static int warehouseEmployeeCount;
    public static Employee[] warehouseEmployees; // Array of employees in the warehouse
    public static int warehouseMaxEmployees; // Maximum number of employees allowed in the warehouse
    // Department data
    public static Department[] departments; // Array of departments in the warehouse
    public static int departmentCount; // Number of departments in the warehouse
    // Player data
    public static int playerId;
    public static string playerName;
    public static int playerLevel;
    public static int playerExp;
}