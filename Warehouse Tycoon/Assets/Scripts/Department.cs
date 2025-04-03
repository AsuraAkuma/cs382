using System.Collections.Generic;
using UnityEngine;

public class Department : MonoBehaviour
{
    public int capacity; // Maximum number of employees that can work in this department
    public string departmentName; // Name of the department
    public Dictionary<string, int> stats;
    public Dictionary<string, int> statTimes; // In seconds, time taken for each department task
    void Start()
    {

    }

    void Update()
    {

    }
}

public class HR : Department
{
    public HR() { }
    public HR(string name, int cap, Dictionary<string, int> stats)
    {
        departmentName = name;
        capacity = cap;
        this.stats = stats;
        statTimes = new Dictionary<string, int>
        {
            { "Assessment", 5 }, // Time taken for assessment
            { "Investigation", 5 }, // Time taken for investigation
            { "Analysis", 5 }, // Time taken for analysis
            { "Resolution", 5 } // Time taken for resolution
        };
        //     hrStats = new Dictionary<string, int>
        // {
        //     { "Assessment", 1 },
        //     { "Investigation", 1 },
        //     { "Analysis", 1 },
        //     {"Resolution", 1 }
        // };

    }
}

public class IT : Department
{
    public IT() { }
    public IT(string name, int cap, Dictionary<string, int> stats)
    {
        departmentName = name;
        capacity = cap;
        this.stats = stats;
        statTimes = new Dictionary<string, int>
        {
            { "Support", 5 }, // Time taken for support tasks
            { "Maintenance", 5 }, // Time taken for maintenance tasks
            { "Troubleshooting", 5 }, // Time taken for troubleshooting
            { "Upgrades", 5 } // Time taken for upgrades
        };
        //     itStats = new Dictionary<string, int>
        // {
        //     { "Support", 1 },
        //     { "Maintenance", 1 }
        // };
    }
}

public class Operations : Department
{
    public Operations() { }
    public Operations(string name, int cap, Dictionary<string, int> stats)
    {
        departmentName = name;
        capacity = cap;
        this.stats = stats;
        statTimes = new Dictionary<string, int>
        {
            { "FlowManagement", 60 }, // Time taken for flow management tasks
            { "IssueResolution", 60 }, // Time taken for issue resolution
            { "MoraleBoost", 60 }, // Time taken for morale boosting activities
            { "Supervision", 60 } // Time taken for supervision tasks
        };
        //     operationsStats = new Dictionary<string, int>
        // {
        //     { "FlowManagement", 1 },
        //     { "IssueResolution", 1 },
        //     { "MoraleBoost", 1 },
        //     {"Supervision", 1 } // Additional Operations stats can be added here
        // };
    }
}

public class Inbound : Department
{
    public Inbound() { }
    public Inbound(string name, int cap, Dictionary<string, int> stats)
    {
        departmentName = name;
        capacity = cap;
        this.stats = stats;
        statTimes = new Dictionary<string, int>
        {
            { "Receiving", 60 }, // Time taken for receiving tasks
            { "Sorting", 30 }, // Time taken for sorting tasks
            { "Throwing", 30 } // Time taken for throwing tasks
        };
        //     inboundStats = new Dictionary<string, int>
        // {
        //     { "Receiving", 1 },
        //     { "Sorting", 1 },
        //     { "Throwing", 1 },
        // };
    }
}

public class Sorting : Department
{
    public Sorting() { }
    public Sorting(string name, int cap, Dictionary<string, int> stats)
    {
        departmentName = name;
        capacity = cap;
        this.stats = stats;
        statTimes = null; // Sorting department does not have specific time stats for tasks
        //     sortingStats = new Dictionary<string, int>
        // {
        //     {"Accuracy", 1 }, // Accuracy of sorting items
        //     {"Speed", 1 }, // Speed of sorting items
        // };
    }
}

public class Repacking : Department
{
    public Repacking() { }
    public Repacking(string name, int cap, Dictionary<string, int> stats)
    {
        departmentName = name;
        capacity = cap;
        this.stats = stats;
        statTimes = new Dictionary<string, int>
        {
            { "PackageOpening", 5 }, // Time taken for package opening
            { "ItemSorting", 30 }, // Time taken for item sorting
            { "Repackaging", 10 } // Time taken for repackaging
        };
        //     repackingStats = new Dictionary<string, int>
        // {
        //     { "PackageOpening", 1 },
        //     {"ItemSorting", 1 },
        //     { "Repackaging", 1 },
        // };
    }
}

public class Palletizing : Department
{
    public Palletizing() { }
    public Palletizing(string name, int cap, Dictionary<string, int> stats)
    {
        departmentName = name;
        capacity = cap;
        this.stats = stats;
        //     palletizingStats = new Dictionary<string, int>
        // {
        //     { "Palletizing", 1 },
        //     { "Closing", 1 },
        // };
    }
}

public class FluidLoad : Department
{
    public FluidLoad() { }
    public FluidLoad(string name, int cap, Dictionary<string, int> stats)
    {
        departmentName = name;
        capacity = cap;
        this.stats = stats;
        //     fluidLoadStats = new Dictionary<string, int>
        // {
        //     { "FluidLoading", 1 },
        //     { "QualityCheck", 1 },
        //     { "Documentation", 1 } // Additional Fluid Load stats can be added here
        // };
    }
}