using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

public class Actions : MonoBehaviour
{
    public NotificationController notificationController; // Reference to the NotificationController for displaying notifications
    public GameController gameController; // Reference to the GameController for managing game state
    public void Start()
    {
        notificationController = FindFirstObjectByType<NotificationController>();
        gameController = FindFirstObjectByType<GameController>();
    }
    // Departments
    public class HR : Actions
    {
        // General actions
        public IEnumerator RaiseEmployeeLevel(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Raised employee cannot be null.");
            }
            if (affectedEmployee.exp >= (affectedEmployee.level + 1) * Globals.playerExpMultiplier)
            {
                affectedEmployee.level++;
            }
            else
            {
                throw new System.InvalidOperationException("Employee does not have enough experience to level up.");
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            Debug.Log($"HR Employee {employee.employeeName} has successfully raised {affectedEmployee.employeeName}'s level to {affectedEmployee.level}.");
            yield break;
        }
        public IEnumerator UpgradeEmployeeStat(Employee employee = null, Employee affectedEmployee = null, StatTypes.Type statType = StatTypes.Type.Efficiency)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            switch (statType)
            {
                case StatTypes.Type.Speed:
                    if (affectedEmployee.speed < Globals.employeeStatMax)
                    {
                        affectedEmployee.speed += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's speed is already at maximum.");
                    }
                    break;
                case StatTypes.Type.Efficiency:
                    if (affectedEmployee.efficiency < Globals.employeeStatMax)
                    {
                        affectedEmployee.efficiency += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's efficiency is already at maximum.");
                    }
                    break;
                case StatTypes.Type.Stamina:
                    if (affectedEmployee.stamina < Globals.employeeStatMax)
                    {
                        affectedEmployee.stamina += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's stamina is already at maximum.");
                    }
                    break;
                case StatTypes.Type.Strength:
                    if (affectedEmployee.strength < Globals.employeeStatMax)
                    {
                        affectedEmployee.strength += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's strength is already at maximum.");
                    }
                    break;
                case StatTypes.Type.Focus:
                    if (affectedEmployee.focus < Globals.employeeStatMax)
                    {
                        affectedEmployee.focus += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's focus is already at maximum.");
                    }
                    break;
                case StatTypes.Type.Experience:
                    if (affectedEmployee.experience < Globals.employeeStatMax)
                    {
                        affectedEmployee.experience += Globals.employeeStatUpgradeValue;
                    }
                    else
                    {
                        throw new System.InvalidOperationException("Employee's experience is already at maximum.");
                    }
                    break;
                default:
                    throw new System.ArgumentOutOfRangeException(nameof(statType), "Invalid stat type.");
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            Debug.Log($"HR Employee {employee.employeeName} has successfully upgraded {affectedEmployee.employeeName}'s {StatTypes.GetStatName(statType)} to {affectedEmployee.GetStatValue(statType)}.");
            yield break;
        }
        public IEnumerator AssignDepartment(Employee employee = null, Employee affectedEmployee = null, Department newDepartment = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            if (newDepartment == null)
            {
                throw new System.ArgumentNullException(newDepartment.departmentName, "Department cannot be null.");
            }
            affectedEmployee.department.RemoveEmployee(affectedEmployee); // Remove employee from the current department
            affectedEmployee.department.UpdateEmployeeUIList(); // Update the UI of the current department
            affectedEmployee.department = newDepartment; // Assign the new department to the employee
            newDepartment.AddEmployee(affectedEmployee); // Add employee to the new department
            newDepartment.UpdateEmployeeUIList(); // Update the UI of the new department
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            Debug.Log($"HR Employee {employee.employeeName} has successfully assigned {affectedEmployee.employeeName} to the {newDepartment.departmentName} department.");
            yield break;
        }
        public IEnumerator FireEmployee(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            affectedEmployee.department.RemoveEmployee(affectedEmployee);
            affectedEmployee.department.UpdateEmployeeUIList(); // Update the UI of the current department
            affectedEmployee.department = null; // Remove employee from the department
            Globals.warehouseEmployees.Remove(affectedEmployee); // Remove employee from the warehouse
            Globals.playerMoney += affectedEmployee.cost / 2; // Refund the cost of the employee
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            Debug.Log($"HR Employee {employee.employeeName} has successfully fired {affectedEmployee.employeeName}. Refund: {affectedEmployee.cost / 2}.");
            yield break;
        }
        public IEnumerator HireEmployee(Employee employee = null, Employee affectedEmployee = null)
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
                Debug.Log($"HR Employee {employee.employeeName} found no new hires to process.");
                yield break;
            }
            if (affectedEmployee.cost > Globals.playerMoney)
            {
                Debug.Log($"HR Employee {employee.employeeName} cannot afford to hire {affectedEmployee.employeeName}. Cost: {affectedEmployee.cost}, Available Money: {Globals.playerMoney}.");
                yield break;
            }
            Debug.Log($"HR Employee {employee.employeeName} is processing a new hire: {affectedEmployee.employeeName}.");
            // Check if the new hire can be assigned to a department
            if (departmentsInNeed.Count == 0)
            {
                Debug.Log($"HR Employee {employee.employeeName} found no departments in need of new hires.");
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
                Debug.Log($"HR Employee {employee.employeeName} could not find a suitable department for {affectedEmployee.employeeName}.");
                yield break;
            }
            selectedDepartment.AddEmployee(affectedEmployee);
            Globals.warehouseEmployees.Add(affectedEmployee);
            Globals.newHires.Remove(affectedEmployee);
            Globals.playerMoney -= affectedEmployee.cost; // Deduct the cost of hiring from player's money
            selectedDepartment.UpdateEmployeeUIList(); // Add the new hire to the department's UI
            gameController.UpdateEmployeeUIList(); // Update the game controller's UI
            Debug.Log($"HR Employee {employee.employeeName} has successfully assigned {affectedEmployee.employeeName} to the {selectedDepartment.departmentType} department.");
            yield break;
        }
        public IEnumerator PromoteEmployee(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "affectedEmployee cannot be null.");
            }
            // Check if the employee is already a manager class
            if (affectedEmployee is HRManager || affectedEmployee is ITManager || affectedEmployee is OperationsManager || affectedEmployee is InboundManager || affectedEmployee is SortingManager || affectedEmployee is RepackingManager || affectedEmployee is PalletizingManager || affectedEmployee is WaterSpiderManager || affectedEmployee is FluidLoadManager || affectedEmployee is QualityControlManager || affectedEmployee is OutboundManager || affectedEmployee is MaintenanceManager || affectedEmployee is RoboticsManager || affectedEmployee is SafetyManager || affectedEmployee is CleaningManager || affectedEmployee is SecurityManager || affectedEmployee is LearningManager || affectedEmployee is RecruitingManager)
            {
                Debug.Log($"{affectedEmployee.employeeName} is already a manager.");
                yield break;
            }
            if (affectedEmployee.level == Globals.employeeMaxLevel)
            {
                employee.level = 1;
                // Create new employee object with the same stats as the affectedEmployee
                Employee newManager;
                switch (affectedEmployee.department.departmentType)
                {
                    case DepartmentTypes.Type.HR:
                        newManager = new HRManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.IT:
                        newManager = new ITManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Operations:
                        newManager = new OperationsManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Inbound:
                        newManager = new InboundManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Sorting:
                        newManager = new SortingManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Repacking:
                        newManager = new RepackingManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Palletizing:
                        newManager = new PalletizingManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.WaterSpidering:
                        newManager = new WaterSpiderManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.FluidLoad:
                        newManager = new FluidLoadManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.QualityControl:
                        newManager = new QualityControlManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Outbound:
                        newManager = new OutboundManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Maintenance:
                        newManager = new MaintenanceManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Robotics:
                        newManager = new RoboticsManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Safety:
                        newManager = new SafetyManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Cleaning:
                        newManager = new CleaningManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Security:
                        newManager = new SecurityManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Learning:
                        newManager = new LearningManager(affectedEmployee);
                        break;
                    case DepartmentTypes.Type.Recruiting:
                        newManager = new RecruitingManager(affectedEmployee);
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException(nameof(affectedEmployee.department.departmentType), "Invalid department type for promotion.");
                }
                // Remove the affectedEmployee from the current department
                affectedEmployee.department.RemoveEmployee(affectedEmployee);
                // Add the new manager to the department
                newManager.department.AddEmployee(newManager);
                // Assign the new manager to the same department as the affectedEmployee
                newManager.department = affectedEmployee.department;
                // Remove the affectedEmployee from the warehouse employees list
                Globals.warehouseEmployees.Remove(affectedEmployee);
                // Add the new manager to the warehouse employees list
                Globals.warehouseEmployees.Add(newManager);
                newManager.department.UpdateEmployeeUIList(); // Update the UI of the new manager's department
                Debug.Log($"HR Employee {employee.employeeName} has successfully promoted {employee.employeeName} to level {employee.level}.");
            }
            else
            {
                throw new System.InvalidOperationException("Employee is not max level.");
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
        public IEnumerator ResetInfractions(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            // Clear infractions for the affected employee
            affectedEmployee.infractions = 0; // Reset the infractions count
            affectedEmployee.department.UpdateEmployeeUIList(); // Update the UI of the affected employee's department
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            Debug.Log($"HR Employee {employee.employeeName} has successfully cleared infractions for {affectedEmployee.employeeName}.");
            yield break;
        }
        // Disabler actions
        public IEnumerator DocumentInjury(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Injured employee cannot be null.");
            }
            // Get disabler for injury in department
            Disablers.Disabler injuryDisabler = affectedEmployee.department.disablers.FirstOrDefault(d => d.name == Disablers.Employee.injury.name);
            if (injuryDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"HR Employee {employee.employeeName} found no injury disabler in the department.");
                yield break;
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            Debug.Log($"HR Employee {employee.employeeName} is reporting an injury for {affectedEmployee.employeeName}.");
            yield break;
        }
        public IEnumerator DocumentMisconduct(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Misconduct employee cannot be null.");
            }
            // Get disabler for employee misconduct in department
            Disablers.Disabler misconductDisabler = affectedEmployee.department.disablers.FirstOrDefault(d => d.name == Disablers.Employee.employeeMisconduct.name);
            if (misconductDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"HR Employee {employee.employeeName} found no misconduct disabler in the department.");
                yield break;
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            Debug.Log($"HR Employee {employee.employeeName} is reporting misconduct for {affectedEmployee.employeeName}.");
            yield break;
        }
    }
    public class IT : Actions
    {
        public IEnumerator FixNetworkFailure(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> networkDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Warehouse.networkFailure.name)).ToList();
            if (networkDisablers.Count == 0)
            {
                Debug.Log($"IT Employee {employee.employeeName} found no network failure disablers in the warehouse.");
                yield break;
            }
            Debug.Log($"IT Employee {employee.employeeName} is fixing network issues.");
            // Logic to fix network failure in the IT department
            foreach (var department in networkDisablers)
            {
                // Remove the network failure disabler from the department
                Disablers.Disabler networkDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Warehouse.networkFailure.name);
                if (networkDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"IT Employee {employee.employeeName} found no network failure disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (networkDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"IT Employee {employee.employeeName} found no enabling departments for the network disabler in the {department.departmentName} department.");
                    continue;
                }
                if (networkDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"IT Employee {employee.employeeName} cannot fix network issues in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }
                networkDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (networkDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(networkDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"IT Employee {employee.employeeName} has successfully fixed the network failure in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"IT Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator FixDeviceFailure(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> deviceDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.deviceFailure.name)).ToList();
            if (deviceDisablers.Count == 0)
            {
                Debug.Log($"IT Employee {employee.employeeName} found no device failure disablers in the warehouse.");
                yield break;
            }
            Debug.Log($"IT Employee {employee.employeeName} is fixing device issues.");
            // Logic to fix device failure in the IT department
            foreach (var department in deviceDisablers)
            {
                // Remove the device failure disabler from the department
                Disablers.Disabler deviceDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.deviceFailure.name);
                if (deviceDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"IT Employee {employee.employeeName} found no device failure disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (deviceDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"IT Employee {employee.employeeName} found no enabling departments for the device disabler in the {department.departmentName} department.");
                    continue;
                }
                if (deviceDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"IT Employee {employee.employeeName} cannot fix device issues in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }
                deviceDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (deviceDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(deviceDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"IT Employee {employee.employeeName} has successfully fixed the device failure in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"IT Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator FixServerFailure(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> serverDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.serverFailure.name)).ToList();
            if (serverDisablers.Count == 0)
            {
                Debug.Log($"IT Employee {employee.employeeName} found no server failure disablers in the warehouse.");
                yield break;
            }
            Debug.Log($"IT Employee {employee.employeeName} is fixing server issues.");
            // Logic to fix server failure in the IT department
            foreach (var department in serverDisablers)
            {
                // Remove the server failure disabler from the department
                Disablers.Disabler serverDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.serverFailure.name);
                if (serverDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"IT Employee {employee.employeeName} found no server failure disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (serverDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"IT Employee {employee.employeeName} found no enabling departments for the server disabler in the {department.departmentName} department.");
                    continue;
                }
                if (serverDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"IT Employee {employee.employeeName} cannot fix server issues in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }
                serverDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (serverDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(serverDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"IT Employee {employee.employeeName} has successfully fixed the server failure in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"IT Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator FixComputerFailure(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            // Get disabler for computer failure in employee
            Disablers.Disabler computerDisabler = affectedEmployee.disablers.FirstOrDefault(d => d.name == Disablers.Employee.computerFailure.name);
            if (computerDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"IT Employee {employee.employeeName} found no computer failure disabler in the {affectedEmployee.department.departmentName} department.");
                yield break;
            }
            Debug.Log($"IT Employee {employee.employeeName} is fixing server issues.");
            computerDisabler.RemoveEnablingDepartment(employee.department.departmentType);
            if (computerDisabler.enablingDepartments.Count == 0)
            {
                affectedEmployee.RemoveDisabler(computerDisabler);
            }
            Debug.Log($"IT Employee {employee.employeeName} has successfully fixed the computer failure for {affectedEmployee.employeeName}.");
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
    }
    public class Maintenance : Actions
    {
        public IEnumerator RepairFireDamages(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> fireDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Warehouse.fire.name)).ToList();
            if (fireDisablers.Count == 0)
            {
                Debug.Log($"Maintenance Employee {employee.employeeName} found no fire damages in the warehouse.");
                yield break;
            }
            Debug.Log($"Maintenance Employee {employee.employeeName} is repairing fire damages.");
            // Logic to repair fire damages in the maintenance department
            foreach (var department in fireDisablers)
            {
                // Remove the fire disabler from the department
                Disablers.Disabler fireDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Warehouse.fire.name);
                if (fireDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no fire disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (fireDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no enabling departments for the fire disabler in the {department.departmentName} department.");
                    continue;
                }
                // If this department is first in the list, remove it from the enabling departments
                if (fireDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} cannot repair fire damages in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }

                fireDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (fireDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(fireDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Maintenance Employee {employee.employeeName} has successfully repaired the fire damages in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator RepairPowerOutage(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> powerDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Warehouse.powerOutage.name)).ToList();
            if (powerDisablers.Count == 0)
            {
                Debug.Log($"Maintenance Employee {employee.employeeName} found no power outage disablers in the warehouse.");
                yield break;
            }
            Debug.Log($"Maintenance Employee {employee.employeeName} is repairing power outages.");
            // Logic to repair power outages in the maintenance department
            foreach (var department in powerDisablers)
            {
                // Remove the power outage disabler from the department
                Disablers.Disabler powerDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Warehouse.powerOutage.name);
                if (powerDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no power outage disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (powerDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no enabling departments for the power outage disabler in the {department.departmentName} department.");
                    continue;
                }
                // If this department is first in the list, remove it from the enabling departments
                if (powerDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} cannot repair power outages in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }

                powerDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (powerDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(powerDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Maintenance Employee {employee.employeeName} has successfully repaired the power outage in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator RepairMechanicalFailure(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> powerDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Warehouse.powerOutage.name)).ToList();
            if (powerDisablers.Count == 0)
            {
                Debug.Log($"Maintenance Employee {employee.employeeName} found no mechanical failure disablers in the warehouse.");
                yield break;
            }
            Debug.Log($"Maintenance Employee {employee.employeeName} is repairing mechanical failures.");
            // Logic to repair power outages in the maintenance department
            foreach (var department in powerDisablers)
            {
                // Remove the power outage disabler from the department
                Disablers.Disabler powerDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.mechanicalFailure.name);
                if (powerDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no mechanical failure disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (powerDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no enabling departments for the mechanical failure disabler in the {department.departmentName} department.");
                    continue;
                }
                // If this department is first in the list, remove it from the enabling departments
                if (powerDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} cannot repair mechanical failures in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }

                powerDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (powerDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(powerDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Maintenance Employee {employee.employeeName} has successfully repaired the mechanical failure in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator RepairElectricalFailure(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> electricalDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.electricalFailure.name)).ToList();
            if (electricalDisablers.Count == 0)
            {
                Debug.Log($"Maintenance Employee {employee.employeeName} found no electrical failure disablers in the warehouse.");
                yield break;
            }
            Debug.Log($"Maintenance Employee {employee.employeeName} is repairing electrical failures.");
            // Logic to repair electrical failures in the maintenance department
            foreach (var department in electricalDisablers)
            {
                // Remove the electrical failure disabler from the department
                Disablers.Disabler electricalDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.electricalFailure.name);
                if (electricalDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no electrical failure disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (electricalDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no enabling departments for the electrical failure disabler in the {department.departmentName} department.");
                    continue;
                }
                // If this department is first in the list, remove it from the enabling departments
                if (electricalDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} cannot repair electrical failures in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }

                electricalDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (electricalDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(electricalDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Maintenance Employee {employee.employeeName} has successfully repaired the electrical failure in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator RepairFloodDamage(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> floodDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.flood.name)).ToList();
            if (floodDisablers.Count == 0)
            {
                Debug.Log($"Maintenance Employee {employee.employeeName} found no flood damages in the warehouse.");
                yield break;
            }
            Debug.Log($"Maintenance Employee {employee.employeeName} is repairing flood damages.");
            // Logic to repair flood damages in the maintenance department
            foreach (var department in floodDisablers)
            {
                // Remove the flood disabler from the department
                Disablers.Disabler floodDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.flood.name);
                if (floodDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no flood disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (floodDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no enabling departments for the flood disabler in the {department.departmentName} department.");
                    continue;
                }
                // If this department is first in the list, remove it from the enabling departments
                if (floodDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} cannot repair flood damages in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }

                floodDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (floodDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(floodDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Maintenance Employee {employee.employeeName} has successfully repaired the flood damages in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator RepairEquipmentFailure(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> equipmentDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.equipmentFailure.name)).ToList();
            if (equipmentDisablers.Count == 0)
            {
                Debug.Log($"Maintenance Employee {employee.employeeName} found no equipment failure disablers in the warehouse.");
                yield break;
            }
            Debug.Log($"Maintenance Employee {employee.employeeName} is repairing equipment failures.");
            // Logic to repair equipment failures in the maintenance department
            foreach (var department in equipmentDisablers)
            {
                // Remove the equipment failure disabler from the department
                Disablers.Disabler equipmentDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.equipmentFailure.name);
                if (equipmentDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no equipment failure disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (equipmentDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no enabling departments for the equipment failure disabler in the {department.departmentName} department.");
                    continue;
                }
                // If this department is first in the list, remove it from the enabling departments
                if (equipmentDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} cannot repair equipment failures in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }

                equipmentDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (equipmentDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(equipmentDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Maintenance Employee {employee.employeeName} has successfully repaired the equipment failure in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator RepairEquipmentMalfunction(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            List<Department> malfunctionDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Employee.equipmentMalfunction.name)).ToList();
            if (malfunctionDisablers.Count == 0)
            {
                Debug.Log($"Maintenance Employee {employee.employeeName} found no equipment malfunctions in the warehouse.");
                yield break;
            }
            Debug.Log($"Maintenance Employee {employee.employeeName} is repairing equipment malfunctions.");
            // Logic to repair equipment malfunctions in the maintenance department
            foreach (var department in malfunctionDisablers)
            {
                // Remove the equipment malfunction disabler from the department
                Disablers.Disabler malfunctionDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Employee.equipmentMalfunction.name);
                if (malfunctionDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no equipment malfunction disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (malfunctionDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found no enabling departments for the equipment malfunction disabler for {affectedEmployee.employeeName}.");
                    continue;
                }
                // If this department is first in the list, remove it from the enabling departments
                if (malfunctionDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} cannot repair equipment malfunctions for {affectedEmployee.employeeName}. This department is not first in the list of enabling departments.");
                    continue;
                }

                malfunctionDisabler.RemoveEnablingDepartment(affectedEmployee.department.departmentType);
                if (malfunctionDisabler.enablingDepartments.Count == 0)
                {
                    affectedEmployee.RemoveDisabler(malfunctionDisabler);
                    // Check if department has any other disablers
                    if (affectedEmployee.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Maintenance Employee {employee.employeeName} has successfully repaired the equipment malfunction for {affectedEmployee.employeeName}.");
                }
                else
                {
                    Debug.Log($"Maintenance Employee {employee.employeeName} found remaining disablers for {affectedEmployee.employeeName}.");
                }
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
    }
    public class Cleaning : Actions
    {
        public IEnumerator CleanChemicalSpill(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> chemicalSpillDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.chemicalSpill.name)).ToList();
            if (chemicalSpillDisablers.Count == 0)
            {
                Debug.Log($"Cleaning Employee {employee.employeeName} found no chemical spills in the warehouse.");
                yield break;
            }
            Debug.Log($"Cleaning Employee {employee.employeeName} is cleaning chemical spills.");
            // Logic to clean chemical spills in the cleaning department
            foreach (var department in chemicalSpillDisablers)
            {
                // Remove the chemical spill disabler from the department
                Disablers.Disabler chemicalSpillDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.chemicalSpill.name);
                if (chemicalSpillDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found no chemical spill disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (chemicalSpillDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found no enabling departments for the chemical spill disabler in the {department.departmentName} department.");
                    continue;
                }
                if (chemicalSpillDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} cannot clean chemical spills in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }
                chemicalSpillDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (chemicalSpillDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(chemicalSpillDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Cleaning Employee {employee.employeeName} has successfully cleaned the chemical spill in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator CleanFlood(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> waterFloodDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.flood.name)).ToList();
            if (waterFloodDisablers.Count == 0)
            {
                Debug.Log($"Cleaning Employee {employee.employeeName} found no water floods in the warehouse.");
                yield break;
            }
            Debug.Log($"Cleaning Employee {employee.employeeName} is cleaning water floods.");
            // Logic to clean water floods in the cleaning department
            foreach (var department in waterFloodDisablers)
            {
                // Remove the water flood disabler from the department
                Disablers.Disabler waterFloodDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.flood.name);
                if (waterFloodDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found no water flood disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (waterFloodDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found no enabling departments for the water flood disabler in the {department.departmentName} department.");
                    continue;
                }
                if (waterFloodDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} cannot clean water floods in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }
                waterFloodDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (waterFloodDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(waterFloodDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Cleaning Employee {employee.employeeName} has successfully cleaned the water flood in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator CleanHazardousMaterial(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> hazardousMaterialDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.hazardousMaterial.name)).ToList();
            if (hazardousMaterialDisablers.Count == 0)
            {
                Debug.Log($"Cleaning Employee {employee.employeeName} found no hazardous material spills in the warehouse.");
                yield break;
            }
            Debug.Log($"Cleaning Employee {employee.employeeName} is cleaning hazardous material spills.");
            // Logic to clean hazardous material spills in the cleaning department
            foreach (var department in hazardousMaterialDisablers)
            {
                // Remove the hazardous material disabler from the department
                Disablers.Disabler hazardousMaterialDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.hazardousMaterial.name);
                if (hazardousMaterialDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found no hazardous material disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (hazardousMaterialDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found no enabling departments for the hazardous material disabler in the {department.departmentName} department.");
                    continue;
                }
                if (hazardousMaterialDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} cannot clean hazardous material spills in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }
                hazardousMaterialDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (hazardousMaterialDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(hazardousMaterialDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Cleaning Employee {employee.employeeName} has successfully cleaned the hazardous material spill in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            yield break;
        }
        public IEnumerator CleanDirtyStation(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> dirtyStationDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Employee.dirtyStation.name)).ToList();
            if (dirtyStationDisablers.Count == 0)
            {
                Debug.Log($"Cleaning Employee {employee.employeeName} found no dirty stations in the warehouse.");
                yield break;
            }
            Debug.Log($"Cleaning Employee {employee.employeeName} is cleaning dirty stations.");
            // Logic to clean dirty stations in the cleaning department
            foreach (var department in dirtyStationDisablers)
            {
                // Remove the dirty station disabler from the department
                Disablers.Disabler dirtyStationDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Employee.dirtyStation.name);
                if (dirtyStationDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found no dirty station disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (dirtyStationDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found no enabling departments for the dirty station disabler in the {department.departmentName} department.");
                    continue;
                }
                if (dirtyStationDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} cannot clean dirty stations in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }
                dirtyStationDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (dirtyStationDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(dirtyStationDisabler);
                    // Check if department has any other disablers
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                    Debug.Log($"Cleaning Employee {employee.employeeName} has successfully cleaned the dirty station in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Cleaning Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
    }
    public class Security : Actions
    {
        public IEnumerator HandleSecurityBreach(Employee employee = null, Department affectedDepartment = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedDepartment == null)
            {
                throw new System.ArgumentNullException(nameof(affectedDepartment), "Affected department cannot be null.");
            }
            // Get disabler for security breach in department
            Disablers.Disabler securityBreachDisabler = affectedDepartment.disablers.FirstOrDefault(d => d.name == Disablers.Warehouse.securityBreach.name);
            if (securityBreachDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"Security Employee {employee.employeeName} found no security breach in the {affectedDepartment.departmentName} department.");
                yield break;
            }
            Debug.Log($"Security Employee {employee.employeeName} is handling a security breach in the {affectedDepartment.departmentName} department.");
            // Logic to handle security breach
            securityBreachDisabler.RemoveEnablingDepartment(employee.department.departmentType);
            if (securityBreachDisabler.enablingDepartments.Count == 0)
            {
                affectedDepartment.RemoveDisabler(securityBreachDisabler);
                // Check if department has any other disablers
                if (affectedDepartment.disablers.Count == 0)
                {
                    Globals.disabledDepartments.Remove(affectedDepartment); // Remove department from disabled departments list
                }
                Debug.Log($"Security Employee {employee.employeeName} has successfully resolved the security breach in the {affectedDepartment.departmentName} department.");
            }
            else
            {
                Debug.Log($"Security Employee {employee.employeeName} found remaining disablers in the {affectedDepartment.departmentName} department.");
            }
            yield break;
        }
        public IEnumerator HandleTheft(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            // Get disabler for theft in employee
            Disablers.Disabler theftDisabler = affectedEmployee.disablers.FirstOrDefault(d => d.name == Disablers.Employee.theft.name);
            if (theftDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"Security Employee {employee.employeeName} found no theft disabler for {affectedEmployee.employeeName}.");
                yield break;
            }
            Debug.Log($"Security Employee {employee.employeeName} is handling theft by {affectedEmployee.employeeName}.");
            theftDisabler.RemoveEnablingDepartment(employee.department.departmentType);
            if (theftDisabler.enablingDepartments.Count == 0)
            {
                affectedEmployee.RemoveDisabler(theftDisabler);
                Debug.Log($"Security Employee {employee.employeeName} has successfully resolved the theft issue involving {affectedEmployee.employeeName}.");
            }
            else
            {
                Debug.Log($"Security Employee {employee.employeeName} found remaining disablers for {affectedEmployee.employeeName}.");
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
    }
    public class Safety : Actions
    {
        public IEnumerator RespondToFire(Employee employee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            List<Department> fireAffectedDepartments = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Warehouse.fire.name)).ToList();
            if (fireAffectedDepartments.Count == 0)
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no fire incidents in the warehouse.");
                yield break;
            }
            Debug.Log($"Safety Employee {employee.employeeName} is responding to a fire affecting the entire warehouse.");
            foreach (var department in fireAffectedDepartments)
            {
                Disablers.Disabler fireDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Warehouse.fire.name);
                if (fireDisabler.Equals(default(Disablers.Disabler)))
                {
                    Debug.Log($"Safety Employee {employee.employeeName} found no fire disabler in the {department.departmentName} department.");
                    continue;
                }
                // Check if this department is first in the list of enabling departments
                if (fireDisabler.enablingDepartments.Count == 0)
                {
                    Debug.Log($"Safety Employee {employee.employeeName} found no enabling departments for the fire disabler in the {department.departmentName} department.");
                    continue;
                }
                // If this department is first in the list, remove it from the enabling departments
                if (fireDisabler.enablingDepartments[0] != employee.department.departmentType)
                {
                    Debug.Log($"Safety Employee {employee.employeeName} cannot respond to fire incidents in the {department.departmentName} department. This department is not first in the list of enabling departments.");
                    continue;
                }
                // Remove the fire disabler from the department
                fireDisabler.RemoveEnablingDepartment(employee.department.departmentType);
                if (fireDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(fireDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department);
                    }
                    Debug.Log($"Safety Employee {employee.employeeName} has successfully resolved the fire in the {department.departmentName} department.");
                }
                else
                {
                    Debug.Log($"Safety Employee {employee.employeeName} found remaining disablers in the {department.departmentName} department.");
                }
                if (fireDisabler.enablingDepartments.Count != 0)
                {
                    if (fireDisabler.enablingDepartments.Contains(employee.department.departmentType))
                    {
                        ActionRequest actionRequest = new ActionRequest(RespondToFire(employee), null, department);
                        employee.department.AddActionRequest(actionRequest);
                    }
                }
            }
            yield break;
        }
        public IEnumerator RespondToChemicalSpill(Employee employee = null, Department affectedDepartment = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedDepartment == null)
            {
                throw new System.ArgumentNullException(nameof(affectedDepartment), "Affected department cannot be null.");
            }
            Disablers.Disabler chemicalSpillDisabler = affectedDepartment.disablers.FirstOrDefault(d => d.name == Disablers.Department.chemicalSpill.name);
            if (chemicalSpillDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no chemical spill disabler in the {affectedDepartment.departmentName} department.");
                yield break;
            }
            if (chemicalSpillDisabler.enablingDepartments.Count == 0)
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no enabling departments for the chemical spill disabler in the {affectedDepartment.departmentName} department.");
                yield break;
            }
            if (chemicalSpillDisabler.enablingDepartments[0] != employee.department.departmentType)
            {
                Debug.Log($"Safety Employee {employee.employeeName} cannot respond to chemical spill incidents in the {affectedDepartment.departmentName} department. This department is not first in the list of enabling departments.");
                yield break;
            }
            chemicalSpillDisabler.RemoveEnablingDepartment(employee.department.departmentType);
            if (chemicalSpillDisabler.enablingDepartments.Count == 0)
            {
                affectedDepartment.RemoveDisabler(chemicalSpillDisabler);
                if (affectedDepartment.disablers.Count == 0)
                {
                    Globals.disabledDepartments.Remove(affectedDepartment);
                }
                Debug.Log($"Safety Employee {employee.employeeName} has successfully resolved the chemical spill in the {affectedDepartment.departmentName} department.");
            }
            else
            {
                Debug.Log($"Safety Employee {employee.employeeName} found remaining disablers in the {affectedDepartment.departmentName} department.");
            }
            if (chemicalSpillDisabler.enablingDepartments.Count != 0)
            {
                if (chemicalSpillDisabler.enablingDepartments.Contains(employee.department.departmentType))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToChemicalSpill(employee, affectedDepartment), null, affectedDepartment);
                    employee.department.AddActionRequest(actionRequest);
                }
            }
            yield break;
        }
        public IEnumerator RespondToFlood(Employee employee = null, Department affectedDepartment = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedDepartment == null)
            {
                throw new System.ArgumentNullException(nameof(affectedDepartment), "Affected department cannot be null.");
            }
            Disablers.Disabler floodDisabler = affectedDepartment.disablers.FirstOrDefault(d => d.name == Disablers.Department.flood.name);
            if (floodDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no flood disabler in the {affectedDepartment.departmentName} department.");
                yield break;
            }
            if (floodDisabler.enablingDepartments.Count == 0)
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no enabling departments for the flood disabler in the {affectedDepartment.departmentName} department.");
                yield break;
            }
            if (floodDisabler.enablingDepartments[0] != employee.department.departmentType)
            {
                Debug.Log($"Safety Employee {employee.employeeName} cannot respond to flood incidents in the {affectedDepartment.departmentName} department. This department is not first in the list of enabling departments.");
                yield break;
            }
            floodDisabler.RemoveEnablingDepartment(employee.department.departmentType);
            if (floodDisabler.enablingDepartments.Count == 0)
            {
                affectedDepartment.RemoveDisabler(floodDisabler);
                if (affectedDepartment.disablers.Count == 0)
                {
                    Globals.disabledDepartments.Remove(affectedDepartment);
                }
                Debug.Log($"Safety Employee {employee.employeeName} has successfully resolved the flood in the {affectedDepartment.departmentName} department.");
            }
            else
            {
                Debug.Log($"Safety Employee {employee.employeeName} found remaining disablers in the {affectedDepartment.departmentName} department.");
            }
            if (floodDisabler.enablingDepartments.Count != 0)
            {
                if (floodDisabler.enablingDepartments.Contains(employee.department.departmentType))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToFlood(employee, affectedDepartment), null, affectedDepartment);
                    employee.department.AddActionRequest(actionRequest);
                }
            }
            yield break;
        }
        public IEnumerator RespondToAccident(Employee employee = null, Department affectedDepartment = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedDepartment == null)
            {
                throw new System.ArgumentNullException(nameof(affectedDepartment), "Affected department cannot be null.");
            }
            Disablers.Disabler accidentDisabler = affectedDepartment.disablers.FirstOrDefault(d => d.name == Disablers.Department.accident.name);
            if (accidentDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no accident disabler in the {affectedDepartment.departmentName} department.");
                yield break;
            }
            if (accidentDisabler.enablingDepartments.Count == 0)
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no enabling departments for the accident disabler in the {affectedDepartment.departmentName} department.");
                yield break;
            }
            if (accidentDisabler.enablingDepartments[0] != employee.department.departmentType)
            {
                Debug.Log($"Safety Employee {employee.employeeName} cannot respond to accident incidents in the {affectedDepartment.departmentName} department. This department is not first in the list of enabling departments.");
                yield break;
            }
            accidentDisabler.RemoveEnablingDepartment(employee.department.departmentType);
            if (accidentDisabler.enablingDepartments.Count == 0)
            {
                affectedDepartment.RemoveDisabler(accidentDisabler);
                if (affectedDepartment.disablers.Count == 0)
                {
                    Globals.disabledDepartments.Remove(affectedDepartment);
                }
                Debug.Log($"Safety Employee {employee.employeeName} has successfully resolved the accident in the {affectedDepartment.departmentName} department.");
            }
            else
            {
                Debug.Log($"Safety Employee {employee.employeeName} found remaining disablers in the {affectedDepartment.departmentName} department.");
            }
            if (accidentDisabler.enablingDepartments.Count != 0)
            {
                if (accidentDisabler.enablingDepartments.Contains(employee.department.departmentType))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToAccident(employee, affectedDepartment), null, affectedDepartment);
                    employee.department.AddActionRequest(actionRequest);
                }
            }
            yield break;
        }
        public IEnumerator RespondToHazardousMaterial(Employee employee = null, Department affectedDepartment = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedDepartment == null)
            {
                throw new System.ArgumentNullException(nameof(affectedDepartment), "Affected department cannot be null.");
            }
            Disablers.Disabler hazardousMaterialDisabler = affectedDepartment.disablers.FirstOrDefault(d => d.name == Disablers.Department.hazardousMaterial.name);
            if (hazardousMaterialDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no hazardous material disabler in the {affectedDepartment.departmentName} department.");
                yield break;
            }
            if (hazardousMaterialDisabler.enablingDepartments.Count == 0)
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no enabling departments for the hazardous material disabler in the {affectedDepartment.departmentName} department.");
                yield break;
            }
            if (hazardousMaterialDisabler.enablingDepartments[0] != employee.department.departmentType)
            {
                Debug.Log($"Safety Employee {employee.employeeName} cannot respond to hazardous material incidents in the {affectedDepartment.departmentName} department. This department is not first in the list of enabling departments.");
                yield break;
            }
            hazardousMaterialDisabler.RemoveEnablingDepartment(employee.department.departmentType);
            if (hazardousMaterialDisabler.enablingDepartments.Count == 0)
            {
                affectedDepartment.RemoveDisabler(hazardousMaterialDisabler);
                if (affectedDepartment.disablers.Count == 0)
                {
                    Globals.disabledDepartments.Remove(affectedDepartment);
                }
                Debug.Log($"Safety Employee {employee.employeeName} has successfully resolved the hazardous material incident in the {affectedDepartment.departmentName} department.");
            }
            else
            {
                Debug.Log($"Safety Employee {employee.employeeName} found remaining disablers in the {affectedDepartment.departmentName} department.");
            }
            if (hazardousMaterialDisabler.enablingDepartments.Count != 0)
            {
                if (hazardousMaterialDisabler.enablingDepartments.Contains(employee.department.departmentType))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToHazardousMaterial(employee, affectedDepartment), null, affectedDepartment);
                    employee.department.AddActionRequest(actionRequest);
                }
            }
            yield break;
        }
        public IEnumerator RespondToInjury(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            Disablers.Disabler injuryDisabler = affectedEmployee.disablers.FirstOrDefault(d => d.name == Disablers.Employee.injury.name);
            if (injuryDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no injury disabler for {affectedEmployee.employeeName}.");
                yield break;
            }
            Debug.Log($"Safety Employee {employee.employeeName} is responding to an injury involving {affectedEmployee.employeeName}.");
            injuryDisabler.RemoveEnablingDepartment(employee.department.departmentType);
            if (injuryDisabler.enablingDepartments.Count == 0)
            {
                affectedEmployee.RemoveDisabler(injuryDisabler);
                Debug.Log($"Safety Employee {employee.employeeName} has successfully resolved the injury issue involving {affectedEmployee.employeeName}.");
            }
            else
            {
                Debug.Log($"Safety Employee {employee.employeeName} found remaining disablers for {affectedEmployee.employeeName}.");
            }
            if (injuryDisabler.enablingDepartments.Count != 0)
            {
                if (injuryDisabler.enablingDepartments.Contains(employee.department.departmentType))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToInjury(employee, affectedEmployee), affectedEmployee);
                    employee.department.AddActionRequest(actionRequest);
                }
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
        public IEnumerator RespondToUnsafeStation(Employee employee = null, Employee affectedEmployee = null)
        {
            if (employee == null)
            {
                throw new System.ArgumentNullException(nameof(employee), "Employee cannot be null.");
            }
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            Disablers.Disabler unsafeStationDisabler = affectedEmployee.disablers.FirstOrDefault(d => d.name == Disablers.Employee.stationUnsafe.name);
            if (unsafeStationDisabler.Equals(default(Disablers.Disabler)))
            {
                Debug.Log($"Safety Employee {employee.employeeName} found no unsafe station disabler for {affectedEmployee.employeeName}.");
                yield break;
            }
            Debug.Log($"Safety Employee {employee.employeeName} is responding to an unsafe station involving {affectedEmployee.employeeName}.");
            unsafeStationDisabler.RemoveEnablingDepartment(employee.department.departmentType);
            if (unsafeStationDisabler.enablingDepartments.Count == 0)
            {
                affectedEmployee.RemoveDisabler(unsafeStationDisabler);
                Debug.Log($"Safety Employee {employee.employeeName} has successfully resolved the unsafe station issue involving {affectedEmployee.employeeName}.");
            }
            else
            {
                Debug.Log($"Safety Employee {employee.employeeName} found remaining disablers for {affectedEmployee.employeeName}.");
            }
            if (unsafeStationDisabler.enablingDepartments.Count != 0)
            {
                if (unsafeStationDisabler.enablingDepartments.Contains(employee.department.departmentType))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToUnsafeStation(employee, affectedEmployee), affectedEmployee);
                    employee.department.AddActionRequest(actionRequest);
                }
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
    }
    // Game system
    public class GameSystem : Actions
    {
        private List<string> firstNames = new List<string>
        {
            "Liam", "Olivia", "Noah", "Emma", "Aiden", "Ava", "Lucas", "Sophia", "Mason", "Isabella",
            "Ethan", "Mia", "Logan", "Amelia", "James", "Harper", "Elijah", "Evelyn", "Benjamin", "Abigail",
            "Jack", "Ella", "Henry", "Scarlett", "Sebastian", "Grace", "Alexander", "Chloe", "William", "Victoria",
            "Daniel", "Aria", "Matthew", "Lily", "Jackson", "Hannah", "Michael", "Zoe", "Owen", "Stella",
            "Gabriel", "Nora", "Carter", "Leah", "Jayden", "Hazel", "Wyatt", "Aurora", "Luke", "Penelope"
        };

        private List<string> lastNames = new List<string>
        {
            "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
            "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
            "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
            "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
            "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts"
        };

        public IEnumerator CreateNewHire()
        {
            // Logic to create a new hire
            Debug.Log("Creating a new hire...");
            // Generate a random first name and last name
            string firstName = firstNames[Random.Range(0, firstNames.Count)];
            string lastName = lastNames[Random.Range(0, lastNames.Count)];
            // Generate a random salary between 250 and 1000
            int salary = Random.Range(250, 1001);
            // Generate random stats based on salary percentage 
            // [ 0% - 50% ] = 1 - 2.5, [ 51% - 70% ] = 2.5 - 4, [ 71% - 100% ] = 4.5 - 6
            // Pick 3 random stats to recieve a bonus
            // Stat order speed, efficiency, stamina, strength, focus, experience
            float[] statValues = new float[6];
            int minStatWholeValue;
            int maxStatWholeValue;
            float salaryPercentage = (float)salary / 1000f; // Assuming max salary is 1000
            if (salaryPercentage <= 0.5f)
            {
                minStatWholeValue = 1;
                maxStatWholeValue = 2;
            }
            else if (salaryPercentage <= 0.7f)
            {
                minStatWholeValue = 2;
                maxStatWholeValue = 4;
            }
            else
            {
                minStatWholeValue = 4;
                maxStatWholeValue = 6;
            }
            for (int i = 0; i < 3;)
            {
                bool isStatWholeValue = Random.Range(0, 2) == 0; // Randomly decide if the stat value should be a whole number or not
                int randomNum = Random.Range(0, 6); // Randomly pick a stat index
                if (statValues[randomNum] != 0) // Check if the stat has already been assigned a value
                {
                    continue; // If it has, skip to the next iteration
                }
                float randomStatValue = Random.Range(minStatWholeValue, maxStatWholeValue + 1); // Generate a random stat value between min and max
                if (!isStatWholeValue)
                {
                    randomStatValue += 0.5f;
                }
                statValues[randomNum] = Mathf.Min(6, randomStatValue);
                statValues[randomNum] = Mathf.Max(1, randomStatValue); // Ensure the stat value is between 1 and 6
                i++;
            }
            // Generate trait count
            int traitCount = Random.Range(1, 4); // Randomly pick a number of traits between 1 and 3
            List<TraitValues> traits = new List<TraitValues>();
            for (int i = 0; i < traitCount; i++)
            {
                // Randomly pick a trait from the TraitValues class
                traits.Add(GetRandomTrait());
            }
            for (int i = 0; i < 6; i++)
            {
                if (statValues[i] == 0)
                {
                    statValues[i] = 1;
                }
            }
            // Create a new employee object
            Employee newEmployee = gameObject.AddComponent<Employee>();
            newEmployee.employeeName = $"{firstName} {lastName}";
            newEmployee.salary = salary;
            newEmployee.speed = statValues[0];
            newEmployee.efficiency = statValues[1];
            newEmployee.stamina = statValues[2];
            newEmployee.strength = statValues[3];
            newEmployee.focus = statValues[4];
            newEmployee.experience = statValues[5];
            newEmployee.traits = traits;
            newEmployee.actionState = ActionState.State.Idle; // Set initial action state to Idle
            Debug.Log(newEmployee.employeeName + " has been created with the following stats:");
            Debug.Log($"Speed: {newEmployee.speed}, Efficiency: {newEmployee.efficiency}, Stamina: {newEmployee.stamina}, Strength: {newEmployee.strength}, Focus: {newEmployee.focus}, Experience: {newEmployee.experience}");

            // Add the new employee to the warehouse new hire list
            Globals.newHires.Add(newEmployee);
            Debug.Log("New hire created successfully.");
            yield break;
        }
        public IEnumerator StartRandomDisabler()
        {
            // Logic to start a random disabler
            Debug.Log("Starting a random disabler...");
            // Choose randomly the type of disabler [types: Warehouse, Employee, Department]
            int randomDisablerType = Random.Range(0, 3); // 0: Warehouse, 1: Employee, 2: Department
            HR hrInstance = new HR();
            IT itInstance = new IT();
            Maintenance maintenanceInstance = new Maintenance();
            Cleaning cleaningInstance = new Cleaning();
            Security securityInstance = new Security();
            Safety safetyInstance = new Safety();
            // Choose a random disabler from the warehouse disablers
            Disablers.Disabler randomDisabler;
            if (randomDisablerType == 0)
            {
                randomDisabler = Disablers.Warehouse.allDisablers[Random.Range(0, Disablers.Warehouse.allDisablers.Count)];
            }
            else if (randomDisablerType == 1)
            {
                randomDisabler = Disablers.Employee.allDisablers[Random.Range(0, Disablers.Employee.allDisablers.Count)];
            }
            else
            {
                randomDisabler = Disablers.Department.allDisablers[Random.Range(0, Disablers.Department.allDisablers.Count)];
            }
            // Get randomDisabler first department from enabling departments
            DepartmentTypes.Type firstDepartmentType = randomDisabler.enablingDepartments[0];
            // Create list of departments that match the type
            List<Department> matchingDepartments = Globals.departments.Where(d => d.departmentType == firstDepartmentType).ToList();
            // Sort the list of departments by newActionRequest count smallest to largest
            matchingDepartments = matchingDepartments.OrderBy(d => d.newActionRequests.Count).ToList();
            // Add the disabler to the first department in the list
            Department resolvingDepartment = null;
            if (matchingDepartments.Count > 0)
            {
                resolvingDepartment = matchingDepartments[0];
            }
            if (randomDisablerType == 0)
            {
                // Enable the disabler in the warehouse
                foreach (var department in Globals.departments)
                {
                    department.AddDisabler(randomDisabler);
                    // Add the disabled department to the disabled departments list
                    if (!Globals.disabledDepartments.Contains(department))
                    {
                        Globals.disabledDepartments.Add(department);
                    }
                    // Decide the IEnumerator action function for the request
                    ActionRequest actionRequest;
                    Employee targetEmployee = resolvingDepartment.employees.OrderBy(e => e.actionRequests.Count).FirstOrDefault();
                    switch (randomDisabler.name)
                    {
                        // Warehouse
                        case "Fire":
                            actionRequest = new ActionRequest(safetyInstance.RespondToFire(targetEmployee), null, department);
                            resolvingDepartment.AddActionRequest(actionRequest);
                            break;
                        case "Power Outage":
                            actionRequest = new ActionRequest(maintenanceInstance.RepairPowerOutage(targetEmployee), null, department);
                            resolvingDepartment.AddActionRequest(actionRequest);
                            break;
                        case "Security Breach":
                            actionRequest = new ActionRequest(securityInstance.HandleSecurityBreach(targetEmployee), null, department);
                            resolvingDepartment.AddActionRequest(actionRequest);
                            break;
                        case "Network Failure":
                            actionRequest = new ActionRequest(itInstance.FixNetworkFailure(targetEmployee), null, department);
                            resolvingDepartment.AddActionRequest(actionRequest);
                            break;
                        default:
                            Debug.Log($"No action function defined for disabler {randomDisabler.name}.");
                            break;
                    }
                }
                // create notification
                notificationController.CreateNotification($"Disabler {randomDisabler.name} has been enabled in the warehouse.");
                Debug.Log($"Disabler {randomDisabler.name} has been enabled in the warehouse.");
            }
            else if (randomDisablerType == 1)
            {
                // Choose a random employee from the warehouse employees
                Employee randomEmployee = Globals.warehouseEmployees[Random.Range(0, Globals.warehouseEmployees.Count)];
                // Enable the disabler in the employee
                randomEmployee.AddDisabler(randomDisabler);
                // create action request for new disabler on employee
                ActionRequest actionRequest;
                Employee targetEmployee = resolvingDepartment.employees.OrderBy(e => e.actionRequests.Count).FirstOrDefault();
                switch (randomDisabler.name)
                {
                    case "Injury":
                        actionRequest = new ActionRequest(safetyInstance.RespondToInjury(targetEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Employee Misconduct":
                        actionRequest = new ActionRequest(hrInstance.DocumentMisconduct(targetEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Computer Failure":
                        actionRequest = new ActionRequest(itInstance.FixComputerFailure(targetEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Equipment Malfunction":
                        actionRequest = new ActionRequest(maintenanceInstance.RepairEquipmentMalfunction(targetEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Theft":
                        actionRequest = new ActionRequest(securityInstance.HandleTheft(targetEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Dirty Station":
                        actionRequest = new ActionRequest(cleaningInstance.CleanDirtyStation(targetEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Unsafe Station":
                        actionRequest = new ActionRequest(safetyInstance.RespondToUnsafeStation(targetEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    default:
                        Debug.Log($"No action function defined for disabler {randomDisabler.name}.");
                        break;
                }
                // Create notification
                notificationController.CreateNotification($"Disabler {randomDisabler.name} has been enabled for employee {randomEmployee.employeeName}.");
                Debug.Log($"Disabler {randomDisabler.name} has been enabled for employee {randomEmployee.employeeName}.");
            }
            else if (randomDisablerType == 2)
            {
                // Choose a random department from the warehouse departments
                Department randomDepartment = Globals.departments[Random.Range(0, Globals.departments.Count)];
                // Enable the disabler in the department
                randomDepartment.AddDisabler(randomDisabler);
                // create action request for new disabler on employee
                ActionRequest actionRequest;
                Employee targetEmployee = resolvingDepartment.employees.OrderBy(e => e.actionRequests.Count).FirstOrDefault();
                switch (randomDisabler.name)
                {
                    case "Device Failure":
                        actionRequest = new ActionRequest(itInstance.FixDeviceFailure(targetEmployee), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Server Failure":
                        actionRequest = new ActionRequest(itInstance.FixServerFailure(targetEmployee), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Mechanical Failure":
                        actionRequest = new ActionRequest(maintenanceInstance.RepairMechanicalFailure(targetEmployee), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Electrical Failure":
                        actionRequest = new ActionRequest(maintenanceInstance.RepairElectricalFailure(targetEmployee), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Flood":
                        actionRequest = new ActionRequest(safetyInstance.RespondToFlood(targetEmployee), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Chemical Spill":
                        actionRequest = new ActionRequest(safetyInstance.RespondToChemicalSpill(targetEmployee), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Equipment Failure":
                        actionRequest = new ActionRequest(maintenanceInstance.RepairEquipmentFailure(targetEmployee), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Accident":
                        actionRequest = new ActionRequest(safetyInstance.RespondToAccident(targetEmployee), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Hazardous Material":
                        actionRequest = new ActionRequest(safetyInstance.RespondToHazardousMaterial(targetEmployee), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    default:
                        Debug.Log($"No action function defined for disabler {randomDisabler.name}.");
                        break;
                }
                // Create notification
                notificationController.CreateNotification($"Disabler {randomDisabler.name} has been enabled in the {randomDepartment.departmentName} department.");
                Debug.Log($"Disabler {randomDisabler.name} has been enabled in the {randomDepartment.departmentName} department.");
                yield break;
            }
        }
    }

    struct DepartmentIndex
    {
        public DepartmentTypes.Type departmentType;
        public int count;
    }

    public static TraitValues GetRandomTrait()
    {
        // Get all public static readonly fields of TraitValues type
        var traits = typeof(EmployeeTraits)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(TraitValues))
            .ToArray();

        // Create Random instance

        // Get random trait field
        var randomField = traits[Random.Range(0, traits.Length)];

        // Return the TraitValues instance
        return (TraitValues)randomField.GetValue(null);
    }

}

