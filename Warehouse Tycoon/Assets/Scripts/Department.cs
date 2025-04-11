using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Department : MonoBehaviour
{
    public int capacity = 0; // Maximum number of employees that can work in this department
    public string departmentName = null; // Name of the department
    public List<ActionRequest> newActionRequests = new List<ActionRequest>(); // Array of new action requests associated with this department
    public List<ActionRequest> claimedActionRequests = new List<ActionRequest>(); // Array of claimed action requests associated with this department
    public int departmentId = 0; // Unique identifier for the department
    public int departmentLevel = 1; // Level of the department
    public int departmentExp = 0; // Experience points of the department
    public DepartmentTypes.Type departmentType = DepartmentTypes.Type.None; // Type of the department (e.g., HR, IT, etc.)
    public int managerCapacity = 0; // Maximum number of managers that can work in this department
    public int employeeCapacity = 0; // Maximum number of employees that can work in this department
    public List<Employee> employees = null; // Array of employees in this department
    public List<Disablers.Disabler> disablers = new List<Disablers.Disabler>(); // Array of disablers associated with this department
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
}

[Serializable]
public class HR : Department
{
    public HR() { }
    public HR(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
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
}

[Serializable]
public class LearningAndDevelopment : Department
{
    public LearningAndDevelopment() { }
    public LearningAndDevelopment(string name, int cap, List<KeyValuePair<string, int>> stats = null)
    {
        departmentName = name;
        capacity = cap;
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
}