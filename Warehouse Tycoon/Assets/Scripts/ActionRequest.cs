public class ActionRequest
{
    public int id; // Unique identifier for the action request
    public Employee employee; // Type of action (e.g., "save", "load")
    public StatusType.Type status; // Status of the action request (e.g., "pending", "completed", "failed")

    // Constructor to initialize an ActionRequest object
    public ActionRequest(int id, Employee employee = null)
    {
        this.id = id;
        this.employee = employee;
        status = StatusType.Type.Pending; // Default status is pending
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
}

