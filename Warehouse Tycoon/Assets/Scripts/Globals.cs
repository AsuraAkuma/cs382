using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;
public struct GlobalVariables
{
    public string warehouseName;
    public int warehouseId;
    public int warehouselevel;
    public int warehouseValue;
    public int warehouseExp;
    public List<Employee> warehouseEmployees;
    public int warehouseMaxEmployees;
    public List<Employee> newHires;
    public List<Department> departments;
    public int departmentCount;
    public List<Department> disabledDepartments;
    public int playerId;
    public string playerName;
    public int playerLevel;
    public double playerExp;
    public int playerExpMultiplier;
    public double playerMoney;
    public double playerMaxMoney;
    public int playerMaxLevel;
    public float employeeStatMax;
    public float employeeStatMin;
    public float employeeStatUpgradeValue;
    public float employeeStatUpgradeCost;
    public float employeeMaxLevel;
    public int employeeInfractionMax;
    public StatusType.Type tutorialStatus;
    public int tutorialStep;
    public int gameState;
    public List<Notification> notifications;
}
public class Globals
{
    public static string apiURL = "http://127.0.0.1:5505/api/v1"; // Test API URL for local development
    // public static string apiURL = "https://api.warehousetycoon.com/api/v1"; // Production API URL for live deployment

    public static int gameState = State.NotPlaying; // Current state of the game, initialized to NotPlaying
    // Warehouse data
    public static string warehouseName;
    public static int warehouseId; // Unique identifier for the warehouse
    public static int warehouselevel;
    public static int warehouseValue;
    public static int warehouseExp;
    public static List<Employee> warehouseEmployees = new List<Employee>(); // Array of employees in the warehouse
    public static int warehouseMaxEmployees; // Maximum number of employees allowed in the warehouse
    // HR data
    public static List<Employee> newHires = new List<Employee>(); // Array of new hires in the warehouse
    // Department data
    public static List<Department> departments = new List<Department>(); // Array of departments in the warehouse
    public static int departmentCount; // Number of departments in the warehouse
    public static List<Department> disabledDepartments = new List<Department>(); // Departments that are currently disabled
    public static int departmentCost = 1000; // Cost to purchase a new department
    // Player data
    public static int playerId;
    public static string playerName = "Guest"; // Default player name
    public static int playerLevel = 1; // Player's current level
    public static double playerExp = 0; // Player's current experience points
    public static int playerExpMultiplier = 250; // Multiplier for experience points gained
    public static double playerMoney = 0; // Amount of money the player has
    public static double playerMaxMoney = 99999999999; // Maximum amount of money the player can have
    public static int playerMaxLevel = 100; // Maximum level the player can reach
    public static float employeeStatMax = 6f; // Maximum value for employee stats
    public static float employeeStatMin = 1f; // Minimum value for employee stats
    public static float employeeStatUpgradeValue = 0.5f; // Multiplier for employee stats
    public static float employeeStatUpgradeCost = 1000f; // Cost to upgrade employee stats
    public static float employeeMaxLevel = 10f; // Maximum level for employees
    public static int employeeInfractionMax = 3; // Maximum number of infractions for employees
    // Tutorial data
    public static StatusType.Type tutorialStatus = StatusType.Type.Completed; // Status of the tutorial
    public static int tutorialStep = 0; // Index for the current step in the tutorial
    // Notification data
    public static NotificationController notificationController; // Reference to the NotificationController
    public static List<Notification> notifications = new List<Notification>(); // Array of notifications for the player
    public static IEnumerator Save()
    {
        // Create a JSON object with the data to send
        var data = new
        {
            warehouseName,
            warehouseId,
            warehouselevel,
            warehouseValue,
            warehouseExp,
            warehouseMaxEmployees,
            playerId,
            playerName,
            playerLevel,
            playerExp,
            departments,
            warehouseEmployees,
            newHires,
            playerMoney,
            playerMaxMoney,
            playerMaxLevel,
            employeeStatMax,
            employeeStatMin,
            employeeStatUpgradeValue,
            employeeStatUpgradeCost,
            employeeMaxLevel,
            gameState,
            employeeInfractionMax,
            tutorialStatus = StatusType.GetStatusName(tutorialStatus), // Convert tutorial status to string
            playerExpMultiplier,
            tutorialStep,
            disabledDepartments = new List<Department>(), // Initialize disabledDepartments as an empty list
            notifications = new List<Notification>() // Initialize notifications as an empty list
        };

        // Convert the data to JSON
        string jsonData = JsonUtility.ToJson(data);

        // Create a UnityWebRequest for a POST request
        using (UnityWebRequest request = new UnityWebRequest($"{apiURL}/save", "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Access-Control-Allow-Origin", "*");

            // Send the request and wait for a response
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Data saved successfully: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error saving data: " + request.error);
            }
        }
    }
    public static IEnumerator Load()
    {
        // Create a UnityWebRequest for a GET request
        using (UnityWebRequest request = UnityWebRequest.Get($"{apiURL}/load"))
        {
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Access-Control-Allow-Origin", "*");
            request.SetRequestHeader("Accept", "application/json");
            {
                // Send the request and wait for a response
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    // Parse the JSON response and update the game state
                    string jsonResponse = request.downloadHandler.text;
                    var data = JsonUtility.FromJson<GlobalVariables>(jsonResponse);
                    warehouseName = data.warehouseName;
                    warehouseId = data.warehouseId;
                    warehouselevel = data.warehouselevel;
                    warehouseValue = data.warehouseValue;
                    warehouseExp = data.warehouseExp;
                    warehouseMaxEmployees = data.warehouseMaxEmployees;
                    playerId = data.playerId;
                    playerName = data.playerName;
                    playerLevel = data.playerLevel;
                    playerExp = data.playerExp;
                    departments = data.departments;
                    warehouseEmployees = data.warehouseEmployees;
                    newHires = data.newHires;
                    playerMoney = data.playerMoney;
                    playerMaxMoney = data.playerMaxMoney;
                    playerMaxLevel = data.playerMaxLevel;
                    employeeStatMax = data.employeeStatMax;
                    employeeStatMin = data.employeeStatMin;
                    employeeStatUpgradeValue = data.employeeStatUpgradeValue;
                    employeeStatUpgradeCost = data.employeeStatUpgradeCost;
                    employeeMaxLevel = data.employeeMaxLevel;
                    gameState = data.gameState;
                    employeeInfractionMax = data.employeeInfractionMax;
                    tutorialStatus = data.tutorialStatus;
                    disabledDepartments = data.disabledDepartments;
                    playerExpMultiplier = data.playerExpMultiplier;
                    tutorialStep = data.tutorialStep;
                    notifications = data.notifications;
                }
                else
                {
                    Debug.LogError("Error loading data: " + request.error);
                }
            }
        }
    }
}
public static class StatusType
{
    public enum Type
    {
        InComplete,
        InProgress,
        Completed
    }

    public static string GetStatusName(Type statusType)
    {
        return statusType.ToString();
    }
}