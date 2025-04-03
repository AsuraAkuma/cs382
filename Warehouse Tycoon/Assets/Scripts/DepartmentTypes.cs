public class DepartmentTypes
{
    public enum Type
    {
        HR,         // Human Resources
        IT,         // Information Technology
        Operations, // Operations Department
        Inbound,  // Inbound Logistics
        Sorting,  // Sorting Department
        Repacking, // Repacking Department
        Palletizing, // Palletizing Department
        FluidLoad, // Fluid Load Department
        QualityControl, // Quality Control Department
        Outbound, // Outbound Logistics
        Maintenance, // Maintenance Department
        Robotics, // Robotics Department
        Safety,    // Safety Department
        Cleaning, // Cleaning Department
        Security,  // Security Department
        LearningAndDevelopment // Learning and Development Department
    }
    public static string GetDepartmentName(Type departmentType)
    {
        return departmentType.ToString();
    }
}