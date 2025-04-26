using System;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;
using Unity.VisualScripting;

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
    string currentPanel = "employeesPanel";
    public Actions.GameSystem gameActions;
    public Actions.HR hrActions;
    public Sprite defaultEmployeeSprite;

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
        hrDepartment.departmentName = "HR";
        hrDepartment.departmentLevel = 1;
        hrDepartment.capacity = 10;
        Inbound inboundDepartment = gameObject.AddComponent<Inbound>();
        inboundDepartment.departmentType = DepartmentTypes.Type.Inbound;
        inboundDepartment.departmentName = "Inbound";
        inboundDepartment.departmentLevel = 1;
        inboundDepartment.capacity = 10;
        FluidLoad fluidDepartment = gameObject.AddComponent<FluidLoad>();
        fluidDepartment.departmentType = DepartmentTypes.Type.FluidLoad;
        fluidDepartment.departmentName = "FluidLoad";
        fluidDepartment.departmentLevel = 1;
        fluidDepartment.capacity = 10;

        // Globals.departments.Add(hrDepartment);
        // UpdateDepartmentUIList();
        StartCoroutine(gameActions.CreateNewHire());
        UpdateNewHireUIList();
        Globals.playerMoney = 10000;
        Globals.gameState = State.Playing;
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
    void FixedUpdate()
    {
        // Debug.Log($"BoxesInStorage: {Globals.boxesInStorage}");
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
                // Debug.Log("No more tutorial steps available.");
                break;
        }
    }
    // Tutorial step logic
    private void TutorialStep1()
    {
        // Add logic for tutorial step 1 here
        // Debug.Log("Tutorial Step 1: Welcome to the Warehouse Tycoon!");

        // Move to the next step
        Globals.tutorialStep++;
    }

    // Method to load the game state
    private void LoadGameState()
    {
        // Add logic to load the game state here
        // StartCoroutine(Globals.Load());
        // Debug.Log("Game state loaded.");
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
            // Set the name of the employee list item for identification
            employeeListItem.name = employee.employeeName;
            // Create a VisualElement for the employee's picture
            VisualElement employeeListItemPicture = new VisualElement();
            employeeListItemPicture.AddToClassList("employeeListItemPicture");
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
        // Loop through each department and add them to the UI
        foreach (Department department in Globals.departments)
        {
            department.AddToUI();
        }
        // Debug.Log("Department UI list updated.");
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
                    newDepartment.AddEmployee(selectedEmployee);
                    selectedEmployee.department = newDepartment;
                    selectedEmployee.department.UpdateEmployeeUIList();
                    // Update the employee list UI
                    UpdateEmployeeUIList();
                    ShowEmployeeDetails(selectedEmployee);
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
            // Debug.LogError("Clicked item is not a VisualElement.");
            return;
        }
        // Get the employee name from the clicked item
        string employeeName = clickedItem.name;
        // Find the employee in the warehouse employees list
        Employee clickedEmployee = Globals.warehouseEmployees.Find(e => e.employeeName == employeeName);
        if (clickedEmployee != null)
        {
            // Show the employee details in a new panel or popup
            ShowEmployeeDetails(clickedEmployee);
            selectedEmployee = clickedEmployee;
        }
        else
        {
            // Debug.LogError($"Employee {employeeName} not found in the warehouse employees list.");
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
            $"Department: {(employee.department != null ? employee.department.departmentName : "None")}\n" +
            $"Level: {employee.level}\n" +
            $"Infractions: {employee.infractions}\n" +
            $"Status: {employee.actionState}\n" +
            $"Salary: ${employee.salary}\n";
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
        // Set traits
        string traitsText = "";
        foreach (TraitValues trait in employee.traits)
        {
            // Get the trait name and value
            string traitName = TraitValues.GetTraitName(trait);
            traitsText += $"{traitName}: ";
            if (trait.speed > 0f)
            {
                if (trait.speed < 0f)
                {
                    traitsText += $"Speed -{trait.speed * 100}%|";
                }
                else
                {
                    traitsText += $"Speed +{trait.speed * 100}%|";
                }
            }
            if (trait.efficiency > 0f)
            {
                if (trait.efficiency < 0f)
                {
                    traitsText += $"Efficiency -{trait.efficiency * 100}%|";
                }
                else
                {
                    traitsText += $"Efficiency +{trait.efficiency * 100}%|";
                }
            }
            if (trait.stamina > 0f)
            {
                if (trait.stamina < 0f)
                {
                    traitsText += $"Stamina -{trait.stamina * 100}%|";
                }
                else
                {
                    traitsText += $"Stamina +{trait.stamina * 100}%|";
                }
            }
            if (trait.strength > 0f)
            {
                if (trait.strength < 0f)
                {
                    traitsText += $"Strength -{trait.strength * 100}%|";
                }
                else
                {
                    traitsText += $"Strength +{trait.strength * 100}%|";
                }
            }
            if (trait.focus > 0f)
            {
                if (trait.focus < 0f)
                {
                    traitsText += $"Focus -{trait.focus * 100}%|";
                }
                else
                {
                    traitsText += $"Focus +{trait.focus * 100}%|";
                }
            }
            if (trait.experience > 0f)
            {
                if (trait.experience < 0f)
                {
                    traitsText += $"Experience -{trait.experience * 100}%|";
                }
                else
                {
                    traitsText += $"Experience +{trait.experience * 100}%|";
                }
            }
            traitsText += $"Department: {trait.departmentType}\n\n";
        }
        employeeManager.Q<Label>("EMModifiers").text = traitsText;
        // Show the Employee Manager
        employeeManager.style.display = DisplayStyle.Flex;
    }
}
