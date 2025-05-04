using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] private UIDocument uiDocument;
    [SerializeField] private string gameSceneName = "GameScene";

    private VisualElement root;
    private VisualElement startScreen;
    private VisualElement inputScreen;
    private VisualElement saveSelectionContainer;
    private Button startButton;
    private Button confirmButton;
    private Button backButton;
    private Button exitButton;
    private Button browseSaveButton;
    private TextField warehouseNameField;
    private TextField playerNameField;
    private Toggle loadSaveToggle;
    private DropdownField saveFileDropdown;
    private string selectedSaveFile = "";
    private Dictionary<string, string> saveFilePaths = new Dictionary<string, string>();

    private void OnEnable()
    {
        root = uiDocument.rootVisualElement;

        // Get references to UI elements
        startScreen = root.Q<VisualElement>("startScreen");
        inputScreen = root.Q<VisualElement>("inputScreen");
        startButton = root.Q<Button>("startButton");
        exitButton = root.Q<Button>("exitButton");
        confirmButton = root.Q<Button>("confirmButton");
        backButton = root.Q<Button>("backButton");
        warehouseNameField = root.Q<TextField>("warehouseNameField");
        playerNameField = root.Q<TextField>("playerNameField");

        // Get references to save selection elements
        loadSaveToggle = root.Q<Toggle>("loadSaveToggle");
        saveSelectionContainer = root.Q<VisualElement>("saveSelectionContainer");
        saveFileDropdown = root.Q<DropdownField>("saveFileDropdown");
        browseSaveButton = root.Q<Button>("browseSaveButton");

        // Set default values from Globals
        warehouseNameField.value = Globals.warehouseName;
        playerNameField.value = Globals.playerName;

        // Register button click events
        startButton.clicked += OnStartButtonClicked;
        confirmButton.clicked += OnConfirmButtonClicked;
        backButton.clicked += OnBackButtonClicked;
        exitButton.clicked += () => ExitGame();

        // Register save selection events
        loadSaveToggle.RegisterValueChangedCallback(OnLoadSaveToggleChanged);
        browseSaveButton.clicked += OnBrowseSaveButtonClicked;
        saveFileDropdown.RegisterValueChangedCallback(OnSaveFileSelected);

        // Check if we need to load data at start
        // StartCoroutine(LoadDataAtStart());
    }
    private void ExitGame()
    {
        Debug.Log("Exiting game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    private IEnumerator LoadDataAtStart()
    {
        // Only load data if we have a saved game
        if (PlayerPrefs.HasKey("HasSavedGame") && PlayerPrefs.GetInt("HasSavedGame") == 1)
        {
            yield return StartCoroutine(Globals.Load());

            // Update UI fields with loaded data
            warehouseNameField.value = Globals.warehouseName;
            playerNameField.value = Globals.playerName;
        }
        else
        {
            yield return null;
        }
    }

    private void OnStartButtonClicked()
    {
        // Show input screen, hide start screen
        startScreen.style.display = DisplayStyle.None;
        inputScreen.style.display = DisplayStyle.Flex;

        // Load available save files
        PopulateSaveFileDropdown();
    }

    private void OnBackButtonClicked()
    {
        // Show start screen, hide input screen
        startScreen.style.display = DisplayStyle.Flex;
        inputScreen.style.display = DisplayStyle.None;
    }

    private void OnLoadSaveToggleChanged(ChangeEvent<bool> evt)
    {
        // Show/hide save selection container based on toggle state
        saveSelectionContainer.style.display = evt.newValue ? DisplayStyle.Flex : DisplayStyle.None;

        if (evt.newValue)
        {
            // If toggle is turned on, select the first save file as default
            if (saveFileDropdown.choices.Count > 0 && saveFileDropdown.choices[0] != "No saved games found")
            {
                saveFileDropdown.index = 0;
                string defaultSaveLabel = saveFileDropdown.choices[0];
                if (saveFilePaths.TryGetValue(defaultSaveLabel, out string filePath))
                {
                    SelectSaveFile(filePath);
                    Globals.loadSave = true;
                    // Disable name fields when loading a save
                    warehouseNameField.SetEnabled(false);
                    playerNameField.SetEnabled(false);
                }
            }
        }
        else
        {
            // If toggle is turned off, clear the selection
            selectedSaveFile = "";
            Globals.loadSave = false;
            warehouseNameField.SetEnabled(true);
            playerNameField.SetEnabled(true);
        }
    }

    private void OnBrowseSaveButtonClicked()
    {
        // Note: Unity WebGL doesn't support file browsing
        // This would typically open a file dialog, but we'll simulate by finding all saves
        PopulateSaveFileDropdown();
    }

    private void OnSaveFileSelected(ChangeEvent<string> evt)
    {
        string selectedSaveLabel = evt.newValue;

        // Get the save file path from the dictionary
        if (saveFilePaths.TryGetValue(selectedSaveLabel, out string filePath))
        {
            SelectSaveFile(filePath);
        }
    }

    private void PopulateSaveFileDropdown()
    {
        // Clear existing dictionary and dropdown choices
        saveFilePaths.Clear();
        List<string> dropdownChoices = new List<string>();

        // Get all warehouse save files using new Globals method
        List<string> saveFiles = Globals.GetAllSaveFiles();

        if (saveFiles.Count == 0)
        {
            dropdownChoices.Add("No saved games found");
            saveFileDropdown.choices = dropdownChoices;
            saveFileDropdown.index = 0;
            return;
        }

        // Process each save file
        foreach (string saveFileName in saveFiles)
        {
            Debug.Log($"Processing save file: {saveFileName}");
            string filePath = Path.Combine(Application.persistentDataPath, saveFileName);
            StartCoroutine(ProcessSaveFileForDropdown(filePath, dropdownChoices, () =>
            {
                // After all files are processed, update the dropdown
                if (dropdownChoices.Count > 0)
                {
                    saveFileDropdown.choices = dropdownChoices;
                    saveFileDropdown.index = 0;
                }
            }));
        }
    }

    private IEnumerator ProcessSaveFileForDropdown(string filePath, List<string> dropdownChoices, System.Action onComplete)
    {
        GlobalVariables data = Globals.LoadFromSpecificFile(filePath);
        // Create descriptive label
        string warehouseName = data.warehouseName;
        double playerMoney = data.playerMoney;
        string saveDate = File.GetLastWriteTime(filePath).ToString("MM/dd/yyyy HH:mm");

        string displayLabel = $"{warehouseName} - ${playerMoney:F2} - {saveDate}";

        // Add to choices and path dictionary
        dropdownChoices.Add(displayLabel);
        saveFilePaths[displayLabel] = filePath;

        onComplete?.Invoke();
        yield return null; // Yield to allow UI to update
    }

    private void SelectSaveFile(string filePath)
    {
        // Store selected save file path
        selectedSaveFile = filePath;
        GlobalVariables data = Globals.LoadFromSpecificFile(filePath);
        // Update the form fields with data from the save
        warehouseNameField.value = data.warehouseName;
        playerNameField.value = data.playerName;
    }

    private void OnConfirmButtonClicked()
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(warehouseNameField.value))
        {
            warehouseNameField.value = "My Warehouse"; // Default value
        }

        if (string.IsNullOrWhiteSpace(playerNameField.value))
        {
            playerNameField.value = "Player"; // Default value
        }

        // Save inputs to Globals
        Globals.warehouseName = warehouseNameField.value;
        Globals.playerName = playerNameField.value;

        // Set game state to playing
        Globals.gameState = State.Playing;

        // Save data
        StartCoroutine(SaveAndStartGame());
    }

    private IEnumerator SaveAndStartGame()
    {
        if (!string.IsNullOrEmpty(selectedSaveFile) && loadSaveToggle.value)
        {
            // Loading an existing save
            Globals.loadSave = true;

            // Extract warehouse name from the save file to load properly
            GlobalVariables data = Globals.LoadFromSpecificFile(selectedSaveFile);

            string originalWarehouseName = data.warehouseName;
            int originalWarehouseId = data.warehouseId;

            // Load from the original save file data
            yield return StartCoroutine(Globals.LoadByWarehouseName(originalWarehouseName));

            // If the user has changed the warehouse name, treat it as a new save
            bool creatingNewSave = warehouseNameField.value != originalWarehouseName;

            if (creatingNewSave)
            {
                // Create a new warehouse with a new ID based on the entered name
                Globals.warehouseName = warehouseNameField.value;
                Globals.warehouseId = System.DateTime.Now.GetHashCode(); // Generate new unique ID
            }
            else
            {
                // Keep the original warehouse ID to prevent duplicates
                Globals.warehouseId = originalWarehouseId;
            }

            // Always update player name as this doesn't affect save identity
            Globals.playerName = playerNameField.value;
        }
        else
        {
            // Starting a new game
            Globals.warehouseName = warehouseNameField.value;
            Globals.playerName = playerNameField.value;
            Globals.loadSave = false;

            // Initialize new game values
            Globals.playerMoney = 100000; // Starting money for new game
            Globals.warehouselevel = 1;
            Globals.warehouseExp = 0;
            Globals.playerLevel = 1;
            Globals.playerExp = 0;

            // Generate a unique ID for the new warehouse
            Globals.warehouseId = System.DateTime.Now.GetHashCode();
        }

        // Mark that we have a saved game
        PlayerPrefs.SetInt("HasSavedGame", 1);
        PlayerPrefs.Save();

        // Load game scene
        SceneManager.LoadScene(gameSceneName);
    }

    private void OnDeleteSaveButtonClicked()
    {
        if (saveFileDropdown.index >= 0 && saveFileDropdown.choices.Count > 0)
        {
            string selectedSaveLabel = saveFileDropdown.value;

            if (saveFilePaths.TryGetValue(selectedSaveLabel, out string filePath))
            {
                // Extract warehouse name from the selected save file
                StartCoroutine(DeleteSaveAfterLoading(filePath));
            }
        }
    }

    private IEnumerator DeleteSaveAfterLoading(string filePath)
    {
        GlobalVariables data = Globals.LoadFromSpecificFile(filePath);

        // Delete the save file
        Globals.DeleteSaveFile(data.warehouseName);

        // Refresh the dropdown
        PopulateSaveFileDropdown();
        yield return null; // Yield to allow UI to update
    }
}
