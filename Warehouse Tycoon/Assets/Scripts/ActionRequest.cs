public class ActionRequest
{
    public int id; // Unique identifier for the action request
    public ActionType actionType; // Type of action (e.g., "save", "load")
    public string data; // Data associated with the action request (e.g., JSON data)
    public StatusType.Type status; // Status of the action request (e.g., "pending", "completed", "failed")
    public string timestamp; // Timestamp of when the action request was created

    // Constructor to initialize an ActionRequest object
    public ActionRequest(int id, ActionType actionType, string data, StatusType.Type status, string timestamp)
    {
        this.id = id;
        this.actionType = actionType;
        this.data = data;
        this.status = status;
        this.timestamp = timestamp;
    }
    public class StatusType
    {
        public enum Type
        {
            Pending, // Action request is pending
            Completed, // Action request has been completed
            Failed // Action request has failed
        }

        public static string GetStatusName(Type statusType)
        {
            return statusType.ToString();
        }
    }
    public class ActionType
    {
        public string name; // Name of the action type
        public string description; // Description of the action type
        public DepartmentTypes.Type departmentType; // Type of department associated with the action type
        public EmployeeType.Type employeeType; // Type of employee associated with the action type
        public ActionType(string name, string description, DepartmentTypes.Type departmentType, EmployeeType.Type employeeType)
        {
            this.name = name;
            this.description = description;
            this.departmentType = departmentType;
            this.employeeType = employeeType;
        }
    }
}

