using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public UIDocument gameUI;
    Button upgradesButton;
    Button employeesButton;
    Button storeButton;
    Button newHiresButton;
    Button editNameButton;
    Button editDepartmentButton;
    Button promoteButton;
    Button fireButton;
    Employee selectedEmployee;
    Button editNamePanelConfirmButton;
    VisualElement editNamePanelCancelButton;
    Button editDepartmentPanelConfirmButton;
    VisualElement editDepartmentPanelCancelButton;
    VisualElement employeeManagerCloseButton;
    VisualElement upgradePanel;
    string currentPanel = "employeesPanel";
    public Actions.GameSystem gameActions;
    public Actions.HR hrActions;
    public Sprite defaultEmployeeSprite;
    VisualElement storeItemHR;
    VisualElement storeItemInbound;
    VisualElement storeItemFluidLoad;
    VisualElement pauseScreen;
    Button pauseScreenSaveButton;
    Button pauseScreenExitButton;
    Button pauseScreenResumeButton;
    Button pauseScreenMainMenuButton;
    float previousTimeScale;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        VisualElement employeeManager = gameUI.rootVisualElement.Q<VisualElement>("employeeManager");
        gameActions = gameObject.AddComponent<Actions.GameSystem>();
        hrActions = gameObject.AddComponent<Actions.HR>();
        upgradesButton = gameUI.rootVisualElement.Q<Button>("panelNavUpgradesButton");
        employeesButton = gameUI.rootVisualElement.Q<Button>("panelNavEmployeesButton");
        storeButton = gameUI.rootVisualElement.Q<Button>("panelNavStoreButton");
        newHiresButton = gameUI.rootVisualElement.Q<Button>("panelNavNewHiresButton");
        editNameButton = employeeManager.Q<Button>("EMEditNameButton");
        editDepartmentButton = employeeManager.Q<Button>("EMEditDepartmentButton");
        promoteButton = employeeManager.Q<Button>("EMPromoteButton");
        fireButton = employeeManager.Q<Button>("EMFireButton");
        editNamePanelConfirmButton = gameUI.rootVisualElement.Q<Button>("editNamePanelConfirmButton");
        editNamePanelCancelButton = gameUI.rootVisualElement.Q<VisualElement>("editNamePanelCancelButton");
        editDepartmentPanelConfirmButton = gameUI.rootVisualElement.Q<Button>("editDepartmentPanelConfirmButton");
        editDepartmentPanelCancelButton = gameUI.rootVisualElement.Q<VisualElement>("editDepartmentPanelCancelButton");
        employeeManagerCloseButton = gameUI.rootVisualElement.Q<VisualElement>("employeeManagerHeaderClose");
        upgradePanel = gameUI.rootVisualElement.Q<VisualElement>("upgradesPanel");
        storeItemHR = gameUI.rootVisualElement.Q<VisualElement>("storeItemDepartmentHR");
        storeItemInbound = gameUI.rootVisualElement.Q<VisualElement>("storeItemDepartmentInbound");
        storeItemFluidLoad = gameUI.rootVisualElement.Q<VisualElement>("storeItemDepartmentFluidLoad");
        pauseScreen = gameUI.rootVisualElement.Q<VisualElement>("pauseScreen");
        pauseScreenSaveButton = pauseScreen.Q<Button>("pauseSaveButton");
        pauseScreenExitButton = pauseScreen.Q<Button>("pauseExitButton");
        pauseScreenResumeButton = pauseScreen.Q<Button>("pauseResumeButton");
        pauseScreenMainMenuButton = pauseScreen.Q<Button>("pauseMenuButton");

        // Hide pause screen initially
        pauseScreen.style.display = DisplayStyle.None;

        // Register pause screen button callbacks
        pauseScreenSaveButton.RegisterCallback<ClickEvent>(OnPauseScreenButtonClick);
        pauseScreenExitButton.RegisterCallback<ClickEvent>(OnPauseScreenButtonClick);
        pauseScreenResumeButton.RegisterCallback<ClickEvent>(OnPauseScreenButtonClick);
        pauseScreenMainMenuButton.RegisterCallback<ClickEvent>(OnPauseScreenButtonClick);

        // Add click event listeners to the buttons
        upgradesButton.RegisterCallback<ClickEvent>(OnPanelNavButtonClick);
        employeesButton.RegisterCallback<ClickEvent>(OnPanelNavButtonClick);
        storeButton.RegisterCallback<ClickEvent>(OnPanelNavButtonClick);
        newHiresButton.RegisterCallback<ClickEvent>(OnPanelNavButtonClick);
        editNameButton.RegisterCallback<ClickEvent>(OnEmployeeManagerButtonClick);
        editDepartmentButton.RegisterCallback<ClickEvent>(OnEmployeeManagerButtonClick);
        promoteButton.RegisterCallback<ClickEvent>(OnEmployeeManagerButtonClick);
        fireButton.RegisterCallback<ClickEvent>(OnEmployeeManagerButtonClick);
        editNamePanelConfirmButton.RegisterCallback<ClickEvent>(OnEditNamePanelButtonClick);
        editNamePanelCancelButton.RegisterCallback<ClickEvent>(OnEditNamePanelButtonClick);
        editDepartmentPanelConfirmButton.RegisterCallback<ClickEvent>(OnEditDepartmentPanelButtonClick);
        editDepartmentPanelCancelButton.RegisterCallback<ClickEvent>(OnEditDepartmentPanelButtonClick);
        employeeManagerCloseButton.RegisterCallback<ClickEvent>(OnEmployeeManagerButtonClick);
        // Register click event for the edit name panel buttons
        Globals.gameController = this;
        if (Globals.tutorialStatus == StatusType.Type.Completed)
        {
            // Load the game state
            LoadGameState();
        }
        else
        {
            // Start the tutorial
            ProgressTutorial();
        }
        // Update the employee UI list
        UpdateEmployeeUIList();
        // Update the new hire UI list
        UpdateNewHireUIList();

        Time.timeScale = 40f;
        if (Globals.loadSave == false && Globals.departments.Count == 0)
        {
            // TESTING ONLY
            // Create a new hire
            // HR hrDepartment = gameObject.AddComponent<HR>();
            // hrDepartment.departmentType = DepartmentTypes.Type.HR;
            // hrDepartment.departmentName = "HR";
            // hrDepartment.departmentLevel = 1;
            // hrDepartment.capacity = 10;
            // Inbound inboundDepartment = gameObject.AddComponent<Inbound>();
            // inboundDepartment.departmentType = DepartmentTypes.Type.Inbound;
            // inboundDepartment.departmentName = "Inbound";
            // inboundDepartment.departmentLevel = 1;
            // inboundDepartment.capacity = 10;
            // FluidLoad fluidDepartment = gameObject.AddComponent<FluidLoad>();
            // fluidDepartment.departmentType = DepartmentTypes.Type.FluidLoad;
            // fluidDepartment.departmentName = "FluidLoad";
            // fluidDepartment.departmentLevel = 1;
            // fluidDepartment.capacity = 10;

            Globals.playerMoney = 170000;
        }
        int departmentCountHR = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.HR);
        int departmentCountInbound = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Inbound);
        int departmentCountFluidLoad = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.FluidLoad);
        int itemCostValueHR = Globals.departmentCost + 5000 * (departmentCountHR + 1);
        int itemCostValueInbound = Globals.departmentCost + 5000 * (departmentCountInbound + 1);
        int itemCostValueFluidLoad = Globals.departmentCost + 5000 * (departmentCountFluidLoad + 1);
        storeItemHR.Q<Label>("storeItemCost").text = $"Cost\n${itemCostValueHR}";
        storeItemInbound.Q<Label>("storeItemCost").text = $"Cost\n${itemCostValueInbound}";
        storeItemFluidLoad.Q<Label>("storeItemCost").text = $"Cost\n${itemCostValueFluidLoad}";
        storeItemHR.RegisterCallback<ClickEvent>(OnStoreItemClick);
        storeItemInbound.RegisterCallback<ClickEvent>(OnStoreItemClick);
        storeItemFluidLoad.RegisterCallback<ClickEvent>(OnStoreItemClick);
        Globals.gameState = State.Playing;
        // Globals.departments.Add(hrDepartment);
        StartCoroutine(gameActions.CreateNewHire());
        UpdateNewHireUIList();
        UpdateDepartmentUIList();

        // Update separtment UI list every 10 seconds
        InvokeRepeating("UpdateDepartmentUIList", 0f, 10f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Globals.gameState != State.Playing)
        {
            // Pause the game logic here
            return;
        }
        if (selectedEmployee != null)
        {
            // UpdateUpgradeUIList();
        }

        UpdateHeaderUI();
        if (Globals.playerMoney < 0)
        {
            GameOver();
        }
        // UpdateDepartmentUIList();
        // Elapse game time
        // One real minute = 1 game hour
        // One real second = 1 game minute
        Globals.gameTimeElapsed += Time.fixedDeltaTime / 60f;
        // When game time is 24 in game hours, reset to 0
        if (Globals.gameTimeElapsed >= 24f)
        {
            Globals.gameTimeElapsed = 0f;
            Globals.gameDaysElapsed++;
            if (Globals.gameDaysElapsed == 15)
            {
                if (Globals.playerMoney < 100000)
                {
                    GameOver();
                }
            }
            // subtract employee salary from balance
            foreach (Employee employee in Globals.warehouseEmployees)
            {
                Globals.playerMoney -= employee.salary;
            }
            if (Globals.daysSinceLastNewHire == 2)
            {
                Globals.daysSinceLastNewHire = 0;
                StartCoroutine(gameActions.CreateNewHire());
                if (Globals.newHires.Count > 5)
                {
                    // Remove the oldest new hire
                    Globals.newHires.RemoveAt(0);
                }
                UpdateNewHireUIList();
            }
            else
            {
                Globals.daysSinceLastNewHire++;
            }
            // Debug.Log("Game time reset to 0.");
        }
        // Debug.Log($"Game time elapsed: {Globals.gameTimeElapsed} hours.");
    }
    void GameOver(bool deleteSave = true)
    {
        // Display game over dialog or notification here
        VisualElement gameOverDialog = gameUI.rootVisualElement.Q<VisualElement>("gameOverDialog");
        if (gameOverDialog != null)
        {
            gameOverDialog.style.display = DisplayStyle.Flex;
            gameOverDialog.Q<Label>("gameOverMessage").text = $"Game Over! You've gone bankrupt after {Globals.gameDaysElapsed} days.";
        }


        if (deleteSave)
        {
            // Delete any existing save files
            Globals.DeleteSaveFile(Globals.warehouseName);
            Debug.Log("Save data deleted due to game over condition");
        }
        // reset globals
        // Reset all game state variables
        Globals.playerMoney = 100000;
        Globals.warehouseName = "My Warehouse";
        Globals.playerName = "Guest";
        Globals.gameTimeElapsed = 0f;
        Globals.gameDaysElapsed = 0;
        Globals.daysSinceLastNewHire = 0;
        Globals.departments.Clear();
        Globals.warehouseEmployees.Clear();
        Globals.newHires.Clear();
        Globals.tutorialStatus = StatusType.Type.InComplete;
        Globals.tutorialStep = 0;
        Globals.gameState = State.NotPlaying;

        // Clean up any remaining game objects and components
        foreach (var department in FindObjectsByType<Department>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            Destroy(department);
        }
        foreach (var employee in FindObjectsByType<Employee>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            Destroy(employee);
        }

        // Return to start menu after a short delay
        SceneManager.LoadScene("StartMenu");
    }
    // Add input handling in Update to check for Escape key press
    void Update()
    {
        // Check for escape key press to toggle pause screen
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseScreen();
        }
    }

    // Method to toggle the pause screen
    private void TogglePauseScreen()
    {
        if (pauseScreen.style.display == DisplayStyle.None)
        {
            // Show pause screen and pause game
            pauseScreen.style.display = DisplayStyle.Flex;
            previousTimeScale = Time.timeScale;
            Time.timeScale = 0f;
            Globals.gameState = State.Paused;
        }
        else
        {
            // Hide pause screen and resume game
            pauseScreen.style.display = DisplayStyle.None;
            Time.timeScale = previousTimeScale;
            Globals.gameState = State.Playing;
        }
    }

    // Handler for pause screen button clicks
    private void OnPauseScreenButtonClick(ClickEvent evt)
    {
        Button clickedButton = evt.currentTarget as Button;

        switch (clickedButton.name)
        {
            case "pauseSaveButton":
                SaveGame();
                break;

            case "pauseExitButton":
                ExitGame();
                break;

            case "pauseResumeButton":
                TogglePauseScreen();
                break;
            case "pauseMenuButton":
                // Return to start menu
                SaveGame();
                GameOver(false);
                break;
            default:
                Debug.Log("Unknown pause screen button clicked");
                break;
        }
    }

    // Method to save the game state
    private void SaveGame()
    {
        Debug.Log("Saving game...");
        StartCoroutine(Globals.Save());
    }

    // Method to exit the game
    private void ExitGame()
    {
        Debug.Log("Exiting game...");
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    // Method to start the tutorial
    private void ProgressTutorial()
    {
        // Get the current tutorial step
        int currentStep = Globals.tutorialStep;
        VisualElement tutorialContainer = gameUI.rootVisualElement.Q<VisualElement>("tutorialContainer");
        VisualElement tutorialStep1 = tutorialContainer.Q<VisualElement>("step1");
        VisualElement tutorialStep2 = tutorialContainer.Q<VisualElement>("step2");
        VisualElement tutorialStep3 = tutorialContainer.Q<VisualElement>("step3");
        VisualElement tutorialStep4 = tutorialContainer.Q<VisualElement>("step4");
        VisualElement tutorialStep5 = tutorialContainer.Q<VisualElement>("step5");
        VisualElement tutorialStep6 = tutorialContainer.Q<VisualElement>("step6");
        VisualElement tutorialStep7 = tutorialContainer.Q<VisualElement>("step7");
        VisualElement tutorialStep8 = tutorialContainer.Q<VisualElement>("step8");
        switch (currentStep)
        {
            case 0:
                // Start the tutorial step 1
                tutorialStep1.style.display = DisplayStyle.Flex;
                tutorialStep1.Q<VisualElement>("highlight").RegisterCallback<ClickEvent>(evt =>
                {
                    // Show the store panel
                    ShowSidePanel("storePanel");
                });
                Globals.tutorialStep++;
                break;
            case 1:
                // Start the tutorial step 2
                tutorialStep1.style.display = DisplayStyle.None;
                tutorialStep2.style.display = DisplayStyle.Flex;
                Globals.tutorialStep++;
                break;

            case 2:
                // Start the tutorial step 3
                tutorialStep2.style.display = DisplayStyle.None;
                tutorialStep3.style.display = DisplayStyle.Flex;
                ShowSidePanel("employeesPanel");
                Globals.tutorialStep++;
                break;
            case 3:
                // Start the tutorial step 4
                tutorialStep3.style.display = DisplayStyle.None;
                tutorialStep4.style.display = DisplayStyle.Flex;
                Globals.tutorialStep++;
                // Wait 10 seconds then progress to next tutorial step
                StartCoroutine(DelayedTutorialProgress(10f));
                break;
            case 4:
                // Start the tutorial step 5
                tutorialStep4.style.display = DisplayStyle.None;
                tutorialStep5.style.display = DisplayStyle.Flex;
                Globals.tutorialStep++;
                break;
            case 5:
                // Start the tutorial step 6
                tutorialStep5.style.display = DisplayStyle.None;
                tutorialStep6.style.display = DisplayStyle.Flex;
                Globals.tutorialStep++;
                break;
            case 6:
                // Start the tutorial step 7
                tutorialStep6.style.display = DisplayStyle.None;
                tutorialStep7.style.display = DisplayStyle.Flex;
                Globals.tutorialStep++;
                ShowSidePanel("employeesPanel");
                StartCoroutine(DelayedTutorialProgress(10f));
                break;
            case 7:
                // Start the tutorial step 8
                tutorialStep7.style.display = DisplayStyle.None;
                tutorialStep8.style.display = DisplayStyle.Flex;
                Globals.tutorialStep++;
                StartCoroutine(DelayedTutorialProgress(10f));
                break;
            case 8:
                // Complete the tutorial
                tutorialStep8.style.display = DisplayStyle.None;
                tutorialContainer.style.display = DisplayStyle.None;
                Globals.tutorialStatus = StatusType.Type.Completed;
                Globals.tutorialStep = 0;
                // Unlock store items
                storeItemHR.SetEnabled(true);
                storeItemInbound.SetEnabled(true);
                storeItemFluidLoad.SetEnabled(true);
                break;
            default:
                // Debug.Log("No more tutorial steps available.");
                break;
        }
    }
    private IEnumerator DelayedTutorialProgress(float delay)
    {
        yield return new WaitForSeconds(delay);
        ProgressTutorial();
    }
    // Method to load the game state
    private void LoadGameState()
    {
        // Add logic to load the game state here
        StartCoroutine(Globals.Load());
        // Debug.Log("Game state loaded.");
    }
    private void UpdateHeaderUI()
    {
        // Get the root visual element of the game UI
        VisualElement root = gameUI.rootVisualElement;
        // Get the header UI elements
        VisualElement header = root.Q<VisualElement>("header");
        Label warehouseName = header.Q<Label>("warehouseName");
        Label balance = header.Q<Label>("balance");
        Label playerName = header.Q<Label>("playerName");
        Label dateTime = header.Q<Label>("dateTime");
        Label dateTime = header.Q<Label>("dateTime");
        // Update the header UI elements with the current values
        warehouseName.text = Globals.warehouseName;
        balance.text = $"Balance: ${Globals.playerMoney}";
        playerName.text = Globals.playerName;
        int hours = Mathf.FloorToInt(Globals.gameTimeElapsed);
        int minutes = Mathf.FloorToInt((Globals.gameTimeElapsed - hours) * 60);
        dateTime.text = $"Days: {Globals.gameDaysElapsed} " +
                $"Time: {hours:D2}:{minutes:D2}";
        int hours = Mathf.FloorToInt(Globals.gameTimeElapsed);
        int minutes = Mathf.FloorToInt((Globals.gameTimeElapsed - hours) * 60);
        dateTime.text = $"Days: {Globals.gameDaysElapsed} " +
                $"Time: {hours:D2}:{minutes:D2}";
    }
    public void UpdateEmployeeUIList()
    {
        // Get the root visual element of the game UI
        VisualElement root = gameUI.rootVisualElement;
        // Get the employee list container from the UI
        ScrollView employeeListContainer = root.Q<ScrollView>("employeeList");
        // Remove all click event listeners from the existing employee list items
        foreach (VisualElement employeeListItem in employeeListContainer.Children())
        {
            employeeListItem.UnregisterCallback<ClickEvent>(OnEmployeeListItemClick);
        }
        // Clear the existing employee list
        employeeListContainer.Clear();
        // Loop through each employee in the warehouse and add them to the UI
        foreach (Employee employee in Globals.warehouseEmployees)
        {
            // Create a new VisualElement for the employee list item
            VisualElement employeeListItem = new VisualElement();
            employeeListItem.AddToClassList("employeeListItem");
            // Set the name of the employee list item for identification
            employeeListItem.name = employee.employeeName;
            // Create a VisualElement for the employee's picture
            VisualElement employeeListItemPicture = new VisualElement();
            employeeListItemPicture.AddToClassList("employeeListItemPicture");

            // Ensure employee sprite is set
            if (employee.employeeSprite == null)
            {
                employee.employeeSprite = defaultEmployeeSprite;
            }
            employeeListItemPicture.style.backgroundImage = new StyleBackground(employee.employeeSprite);

            employeeListItem.Add(employeeListItemPicture);

            // Create a Label for the employee's details
            Label employeeListItemDetails = new Label
            {
                text = $"Name: {employee.employeeName}\n" +
                        $"Department: {(employee.department != null ? employee.department.departmentName : "None")}\n" +
                        $"Speed: {employee.speed:F2}\n" +
                        $"Efficiency: {employee.efficiency:F2}\n" +
                        $"Stamina: {employee.stamina:F2}\n" +
                        $"Strength: {employee.strength:F2}\n" +
                        $"Focus: {employee.focus:F2}\n" +
                        $"Salary/Cost: ${employee.salary}\n"
            };
            employeeListItemDetails.AddToClassList("employeeListItemDetails");
            employeeListItem.Add(employeeListItemDetails);

            // Add the employee list item to the container
            employeeListContainer.Add(employeeListItem);
        }
        // Add click event listener to each employee list item
        foreach (VisualElement employeeListItem in employeeListContainer.Children())
        {
            employeeListItem.RegisterCallback<ClickEvent>(OnEmployeeListItemClick);
        }
        // Debug.Log("Employee UI list updated.");
    }
    public void UpdateNewHireUIList()
    {
        // Get the root visual element of the game UI
        VisualElement root = gameUI.rootVisualElement;
        // Get the new hire list container from the UI
        ScrollView newHireListContainer = root.Q<ScrollView>("newHireList");
        // Clear the existing new hire list
        newHireListContainer.Clear();
        // Loop through each new hire and add them to the UI
        foreach (Employee employee in Globals.newHires)
        {
            if (employee.isHired == true)
            {
                // Skip the new hire if they are already hired or rejected
                continue;
            }
            // Create a new VisualElement for the new hire list item
            VisualElement newHireListItem = new VisualElement();
            newHireListItem.AddToClassList("newHireListItem");

            // Create a VisualElement for the new hire's picture
            VisualElement newHireListItemPicture = new VisualElement();
            newHireListItemPicture.AddToClassList("newHireListItemPicture");
            if (employee.employeeSprite == null)
            {
                employee.employeeSprite = defaultEmployeeSprite;
            }
            newHireListItemPicture.style.backgroundImage = new StyleBackground(employee.employeeSprite);
            newHireListItem.Add(newHireListItemPicture);
            // Create a Label for the new hire's details
            Label newHireListItemDetails = new Label
            {
                text = $"Name: {employee.employeeName}\n" +
                       $"Department: {(employee.department != null ? employee.department.departmentName : "None")}\n" +
                       $"Speed: {employee.GetSpeed():F2}\n" +
                       $"Efficiency: {employee.GetEfficiency():F2}\n" +
                       $"Stamina: {employee.GetStamina():F2}\n" +
                       $"Strength: {employee.GetStrength():F2}\n" +
                       $"Focus: {employee.GetFocus():F2}\n" +
                       $"Salary/Cost: ${employee.salary}\n"
            };
            newHireListItemDetails.AddToClassList("newHireListItemDetails");
            newHireListItem.Add(newHireListItemDetails);
            // Get Hr department
            Department hrDepartment = Globals.departments.Find(d => d.departmentType == DepartmentTypes.Type.HR);
            if (hrDepartment == null)
            {
                // Debug.LogError("HR Department not found.");
                return;
            }
            ActionRequest actionRequest = new ActionRequest(hrActions.HireEmployee(employee), employee);
            // Create Hire Button
            Button hireButton = new Button(() =>
            {
                if (employee.cost > Globals.playerMoney)
                {
                    // Debug.Log("Not enough money to hire this employee.");
                    return;
                }
                hrDepartment.AddActionRequest(actionRequest);
                // Debug.Log($"Hr Department reqs: {hrDepartment.newActionRequests.Count}");
                // Remove the new hire from the list and update the UI
                employee.isHired = true;
                employee.isFired = false;
                UpdateNewHireUIList();
                if (Globals.tutorialStep == 6)
                {
                    ProgressTutorial();
                }
            })
            {
                text = "Hire",
                name = "newHireListItemHireButton"
            };
            hireButton.AddToClassList("newHireListItemHireButton");
            newHireListItem.Add(hireButton);
            // Create Reject Button
            Button rejectButton = new Button(() => RejectNewHire(employee))
            {
                text = "Reject",
                name = "newHireListItemRejectButton"
            };
            rejectButton.AddToClassList("newHireListItemRejectButton");
            newHireListItem.Add(rejectButton);
            // Add the new hire list item to the container
            newHireListContainer.Add(newHireListItem);
        }
        // Debug.Log("New Hire UI list updated.");
    }
    public void UpdateDepartmentUIList()
    {
        // Get the root visual element of the game UI
        VisualElement root = gameUI.rootVisualElement;
        // Get the department list container from the UI
        ScrollView departmentListContainer = root.Q<ScrollView>("departmentsMainList");

        // Clear the existing department list
        departmentListContainer.Clear();
        // Debug.Log($"Department list count: {Globals.departments.Count}");

        // Loop through each department and add them to the UI
        foreach (Department department in Globals.departments)
        {
            department.AddToUI();
        }
        // Debug.Log("Department UI list updated.");

        // Force the ScrollView to update its layout
        departmentListContainer.style.display = DisplayStyle.None;
        departmentListContainer.style.display = DisplayStyle.Flex;

        // Force layout recalculation
        departmentListContainer.contentContainer.style.height = new StyleLength(StyleKeyword.Auto);

        // Schedule a deferred action to update the ScrollView after the frame completes
        departmentListContainer.schedule.Execute(() =>
        {
            departmentListContainer.contentContainer.style.minHeight = new StyleLength(
                new Length(100f * Globals.departments.Count, LengthUnit.Pixel));
            departmentListContainer.contentViewport.MarkDirtyRepaint();
        }).StartingIn(100);
    }
    private void RejectNewHire(Employee newHire)
    {
        // Add logic to reject the new hire here
        // For example, remove them from the new hires list and update the UI
        Globals.newHires.Remove(newHire);
        UpdateNewHireUIList();
        // Debug.Log($"New hire {newHire.employeeName} rejected.");
    }
    // UI functionality
    // Side panel navigation
    public void ShowSidePanel(string panelName)
    {
        // Get the root visual element of the game UI
        VisualElement root = gameUI.rootVisualElement;
        // Hide the current panel
        root.Q<VisualElement>(currentPanel).style.display = DisplayStyle.None;
        // Show the new panel
        root.Q<VisualElement>(panelName).style.display = DisplayStyle.Flex;
        // Update the current panel name
        currentPanel = panelName;
        if (Globals.tutorialStep == 1 && panelName == "storePanel")
        {
            ProgressTutorial();
        }
        else if (Globals.tutorialStep == 5 && panelName == "newHiresPanel")
        {
            ProgressTutorial();
        }
    }
    private void OnPanelNavButtonClick(ClickEvent evt)
    {
        // Get the button that was clicked
        Button clickedButton = evt.currentTarget as Button;
        // Disable the current panel
        // Get the root visual element of the game UI
        gameUI.rootVisualElement.Q<VisualElement>(currentPanel).style.display = DisplayStyle.None;
        // Get the name of the panel to show from the button's name attribute
        switch (clickedButton.name)
        {
            case "panelNavUpgradesButton":
                ShowSidePanel("upgradesPanel");
                break;
            case "panelNavEmployeesButton":
                ShowSidePanel("employeesPanel");
                break;
            case "panelNavStoreButton":
                ShowSidePanel("storePanel");
                break;
            case "panelNavNewHiresButton":
                ShowSidePanel("newHiresPanel");
                break;
            default:
                // Debug.Log("Invalid button name.");
                return;
        }
    }
    private void OnEmployeeManagerButtonClick(ClickEvent evt)
    {
        // Get the button that was clicked
        Button clickedButton = evt.currentTarget as Button;
        // Get the root visual element of the game UI
        VisualElement root = gameUI.rootVisualElement;
        // Get the employee manager panel
        VisualElement employeeManager = root.Q<VisualElement>("employeeManager");
        Employee targetEmployee;
        switch (clickedButton.name)
        {
            case "EMEditNameButton":
                VisualElement editNamePanel = employeeManager.Q<VisualElement>("EditNamePanel");
                // Show the edit name panel
                editNamePanel.style.display = DisplayStyle.Flex;
                editNamePanel.Q<Label>("EditNamePanelOldValue").text = $"Current Name: {selectedEmployee.employeeName}";
                break;
            case "EMEditDepartmentButton":
                VisualElement editDepartmentPanel = employeeManager.Q<VisualElement>("EditDepartmentPanel");
                DropdownField departmentDropdown = employeeManager.Q<DropdownField>("EditDepartmentPanelInput");
                editDepartmentPanel.style.display = DisplayStyle.Flex;
                departmentDropdown.choices = Globals.departments.Select(d => d.departmentName).ToList();
                editDepartmentPanel.Q<Label>("EditDepartmentPanelOldValue").text = $"Current Department: {(selectedEmployee.department != null ? selectedEmployee.department.departmentName : "None")}";
                break;
            case "EMPromoteButton":
                // Get the HR department
                Department hrDepartment = Globals.departments.Find(d => d.departmentType == DepartmentTypes.Type.HR);
                // Add the action request to the HR department
                targetEmployee = hrDepartment.employees.OrderBy(e => e.actionRequests.Count).FirstOrDefault();
                ActionRequest actionRequest = new ActionRequest(hrActions.PromoteEmployee(selectedEmployee), selectedEmployee);
                hrDepartment.AddActionRequest(actionRequest);
                break;
            case "EMFireButton":
                // Get the HR department
                Department hrDepartmentFire = Globals.departments.Find(d => d.departmentType == DepartmentTypes.Type.HR);
                // Add the action request to the HR department
                targetEmployee = hrDepartmentFire.employees.OrderBy(e => e.actionRequests.Count).FirstOrDefault();
                ActionRequest actionRequestFire = new ActionRequest(hrActions.FireEmployee(selectedEmployee), selectedEmployee);
                hrDepartmentFire.AddActionRequest(actionRequestFire);
                break;
            case "employeeManagerHeaderClose":
                // Hide the employee manager panel
                employeeManager.style.display = DisplayStyle.None;
                ShowSidePanel("employeesPanel");
                // Clear the selected employee
                selectedEmployee = null;
                break;
            default:
                // Debug.Log("Invalid button name.");
                return;
        }
    }
    private void OnEditNamePanelButtonClick(ClickEvent evt)
    {
        // Get the button that was clicked
        Button clickedButton = evt.currentTarget as Button;
        // Get the root visual element of the game UI
        VisualElement root = gameUI.rootVisualElement;
        VisualElement employeeManager = root.Q<VisualElement>("employeeManager");
        // Get the edit name panel
        VisualElement editNamePanel = employeeManager.Q<VisualElement>("EditNamePanel");

        switch (clickedButton.name)
        {
            case "editNamePanelConfirmButton":
                // Get the input field for the new name
                TextField nameInputField = editNamePanel.Q<TextField>("EditNamePanelInput");
                string newName = nameInputField.value;
                // Add logic to update the employee's name
                if (selectedEmployee != null)
                {
                    selectedEmployee.employeeName = newName;
                    // Update the employee list UI
                    UpdateEmployeeUIList();
                    selectedEmployee.department.UpdateEmployeeUIList();
                    // Hide the edit name panel
                    editNamePanel.style.display = DisplayStyle.None;
                    nameInputField.value = null;
                    ShowEmployeeDetails(selectedEmployee);
                }
                else
                {
                    // Debug.LogError("No employee selected.");
                }

                break;
            case "editNamePanelCancelButton":
                // Hide the edit name panel
                editNamePanel.style.display = DisplayStyle.None;
                break;
            default:
                // Debug.Log("Invalid button name.");
                return;
        }
    }
    private void OnEditDepartmentPanelButtonClick(ClickEvent evt)
    {
        // Get the button that was clicked
        Button clickedButton = evt.currentTarget as Button;
        // Get the root visual element of the game UI
        VisualElement root = gameUI.rootVisualElement;
        VisualElement employeeManager = root.Q<VisualElement>("employeeManager");
        // Get the edit department panel
        VisualElement editDepartmentPanel = employeeManager.Q<VisualElement>("EditDepartmentPanel");

        switch (clickedButton.name)
        {
            case "editDepartmentPanelConfirmButton":
                // Get the input field for the new department
                DropdownField departmentDropdown = editDepartmentPanel.Q<DropdownField>("EditDepartmentPanelInput");
                Department newDepartment = Globals.departments.Find(d => d.departmentName == departmentDropdown.value);
                // Add logic to update the employee's department
                if (selectedEmployee != null)
                {
                    selectedEmployee.CancelAction();
                    // Create new employee to match department type
                    Employee newEmployee;
                    switch (newDepartment.departmentType)
                    {
                        case DepartmentTypes.Type.HR:
                            newEmployee = gameObject.AddComponent<HREmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.HREmployee;
                            break;
                        case DepartmentTypes.Type.IT:
                            newEmployee = gameObject.AddComponent<ITEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.ITEmployee;
                            break;
                        case DepartmentTypes.Type.Operations:
                            newEmployee = gameObject.AddComponent<OperationsEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.OperationsEmployee;
                            break;
                        case DepartmentTypes.Type.Inbound:
                            newEmployee = gameObject.AddComponent<InboundEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.InboundEmployee;
                            break;
                        case DepartmentTypes.Type.Sorting:
                            newEmployee = gameObject.AddComponent<SortingEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.SortingEmployee;
                            break;
                        case DepartmentTypes.Type.Repacking:
                            newEmployee = gameObject.AddComponent<RepackingEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.RepackingEmployee;
                            break;
                        case DepartmentTypes.Type.Palletizing:
                            newEmployee = gameObject.AddComponent<PalletizingEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.PalletizingEmployee;
                            break;
                        case DepartmentTypes.Type.WaterSpidering:
                            newEmployee = gameObject.AddComponent<WaterSpiderEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.WaterSpiderEmployee;
                            break;
                        case DepartmentTypes.Type.FluidLoad:
                            newEmployee = gameObject.AddComponent<FluidLoadEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.FluidLoadEmployee;
                            break;
                        case DepartmentTypes.Type.QualityControl:
                            newEmployee = gameObject.AddComponent<QualityControlEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.QualityControlEmployee;
                            break;
                        case DepartmentTypes.Type.Outbound:
                            newEmployee = gameObject.AddComponent<OutboundEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.OutboundEmployee;
                            break;
                        case DepartmentTypes.Type.Maintenance:
                            newEmployee = gameObject.AddComponent<MaintenanceEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.MaintenanceEmployee;
                            break;
                        case DepartmentTypes.Type.Robotics:
                            newEmployee = gameObject.AddComponent<RoboticsEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.RoboticsEmployee;
                            break;
                        case DepartmentTypes.Type.Safety:
                            newEmployee = gameObject.AddComponent<SafetyEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.SafetyEmployee;
                            break;
                        case DepartmentTypes.Type.Cleaning:
                            newEmployee = gameObject.AddComponent<CleaningEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.CleaningEmployee;
                            break;
                        case DepartmentTypes.Type.Security:
                            newEmployee = gameObject.AddComponent<SecurityEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.SecurityEmployee;
                            break;
                        case DepartmentTypes.Type.Learning:
                            newEmployee = gameObject.AddComponent<LearningEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.LearningEmployee;
                            break;
                        case DepartmentTypes.Type.Recruiting:
                            newEmployee = gameObject.AddComponent<RecruitingEmployee>();
                            newEmployee.Paste(selectedEmployee);
                            newEmployee.employeeType = EmployeeType.Type.RecruitingEmployee;
                            break;
                        default:
                            throw new System.ArgumentOutOfRangeException(nameof(newDepartment.departmentType), "Invalid department type.");
                    }
                    // Copy stats from the old employee data to the new employee
                    if (selectedEmployee.department != null)
                    {
                        selectedEmployee.department.RemoveEmployee(selectedEmployee); // Remove old employee from the current department
                        selectedEmployee.department.UpdateEmployeeUIList(); // Update the UI of the current department
                    }
                    Destroy(selectedEmployee); // Destroy the old employee data
                    newEmployee.department = newDepartment; // Assign the new department to the employee
                    newEmployee.departmentType = newDepartment.departmentType; // Set the department type
                    newDepartment.AddEmployee(newEmployee);
                    newEmployee.department = newDepartment;
                    newEmployee.department.UpdateEmployeeUIList();

                    // Update the warehouse employees list
                    int oldIndex = Globals.warehouseEmployees.FindIndex(e => e.employeeName == newEmployee.employeeName);
                    if (oldIndex != -1)
                    {
                        Globals.warehouseEmployees[oldIndex] = newEmployee;
                    }
                    else
                    {
                        Globals.warehouseEmployees.Add(newEmployee);
                    }

                    // Update the employee list UI
                    UpdateEmployeeUIList();
                    ShowEmployeeDetails(newEmployee);
                    // Hide the edit department panel
                    editDepartmentPanel.style.display = DisplayStyle.None;
                    departmentDropdown.value = null;
                }
                else
                {
                    // Debug.LogError("No employee selected.");
                }

                break;
            case "editDepartmentPanelCancelButton":
                // Hide the edit department panel
                editDepartmentPanel.style.display = DisplayStyle.None;
                break;
            default:
                // Debug.Log("Invalid button name.");
                return;
        }
    }
    private void OnEmployeeListItemClick(ClickEvent evt)
    {
        // Get the clicked employee list item
        VisualElement clickedItem = evt.currentTarget as VisualElement;

        if (clickedItem == null)
        {
            Debug.LogError("Clicked item is not a VisualElement.");
            return;
        }
        // Get the employee name from the clicked item
        string employeeName = clickedItem.name;
        // Find the employee in the warehouse employees list
        Employee clickedEmployee = Globals.warehouseEmployees.Find(e => e.employeeName == employeeName);
        if (clickedEmployee != null)
        {
            // Show the employee details in a new panel or popup
            selectedEmployee = clickedEmployee;
            ShowEmployeeDetails(clickedEmployee);
            if (Globals.tutorialStep == 3)
            {
                ProgressTutorial();
            }
        }
        else
        {
            Debug.LogError($"Employee {employeeName} not found in the warehouse employees list.");
        }
    }
    private void ShowEmployeeDetails(Employee employee)
    {
        ShowSidePanel("upgradesPanel");
        // Get the Employee Manager
        VisualElement employeeManager = gameUI.rootVisualElement.Q<VisualElement>("employeeManager");
        if (employeeManager == null) return;

        // Set the employee details in the UI
        // Check if employee sprite is null and assign default sprite if needed
        if (employee.employeeSprite == null)
        {
            employee.employeeSprite = defaultEmployeeSprite;
            Debug.LogWarning($"Employee {employee.employeeName} had null sprite, assigned default sprite");
        }
        employeeManager.Q<VisualElement>("employeeManagerImage").style.backgroundImage = new StyleBackground(employee.employeeSprite);
        employeeManager.Q<Label>("employeeManagerDetails").text = $"Name: {employee.employeeName}\n" +
            $"Department: {(employee.department != null ? employee.department.departmentName : "None")}\n" +
            $"Level: {employee.level}\n" +
            $"Infractions: {employee.infractions}\n" +
            $"Status: {employee.actionState}\n" +
            $"Salary: ${employee.salary}\n" +
            $"Exp: {employee.exp}\n";
        // Set EMStats
        employeeManager.Q<VisualElement>("EMSpeedStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.speed / Globals.employeeStatMax * 100, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMSpeedStat").Q<Label>("EMStatText").text = $"Speed: {employee.speed} / {Globals.employeeStatMax}";
        employeeManager.Q<VisualElement>("EMEfficiencyStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.efficiency / Globals.employeeStatMax * 100, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMEfficiencyStat").Q<Label>("EMStatText").text = $"Efficiency: {employee.efficiency} / {Globals.employeeStatMax}";
        employeeManager.Q<VisualElement>("EMStaminaStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.stamina / Globals.employeeStatMax * 100, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMStaminaStat").Q<Label>("EMStatText").text = $"Stamina: {employee.stamina} / {Globals.employeeStatMax}";
        employeeManager.Q<VisualElement>("EMStrengthStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.strength / Globals.employeeStatMax * 100, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMStrengthStat").Q<Label>("EMStatText").text = $"Strength: {employee.strength} / {Globals.employeeStatMax}";
        employeeManager.Q<VisualElement>("EMFocusStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.focus / Globals.employeeStatMax * 100, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMFocusStat").Q<Label>("EMStatText").text = $"Focus: {employee.focus} / {Globals.employeeStatMax}";
        employeeManager.Q<VisualElement>("EMExperienceStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.experience / Globals.employeeStatMax * 100, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMExperienceStat").Q<Label>("EMStatText").text = $"Experience: {employee.experience} / {Globals.employeeStatMax}";

        // Set traits - add null check and initialization
        string traitsText = "";
        if (employee.traits == null)
        {
            employee.traits = new List<TraitValues>();
            Debug.LogWarning($"Employee {employee.employeeName} had null traits collection, initialized empty list");
            traitsText = "No traits";
        }
        else if (employee.traits.Count == 0)
        {
            traitsText = "No traits";
        }
        else
        {
            foreach (TraitValues trait in employee.traits)
            {
                if (trait == null) continue; // Skip null traits

                // Get the trait name and value
                string traitName = TraitValues.GetTraitName(trait);
                traitsText += $"{traitName}: ";
                if (trait.speed != 0f)
                {
                    if (trait.speed < 0f)
                    {
                        traitsText += $"Speed -{Math.Abs(trait.speed) * 100}%|";
                    }
                    else
                    {
                        traitsText += $"Speed +{trait.speed * 100}%|";
                    }
                }
                if (trait.efficiency != 0f)
                {
                    if (trait.efficiency < 0f)
                    {
                        traitsText += $"Efficiency -{Math.Abs(trait.efficiency) * 100}%|";
                    }
                    else
                    {
                        traitsText += $"Efficiency +{trait.efficiency * 100}%|";
                    }
                }
                if (trait.stamina != 0f)
                {
                    if (trait.stamina < 0f)
                    {
                        traitsText += $"Stamina -{Math.Abs(trait.stamina) * 100}%|";
                    }
                    else
                    {
                        traitsText += $"Stamina +{trait.stamina * 100}%|";
                    }
                }
                if (trait.strength != 0f)
                {
                    if (trait.strength < 0f)
                    {
                        traitsText += $"Strength -{Math.Abs(trait.strength) * 100}%|";
                    }
                    else
                    {
                        traitsText += $"Strength +{trait.strength * 100}%|";
                    }
                }
                if (trait.focus != 0f)
                {
                    if (trait.focus < 0f)
                    {
                        traitsText += $"Focus -{Math.Abs(trait.focus) * 100}%|";
                    }
                    else
                    {
                        traitsText += $"Focus +{trait.focus * 100}%|";
                    }
                }
                if (trait.experience != 0f)
                {
                    if (trait.experience < 0f)
                    {
                        traitsText += $"Experience -{Math.Abs(trait.experience) * 100}%|";
                    }
                    else
                    {
                        traitsText += $"Experience +{trait.experience * 100}%|";
                    }
                }
                traitsText += $"Department: {trait.departmentType}\n\n";
            }
        }

        employeeManager.Q<Label>("EMModifiers").text = traitsText;
        UpdateUpgradeUIList();
        // Show the Employee Manager
        employeeManager.style.display = DisplayStyle.Flex;
    }
    private void UpdateUpgradeUIList()
    {
        if (selectedEmployee == null || Globals.warehouseEmployees == null || Globals.warehouseEmployees.Count == 0)
        {
            // Debug.LogError("No selected employee or no employees found in the warehouse employees list.");
            return;
        }
        Employee employee = Globals.warehouseEmployees.Find(e => e.employeeName == selectedEmployee.employeeName);
        if (employee == null)
        {
            // Debug.LogError("Selected employee not found in the warehouse employees list.");
            return;
        }
        VisualElement upgradeList = upgradePanel.Q<VisualElement>("upgradeList");
        if (upgradeList == null)
        {
            // Debug.LogError("Upgrade list not found in the upgrade panel.");
            return;
        }
        // Remove any click event listeners from the upgrade list items
        foreach (VisualElement upgradeListItem in upgradeList.Children())
        {
            upgradeListItem.UnregisterCallback<ClickEvent>(OnUpgradeListItemClick);
        }
        // Clear the existing upgrade list
        upgradeList.Clear();
        // Create upgrade list items
        string[] upgradeNames = { "speed", "efficiency", "stamina", "strength", "focus", "experience" };
        foreach (string upgradeName in upgradeNames)
        {
            // Create a new VisualElement for the upgrade list item
            VisualElement upgradeListItem = new VisualElement();
            upgradeListItem.AddToClassList("upgradeContainer");
            // Set the name of the upgrade list item for identification
            upgradeListItem.name = $"{upgradeName}UpgradeButton";
            // Create a Label for the upgrade details
            string upgradeLevelText = "LVL\n1";
            int upgradeCostAmount = 0;
            switch (upgradeName)
            {
                case "speed":
                    upgradeLevelText = $"LVL\n{employee.speed}";
                    upgradeCostAmount = (int)(Globals.employeeStatUpgradeCost * employee.speed);
                    break;
                case "efficiency":
                    upgradeLevelText = $"LVL\n{employee.efficiency}";
                    upgradeCostAmount = (int)(Globals.employeeStatUpgradeCost * employee.efficiency);
                    break;
                case "stamina":
                    upgradeLevelText = $"LVL\n{employee.stamina}";
                    upgradeCostAmount = (int)(Globals.employeeStatUpgradeCost * employee.stamina);
                    break;
                case "strength":
                    upgradeLevelText = $"LVL\n{employee.strength}";
                    upgradeCostAmount = (int)(Globals.employeeStatUpgradeCost * employee.strength);
                    break;
                case "focus":
                    upgradeLevelText = $"LVL\n{employee.focus}";
                    upgradeCostAmount = (int)(Globals.employeeStatUpgradeCost * employee.focus);
                    break;
                case "experience":
                    upgradeLevelText = $"LVL\n{employee.experience}";
                    upgradeCostAmount = (int)(Globals.employeeStatUpgradeCost * employee.experience);
                    break;
                default:
                    Debug.LogError($"Upgrade {upgradeName} is not a valid upgrade.");
                    continue;
            }
            Label upgradeLevel = new Label
            {
                name = "upgradeLevel",
                text = upgradeLevelText,
            };
            upgradeLevel.AddToClassList("upgradeLevel");
            upgradeListItem.Add(upgradeLevel);
            Label upgradeNameLabel = new Label
            {
                name = "upgradeName",
                text = upgradeName[0].ToString().ToUpper() + upgradeName.Substring(1),
            };
            upgradeNameLabel.AddToClassList("upgradeName");
            upgradeListItem.Add(upgradeNameLabel);
            Label upgradeCost = new Label
            {
                name = "upgradeCost",
                text = $"COST\n${upgradeCostAmount}",
            };
            upgradeCost.AddToClassList("upgradeLevel");
            if (Globals.playerMoney < upgradeCostAmount)
            {
                upgradeCost.AddToClassList("cannotUpgrade");
            }
            else
            {
                upgradeCost.AddToClassList("canUpgrade");
            }
            upgradeListItem.Add(upgradeCost);
            // Add click event listener to the upgrade list item
            upgradeListItem.RegisterCallback<ClickEvent>(OnUpgradeListItemClick);
            // Add the upgrade list item to the container
            upgradeList.Add(upgradeListItem);
        }
        // Debug.Log("Upgrade UI list updated.");
    }
    private void OnUpgradeListItemClick(ClickEvent evt)
    {
        // Get the clicked upgrade list item
        VisualElement clickedItem = evt.currentTarget as VisualElement;
        if (clickedItem == null)
        {
            Debug.LogError("Clicked item is not a VisualElement.");
            return;
        }
        if (selectedEmployee == null)
        {
            Debug.LogError("No employee selected for upgrade.");
            return;
        }
        // Get the upgrade name from the clicked item
        string upgradeName = clickedItem.name;
        // Get the employee from the global list
        Employee clickedEmployee = Globals.warehouseEmployees.Find(e => e.employeeName == selectedEmployee.employeeName);
        switch (upgradeName)
        {
            case "speedUpgradeButton":
                // Check if the employee has enough experience to upgrade speed
                if (Globals.playerMoney >= Globals.employeeStatUpgradeCost * clickedEmployee.speed)
                {
                    clickedEmployee.speed += Globals.employeeStatUpgradeValue;
                    Globals.playerMoney -= Globals.employeeStatUpgradeCost * clickedEmployee.speed;
                    UpdateEmployeeUIList();
                    ShowEmployeeDetails(clickedEmployee);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade speed.");
                }
                break;
            case "efficiencyUpgradeButton":
                // Check if the employee has enough experience to upgrade efficiency
                if (Globals.playerMoney >= Globals.employeeStatUpgradeCost * clickedEmployee.efficiency)
                {
                    clickedEmployee.efficiency += Globals.employeeStatUpgradeValue;
                    Globals.playerMoney -= Globals.employeeStatUpgradeCost * clickedEmployee.efficiency;
                    UpdateEmployeeUIList();
                    ShowEmployeeDetails(clickedEmployee);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade efficiency.");
                }
                break;
            case "staminaUpgradeButton":
                // Check if the employee has enough experience to upgrade stamina
                if (Globals.playerMoney >= Globals.employeeStatUpgradeCost * clickedEmployee.stamina)
                {
                    clickedEmployee.stamina += Globals.employeeStatUpgradeValue;
                    Globals.playerMoney -= Globals.employeeStatUpgradeCost * clickedEmployee.stamina;
                    UpdateEmployeeUIList();
                    ShowEmployeeDetails(clickedEmployee);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade stamina.");
                }
                break;
            case "strengthUpgradeButton":
                // Check if the employee has enough experience to upgrade strength
                if (Globals.playerMoney >= Globals.employeeStatUpgradeCost * clickedEmployee.strength)
                {
                    clickedEmployee.strength += Globals.employeeStatUpgradeValue;
                    Globals.playerMoney -= Globals.employeeStatUpgradeCost * clickedEmployee.strength;
                    UpdateEmployeeUIList();
                    ShowEmployeeDetails(clickedEmployee);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade strength.");
                }
                break;
            case "focusUpgradeButton":
                // Check if the employee has enough experience to upgrade focus
                if (Globals.playerMoney >= Globals.employeeStatUpgradeCost * clickedEmployee.focus)
                {
                    clickedEmployee.focus += Globals.employeeStatUpgradeValue;
                    Globals.playerMoney -= Globals.employeeStatUpgradeCost * clickedEmployee.focus;
                    UpdateEmployeeUIList();
                    ShowEmployeeDetails(clickedEmployee);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade focus.");
                }
                break;
            case "experienceUpgradeButton":
                // Check if the employee has enough experience to upgrade experience
                if (Globals.playerMoney >= Globals.employeeStatUpgradeCost * clickedEmployee.experience)
                {
                    clickedEmployee.experience += Globals.employeeStatUpgradeValue;
                    Globals.playerMoney -= Globals.employeeStatUpgradeCost * clickedEmployee.experience;
                    UpdateEmployeeUIList();
                    ShowEmployeeDetails(clickedEmployee);
                }
                else
                {
                    Debug.Log("Not enough money to upgrade experience.");
                }
                break;
            default:
                Debug.LogError($"Upgrade {upgradeName} is not a valid upgrade.");
                break;
        }
    }
    private void OnStoreItemClick(ClickEvent evt)
    {
        // Get the clicked store item
        VisualElement clickedItem = evt.currentTarget as VisualElement;
        if (clickedItem == null)
        {
            Debug.LogError("Clicked item is not a VisualElement.");
            return;
        }
        // Get the item name from the clicked item
        string itemName = clickedItem.name;
        Label itemCostLabel = clickedItem.Q<Label>("storeItemCost");
        int itemCostValue = Globals.departmentCost;
        switch (itemName)
        {
            case "storeItemDepartmentHR":
                int departmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.HR);
                itemCostValue = Globals.departmentCost + 5000 * (departmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                HR hrDepartment = gameObject.AddComponent<HR>();
                hrDepartment.departmentType = DepartmentTypes.Type.HR;
                hrDepartment.departmentName = $"HR [{departmentCount + 1}]";
                hrDepartment.departmentLevel = 1;
                hrDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(hrDepartment));
                if (Globals.tutorialStep == 2)
                {
                    clickedItem.SetEnabled(false);
                    if (Globals.departments.Count == 2)
                    {
                        ProgressTutorial();
                    }
                }
                break;
            case "storeItemDepartmentIT":
                int itDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.IT);
                itemCostValue = Globals.departmentCost + 5000 * (itDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                IT itDepartment = gameObject.AddComponent<IT>();
                itDepartment.departmentType = DepartmentTypes.Type.IT;
                itDepartment.departmentName = $"IT [{itDepartmentCount + 1}]";
                itDepartment.departmentLevel = 1;
                itDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(itDepartment));
                break;
            case "storeItemDepartmentOperations":
                int operationsDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Operations);
                itemCostValue = Globals.departmentCost + 5000 * (operationsDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Operations operationsDepartment = gameObject.AddComponent<Operations>();
                operationsDepartment.departmentType = DepartmentTypes.Type.Operations;
                operationsDepartment.departmentName = $"Operations [{operationsDepartmentCount + 1}]";
                operationsDepartment.departmentLevel = 1;
                operationsDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(operationsDepartment));
                break;
            case "storeItemDepartmentInbound":
                int inboundDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Inbound);
                itemCostValue = Globals.departmentCost + 5000 * (inboundDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Inbound inboundDepartment = gameObject.AddComponent<Inbound>();
                inboundDepartment.departmentType = DepartmentTypes.Type.Inbound;
                inboundDepartment.departmentName = $"Inbound [{inboundDepartmentCount + 1}]";
                inboundDepartment.departmentLevel = 1;
                inboundDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(inboundDepartment));
                if (Globals.tutorialStep == 2)
                {
                    clickedItem.SetEnabled(false);
                    if (Globals.departments.Count == 2)
                    {
                        ProgressTutorial();
                    }
                }
                break;
            case "storeItemDepartmentFluidLoad":
                int fluidDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.FluidLoad);
                itemCostValue = Globals.departmentCost + 5000 * (fluidDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                FluidLoad fluidDepartment = gameObject.AddComponent<FluidLoad>();
                fluidDepartment.departmentType = DepartmentTypes.Type.FluidLoad;
                fluidDepartment.departmentName = $"FluidLoad [{fluidDepartmentCount + 1}]";
                fluidDepartment.departmentLevel = 1;
                fluidDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(fluidDepartment));
                if (Globals.tutorialStep == 2)
                {
                    clickedItem.SetEnabled(false);
                    if (Globals.departments.Count == 2)
                    {
                        ProgressTutorial();
                    }
                }
                break;
            case "storeItemDepartmentOutbound":
                int outboundDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Outbound);
                itemCostValue = Globals.departmentCost + 5000 * (outboundDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Outbound outboundDepartment = gameObject.AddComponent<Outbound>();
                outboundDepartment.departmentType = DepartmentTypes.Type.Outbound;
                outboundDepartment.departmentName = $"Outbound [{outboundDepartmentCount + 1}]";
                outboundDepartment.departmentLevel = 1;
                outboundDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(outboundDepartment));
                break;
            case "storeItemDepartmentSorting":
                int sortingDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Sorting);
                itemCostValue = Globals.departmentCost + 5000 * (sortingDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Sorting sortingDepartment = gameObject.AddComponent<Sorting>();
                sortingDepartment.departmentType = DepartmentTypes.Type.Sorting;
                sortingDepartment.departmentName = $"Sorting [{sortingDepartmentCount + 1}]";
                sortingDepartment.departmentLevel = 1;
                sortingDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(sortingDepartment));
                break;
            case "storeItemDepartmentRepacking":
                int repackingDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Repacking);
                itemCostValue = Globals.departmentCost + 5000 * (repackingDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Repacking repackingDepartment = gameObject.AddComponent<Repacking>();
                repackingDepartment.departmentType = DepartmentTypes.Type.Repacking;
                repackingDepartment.departmentName = $"Repacking [{repackingDepartmentCount + 1}]";
                repackingDepartment.departmentLevel = 1;
                repackingDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(repackingDepartment));
                break;
            case "storeItemDepartmentPalletizing":
                int palletizingDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Palletizing);
                itemCostValue = Globals.departmentCost + 5000 * (palletizingDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Palletizing palletizingDepartment = gameObject.AddComponent<Palletizing>();
                palletizingDepartment.departmentType = DepartmentTypes.Type.Palletizing;
                palletizingDepartment.departmentName = $"Palletizing [{palletizingDepartmentCount + 1}]";
                palletizingDepartment.departmentLevel = 1;
                palletizingDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(palletizingDepartment));
                break;
            case "storeItemDepartmentWaterSpidering":
                int waterSpideringDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.WaterSpidering);
                itemCostValue = Globals.departmentCost + 5000 * (waterSpideringDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                WaterSpidering waterSpideringDepartment = gameObject.AddComponent<WaterSpidering>();
                waterSpideringDepartment.departmentType = DepartmentTypes.Type.WaterSpidering;
                waterSpideringDepartment.departmentName = $"Water Spidering [{waterSpideringDepartmentCount + 1}]";
                waterSpideringDepartment.departmentLevel = 1;
                waterSpideringDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(waterSpideringDepartment));
                break;
            case "storeItemDepartmentQualityControl":
                int qualityControlDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.QualityControl);
                itemCostValue = Globals.departmentCost + 5000 * (qualityControlDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                QualityControl qualityControlDepartment = gameObject.AddComponent<QualityControl>();
                qualityControlDepartment.departmentType = DepartmentTypes.Type.QualityControl;
                qualityControlDepartment.departmentName = $"Quality Control [{qualityControlDepartmentCount + 1}]";
                qualityControlDepartment.departmentLevel = 1;
                qualityControlDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(qualityControlDepartment));
                break;
            case "storeItemDepartmentMaintenance":
                int maintenanceDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Maintenance);
                itemCostValue = Globals.departmentCost + 5000 * (maintenanceDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Maintenance maintenanceDepartment = gameObject.AddComponent<Maintenance>();
                maintenanceDepartment.departmentType = DepartmentTypes.Type.Maintenance;
                maintenanceDepartment.departmentName = $"Maintenance [{maintenanceDepartmentCount + 1}]";
                maintenanceDepartment.departmentLevel = 1;
                maintenanceDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(maintenanceDepartment));
                break;
            case "storeItemDepartmentRobotics":
                int roboticsDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Robotics);
                itemCostValue = Globals.departmentCost + 5000 * (roboticsDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Robotics roboticsDepartment = gameObject.AddComponent<Robotics>();
                roboticsDepartment.departmentType = DepartmentTypes.Type.Robotics;
                roboticsDepartment.departmentName = $"Robotics [{roboticsDepartmentCount + 1}]";
                roboticsDepartment.departmentLevel = 1;
                roboticsDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(roboticsDepartment));
                break;
            case "storeItemDepartmentSafety":
                int safetyDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Safety);
                itemCostValue = Globals.departmentCost + 5000 * (safetyDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Safety safetyDepartment = gameObject.AddComponent<Safety>();
                safetyDepartment.departmentType = DepartmentTypes.Type.Safety;
                safetyDepartment.departmentName = $"Safety [{safetyDepartmentCount + 1}]";
                safetyDepartment.departmentLevel = 1;
                safetyDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(safetyDepartment));
                break;
            case "storeItemDepartmentCleaning":
                int cleaningDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Cleaning);
                itemCostValue = Globals.departmentCost + 5000 * (cleaningDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Cleaning cleaningDepartment = gameObject.AddComponent<Cleaning>();
                cleaningDepartment.departmentType = DepartmentTypes.Type.Cleaning;
                cleaningDepartment.departmentName = $"Cleaning [{cleaningDepartmentCount + 1}]";
                cleaningDepartment.departmentLevel = 1;
                cleaningDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(cleaningDepartment));
                break;
            case "storeItemDepartmentSecurity":
                int securityDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Security);
                itemCostValue = Globals.departmentCost + 5000 * (securityDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Security securityDepartment = gameObject.AddComponent<Security>();
                securityDepartment.departmentType = DepartmentTypes.Type.Security;
                securityDepartment.departmentName = $"Security [{securityDepartmentCount + 1}]";
                securityDepartment.departmentLevel = 1;
                securityDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(securityDepartment));
                break;
            case "storeItemDepartmentLearning":
                int learningDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Learning);
                itemCostValue = Globals.departmentCost + 5000 * (learningDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Learning learningDepartment = gameObject.AddComponent<Learning>();
                learningDepartment.departmentType = DepartmentTypes.Type.Learning;
                learningDepartment.departmentName = $"Learning [{learningDepartmentCount + 1}]";
                learningDepartment.departmentLevel = 1;
                learningDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(learningDepartment));
                break;
            case "storeItemDepartmentRecruiting":
                int recruitingDepartmentCount = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Recruiting);
                itemCostValue = Globals.departmentCost + 5000 * (recruitingDepartmentCount + 1);
                if (Globals.playerMoney < itemCostValue)
                {
                    Debug.Log("Not enough money to buy this item.");
                    return;
                }
                Recruiting recruitingDepartment = gameObject.AddComponent<Recruiting>();
                recruitingDepartment.departmentType = DepartmentTypes.Type.Recruiting;
                recruitingDepartment.departmentName = $"Recruiting [{recruitingDepartmentCount + 1}]";
                recruitingDepartment.departmentLevel = 1;
                recruitingDepartment.capacity = 10;
                Globals.playerMoney -= itemCostValue;
                StartCoroutine(DelayDepartmentUpdate(recruitingDepartment));
                break;
            default:
                Debug.LogError($"Store item {itemName} is not a valid store item.");
                break;
        }
    }

    private IEnumerator DelayDepartmentUpdate(Department newDepartment)
    {
        Debug.Log($"Starting department update for {newDepartment.departmentName}");

        // Ensure department has reference to game controller
        newDepartment.gameController = this;

        // Update UI lists
        UpdateEmployeeUIList();
        UpdateStoreDepartmentCosts();

        if (Globals.tutorialStatus == StatusType.Type.Completed)
        {
            // Force departments panel display
            ShowSidePanel("employeesPanel");
        }

        // Update department list
        UpdateDepartmentUIList();

        // Force another update after a short delay to ensure proper rendering
        yield return new WaitForSeconds(0.2f);
        VisualElement root = gameUI.rootVisualElement;
        ScrollView departmentListContainer = root.Q<ScrollView>("departmentsMainList");
        departmentListContainer.contentContainer.style.minHeight = new StyleLength(
            new Length(100f * Globals.departments.Count, LengthUnit.Pixel));
        departmentListContainer.contentViewport.MarkDirtyRepaint();

        // Log for debugging
        Debug.Log($"Department {newDepartment.departmentName} UI update complete");
    }

    private void UpdateStoreDepartmentCosts()
    {
        // Update store item costs based on department counts
        int departmentCountHR = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.HR);
        int departmentCountInbound = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.Inbound);
        int departmentCountFluidLoad = Globals.departments.Count(d => d.departmentType == DepartmentTypes.Type.FluidLoad);

        storeItemHR.Q<Label>("storeItemCost").text = $"Cost\n${Globals.departmentCost + 5000 * (departmentCountHR + 1)}";
        storeItemInbound.Q<Label>("storeItemCost").text = $"Cost\n${Globals.departmentCost + 5000 * (departmentCountInbound + 1)}";
        storeItemFluidLoad.Q<Label>("storeItemCost").text = $"Cost\n${Globals.departmentCost + 5000 * (departmentCountFluidLoad + 1)}";
    }
}