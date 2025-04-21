using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    public UIDocument gameUI;
    Button upgradesButton;
    Button employeesButton;
    Button storeButton;
    Button newHiresButton;
    string currentPanel = "employeesPanel";
    public Actions.GameSystem gameActions;
    public Actions.HR hrActions;
    public Sprite defaultEmployeeSprite;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameActions = gameObject.AddComponent<Actions.GameSystem>();
        hrActions = gameObject.AddComponent<Actions.HR>();
        upgradesButton = gameUI.rootVisualElement.Q<Button>("panelNavUpgradesButton");
        employeesButton = gameUI.rootVisualElement.Q<Button>("panelNavEmployeesButton");
        storeButton = gameUI.rootVisualElement.Q<Button>("panelNavStoreButton");
        newHiresButton = gameUI.rootVisualElement.Q<Button>("panelNavNewHiresButton");
        // Add click event listeners to the buttons
        upgradesButton.RegisterCallback<ClickEvent>(OnPanelNavButtonClick);
        employeesButton.RegisterCallback<ClickEvent>(OnPanelNavButtonClick);
        storeButton.RegisterCallback<ClickEvent>(OnPanelNavButtonClick);
        newHiresButton.RegisterCallback<ClickEvent>(OnPanelNavButtonClick);

        // Check if the tutorial has been completed
        if (Globals.tutorialStatus == StatusType.Type.Completed)
        {
            // Load the game state
            LoadGameState();
            // Update the employee UI list
            UpdateEmployeeUIList();
            // Update the new hire UI list
            UpdateNewHireUIList();
        }
        else
        {
            // Start the tutorial
            ProgressTutorial();
        }
        // TESTING ONLY
        // Create a new hire
        HR hrDepartment = gameObject.AddComponent<HR>();
        hrDepartment.departmentType = DepartmentTypes.Type.HR;
        hrDepartment.departmentName = "HR Department";
        hrDepartment.departmentLevel = 1;
        hrDepartment.capacity = 10;
        // Globals.departments.Add(hrDepartment);
        // UpdateDepartmentUIList();
        StartCoroutine(gameActions.CreateNewHire());
        UpdateNewHireUIList();
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the game is paused
        if (Globals.gameState == State.Paused)
        {
            // Pause the game logic here
            return;
        }
    }

    // Method to start the tutorial
    private void ProgressTutorial()
    {
        // Get the current tutorial step
        int currentStep = Globals.tutorialStep;
        switch (currentStep)
        {
            case 0:
                // Start the tutorial step 1
                TutorialStep1();
                break;

            default:
                Debug.Log("No more tutorial steps available.");
                break;
        }
    }
    // Tutorial step logic
    private void TutorialStep1()
    {
        // Add logic for tutorial step 1 here
        Debug.Log("Tutorial Step 1: Welcome to the Warehouse Tycoon!");

        // Move to the next step
        Globals.tutorialStep++;
    }

    // Method to load the game state
    private void LoadGameState()
    {
        // Add logic to load the game state here
        // StartCoroutine(Globals.Load());
        Debug.Log("Game state loaded.");
    }

    public void UpdateEmployeeUIList()
    {
        // Get the root visual element of the game UI
        VisualElement root = gameUI.rootVisualElement;
        // Get the employee list container from the UI
        ScrollView employeeListContainer = root.Q<ScrollView>("employeeList");
        // Clear the existing employee list
        employeeListContainer.Clear();
        // Loop through each employee in the warehouse and add them to the UI
        foreach (Employee employee in Globals.warehouseEmployees)
        {
            // Create a new VisualElement for the employee list item
            VisualElement employeeListItem = new VisualElement();
            employeeListItem.AddToClassList("employeeListItem");

            // Create a VisualElement for the employee's picture
            VisualElement employeeListItemPicture = new VisualElement();
            employeeListItemPicture.AddToClassList("employeeListItemPicture");
            employeeListItem.Add(employeeListItemPicture);

            // Create a Label for the employee's details
            Label employeeListItemDetails = new Label
            {
                text = $"Name: {employee.employeeName}\n" +
                       $"Department: {employee.department}\n" +
                       $"Speed: {employee.speed}\n" +
                       $"Efficiency: {employee.efficiency}\n" +
                       $"Stamina: {employee.stamina}\n" +
                       $"Strength: {employee.strength}\n" +
                       $"Focus: {employee.focus}\n" +
                       $"Level: {employee.level}\n"
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
        Debug.Log("Employee UI list updated.");
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
                       $"Department: {employee.department}\n" +
                       $"Speed: {employee.speed}\n" +
                       $"Efficiency: {employee.efficiency}\n" +
                       $"Stamina: {employee.stamina}\n" +
                       $"Strength: {employee.strength}\n" +
                       $"Focus: {employee.focus}\n" +
                       $"Salary/Cost: ${employee.salary}\n"
            };
            newHireListItemDetails.AddToClassList("newHireListItemDetails");
            newHireListItem.Add(newHireListItemDetails);
            // Get Hr department
            Department hrDepartment = Globals.departments.Find(d => d.departmentType == DepartmentTypes.Type.HR);
            ActionRequest actionRequest = new ActionRequest(hrActions.HireEmployee(employee), employee);
            // Create Hire Button
            Button hireButton = new Button(() => hrDepartment.AddActionRequest(actionRequest))
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
        Debug.Log("New Hire UI list updated.");
    }
    public void UpdateDepartmentUIList()
    {
        // Get the root visual element of the game UI
        VisualElement root = gameUI.rootVisualElement;
        // Get the department list container from the UI
        ScrollView departmentListContainer = root.Q<ScrollView>("departmentsMainList");
        // Clear the existing department list
        departmentListContainer.Clear();
        // Loop through each department and add them to the UI
        Debug.Log($"Departments: {Globals.departments.Count}");
        foreach (Department department in Globals.departments)
        {
            department.AddToUI();
        }
        Debug.Log("Department UI list updated.");
    }
    private void RejectNewHire(Employee newHire)
    {
        // Add logic to reject the new hire here
        // For example, remove them from the new hires list and update the UI
        Globals.newHires.Remove(newHire);
        UpdateNewHireUIList();
        Debug.Log($"New hire {newHire.employeeName} rejected.");
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
                Debug.Log("Invalid button name.");
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
        string employeeName = clickedItem.Q<Label>("employeeNameLabel").text;
        // Find the employee in the warehouse employees list
        Employee clickedEmployee = Globals.warehouseEmployees.Find(e => e.employeeName == employeeName);
        if (clickedEmployee != null)
        {
            // Show the employee details in a new panel or popup
            ShowEmployeeDetails(clickedEmployee);
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
        employeeManager.Q<VisualElement>("employeeManagerImage").style.backgroundImage = new StyleBackground(employee.employeeSprite);
        employeeManager.Q<Label>("employeeManagerDetails").text = $"Name: {employee.employeeName}\n" +
            $"Department: {employee.department}\n" +
            $"Level: {employee.level}\n" +
            $"Infractions: {employee.infractions}\n" +
            $"Status: {employee.actionState}\n" +
            $"Salary: {employee.salary}\n";
        // Set EMStats
        employeeManager.Q<VisualElement>("EMSpeedStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.speed / Globals.employeeStatMax, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMSpeedStat").Q<Label>("EMStatText").text = $"Speed: {employee.speed} / {Globals.employeeStatMax}";
        employeeManager.Q<VisualElement>("EMEfficiencyStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.efficiency / Globals.employeeStatMax, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMEfficiencyStat").Q<Label>("EMStatText").text = $"Efficiency: {employee.efficiency} / {Globals.employeeStatMax}";
        employeeManager.Q<VisualElement>("EMStaminaStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.stamina / Globals.employeeStatMax, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMStaminaStat").Q<Label>("EMStatText").text = $"Stamina: {employee.stamina} / {Globals.employeeStatMax}";
        employeeManager.Q<VisualElement>("EMStrengthStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.strength / Globals.employeeStatMax, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMStrengthStat").Q<Label>("EMStatText").text = $"Strength: {employee.strength} / {Globals.employeeStatMax}";
        employeeManager.Q<VisualElement>("EMFocusStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.focus / Globals.employeeStatMax, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMFocusStat").Q<Label>("EMStatText").text = $"Focus: {employee.focus} / {Globals.employeeStatMax}";
        employeeManager.Q<VisualElement>("EMFExperienceStat").Q<VisualElement>("EMStatProgress").style.width = new StyleLength(new Length(employee.experience / Globals.employeeStatMax, LengthUnit.Percent));
        employeeManager.Q<VisualElement>("EMFExperienceStat").Q<Label>("EMStatText").text = $"Experience: {employee.experience} / {Globals.employeeStatMax}";
        // Set traits
        foreach (TraitValues trait in employee.traits)
        {
            // Get the trait name and value
            string traitName = trait.GetType().Name;
            Debug.Log($"Trait: {traitName}");

        }
        // employeeManager.Q<Label>("EMModifiers").text

        // Show the Employee Manager
        employeeManager.style.display = DisplayStyle.Flex;
    }
}
