public class EmployeeType
{
    public enum Type
    {
        HRManager,
        HREmployee,
        ITManager,
        ITEmployee,
        OperationsManager,
        InboundManager,
        InboundEmployee,
        SortingManager,
        SortingEmployee,
        RepackingManager,
        RepackingEmployee,
        PalletizingManager,
        PalletizingEmployee,
        WaterSpideringManager,
        WaterSpideringEmployee,
        FluidLoadManager,
        FluidLoadEmployee,
        QualityControlManager,
        QualityControlEmployee,
        OutboundManager,
        OutboundEmployee,
        MaintenanceManager,
        MaintenanceEmployee,
        RoboticsManager,
        RoboticsEmployee,
        SafetyManager,
        SafetyEmployee,
        CleaningManager,
        CleaningEmployee,
        SecurityManager,
        SecurityEmployee,
        LearningAndDevelopmentManager,
        LearningAndDevelopmentEmployee
    }

    public static string GetEmployeeType(Type employeeType)
    {
        return employeeType.ToString();
    }
}