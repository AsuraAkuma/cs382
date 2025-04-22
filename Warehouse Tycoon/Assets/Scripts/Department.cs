using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class Department : MonoBehaviour
{
    public int capacity = 0; // Maximum number of employees that can work in this department
    public string departmentName = null; // Name of the department
    public List<ActionRequest> newActionRequests = new List<ActionRequest>(); // Array of new action requests associated with this department
    public List<ActionRequest> claimedActionRequests = new List<ActionRequest>(); // Array of claimed action requests associated with this department
    public GameController gameController; // Reference to the GameController script
    public int departmentId = 0; // Unique identifier for the department
    public int departmentLevel = 1; // Level of the department
    public int departmentExp = 0; // Experience points of the department
    public DepartmentTypes.Type departmentType = DepartmentTypes.Type.None; // Type of the department (e.g., HR, IT, etc.)
    public int managerCapacity = 0; // Maximum number of managers that can work in this department
    public List<Employee> employees = null; // Array of employees in this department
    public List<Disablers.Disabler> disablers = new List<Disablers.Disabler>(); // Array of disablers associated with this department
    private int managerIndex = 0; // Index of the current manager in the department
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static T FromJson<T>(string json) where T : Department
    {
        return JsonUtility.FromJson<T>(json);
    }

    public void AddActionRequest(ActionRequest actionRequest)
    {
        if (!newActionRequests.Contains(actionRequest))
        {
            newActionRequests.Add(actionRequest);
        }
    }
    public void ClaimActionRequest(ActionRequest actionRequest)
    {
        if (!claimedActionRequests.Contains(actionRequest))
        {
            claimedActionRequests.Add(actionRequest);
        }
        if (newActionRequests.Contains(actionRequest))
        {
            newActionRequests.Remove(actionRequest);
        }
    }
    public void RemoveActionRequest(ActionRequest actionRequest)
    {
        if (claimedActionRequests.Contains(actionRequest))
        {
            claimedActionRequests.Remove(actionRequest);
        }
        if (newActionRequests.Contains(actionRequest))
        {
            claimedActionRequests.Remove(actionRequest);
        }
    }
    public void AddDisabler(Disablers.Disabler disabler)
    {
        if (!disablers.Contains(disabler))
        {
            disablers.Add(disabler);
        }
        if (!Globals.disabledDepartments.Contains(this))
        {
            Globals.disabledDepartments.Add(this);
        }
    }
    public void RemoveDisabler(Disablers.Disabler disabler)
    {
        if (disablers.Contains(disabler))
        {
            disablers.Remove(disabler);
        }
        if (disablers.Count == 0 && Globals.disabledDepartments.Contains(this))
        {
            Globals.disabledDepartments.Remove(this);
        }
    }
    public void AddEmployee(Employee employee)
    {
        if (employees == null)
        {
            employees = new List<Employee>();
        }
        if (!employees.Contains(employee))
        {
            employees.Add(employee);
        }
    }
    public void RemoveEmployee(Employee employee)
    {
        if (employees != null && employees.Contains(employee))
        {
            employees.Remove(employee);
        }
    }

    public void AddToUI()
    {
        Debug.Log($"Starting AddToUI for department: {departmentName}");

        // Find the parent element in the UI where the department will be added
        UIDocument uiDocument = gameController.gameUI;
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found in the scene!");
            return;
        }

        ScrollView parentElement = uiDocument.rootVisualElement.Q<ScrollView>("departmentsMainList");
        if (parentElement == null)
        {
            Debug.LogError("Department list ScrollView not found! Make sure 'departmentsMainList' exists in UI.");
            return;
        }

        // Check if department already exists in UI
        if (parentElement.Q(departmentName) != null)
        {
            Debug.Log($"Department {departmentName} already exists in UI");
            return;
        }

        Debug.Log($"Creating UI elements for department: {departmentName}");

        // Create a new VisualElement for the department
        VisualElement departmentElement = new VisualElement();
        departmentElement.name = departmentName;
        departmentElement.AddToClassList("department");
        departmentElement.style.display = DisplayStyle.Flex; // Ensure the element is visible

        // Create the department header
        VisualElement departmentHeader = new VisualElement();
        departmentHeader.name = "departmentHeader";
        departmentHeader.AddToClassList("departmentHeader");
        departmentHeader.style.flexDirection = FlexDirection.Row;

        // Add labels to the department header
        Label nameLabel = new Label(departmentName);
        nameLabel.name = "departmentHeaderLabel";
        nameLabel.AddToClassList("departmentHeaderLabel");
        departmentHeader.Add(nameLabel);

        Label levelLabel = new Label($"lvl: {departmentLevel}");
        levelLabel.name = "departmentHeaderLabel";
        levelLabel.AddToClassList("departmentHeaderLabel");
        departmentHeader.Add(levelLabel);

        Label expLabel = new Label($"Exp: {departmentExp}");
        expLabel.name = "departmentHeaderLabel";
        expLabel.AddToClassList("departmentHeaderLabel");
        departmentHeader.Add(expLabel);

        Label newTasksLabel = new Label($"New Tasks: {newActionRequests.Count}");
        newTasksLabel.name = "departmentHeaderLabel";
        newTasksLabel.AddToClassList("departmentHeaderLabel");
        departmentHeader.Add(newTasksLabel);

        Label doneTasksLabel = new Label($"Done Tasks: {claimedActionRequests.Count}");
        doneTasksLabel.name = "departmentHeaderLabel";
        doneTasksLabel.AddToClassList("departmentHeaderLabel");
        departmentHeader.Add(doneTasksLabel);

        departmentElement.Add(departmentHeader);
        // Create the department section
        VisualElement departmentSection = new VisualElement();
        departmentSection.name = "departmentSection";
        departmentSection.AddToClassList("departmentSection");
        departmentSection.style.flexDirection = FlexDirection.Row;
        departmentElement.Add(departmentSection);

        // Create the department manager section
        VisualElement departmentManager = new VisualElement();
        departmentManager.name = "departmentManager";
        departmentManager.AddToClassList("departmentManager");

        VisualElement managerContainer = new VisualElement();
        managerContainer.name = "departmentManagerContainer";
        managerContainer.AddToClassList("departmentManagerContainer");

        VisualElement managerImg = new VisualElement();
        managerImg.name = "departmentManagerImg";
        managerImg.AddToClassList("departmentManagerImg");
        managerContainer.Add(managerImg);

        VisualElement managerButtons = new VisualElement();
        managerButtons.name = "departmentManagerButtons";
        managerButtons.AddToClassList("departmentManagerButtons");

        Button buttonUp = new Button();
        buttonUp.name = "departmentManagerButtonUp";
        buttonUp.AddToClassList("departmentManagerButtonUp");
        managerButtons.Add(buttonUp);

        Button buttonDown = new Button();
        buttonDown.name = "departmentManagerButtonDown";
        buttonDown.AddToClassList("departmentManagerButtonDown");
        managerButtons.Add(buttonDown);

        managerContainer.Add(managerButtons);
        departmentManager.Add(managerContainer);

        Label managerLabel = new Label("No Manager");
        managerLabel.name = "managerLabel";
        managerLabel.AddToClassList("managerLabel");
        departmentManager.Add(managerLabel);

        departmentSection.Add(departmentManager);

        // Create the department employees section
        VisualElement departmentEmployees = new VisualElement();
        departmentEmployees.name = "departmentEmployees";
        departmentEmployees.AddToClassList("departmentEmployees");

        ScrollView employeeList = new ScrollView();
        employeeList.name = "departmentEmployeeList";
        employeeList.AddToClassList("departmentEmployeeList");
        employeeList.mode = ScrollViewMode.Horizontal;

        if (employees != null)
        {
            foreach (Employee employee in employees)
            {
                VisualElement employeeElement = new VisualElement();
                employeeElement.name = "departmentEmployee";
                employeeElement.AddToClassList("departmentEmployee");

                VisualElement employeePicture = new VisualElement();
                employeePicture.name = "departmentEmployeePicture";
                employeePicture.AddToClassList("departmentEmployeePicture");
                employeePicture.style.backgroundImage = new StyleBackground(employee.employeeSprite);
                employeeElement.Add(employeePicture);

                Label employeeLabel = new Label($"{employee.employeeName}\nlvl: {employee.level}");
                employeeLabel.name = "departmentEmployeeLabel";
                employeeLabel.AddToClassList("departmentEmployeeLabel");
                employeeElement.Add(employeeLabel);

                employeeList.Add(employeeElement);
                employeeElement.RegisterCallback<ClickEvent>(evt => ScrollToEmployee(employee));
            }
        }

        departmentEmployees.Add(employeeList);
        departmentSection.Add(departmentEmployees);

        // Add the department element to the parent ScrollView
        parentElement.Add(departmentElement);
        Debug.Log($"Added department {departmentName} to UI successfully");

        // Check if department was added to the parent element
        if (parentElement.Q(departmentName) == null)
        {
            Debug.LogError($"Failed to add department {departmentName} to UI");
            return;
        }
        // Initialize manager display
        if (employees != null && employees.Count > 0)
        {
            Employee potentialManager = employees[managerIndex];
            if (potentialManager is Manager manager)
            {
                managerLabel.text = $"{manager.employeeName}\nlvl: {manager.level}";
                managerImg.style.backgroundImage = new StyleBackground(manager.employeeSprite);
            }
        }

        // Click event functions
        void OnButtonUpClick(ClickEvent evt)
        {
            // Check if there is more than one manager
            if (managerCapacity == 1) return;
            if (managerIndex >= managerCapacity - 1)
            {
                managerIndex = 0;
            }
            else
            {
                managerIndex++;
            }
            UpdateManager();
        }
        void OnButtonDownClick(ClickEvent evt)
        {
            // Check if there is more than one manager
            if (managerCapacity == 1) return;
            if (managerIndex == 0)
            {
                managerIndex = managerCapacity - 1;
            }
            else
            {
                managerIndex--;
            }
            UpdateManager();
        }
        void UpdateManager()
        {
            if (employees == null || employees.Count == 0)
            {
                managerLabel.text = "No Manager";
                return;
            }

            Employee potentialManager = employees[managerIndex];
            if (potentialManager is Manager manager)
            {
                managerLabel.text = $"{manager.employeeName}\nlvl: {manager.level}";
                if (managerImg != null)
                {
                    managerImg.style.backgroundImage = new StyleBackground(manager.employeeSprite);
                }
            }
            else
            {
                managerLabel.text = "Invalid Manager";
                Debug.LogWarning($"Employee at index {managerIndex} is not a Manager");
            }
        }
        buttonUp.RegisterCallback<ClickEvent>(OnButtonUpClick);
        buttonDown.RegisterCallback<ClickEvent>(OnButtonDownClick);
    }
    public void UpdateEmployeeUIList()
    {
        Debug.Log("Adding employee to UI: " + departmentName);
        // Find the parent element in the UI where the department will be added
        UIDocument uiDocument = gameController.gameUI;
        VisualElement parentElement = uiDocument.rootVisualElement.Q<VisualElement>(departmentName).Q<ScrollView>("departmentEmployeeList");
        if (parentElement == null)
        {
            Debug.LogError("Parent element not found for department: " + departmentName);
            return;
        }

        parentElement.Clear(); // Clear the existing employee list
        foreach (Employee emp in employees)
        {
            // Create a new VisualElement for the employee
            VisualElement employeeElement = new VisualElement();
            employeeElement.name = emp.employeeName;
            employeeElement.AddToClassList("departmentEmployee");

            // Create the employee picture
            VisualElement employeePicture = new VisualElement();
            employeePicture.name = "departmentEmployeePicture";
            employeePicture.AddToClassList("departmentEmployeePicture");
            employeePicture.style.backgroundImage = new StyleBackground(emp.employeeSprite);
            employeeElement.Add(employeePicture);

            // Create the employee label
            Label employeeLabel = new Label($"{emp.employeeName}\nlvl: {emp.level}");
            employeeLabel.name = "departmentEmployeeLabel";
            employeeLabel.AddToClassList("departmentEmployeeLabel");
            employeeElement.Add(employeeLabel);

            // Add the employee element to the parent ScrollView
            parentElement.Add(employeeElement);
        }
    }
    public void ScrollToEmployee(Employee employeeElement)
    {

    }
    public void Start()
    {
        gameController = FindFirstObjectByType<GameController>();
        if (gameController == null)
        {
            Debug.LogError("GameController not found in the scene.");
            return;
        }

        // Ensure we add to Globals.departments before updating UI
        if (!Globals.departments.Contains(this))
        {
            Globals.departments.Add(this);
            Debug.Log($"Added {departmentName} to Globals.departments");
        }

        Debug.Log($"Department Start() completed for {departmentName}");
    }

}

[Serializable]
public class HR : Department
{
    public HR()
    {
        departmentType = DepartmentTypes.Type.HR;
        departmentName = "HR";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public HR(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<HREmployee>();
            employee.name = "HREmployee";
            employee.id = 1;
            employee.employeeName = "John Doe";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.HREmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 250;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            // Add trait to employee
            employee.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }

    // Create ticket function
    public void CreateTicket(string ticketType, string description)
    {
        // Logic to create a ticket in the HR department
        Debug.Log($"Ticket Created: {ticketType} - {description}");
    }
}

[Serializable]
public class IT : Department
{
    public IT() { }
    public IT(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<ITEmployee>();
            employee.name = "ITEmployee";
            employee.id = 2;
            employee.employeeName = "Jane Smith";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.ITEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 300;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Operations : Department
{
    public Operations() { }
    public Operations(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<OperationsEmployee>();
            employee.name = "OperationsEmployee";
            employee.id = 3;
            employee.employeeName = "Mike Johnson";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.OperationsManager;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 275;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Inbound : Department
{
    public Inbound() { }
    public Inbound(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<InboundEmployee>();
            employee.name = "InboundEmployee";
            employee.id = 4;
            employee.employeeName = "Sarah Wilson";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.InboundEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 260;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Sorting : Department
{
    public Sorting() { }
    public Sorting(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<SortingEmployee>();
            employee.name = "SortingEmployee";
            employee.id = 5;
            employee.employeeName = "Tom Brown";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.SortingEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 270;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Repacking : Department
{
    public Repacking() { }
    public Repacking(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<RepackingEmployee>();
            employee.name = "RepackingEmployee";
            employee.id = 6;
            employee.employeeName = "Lisa Davis";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.RepackingEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 280;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Palletizing : Department
{
    public Palletizing() { }
    public Palletizing(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<PalletizingEmployee>();
            employee.name = "PalletizingEmployee";
            employee.id = 7;
            employee.employeeName = "James Miller";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.PalletizingEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 290;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class WaterSpidering : Department
{
    public WaterSpidering() { }
    public WaterSpidering(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<WaterSpiderEmployee>();
            employee.name = "WaterSpideringEmployee";
            employee.id = 8;
            employee.employeeName = "Emma White";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.WaterSpiderEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 300;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class FluidLoad : Department
{
    public FluidLoad() { }
    public FluidLoad(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<FluidLoadEmployee>();
            employee.name = "FluidLoadEmployee";
            employee.id = 9;
            employee.employeeName = "David Taylor";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.FluidLoadEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 310;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class QualityControl : Department
{
    public QualityControl() { }
    public QualityControl(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<QualityControlEmployee>();
            employee.name = "QualityControlEmployee";
            employee.id = 10;
            employee.employeeName = "Mary Anderson";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.QualityControlEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 320;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Outbound : Department
{
    public Outbound() { }
    public Outbound(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<OutboundEmployee>();
            employee.name = "OutboundEmployee";
            employee.id = 11;
            employee.employeeName = "Robert Martin";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.OutboundEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 330;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Maintenance : Department
{
    public Maintenance() { }
    public Maintenance(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<MaintenanceEmployee>();
            employee.name = "MaintenanceEmployee";
            employee.id = 12;
            employee.employeeName = "Patricia Garcia";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.MaintenanceEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 340;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Robotics : Department
{
    public Robotics() { }
    public Robotics(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<RoboticsEmployee>();
            employee.name = "RoboticsEmployee";
            employee.id = 13;
            employee.employeeName = "Michael Lee";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.RoboticsEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 350;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Safety : Department
{
    public Safety() { }
    public Safety(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<SafetyEmployee>();
            employee.name = "SafetyEmployee";
            employee.id = 14;
            employee.employeeName = "Jennifer Clark";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.SafetyEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 360;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Cleaning : Department
{
    public Cleaning() { }
    public Cleaning(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<CleaningEmployee>();
            employee.name = "CleaningEmployee";
            employee.id = 15;
            employee.employeeName = "William Rodriguez";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.CleaningEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 370;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Security : Department
{
    public Security() { }
    public Security(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<SecurityEmployee>();
            employee.name = "SecurityEmployee";
            employee.id = 16;
            employee.employeeName = "Linda Martinez";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.SecurityEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 380;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Learning : Department
{
    public Learning() { }
    public Learning(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<LearningEmployee>();
            employee.name = "LearningEmployee";
            employee.id = 17;
            employee.employeeName = "Richard Thompson";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.LearningEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 390;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}

[Serializable]
public class Recruiting : Department
{
    public Recruiting() { }
    public Recruiting(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0)
        {
            Employee employee = gameObject.AddComponent<RecruitingEmployee>();
            employee.name = "RecruitingEmployee";
            employee.id = 18;
            employee.employeeName = "Elizabeth Wright";
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.RecruitingEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 400;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
    }
}