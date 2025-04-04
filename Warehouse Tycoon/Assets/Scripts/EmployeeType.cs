public class EmployeeType
{
    public enum Type
    {
        HRManager,
        ITManager,
        OperationsManager,
        InboundManager,
        SortingManager,
        RepackingManager,
        PalletizingManager,
        WaterSpideringManager,
        FluidLoadManager,
        QualityControlManager,
        OutboundManager,
        MaintenanceManager,
        RoboticsManager,
        SafetyManager,
        CleaningManager,
        SecurityManager,
        LearningAndDevelopmentManager,
    }

    public static string GetEmployeeType(Type employeeType)
    {
        return employeeType.ToString();
    }
}