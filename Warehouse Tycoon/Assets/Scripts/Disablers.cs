using System.Collections.Generic;

public class Disablers
{
    public class Warehouse
    {
        public static List<Disabler> allDisablers = new List<Disabler>
        {
            fire,
            powerOutage,
            securityBreach,
            networkFailure
        };
        public static Disabler fire = new Disabler("Fire", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety, DepartmentTypes.Type.Maintenance });
        public static Disabler powerOutage = new Disabler("Power Outage", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Maintenance });
        public static Disabler securityBreach = new Disabler("Security Breach", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Security });
        public static Disabler networkFailure = new Disabler("Network Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.IT });
    }
    public class Department
    {
        public static List<Disabler> allDisablers = new List<Disabler>
        {
            deviceFailure,
            serverFailure,
            mechanicalFailure,
            electricalFailure,
            chemicalSpill,
            flood,
            equipmentFailure,
            accident,
            hazardousMaterial
        };
        public static Disabler deviceFailure = new Disabler("Device Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.IT });
        public static Disabler serverFailure = new Disabler("Server Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.IT });
        public static Disabler mechanicalFailure = new Disabler("Mechanical Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Maintenance });
        public static Disabler electricalFailure = new Disabler("Electrical Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Maintenance });
        public static Disabler chemicalSpill = new Disabler("Chemical Spill", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety, DepartmentTypes.Type.Cleaning });
        public static Disabler flood = new Disabler("Flood", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety, DepartmentTypes.Type.Cleaning, DepartmentTypes.Type.Maintenance });
        public static Disabler equipmentFailure = new Disabler("Equipment Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Maintenance });
        public static Disabler accident = new Disabler("Accident", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety });
        public static Disabler hazardousMaterial = new Disabler("Hazardous Material", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety, DepartmentTypes.Type.Cleaning });
    }
    public class Employee
    {
        public static List<Disabler> allDisablers = new List<Disabler>
        {
            injury,
            employeeMisconduct,
            computerFailure,
            equipmentMalfunction,
            theft,
            dirtyStation,
            stationUnsafe
        };
        public static Disabler injury = new Disabler("Injury", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety, DepartmentTypes.Type.HR, DepartmentTypes.Type.Safety });
        public static Disabler employeeMisconduct = new Disabler("Employee Misconduct", new List<DepartmentTypes.Type> { DepartmentTypes.Type.HR });
        public static Disabler computerFailure = new Disabler("Computer Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.IT });
        public static Disabler equipmentMalfunction = new Disabler("Equipment Malfunction", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Maintenance });
        public static Disabler theft = new Disabler("Theft", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Security, DepartmentTypes.Type.HR });
        public static Disabler dirtyStation = new Disabler("Dirty Station", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Cleaning });
        public static Disabler stationUnsafe = new Disabler("Unsafe Station", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety });
    }
    public struct Disabler
    {
        public string name;
        public List<DepartmentTypes.Type> enablingDepartments; // Department that enables this disabler
        public List<ActionRequest> actionRequests; // List of action requests associated with this disabler
        public Disabler(string name, List<DepartmentTypes.Type> enablingDepartments = null, List<ActionRequest> actionRequests = null)
        {
            this.name = name;
            this.enablingDepartments = enablingDepartments ?? new List<DepartmentTypes.Type>();
            this.actionRequests = actionRequests ?? new List<ActionRequest>();
        }
        public void RemoveEnablingDepartment(DepartmentTypes.Type departmentType)
        {
            if (enablingDepartments.Contains(departmentType))
            {
                enablingDepartments.Remove(departmentType);
            }
        }
        public void AddEnablingDepartment(DepartmentTypes.Type departmentType)
        {
            if (!enablingDepartments.Contains(departmentType))
            {
                enablingDepartments.Add(departmentType);
            }
        }

        // Add this copy constructor
        public Disabler Clone()
        {
            return new Disabler(name, new List<DepartmentTypes.Type>(enablingDepartments), actionRequests);
        }
    }

}
