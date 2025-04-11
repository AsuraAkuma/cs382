using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;

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
    public static List<Employee> warehouseEmployees; // Array of employees in the warehouse
    public static int warehouseMaxEmployees; // Maximum number of employees allowed in the warehouse
    // HR data
    public static List<Employee> newHires; // Array of new hires in the warehouse
    // Department data
    public static List<Department> departments; // Array of departments in the warehouse
    public static int departmentCount; // Number of departments in the warehouse
    public static List<Department> disabledDepartments; // Departments that are currently disabled
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
    public IEnumerator Save()
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
            disabledDepartments = new List<Department>() // Initialize disabledDepartments as an empty list
        };

        // Convert the data to JSON
        string jsonData = JsonUtility.ToJson(data);

        // Create a UnityWebRequest for a POST request
        using (UnityWebRequest request = new UnityWebRequest("https://example.com/api/save", "POST"))
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
}