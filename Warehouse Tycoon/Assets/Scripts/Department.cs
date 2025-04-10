using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Department : MonoBehaviour
{
    public int capacity = 0; // Maximum number of employees that can work in this department
    public string departmentName = null; // Name of the department
    public List<KeyValuePair<string, int>> stats = null; // Replace Dictionary with List
    public List<KeyValuePair<string, int>> statTimes = null; // Replace Dictionary with List
    public List<ActionRequest> newActionRequests = new List<ActionRequest>(); // Array of new action requests associated with this department
    public List<ActionRequest> claimedActionRequests = new List<ActionRequest>(); // Array of claimed action requests associated with this department
    public int departmentId = 0; // Unique identifier for the department
    public int departmentLevel = 1; // Level of the department
    public int departmentExp = 0; // Experience points of the department
    public DepartmentTypes.Type departmentType = DepartmentTypes.Type.None; // Type of the department (e.g., HR, IT, etc.)
    public int managerCapacity = 0; // Maximum number of managers that can work in this department
    public int employeeCapacity = 0; // Maximum number of employees that can work in this department
    public List<Employee> employees = null; // Array of employees in this department
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }

    public static T FromJson<T>(string json) where T : Department
    {
        return JsonUtility.FromJson<T>(json);
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Assessment", 5), // Time taken for assessment
            new KeyValuePair<string, int>("Investigation", 5), // Time taken for investigation
            new KeyValuePair<string, int>("Analysis", 5), // Time taken for analysis
            new KeyValuePair<string, int>("Resolution", 5) // Time taken for resolution
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Assessment", 1),
                new KeyValuePair<string, int>("Investigation", 1),
                new KeyValuePair<string, int>("Analysis", 1),
                new KeyValuePair<string, int>("Resolution", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Support", 5), // Time taken for support tasks
            new KeyValuePair<string, int>("Maintenance", 5), // Time taken for maintenance tasks
            new KeyValuePair<string, int>("Troubleshooting", 5), // Time taken for troubleshooting
            new KeyValuePair<string, int>("Upgrades", 5) // Time taken for upgrades
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Support", 1),
                new KeyValuePair<string, int>("Maintenance", 1),
                new KeyValuePair<string, int>("Troubleshooting", 1),
                new KeyValuePair<string, int>("Upgrades", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("FlowManagement", 60), // Time taken for flow management tasks
            new KeyValuePair<string, int>("IssueResolution", 60), // Time taken for issue resolution
            new KeyValuePair<string, int>("MoraleBoost", 60), // Time taken for morale boosting activities
            new KeyValuePair<string, int>("Supervision", 60) // Time taken for supervision tasks
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("FlowManagement", 1),
                new KeyValuePair<string, int>("IssueResolution", 1),
                new KeyValuePair<string, int>("MoraleBoost", 1),
                new KeyValuePair<string, int>("Supervision", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Receiving", 60), // Time taken for receiving tasks
            new KeyValuePair<string, int>("Sorting", 30), // Time taken for sorting tasks
            new KeyValuePair<string, int>("Throwing", 30) // Time taken for throwing tasks
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Receiving", 1),
                new KeyValuePair<string, int>("Sorting", 1),
                new KeyValuePair<string, int>("Throwing", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = null; // Sorting department does not have specific time stats for tasks
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Accuracy", 1), // Accuracy of sorting items
                new KeyValuePair<string, int>("Speed", 1) // Speed of sorting items
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("PackageOpening", 5), // Time taken for package opening
            new KeyValuePair<string, int>("ItemSorting", 30), // Time taken for item sorting
            new KeyValuePair<string, int>("Repackaging", 10) // Time taken for repackaging
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("PackageOpening", 1),
                new KeyValuePair<string, int>("ItemSorting", 1),
                new KeyValuePair<string, int>("Repackaging", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Opening", 10), // Time taken for opening tasks
            new KeyValuePair<string, int>("Palletizing", 60), // Time taken for palletizing tasks
            new KeyValuePair<string, int>("Wrapping", 30), // Time taken for wrapping tasks
            new KeyValuePair<string, int>("Labeling", 10), // Time taken for labeling tasks
            new KeyValuePair<string, int>("Closing", 10) // Time taken for closing tasks
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Opening", 1),
                new KeyValuePair<string, int>("Palletizing", 1), // Putting boxes into pallets
                new KeyValuePair<string, int>("Wrapping", 1),
                new KeyValuePair<string, int>("Labeling", 1),
                new KeyValuePair<string, int>("Closing", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Delivery", 5), // Time taken for delivery tasks
            new KeyValuePair<string, int>("Restocking", 5), // Time taken for restocking tasks
            new KeyValuePair<string, int>("InventoryCheck", 5) // Time taken for inventory check tasks
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Delivery", 1),
                new KeyValuePair<string, int>("Restocking", 1),
                new KeyValuePair<string, int>("InventoryCheck", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("DoorOpening", 5), // Time taken for door opening
            new KeyValuePair<string, int>("FluidLoading", 60), // Time taken for fluid loading
            new KeyValuePair<string, int>("WallChecking", 5), // Time taken for wall checking
            new KeyValuePair<string, int>("DoorClosing", 5), // Time taken for door closing
            new KeyValuePair<string, int>("TruckDeparting", 10), // Time taken for truck departing
            new KeyValuePair<string, int>("TruckArriving", 10) // Time taken for truck arriving
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("DoorOpening", 1),
                new KeyValuePair<string, int>("FluidLoading", 1),
                new KeyValuePair<string, int>("WallChecking", 1),
                new KeyValuePair<string, int>("DoorClosing", 1),
                new KeyValuePair<string, int>("TruckDeparting", 1),
                new KeyValuePair<string, int>("TruckArriving", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Inspection", 5), // Time taken for inspection tasks
            new KeyValuePair<string, int>("Testing", 5), // Time taken for testing tasks
            new KeyValuePair<string, int>("Reporting", 5) // Time taken for reporting tasks
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Inspection", 1),
                new KeyValuePair<string, int>("Testing", 1),
                new KeyValuePair<string, int>("Reporting", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("DoorOpening", 5), // Time taken for door opening
            new KeyValuePair<string, int>("DockLoading", 30), // Time taken for fluid loading
            new KeyValuePair<string, int>("LoadSecurement", 5), // Time taken for wall checking
            new KeyValuePair<string, int>("DoorClosing", 5), // Time taken for door closing
            new KeyValuePair<string, int>("TruckDeparting", 10), // Time taken for truck departing
            new KeyValuePair<string, int>("TruckArriving", 10) // Time taken for truck arriving
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("DoorOpening", 1),
                new KeyValuePair<string, int>("DockLoading", 1),
                new KeyValuePair<string, int>("LoadSecurement", 1),
                new KeyValuePair<string, int>("DoorClosing", 1),
                new KeyValuePair<string, int>("TruckDeparting", 1),
                new KeyValuePair<string, int>("TruckArriving", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Inspection", 10), // Time taken for inspection tasks
            new KeyValuePair<string, int>("Repair", 30), // Time taken for repair tasks
            new KeyValuePair<string, int>("Maintenance", 30) // Time taken for maintenance tasks
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Inspection", 1),
                new KeyValuePair<string, int>("Repair", 1),
                new KeyValuePair<string, int>("Maintenance", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Scan", 2),
            new KeyValuePair<string, int>("Sort", 2),
            new KeyValuePair<string, int>("Placement", 2),
            new KeyValuePair<string, int>("Reset", 1),
            new KeyValuePair<string, int>("PalletSwap", 10)
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Scan", 1),
                new KeyValuePair<string, int>("Sort", 1),
                new KeyValuePair<string, int>("Placement", 1),
                new KeyValuePair<string, int>("Reset", 1),
                new KeyValuePair<string, int>("PalletSwap", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Inspection", 5), // Time taken for inspection tasks
            new KeyValuePair<string, int>("Response", 30), // Time taken for training tasks
            new KeyValuePair<string, int>("Treatment", 60) // Time taken for reporting tasks
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Inspection", 1),
                new KeyValuePair<string, int>("Response", 1),
                new KeyValuePair<string, int>("Treatment", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Inspection", 5), // Time taken for inspection tasks
            new KeyValuePair<string, int>("Cleaning", 30) // Time taken for cleaning tasks
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Inspection", 1),
                new KeyValuePair<string, int>("Cleaning", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Inspection", 5), // Time taken for inspection tasks
            new KeyValuePair<string, int>("Monitoring", 30), // Time taken for monitoring tasks
            new KeyValuePair<string, int>("Response", 30), // Time taken for response tasks
            new KeyValuePair<string, int>("Resolution", 30) // Time taken for resolution tasks
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Inspection", 1),
                new KeyValuePair<string, int>("Monitoring", 1),
                new KeyValuePair<string, int>("Response", 1),
                new KeyValuePair<string, int>("Resolution", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
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
        this.stats = stats;
        statTimes = new List<KeyValuePair<string, int>>
        {
            new KeyValuePair<string, int>("Onboarding", 360),
            new KeyValuePair<string, int>("Training", 180), // Time taken for training tasks
            new KeyValuePair<string, int>("Assessment", 60), // Time taken for assessment tasks
            new KeyValuePair<string, int>("Feedback", 30), // Time taken for feedback tasks
            new KeyValuePair<string, int>("Retraining", 90) // Time taken for retraining tasks
        };
        if (stats == null)
        {
            this.stats = new List<KeyValuePair<string, int>>
            {
                new KeyValuePair<string, int>("Onboarding", 1),
                new KeyValuePair<string, int>("Training", 1),
                new KeyValuePair<string, int>("Assessment", 1),
                new KeyValuePair<string, int>("Feedback", 1),
                new KeyValuePair<string, int>("Retraining", 1)
            };
        }
        else
        {
            this.stats = stats;
        }
    }
}