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

        // Register save selection events
        loadSaveToggle.RegisterValueChangedCallback(OnLoadSaveToggleChanged);
        browseSaveButton.clicked += OnBrowseSaveButtonClicked;
        saveFileDropdown.RegisterValueChangedCallback(OnSaveFileSelected);

        // Check if we need to load data at start
        // StartCoroutine(LoadDataAtStart());
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
                }
            }
        }
        else
        {
            // If toggle is turned off, clear the selection
            selectedSaveFile = "";
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

        // Find all save files
        string saveDirectory = Application.persistentDataPath;
        string[] saveFiles = Directory.GetFiles(saveDirectory, "*.json");

        if (saveFiles.Length == 0)
        {
            dropdownChoices.Add("No saved games found");
            saveFileDropdown.choices = dropdownChoices;
            saveFileDropdown.index = 0;
            return;
        }

        // Process each save file
        foreach (string saveFile in saveFiles)
        {
            StartCoroutine(ProcessSaveFileForDropdown(saveFile, dropdownChoices, () =>
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
        yield return StartCoroutine(Globals.LoadFromSpecificFile(filePath));

        // Create descriptive label
        string warehouseName = Globals.tempSaveData.warehouseName;
        double playerMoney = Globals.tempSaveData.playerMoney;
        string saveDate = File.GetLastWriteTime(filePath).ToString("MM/dd/yyyy HH:mm");

        string displayLabel = $"{warehouseName} - ${playerMoney:F2} - {saveDate}";

        // Add to choices and path dictionary
        dropdownChoices.Add(displayLabel);
        saveFilePaths[displayLabel] = filePath;

        onComplete?.Invoke();
    }

    private void SelectSaveFile(string filePath)
    {
        // Store selected save file path
        Globals.saveFilePath = filePath;

        // Load the save data to populate fields
        // StartCoroutine(Globals.Load());

        // Update the form fields with data from the save
        warehouseNameField.value = Globals.tempSaveData.warehouseName;
        playerNameField.value = Globals.tempSaveData.playerName;
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
            // Load the selected save file
            yield return StartCoroutine(Globals.LoadFromSpecificFile(selectedSaveFile));

            // Apply the temp save data to the global state
            Globals.warehouseName = Globals.tempSaveData.warehouseName;
            Globals.warehouseId = Globals.tempSaveData.warehouseId;
            Globals.warehouselevel = Globals.tempSaveData.warehouselevel;
            Globals.warehouseValue = Globals.tempSaveData.warehouseValue;
            Globals.warehouseExp = Globals.tempSaveData.warehouseExp;
            // Add other fields as needed

            // Override with user-entered values
            Globals.warehouseName = warehouseNameField.value;
            Globals.playerName = playerNameField.value;
        }
        else
        {
            // Just use the entered values for a new game
            Globals.warehouseName = warehouseNameField.value;
            Globals.playerName = playerNameField.value;
        }

        // Save the data
        yield return StartCoroutine(Globals.Save());

        // Mark that we have a saved game
        PlayerPrefs.SetInt("HasSavedGame", 1);
        PlayerPrefs.Save();

        // Load game scene
        SceneManager.LoadScene(gameSceneName);
    }
}
