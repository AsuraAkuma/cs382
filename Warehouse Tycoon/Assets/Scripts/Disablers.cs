using System.Collections.Generic;

public class Disablers
{
    public static Disabler deviceFailure = new Disabler("Device Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.IT });
    public static Disabler networkFailure = new Disabler("Network Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.IT });
    public static Disabler serverFailure = new Disabler("Server Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.IT });
    public static Disabler mechanicalFailure = new Disabler("Mechanical Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Maintenance });
    public static Disabler electricalFailure = new Disabler("Electrical Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Maintenance });
    public static Disabler injury = new Disabler("Injury", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety, DepartmentTypes.Type.HR });
    public static Disabler chemicalSpill = new Disabler("Chemical Spill", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety, DepartmentTypes.Type.Cleaning });
    public static Disabler fire = new Disabler("Fire", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety, DepartmentTypes.Type.Maintenance });
    public static Disabler flood = new Disabler("Flood", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety, DepartmentTypes.Type.Cleaning, DepartmentTypes.Type.Maintenance });
    public static Disabler securityBreach = new Disabler("Security Breach", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Security });
    public static Disabler powerOutage = new Disabler("Power Outage", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Maintenance });
    public static Disabler equipmentFailure = new Disabler("Equipment Failure", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Maintenance });
    public static Disabler accident = new Disabler("Accident", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety });
    public static Disabler hazardousMaterial = new Disabler("Hazardous Material", new List<DepartmentTypes.Type> { DepartmentTypes.Type.Safety, DepartmentTypes.Type.Cleaning });
    public static Disabler employeeMisconduct = new Disabler("Employee Misconduct", new List<DepartmentTypes.Type> { DepartmentTypes.Type.HR });

    public struct Disabler
    {
        public string name;
        public List<DepartmentTypes.Type> enablingDepartments; // Department that enables this disabler
        public Disabler(string name, List<DepartmentTypes.Type> enablingDepartments = null)
        {
            this.name = name;
            this.enablingDepartments = enablingDepartments ?? new List<DepartmentTypes.Type>();
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
    }

}
