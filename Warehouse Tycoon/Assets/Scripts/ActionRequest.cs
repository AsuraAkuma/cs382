using System.Collections;

[System.Serializable]
public class ActionRequest
{
    public Employee employee; // Employee associated with the action request (if any)
    public StatusType.Type status; // Status of the action request (e.g., "pending", "completed", "failed")
    public IEnumerator action; // Action associated with the request 
    public Employee affectedEmployee; // Employee affected by the action request (if any)
    public Department affectedDepartment; // Department associated with the action request (if any)
    // Constructor to initialize an ActionRequest object
    public ActionRequest(IEnumerator action, Employee affectedEmployee = null, Department affectedDepartment = null)
    {
        this.affectedEmployee = affectedEmployee;
        this.affectedDepartment = affectedDepartment;
        this.action = action;
        status = StatusType.Type.Pending; // Default status is pending
    }

    [System.Serializable]
    public class StatusType
    {
        public enum Type
        {
            Pending, // Action request is pending
            InProgress, // Action request is in progress
            Completed, // Action request has been completed
            Failed // Action request has failed
        }

        public static string GetStatusName(Type statusType)
        {
            return statusType.ToString();
        }
    }
}

