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
        Palletizing, // Palletizing Department (loading boxes onto pallets)
        WaterSpidering, // Water Spidering Department (water spiders are employees who bring supplies to other employees in the warehouse and take away full pallets)
        FluidLoad, // Fluid Load Department (load boxes into trucks by hand wearing hard hats)
        QualityControl, // Quality Control Department
        Outbound, // Outbound Logistics
        Maintenance, // Maintenance Department
        Robotics, // Robotics Department (robot arms that load pallets with totes full of products)
        Safety,    // Safety Department
        Cleaning, // Cleaning Department
        Security,  // Security Department
        LearningAndDevelopment, // Learning and Development Department
        None
    }
    public static string GetDepartmentName(Type departmentType)
    {
        return departmentType.ToString();
    }
}