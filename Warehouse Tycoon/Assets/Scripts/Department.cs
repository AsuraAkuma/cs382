using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[Serializable]
public class NameGenerator
{
    private static NameGenerator _instance;
    public static NameGenerator Instance
    {
        get
        {
            if (_instance == null)
                _instance = new NameGenerator();
            return _instance;
        }
    }

    [SerializeField] private List<string> usedNames = new List<string>();
    [SerializeField] private HashSet<int> usedIds = new HashSet<int>();
    [SerializeField]
    private readonly string[] firstNames = {
        "John", "Emma", "Michael", "Sophia", "William", "Olivia", "James", "Ava", "Alexander", "Isabella",
        "Benjamin", "Mia", "Daniel", "Charlotte", "Henry", "Amelia", "Joseph", "Harper", "Samuel", "Evelyn",
        "David", "Abigail", "Carter", "Emily", "Owen", "Elizabeth", "Wyatt", "Sofia", "Jack", "Victoria",
        "Luke", "Camila", "Gabriel", "Aria", "Anthony", "Scarlett", "Isaac", "Luna", "Grayson", "Chloe"
    };
    [SerializeField]
    private readonly string[] lastNames = {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
        "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
        "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
        "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores"
    };

    // Static method for backward compatibility
    public static string GetRandomUnusedName()
    {
        return Instance.GetRandomUnusedNameInternal();
    }

    // Instance method
    private string GetRandomUnusedNameInternal()
    {
        string fullName;
        do
        {
            string firstName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
            string lastName = lastNames[UnityEngine.Random.Range(0, lastNames.Length)];
            fullName = $"{firstName} {lastName}";
        } while (usedNames.Contains(fullName));

        usedNames.Add(fullName);
        return fullName;
    }

    // Static method for backward compatibility
    public static void ResetUsedNames()
    {
        Instance.ResetUsedNamesInternal();
    }

    // Instance method
    private void ResetUsedNamesInternal()
    {
        usedNames.Clear();
    }

    // Static method for ID generation
    public static int GetUniqueId()
    {
        return Instance.GetUniqueIdInternal();
    }

    // Instance method for ID generation
    private int GetUniqueIdInternal()
    {
        int id;
        do
        {
            id = UnityEngine.Random.Range(1, 100000);
        } while (usedIds.Contains(id));

        usedIds.Add(id);
        return id;
    }

    // Method to check if an ID exists
    public static bool IdExists(int id)
    {
        return Instance.usedIds.Contains(id);
    }

    // Reset all used IDs
    public static void ResetUsedIds()
    {
        Instance.usedIds.Clear();
    }
}

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
    public List<Employee> employees = new List<Employee>(); // Array of employees in this department
    public List<Disablers.Disabler> disablers = new List<Disablers.Disabler>(); // Array of disablers associated with this department
    public int managerIndex = 0; // Index of the current manager in the department
    public List<Employee> managers = new List<Employee>(); // Array of managers in this department
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
        // Debug.Log($"Starting AddToUI for department: {departmentName}");

        // Find the parent element in the UI where the department will be added
        gameController = Globals.gameController;
        if (gameController == null)
        {
            Debug.LogError("GameController not found in the scene!");
            return;
        }
        UIDocument uiDocument = gameController.gameUI;
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument not found in the scene!");
            return;
        }

        VisualElement parentElement = uiDocument.rootVisualElement.Q<ScrollView>("departmentsMainList").contentContainer;
        if (parentElement == null)
        {
            Debug.LogError("Department list ScrollView not found! Make sure 'departmentsMainList' exists in UI.");
            return;
        }

        // Check if department already exists in UI
        VisualElement existingDepartment = parentElement.Q<VisualElement>(departmentName);
        if (existingDepartment != null)
        {
            // Debug.Log($"Department {departmentName} already exists in UI\n{existingDepartment.parent.name}");
            return;
        }

        // Debug.Log($"Creating UI elements for department: {departmentName}");

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
        // Debug.Log($"Adding {employees.Count} employees to UI for department: {departmentName}");
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
        // Debug.Log($"Added department {departmentName} to UI successfully");

        // Check if department was added to the parent element
        if (parentElement.Q(departmentName) == null)
        {
            // Debug.LogError($"Failed to add department {departmentName} to UI");
            return;
        }
        // Initialize manager display
        UpdateManager();
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

            Employee manager = managers[managerIndex];
            managerLabel.text = $"{manager.employeeName}\nlvl: {manager.level}";
            if (managerImg != null)
            {
                managerImg.style.backgroundImage = new StyleBackground(manager.employeeSprite);
            }
        }
        buttonUp.RegisterCallback<ClickEvent>(OnButtonUpClick);
        buttonDown.RegisterCallback<ClickEvent>(OnButtonDownClick);
    }
    public void UpdateEmployeeUIList()
    {
        // Debug.Log("Adding employee to UI: " + departmentName);
        // Find the parent element in the UI where the department will be added
        UIDocument uiDocument = gameController.gameUI;
        VisualElement parentElement = uiDocument.rootVisualElement.Q<VisualElement>(departmentName).Q<ScrollView>("departmentEmployeeList");
        if (parentElement == null)
        {
            // Debug.LogError("Parent element not found for department: " + departmentName);
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
        if (departmentId == 0)
        {
            departmentId = NameGenerator.GetUniqueId();
        }
        gameController = FindFirstObjectByType<GameController>();
        if (gameController == null)
        {
            // Debug.LogError("GameController not found in the scene.");
            return;
        }
        // Ensure we add to Globals.departments before updating UI
        Globals.departments.Add(this);
        // Debug.Log($"Added {departmentName} to Globals.departments");

        // Debug.Log($"Department Start() completed for {departmentName}");
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
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            Debug.Log("Creating HR department employees and managers");
            // Create Employee and add to the department
            HREmployee employee = gameObject.AddComponent<HREmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.HREmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 1000;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            employee.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(employee);

            // Create Manager and add to the department
            HRManager manager = gameObject.AddComponent<HRManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.HRManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 1500;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class IT : Department
{
    public IT()
    {
        departmentType = DepartmentTypes.Type.IT;
        departmentName = "IT";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public IT(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            ITEmployee employee = gameObject.AddComponent<ITEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.ITEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 1100;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            ITManager manager = gameObject.AddComponent<ITManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.ITManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 1650;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Operations : Department
{
    public Operations()
    {
        departmentType = DepartmentTypes.Type.Operations;
        departmentName = "Operations";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Operations(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            OperationsEmployee employee = gameObject.AddComponent<OperationsEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.OperationsManager;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 1200;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            OperationsManager manager = gameObject.AddComponent<OperationsManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.OperationsManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 1800;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Inbound : Department
{
    public Inbound()
    {
        departmentType = DepartmentTypes.Type.Inbound;
        departmentName = "Inbound";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Inbound(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            InboundEmployee employee = gameObject.AddComponent<InboundEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.InboundEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 1300;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            InboundManager manager = gameObject.AddComponent<InboundManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.InboundManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 1950;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Sorting : Department
{
    public Sorting()
    {
        departmentType = DepartmentTypes.Type.Sorting;
        departmentName = "Sorting";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Sorting(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            SortingEmployee employee = gameObject.AddComponent<SortingEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.SortingEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 1400;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            SortingManager manager = gameObject.AddComponent<SortingManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.SortingManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 2100;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Repacking : Department
{
    public Repacking()
    {
        departmentType = DepartmentTypes.Type.Repacking;
        departmentName = "Repacking";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Repacking(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            RepackingEmployee employee = gameObject.AddComponent<RepackingEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.RepackingEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 1500;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            RepackingManager manager = gameObject.AddComponent<RepackingManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.RepackingManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 2250;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Palletizing : Department
{
    public Palletizing()
    {
        departmentType = DepartmentTypes.Type.Palletizing;
        departmentName = "Palletizing";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Palletizing(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            PalletizingEmployee employee = gameObject.AddComponent<PalletizingEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.PalletizingEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 1600;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            PalletizingManager manager = gameObject.AddComponent<PalletizingManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.PalletizingManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 2400;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class WaterSpidering : Department
{
    public WaterSpidering()
    {
        departmentType = DepartmentTypes.Type.WaterSpidering;
        departmentName = "Water Spidering";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public WaterSpidering(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            WaterSpiderEmployee employee = gameObject.AddComponent<WaterSpiderEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.WaterSpiderEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 1700;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            WaterSpiderManager manager = gameObject.AddComponent<WaterSpiderManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.WaterSpiderManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 2550;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class FluidLoad : Department
{
    public FluidLoad()
    {
        departmentType = DepartmentTypes.Type.FluidLoad;
        // departmentName = "Fluid Load";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public FluidLoad(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            FluidLoadEmployee employee = gameObject.AddComponent<FluidLoadEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.FluidLoadEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 1800;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            FluidLoadManager manager = gameObject.AddComponent<FluidLoadManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.FluidLoadManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 2700;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class QualityControl : Department
{
    public QualityControl()
    {
        departmentType = DepartmentTypes.Type.QualityControl;
        departmentName = "Quality Control";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public QualityControl(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            QualityControlEmployee employee = gameObject.AddComponent<QualityControlEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.QualityControlEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 1900;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            QualityControlManager manager = gameObject.AddComponent<QualityControlManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.QualityControlManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 2850;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Outbound : Department
{
    public Outbound()
    {
        departmentType = DepartmentTypes.Type.Outbound;
        departmentName = "Outbound";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Outbound(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            OutboundEmployee employee = gameObject.AddComponent<OutboundEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.OutboundEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 2000;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            OutboundManager manager = gameObject.AddComponent<OutboundManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.OutboundManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 3000;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Maintenance : Department
{
    public Maintenance()
    {
        departmentType = DepartmentTypes.Type.Maintenance;
        departmentName = "Maintenance";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Maintenance(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            MaintenanceEmployee employee = gameObject.AddComponent<MaintenanceEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.MaintenanceEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 2100;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            MaintenanceManager manager = gameObject.AddComponent<MaintenanceManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.MaintenanceManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 3150;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Robotics : Department
{
    public Robotics()
    {
        departmentType = DepartmentTypes.Type.Robotics;
        departmentName = "Robotics";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Robotics(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            RoboticsEmployee employee = gameObject.AddComponent<RoboticsEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.RoboticsEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 2200;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            RoboticsManager manager = gameObject.AddComponent<RoboticsManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.RoboticsManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 3300;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Safety : Department
{
    public Safety()
    {
        departmentType = DepartmentTypes.Type.Safety;
        departmentName = "Safety";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Safety(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            SafetyEmployee employee = gameObject.AddComponent<SafetyEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.SafetyEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 2300;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            SafetyManager manager = gameObject.AddComponent<SafetyManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.SafetyManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 3450;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Cleaning : Department
{
    public Cleaning()
    {
        departmentType = DepartmentTypes.Type.Cleaning;
        departmentName = "Cleaning";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Cleaning(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            CleaningEmployee employee = gameObject.AddComponent<CleaningEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.CleaningEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 2400;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            CleaningManager manager = gameObject.AddComponent<CleaningManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.CleaningManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 3600;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Security : Department
{
    public Security()
    {
        departmentType = DepartmentTypes.Type.Security;
        departmentName = "Security";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Security(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            SecurityEmployee employee = gameObject.AddComponent<SecurityEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.SecurityEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 2500;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            SecurityManager manager = gameObject.AddComponent<SecurityManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.SecurityManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 3750;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Learning : Department
{
    public Learning()
    {
        departmentType = DepartmentTypes.Type.Learning;
        departmentName = "Learning";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Learning(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            LearningEmployee employee = gameObject.AddComponent<LearningEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.LearningEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 2600;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            LearningManager manager = gameObject.AddComponent<LearningManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.LearningManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 3900;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}

[Serializable]
public class Recruiting : Department
{
    public Recruiting()
    {
        departmentType = DepartmentTypes.Type.Recruiting;
        departmentName = "Recruiting";
        capacity = 10;
        managerCapacity = 1;
        employees = new List<Employee>();
    }

    public Recruiting(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
    }

    new void Start()
    {
        base.Start();
        if (employees.Count == 0 && Globals.loadSave == false)
        {
            RecruitingEmployee employee = gameObject.AddComponent<RecruitingEmployee>();
            employee.id = NameGenerator.GetUniqueId();
            employee.employeeName = NameGenerator.GetRandomUnusedName();
            employee.level = 1;
            employee.department = this;
            employee.employeeSprite = gameController.defaultEmployeeSprite;
            employee.employeeType = EmployeeType.Type.RecruitingEmployee;
            employee.speed = 1;
            employee.efficiency = 1;
            employee.stamina = 1;
            employee.strength = 1;
            employee.focus = 1;
            employee.salary = 2700;
            employee.actionState = ActionState.State.Idle;
            employees.Add(employee);
            Globals.warehouseEmployees.Add(employee);

            RecruitingManager manager = gameObject.AddComponent<RecruitingManager>();
            manager.id = NameGenerator.GetUniqueId();
            manager.employeeName = NameGenerator.GetRandomUnusedName();
            manager.level = 1;
            manager.department = this;
            manager.employeeSprite = gameController.defaultEmployeeSprite;
            manager.employeeType = EmployeeType.Type.RecruitingManager;
            manager.speed = 1;
            manager.efficiency = 1;
            manager.stamina = 1;
            manager.strength = 1;
            manager.focus = 1;
            manager.salary = 4050;
            manager.actionState = ActionState.State.Idle;
            managers.Add(manager);
            manager.traits.Add(EmployeeTraits.RobotTuner);
            Globals.warehouseEmployees.Add(manager);
        }
        gameController.UpdateDepartmentUIList();
        gameController.UpdateEmployeeUIList();
        gameController.UpdateNewHireUIList();
    }
}