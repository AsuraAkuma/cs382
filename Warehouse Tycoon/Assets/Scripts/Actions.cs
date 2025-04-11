using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Actions : MonoBehaviour
{
    public class HR
    {
        // General actions
        public IEnumerator RaiseEmployeeLevel(Employee employee)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (employee.exp >= (employee.level + 1) * Globals.playerExpMultiplier)
            {
                employee.level++;
            }
            else
            {
                throw new System.InvalidOperationException("Employee does not have enough experience to level up.");
            }
            Debug.Log($"HR Manager {employee.employeeName} has successfully raised {employee.employeeName}'s level to {employee.level}.");
            yield break;
        }
        public IEnumerator UpgradeEmployeeStat(Employee employee, StatTypes.Type statType)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            switch (statType)
            {
                case StatTypes.Type.Speed:
                    if (employee.speed < Globals.employeeStatMax)
                    {
                        employee.speed += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's speed is already at maximum.");
                    }
                    break;
                case StatTypes.Type.Efficiency:
                    if (employee.efficiency < Globals.employeeStatMax)
                    {
                        employee.efficiency += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's efficiency is already at maximum.");
                    }
                    break;
                case StatTypes.Type.Stamina:
                    if (employee.stamina < Globals.employeeStatMax)
                    {
                        employee.stamina += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's stamina is already at maximum.");
                    }
                    break;
                case StatTypes.Type.Strength:
                    if (employee.strength < Globals.employeeStatMax)
                    {
                        employee.strength += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's strength is already at maximum.");
                    }
                    break;
                case StatTypes.Type.Focus:
                    if (employee.focus < Globals.employeeStatMax)
                    {
                        employee.focus += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's focus is already at maximum.");
                    }
                    break;
                case StatTypes.Type.Experience:
                    if (employee.experience < Globals.employeeStatMax)
                    {
                        employee.experience += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's experience is already at maximum.");
                    }
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(statType), "Invalid stat type.");
            }
            Debug.Log($"HR Manager {employee.employeeName} has successfully upgraded {employee.employeeName}'s {StatTypes.GetStatName(statType)} to {employee.GetStatValue(statType)}.");
            yield break;
        }
        public IEnumerator AssignDepartment(Employee employee, Department newDepartment)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (newDepartment == null)
            {
                throw new System.ArgumentNullException(newDepartment.departmentName, "Department cannot be null.");
            }
            employee.department.RemoveEmployee(employee); // Remove employee from the current department
            employee.department = newDepartment; // Assign the new department to the employee
            newDepartment.AddEmployee(employee); // Add employee to the new department
            Debug.Log($"HR Manager {employee.employeeName} has successfully assigned {employee.employeeName} to the {newDepartment.departmentName} department.");
            yield break;
        }
        public IEnumerator FireEmployee(Employee employee)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            employee.department.RemoveEmployee(employee);
            employee.department = null; // Remove employee from the department
            Globals.warehouseEmployees.Remove(employee); // Remove employee from the warehouse
            Globals.playerMoney += employee.cost / 2; // Refund the cost of the employee
            Debug.Log($"HR Manager {employee.employeeName} has successfully fired {employee.employeeName}. Refund: {employee.cost / 2}.");
            yield break;
        }
        public IEnumerator HireEmployee(Employee employee, Employee newHire)
        {
            // Check if there are any new hires available
            List<Department> departmentsInNeed = new List<Department>();
            foreach (var department in Globals.departments)
            {
                if (department.capacity > department.employees.Count)
                {
                    departmentsInNeed.Add(department);
                }
            }
            // Process the first new hire in the list
            if (Globals.newHires.Count == 0)
            {
                Debug.Log($"HR Manager {employee.employeeName} found no new hires to process.");
                yield break;
            }
            if (newHire.cost > Globals.playerMoney)
            {
                Debug.Log($"HR Manager {employee.employeeName} cannot afford to hire {newHire.employeeName}. Cost: {newHire.cost}, Available Money: {Globals.playerMoney}.");
                yield break;
            }
            Debug.Log($"HR Manager {employee.employeeName} is processing a new hire: {newHire.employeeName}.");
            // Check if the new hire can be assigned to a department
            if (departmentsInNeed.Count == 0)
            {
                Debug.Log($"HR Manager {employee.employeeName} found no departments in need of new hires.");
                yield break;
            }
            // Get the most suitable department for the new hire
            List<DepartmentIndex> departmentIndices = new List<DepartmentIndex>();
            foreach (var department in departmentsInNeed)
            {
                var existingIndex = departmentIndices.FirstOrDefault(d => d.departmentType == department.departmentType);
                if (existingIndex.departmentType == department.departmentType)
                {
                    existingIndex.count++;
                }
                else
                {
                    departmentIndices.Add(new DepartmentIndex { departmentType = department.departmentType, count = 1 });
                }
            }
            // Sort departments by index to find the most suitable one
            departmentIndices = departmentIndices.OrderByDescending(d => d.count).ToList();
            // Assign the new hire to the most suitable department
            Department selectedDepartment = departmentsInNeed.FirstOrDefault(d => d.departmentType == departmentIndices[0].departmentType);
            if (selectedDepartment == null)
            {
                Debug.Log($"HR Manager {employee.employeeName} could not find a suitable department for {newHire.employeeName}.");
                yield break;
            }
            selectedDepartment.AddEmployee(newHire);
            Globals.warehouseEmployees.Add(newHire);
            Globals.newHires.Remove(newHire);
            Globals.playerMoney -= newHire.cost; // Deduct the cost of hiring from player's money
            Debug.Log($"HR Manager {employee.employeeName} has successfully assigned {newHire.employeeName} to the {selectedDepartment.departmentType} department.");
            yield break;
        }
        public IEnumerator PromoteEmployee(Employee employee, Employee promotee)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (promotee == null)
            {
                throw new System.ArgumentNullException(nameof(promotee), "Promotee cannot be null.");
            }
            // Check if the employee is already a manager class
            if (promotee is HRManager || promotee is ITManager || promotee is OperationsManager || promotee is InboundManager || promotee is SortingManager || promotee is RepackingManager || promotee is PalletizingManager || promotee is WaterSpiderManager || promotee is FluidLoadManager || promotee is QualityControlManager || promotee is OutboundManager || promotee is MaintenanceManager || promotee is RoboticsManager || promotee is SafetyManager || promotee is CleaningManager || promotee is SecurityManager || promotee is LearningManager || promotee is RecruitingManager)
            {
                Debug.Log($"{promotee.employeeName} is already a manager.");
                yield break;
            }
            if (promotee.level == Globals.employeeMaxLevel)
            {
                employee.level = 1;
                // Create new employee object with the same stats as the promotee
                Employee newManager;
                switch (promotee.department.departmentType)
                {
                    case DepartmentTypes.Type.HR:
                        newManager = new HRManager(promotee);
                        break;
                    case DepartmentTypes.Type.IT:
                        newManager = new ITManager(promotee);
                        break;
                    case DepartmentTypes.Type.Operations:
                        newManager = new OperationsManager(promotee);
                        break;
                    case DepartmentTypes.Type.Inbound:
                        newManager = new InboundManager(promotee);
                        break;
                    case DepartmentTypes.Type.Sorting:
                        newManager = new SortingManager(promotee);
                        break;
                    case DepartmentTypes.Type.Repacking:
                        newManager = new RepackingManager(promotee);
                        break;
                    case DepartmentTypes.Type.Palletizing:
                        newManager = new PalletizingManager(promotee);
                        break;
                    case DepartmentTypes.Type.WaterSpidering:
                        newManager = new WaterSpiderManager(promotee);
                        break;
                    case DepartmentTypes.Type.FluidLoad:
                        newManager = new FluidLoadManager(promotee);
                        break;
                    case DepartmentTypes.Type.QualityControl:
                        newManager = new QualityControlManager(promotee);
                        break;
                    case DepartmentTypes.Type.Outbound:
                        newManager = new OutboundManager(promotee);
                        break;
                    case DepartmentTypes.Type.Maintenance:
                        newManager = new MaintenanceManager(promotee);
                        break;
                    case DepartmentTypes.Type.Robotics:
                        newManager = new RoboticsManager(promotee);
                        break;
                    case DepartmentTypes.Type.Safety:
                        newManager = new SafetyManager(promotee);
                        break;
                    case DepartmentTypes.Type.Cleaning:
                        newManager = new CleaningManager(promotee);
                        break;
                    case DepartmentTypes.Type.Security:
                        newManager = new SecurityManager(promotee);
                        break;
                    case DepartmentTypes.Type.LearningAndDevelopment:
                        newManager = new LearningManager(promotee);
                        break;
                    case DepartmentTypes.Type.Recruiting:
                        newManager = new RecruitingManager(promotee);
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException(nameof(promotee.department.departmentType), "Invalid department type for promotion.");
                }
                // Remove the promotee from the current department
                promotee.department.RemoveEmployee(promotee);
                // Add the new manager to the department
                newManager.department.AddEmployee(newManager);
                // Assign the new manager to the same department as the promotee
                newManager.department = promotee.department;
                // Remove the promotee from the warehouse employees list
                Globals.warehouseEmployees.Remove(promotee);
                // Add the new manager to the warehouse employees list
                Globals.warehouseEmployees.Add(newManager);
                Debug.Log($"HR Manager {employee.employeeName} has successfully promoted {employee.employeeName} to level {employee.level}.");
            }
            else
            {
                throw new System.InvalidOperationException("Employee is already at maximum level.");
            }
            yield break;
        }
        // Disabler actions
        public IEnumerator DocumentInjury(Employee employee, Employee injuredEmployee)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (injuredEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(injuredEmployee), "Injured employee cannot be null.");
            }
            // Get disabler for injury in department
            Disablers.Disabler injuryDisabler = injuredEmployee.department.disablers.FirstOrDefault(d => d.name == Disablers.Employee.injury.name);
            if (injuryDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"HR Manager {employee.employeeName} found no injury disabler in the department.");
                yield break;
            }
            Debug.Log($"HR Manager {employee.employeeName} is reporting an injury for {injuredEmployee.employeeName}.");
            yield break;
        }
        public IEnumerator DocumentMisconduct(Employee employee, Employee misconductEmployee)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (misconductEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(misconductEmployee), "Misconduct employee cannot be null.");
            }
            // Get disabler for employee misconduct in department
            Disablers.Disabler misconductDisabler = misconductEmployee.department.disablers.FirstOrDefault(d => d.name == Disablers.Employee.employeeMisconduct.name);
            if (misconductDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"HR Manager {employee.employeeName} found no misconduct disabler in the department.");
                yield break;
            }
            Debug.Log($"HR Manager {employee.employeeName} is reporting misconduct for {misconductEmployee.employeeName}.");
            yield break;
        }
        //    Idle action
        public IEnumerator ReviewDocuments(Employee employee)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            // Logic to review documents in the HR department
            Debug.Log($"HR Manager {employee.employeeName} is reviewing documents.");
            yield break;
        }
    }
    public class IT
    {

    }
}

struct DepartmentIndex
{
    public DepartmentTypes.Type departmentType;
    public int count;
}
