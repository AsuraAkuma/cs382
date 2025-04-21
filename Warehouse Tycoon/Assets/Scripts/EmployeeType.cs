public class EmployeeType
{
    public enum Type
    {
        HRManager,
        HREmployee,
        ITManager,
        ITEmployee,
        OperationsManager,
        OperationsEmployee,
        InboundManager,
        InboundEmployee,
        SortingManager,
        SortingEmployee,
        RepackingManager,
        RepackingEmployee,
        PalletizingManager,
        PalletizingEmployee,
        WaterSpideringManager,
        WaterSpiderEmployee,
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
        LearningManager,
        LearningEmployee,
        RecruitingManager,
        RecruitingEmployee
    }

    public static string GetEmployeeType(Type employeeType)
    {
        return employeeType.ToString();
    }
}