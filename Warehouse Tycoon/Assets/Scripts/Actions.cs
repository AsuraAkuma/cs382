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
        public IEnumerator RaiseEmployeeLevel(Employee affectedEmployee = null)
        {
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
            yield break;
        }
        public IEnumerator UpgradeEmployeeStat(Employee affectedEmployee = null, StatTypes.Type statType = StatTypes.Type.Efficiency)
        {
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
            yield break;
        }
        public IEnumerator AssignDepartment(Employee affectedEmployee = null, Department newDepartment = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            if (newDepartment == null)
            {
                throw new System.ArgumentNullException(newDepartment.departmentName, "Department cannot be null.");
            }
            newDepartment.UpdateEmployeeUIList(); // Update the UI of the new department
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
        public IEnumerator FireEmployee(Employee affectedEmployee = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            affectedEmployee.department.RemoveEmployee(affectedEmployee);
            affectedEmployee.department.UpdateEmployeeUIList(); // Update the UI of the current department
            affectedEmployee.department = null; // Remove employee from the department
            Globals.warehouseEmployees.Remove(affectedEmployee); // Remove employee from the warehouse
            Globals.playerMoney += affectedEmployee.cost / 2; // Refund the cost of the employee
            Destroy(affectedEmployee); // Destroy the employee object
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
        public IEnumerator HireEmployee(Employee affectedEmployee = null)
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
                yield break;
            }
            if (affectedEmployee.cost > Globals.playerMoney)
            {
                yield break;
            }
            // Check if the new hire can be assigned to a department
            if (departmentsInNeed.Count == 0)
            {
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
            Globals.warehouseEmployees.Add(affectedEmployee);
            Globals.newHires.Remove(affectedEmployee);
            Globals.playerMoney -= affectedEmployee.cost; // Deduct the cost of hiring from player's money
            gameController.UpdateEmployeeUIList(); // Update the game controller's UI
            yield break;
        }
        public IEnumerator PromoteEmployee(Employee affectedEmployee = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "affectedEmployee cannot be null.");
            }
            // Check if the employee is already a manager class
            if (affectedEmployee is HRManager || affectedEmployee is ITManager || affectedEmployee is OperationsManager || affectedEmployee is InboundManager || affectedEmployee is SortingManager || affectedEmployee is RepackingManager || affectedEmployee is PalletizingManager || affectedEmployee is WaterSpiderManager || affectedEmployee is FluidLoadManager || affectedEmployee is QualityControlManager || affectedEmployee is OutboundManager || affectedEmployee is MaintenanceManager || affectedEmployee is RoboticsManager || affectedEmployee is SafetyManager || affectedEmployee is CleaningManager || affectedEmployee is SecurityManager || affectedEmployee is LearningManager || affectedEmployee is RecruitingManager)
            {
                yield break;
            }
            if (affectedEmployee.level == Globals.employeeMaxLevel)
            {
                affectedEmployee.level = 1;
                // Create new employee object with the same stats as the affectedEmployee
                Employee newManager;
                switch (affectedEmployee.department.departmentType)
                {
                    case DepartmentTypes.Type.HR:
                        newManager = gameController.gameObject.AddComponent<HRManager>();
                        break;
                    case DepartmentTypes.Type.IT:
                        newManager = gameController.gameObject.AddComponent<ITManager>();
                        break;
                    case DepartmentTypes.Type.Operations:
                        newManager = gameController.gameObject.AddComponent<OperationsManager>();
                        break;
                    case DepartmentTypes.Type.Inbound:
                        newManager = gameController.gameObject.AddComponent<InboundManager>();
                        break;
                    case DepartmentTypes.Type.Sorting:
                        newManager = gameController.gameObject.AddComponent<SortingManager>();
                        break;
                    case DepartmentTypes.Type.Repacking:
                        newManager = gameController.gameObject.AddComponent<RepackingManager>();
                        break;
                    case DepartmentTypes.Type.Palletizing:
                        newManager = gameController.gameObject.AddComponent<PalletizingManager>();
                        break;
                    case DepartmentTypes.Type.WaterSpidering:
                        newManager = gameController.gameObject.AddComponent<WaterSpiderManager>();
                        break;
                    case DepartmentTypes.Type.FluidLoad:
                        newManager = gameController.gameObject.AddComponent<FluidLoadManager>();
                        break;
                    case DepartmentTypes.Type.QualityControl:
                        newManager = gameController.gameObject.AddComponent<QualityControlManager>();
                        break;
                    case DepartmentTypes.Type.Outbound:
                        newManager = gameController.gameObject.AddComponent<OutboundManager>();
                        break;
                    case DepartmentTypes.Type.Maintenance:
                        newManager = gameController.gameObject.AddComponent<MaintenanceManager>();
                        break;
                    case DepartmentTypes.Type.Robotics:
                        newManager = gameController.gameObject.AddComponent<RoboticsManager>();
                        break;
                    case DepartmentTypes.Type.Safety:
                        newManager = gameController.gameObject.AddComponent<SafetyManager>();
                        break;
                    case DepartmentTypes.Type.Cleaning:
                        newManager = gameController.gameObject.AddComponent<CleaningManager>();
                        break;
                    case DepartmentTypes.Type.Security:
                        newManager = gameController.gameObject.AddComponent<SecurityManager>();
                        break;
                    case DepartmentTypes.Type.Learning:
                        newManager = gameController.gameObject.AddComponent<LearningManager>();
                        break;
                    case DepartmentTypes.Type.Recruiting:
                        newManager = gameController.gameObject.AddComponent<RecruitingManager>();
                        break;
                    default:
                        throw new System.ArgumentOutOfRangeException(nameof(affectedEmployee.department.departmentType), "Invalid department type for promotion.");
                }
                // Remove the affectedEmployee from the current department
                affectedEmployee.department.RemoveEmployee(affectedEmployee);
                // Copy stats from the affectedEmployee to the new manager
                newManager.Paste(affectedEmployee);
                // Add the new manager to the department
                newManager.department.AddEmployee(newManager);
                // Remove the affectedEmployee from the warehouse employees list
                Globals.warehouseEmployees.Remove(affectedEmployee);
                Destroy(affectedEmployee); // Destroy the old employee data
                // Add the new manager to the warehouse employees list
                Globals.warehouseEmployees.Add(newManager);
                newManager.department.UpdateEmployeeUIList(); // Update the UI of the new manager's department
            }
            else
            {
                throw new System.InvalidOperationException("Employee is not max level.");
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
        public IEnumerator ResetInfractions(Employee affectedEmployee = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            // Clear infractions for the affected employee
            affectedEmployee.infractions = 0; // Reset the infractions count
            affectedEmployee.department.UpdateEmployeeUIList(); // Update the UI of the affected employee's department
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
        // Disabler actions
        public IEnumerator DocumentInjury(Employee affectedEmployee = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Injured employee cannot be null.");
            }
            // Get disabler for injury in department
            Disablers.Disabler injuryDisabler = affectedEmployee.department.disablers.FirstOrDefault(d => d.name == Disablers.Employee.injury.name);
            if (injuryDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
        public IEnumerator DocumentMisconduct(Employee affectedEmployee = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Misconduct employee cannot be null.");
            }
            // Get disabler for employee misconduct in department
            Disablers.Disabler misconductDisabler = affectedEmployee.department.disablers.FirstOrDefault(d => d.name == Disablers.Employee.employeeMisconduct.name);
            if (misconductDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
    }
    public class IT : Actions
    {
        public IEnumerator FixNetworkFailure()
        {
            List<Department> networkDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Warehouse.networkFailure.name)).ToList();
            if (networkDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in networkDisablers)
            {
                Disablers.Disabler networkDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Warehouse.networkFailure.name);
                if (networkDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (networkDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (networkDisabler.enablingDepartments[0] != DepartmentTypes.Type.IT)
                {
                    continue;
                }
                networkDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.IT);
                if (networkDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(networkDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator FixDeviceFailure()
        {
            List<Department> deviceDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.deviceFailure.name)).ToList();
            if (deviceDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in deviceDisablers)
            {
                Disablers.Disabler deviceDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.deviceFailure.name);
                if (deviceDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (deviceDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (deviceDisabler.enablingDepartments[0] != DepartmentTypes.Type.IT)
                {
                    continue;
                }
                deviceDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.IT);
                if (deviceDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(deviceDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator FixServerFailure()
        {
            List<Department> serverDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.serverFailure.name)).ToList();
            if (serverDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in serverDisablers)
            {
                Disablers.Disabler serverDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.serverFailure.name);
                if (serverDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (serverDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (serverDisabler.enablingDepartments[0] != DepartmentTypes.Type.IT)
                {
                    continue;
                }
                serverDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.IT);
                if (serverDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(serverDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator FixComputerFailure(Employee affectedEmployee = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            Disablers.Disabler computerDisabler = affectedEmployee.disablers.FirstOrDefault(d => d.name == Disablers.Employee.computerFailure.name);
            if (computerDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            computerDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.IT);
            if (computerDisabler.enablingDepartments.Count == 0)
            {
                affectedEmployee.RemoveDisabler(computerDisabler);
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
    }
    public class Maintenance : Actions
    {
        public IEnumerator RepairFireDamages()
        {
            List<Department> fireDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Warehouse.fire.name)).ToList();
            if (fireDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in fireDisablers)
            {
                Disablers.Disabler fireDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Warehouse.fire.name);
                if (fireDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (fireDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (fireDisabler.enablingDepartments[0] != DepartmentTypes.Type.Maintenance)
                {
                    continue;
                }

                fireDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Maintenance);
                if (fireDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(fireDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator RepairPowerOutage()
        {
            List<Department> powerDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Warehouse.powerOutage.name)).ToList();
            if (powerDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in powerDisablers)
            {
                Disablers.Disabler powerDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Warehouse.powerOutage.name);
                if (powerDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (powerDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (powerDisabler.enablingDepartments[0] != DepartmentTypes.Type.Maintenance)
                {
                    continue;
                }

                powerDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Maintenance);
                if (powerDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(powerDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator RepairMechanicalFailure()
        {
            List<Department> powerDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Warehouse.powerOutage.name)).ToList();
            if (powerDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in powerDisablers)
            {
                Disablers.Disabler powerDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.mechanicalFailure.name);
                if (powerDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (powerDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (powerDisabler.enablingDepartments[0] != DepartmentTypes.Type.Maintenance)
                {
                    continue;
                }

                powerDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Maintenance);
                if (powerDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(powerDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator RepairElectricalFailure()
        {
            List<Department> electricalDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.electricalFailure.name)).ToList();
            if (electricalDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in electricalDisablers)
            {
                Disablers.Disabler electricalDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.electricalFailure.name);
                if (electricalDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (electricalDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (electricalDisabler.enablingDepartments[0] != DepartmentTypes.Type.Maintenance)
                {
                    continue;
                }

                electricalDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Maintenance);
                if (electricalDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(electricalDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator RepairFloodDamage()
        {
            List<Department> floodDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.flood.name)).ToList();
            if (floodDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in floodDisablers)
            {
                Disablers.Disabler floodDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.flood.name);
                if (floodDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (floodDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (floodDisabler.enablingDepartments[0] != DepartmentTypes.Type.Maintenance)
                {
                    continue;
                }

                floodDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Maintenance);
                if (floodDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(floodDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator RepairEquipmentFailure()
        {
            List<Department> equipmentDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.equipmentFailure.name)).ToList();
            if (equipmentDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in equipmentDisablers)
            {
                Disablers.Disabler equipmentDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.equipmentFailure.name);
                if (equipmentDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (equipmentDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (equipmentDisabler.enablingDepartments[0] != DepartmentTypes.Type.Maintenance)
                {
                    continue;
                }

                equipmentDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Maintenance);
                if (equipmentDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(equipmentDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator RepairEquipmentMalfunction(Employee affectedEmployee = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            List<Department> malfunctionDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Employee.equipmentMalfunction.name)).ToList();
            if (malfunctionDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in malfunctionDisablers)
            {
                Disablers.Disabler malfunctionDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Employee.equipmentMalfunction.name);
                if (malfunctionDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (malfunctionDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (malfunctionDisabler.enablingDepartments[0] != DepartmentTypes.Type.Maintenance)
                {
                    continue;
                }

                malfunctionDisabler.RemoveEnablingDepartment(affectedEmployee.department.departmentType);
                if (malfunctionDisabler.enablingDepartments.Count == 0)
                {
                    affectedEmployee.RemoveDisabler(malfunctionDisabler);
                    if (affectedEmployee.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
    }
    public class Cleaning : Actions
    {
        public IEnumerator CleanChemicalSpill()
        {
            List<Department> chemicalSpillDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.chemicalSpill.name)).ToList();
            if (chemicalSpillDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in chemicalSpillDisablers)
            {
                Disablers.Disabler chemicalSpillDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.chemicalSpill.name);
                if (chemicalSpillDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (chemicalSpillDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (chemicalSpillDisabler.enablingDepartments[0] != DepartmentTypes.Type.Cleaning)
                {
                    continue;
                }
                chemicalSpillDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Cleaning);
                if (chemicalSpillDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(chemicalSpillDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator CleanFlood()
        {
            List<Department> waterFloodDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.flood.name)).ToList();
            if (waterFloodDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in waterFloodDisablers)
            {
                Disablers.Disabler waterFloodDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.flood.name);
                if (waterFloodDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (waterFloodDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (waterFloodDisabler.enablingDepartments[0] != DepartmentTypes.Type.Cleaning)
                {
                    continue;
                }
                waterFloodDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Cleaning);
                if (waterFloodDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(waterFloodDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator CleanHazardousMaterial()
        {
            List<Department> hazardousMaterialDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Department.hazardousMaterial.name)).ToList();
            if (hazardousMaterialDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in hazardousMaterialDisablers)
            {
                Disablers.Disabler hazardousMaterialDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Department.hazardousMaterial.name);
                if (hazardousMaterialDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (hazardousMaterialDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (hazardousMaterialDisabler.enablingDepartments[0] != DepartmentTypes.Type.Cleaning)
                {
                    continue;
                }
                hazardousMaterialDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Cleaning);
                if (hazardousMaterialDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(hazardousMaterialDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            yield break;
        }
        public IEnumerator CleanDirtyStation()
        {
            List<Department> dirtyStationDisablers = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Employee.dirtyStation.name)).ToList();
            if (dirtyStationDisablers.Count == 0)
            {
                yield break;
            }
            foreach (var department in dirtyStationDisablers)
            {
                Disablers.Disabler dirtyStationDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Employee.dirtyStation.name);
                if (dirtyStationDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (dirtyStationDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (dirtyStationDisabler.enablingDepartments[0] != DepartmentTypes.Type.Cleaning)
                {
                    continue;
                }
                dirtyStationDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Cleaning);
                if (dirtyStationDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(dirtyStationDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department); // Remove department from disabled departments list
                    }
                }
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
    }
    public class Security : Actions
    {
        public IEnumerator HandleSecurityBreach(Department affectedDepartment = null)
        {
            if (affectedDepartment == null)
            {
                throw new System.ArgumentNullException(nameof(affectedDepartment), "Affected department cannot be null.");
            }
            Disablers.Disabler securityBreachDisabler = affectedDepartment.disablers.FirstOrDefault(d => d.name == Disablers.Warehouse.securityBreach.name);
            if (securityBreachDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            securityBreachDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Security);
            if (securityBreachDisabler.enablingDepartments.Count == 0)
            {
                affectedDepartment.RemoveDisabler(securityBreachDisabler);
                if (affectedDepartment.disablers.Count == 0)
                {
                    Globals.disabledDepartments.Remove(affectedDepartment); // Remove department from disabled departments list
                }
            }
            yield break;
        }
        public IEnumerator HandleTheft(Employee affectedEmployee = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            Disablers.Disabler theftDisabler = affectedEmployee.disablers.FirstOrDefault(d => d.name == Disablers.Employee.theft.name);
            if (theftDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            theftDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Security);
            if (theftDisabler.enablingDepartments.Count == 0)
            {
                affectedEmployee.RemoveDisabler(theftDisabler);
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
    }
    public class Safety : Actions
    {
        public IEnumerator RespondToFire()
        {
            List<Department> fireAffectedDepartments = Globals.disabledDepartments.Where(d => d.disablers.Any(disabler => disabler.name == Disablers.Warehouse.fire.name)).ToList();
            if (fireAffectedDepartments.Count == 0)
            {
                yield break;
            }
            foreach (var department in fireAffectedDepartments)
            {
                Disablers.Disabler fireDisabler = department.disablers.FirstOrDefault(d => d.name == Disablers.Warehouse.fire.name);
                if (fireDisabler.Equals(default(Disablers.Disabler)))
                {
                    continue;
                }
                if (fireDisabler.enablingDepartments.Count == 0)
                {
                    continue;
                }
                if (fireDisabler.enablingDepartments[0] != DepartmentTypes.Type.Safety)
                {
                    continue;
                }
                fireDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Safety);
                if (fireDisabler.enablingDepartments.Count == 0)
                {
                    department.RemoveDisabler(fireDisabler);
                    if (department.disablers.Count == 0)
                    {
                        Globals.disabledDepartments.Remove(department);
                    }
                }
                if (fireDisabler.enablingDepartments.Count != 0)
                {
                    if (fireDisabler.enablingDepartments.Contains(DepartmentTypes.Type.Safety))
                    {
                        ActionRequest actionRequest = new ActionRequest(RespondToFire(), null, department);
                        Globals.departments.Where(d => d.departmentType == DepartmentTypes.Type.Safety).FirstOrDefault().AddActionRequest(actionRequest);
                    }
                }
            }
            yield break;
        }
        public IEnumerator RespondToChemicalSpill(Department affectedDepartment = null)
        {
            if (affectedDepartment == null)
            {
                throw new System.ArgumentNullException(nameof(affectedDepartment), "Affected department cannot be null.");
            }
            Disablers.Disabler chemicalSpillDisabler = affectedDepartment.disablers.FirstOrDefault(d => d.name == Disablers.Department.chemicalSpill.name);
            if (chemicalSpillDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            if (chemicalSpillDisabler.enablingDepartments.Count == 0)
            {
                yield break;
            }
            if (chemicalSpillDisabler.enablingDepartments[0] != DepartmentTypes.Type.Safety)
            {
                yield break;
            }
            chemicalSpillDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Safety);
            if (chemicalSpillDisabler.enablingDepartments.Count == 0)
            {
                affectedDepartment.RemoveDisabler(chemicalSpillDisabler);
                if (affectedDepartment.disablers.Count == 0)
                {
                    Globals.disabledDepartments.Remove(affectedDepartment);
                }
            }
            if (chemicalSpillDisabler.enablingDepartments.Count != 0)
            {
                if (chemicalSpillDisabler.enablingDepartments.Contains(DepartmentTypes.Type.Safety))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToChemicalSpill(affectedDepartment), null, affectedDepartment);
                    Globals.departments.Where(d => d.departmentType == DepartmentTypes.Type.Safety).FirstOrDefault().AddActionRequest(actionRequest);
                }
            }
            yield break;
        }
        public IEnumerator RespondToFlood(Department affectedDepartment = null)
        {
            if (affectedDepartment == null)
            {
                throw new System.ArgumentNullException(nameof(affectedDepartment), "Affected department cannot be null.");
            }
            Disablers.Disabler floodDisabler = affectedDepartment.disablers.FirstOrDefault(d => d.name == Disablers.Department.flood.name);
            if (floodDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            if (floodDisabler.enablingDepartments.Count == 0)
            {
                yield break;
            }
            if (floodDisabler.enablingDepartments[0] != DepartmentTypes.Type.Safety)
            {
                yield break;
            }
            floodDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Safety);
            if (floodDisabler.enablingDepartments.Count == 0)
            {
                affectedDepartment.RemoveDisabler(floodDisabler);
                if (affectedDepartment.disablers.Count == 0)
                {
                    Globals.disabledDepartments.Remove(affectedDepartment);
                }
            }
            if (floodDisabler.enablingDepartments.Count != 0)
            {
                if (floodDisabler.enablingDepartments.Contains(DepartmentTypes.Type.Safety))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToFlood(affectedDepartment), null, affectedDepartment);
                    Globals.departments.Where(d => d.departmentType == DepartmentTypes.Type.Safety).FirstOrDefault().AddActionRequest(actionRequest);
                }
            }
            yield break;
        }
        public IEnumerator RespondToAccident(Department affectedDepartment = null)
        {
            if (affectedDepartment == null)
            {
                throw new System.ArgumentNullException(nameof(affectedDepartment), "Affected department cannot be null.");
            }
            Disablers.Disabler accidentDisabler = affectedDepartment.disablers.FirstOrDefault(d => d.name == Disablers.Department.accident.name);
            if (accidentDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            if (accidentDisabler.enablingDepartments.Count == 0)
            {
                yield break;
            }
            if (accidentDisabler.enablingDepartments[0] != DepartmentTypes.Type.Safety)
            {
                yield break;
            }
            accidentDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Safety);
            if (accidentDisabler.enablingDepartments.Count == 0)
            {
                affectedDepartment.RemoveDisabler(accidentDisabler);
                if (affectedDepartment.disablers.Count == 0)
                {
                    Globals.disabledDepartments.Remove(affectedDepartment);
                }
            }
            if (accidentDisabler.enablingDepartments.Count != 0)
            {
                if (accidentDisabler.enablingDepartments.Contains(DepartmentTypes.Type.Safety))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToAccident(affectedDepartment), null, affectedDepartment);
                    Globals.departments.Where(d => d.departmentType == DepartmentTypes.Type.Safety).FirstOrDefault().AddActionRequest(actionRequest);
                }
            }
            yield break;
        }
        public IEnumerator RespondToHazardousMaterial(Department affectedDepartment = null)
        {
            if (affectedDepartment == null)
            {
                throw new System.ArgumentNullException(nameof(affectedDepartment), "Affected department cannot be null.");
            }
            Disablers.Disabler hazardousMaterialDisabler = affectedDepartment.disablers.FirstOrDefault(d => d.name == Disablers.Department.hazardousMaterial.name);
            if (hazardousMaterialDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            if (hazardousMaterialDisabler.enablingDepartments.Count == 0)
            {
                yield break;
            }
            if (hazardousMaterialDisabler.enablingDepartments[0] != DepartmentTypes.Type.Safety)
            {
                yield break;
            }
            hazardousMaterialDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Safety);
            if (hazardousMaterialDisabler.enablingDepartments.Count == 0)
            {
                affectedDepartment.RemoveDisabler(hazardousMaterialDisabler);
                if (affectedDepartment.disablers.Count == 0)
                {
                    Globals.disabledDepartments.Remove(affectedDepartment);
                }
            }
            if (hazardousMaterialDisabler.enablingDepartments.Count != 0)
            {
                if (hazardousMaterialDisabler.enablingDepartments.Contains(DepartmentTypes.Type.Safety))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToHazardousMaterial(affectedDepartment), null, affectedDepartment);
                    Globals.departments.Where(d => d.departmentType == DepartmentTypes.Type.Safety).FirstOrDefault().AddActionRequest(actionRequest);
                }
            }
            yield break;
        }
        public IEnumerator RespondToInjury(Employee affectedEmployee = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            Disablers.Disabler injuryDisabler = affectedEmployee.disablers.FirstOrDefault(d => d.name == Disablers.Employee.injury.name);
            if (injuryDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            injuryDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Safety);
            if (injuryDisabler.enablingDepartments.Count == 0)
            {
                affectedEmployee.RemoveDisabler(injuryDisabler);
            }
            if (injuryDisabler.enablingDepartments.Count != 0)
            {
                if (injuryDisabler.enablingDepartments.Contains(DepartmentTypes.Type.Safety))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToInjury(affectedEmployee), affectedEmployee);
                    Globals.departments.Where(d => d.departmentType == DepartmentTypes.Type.Safety).FirstOrDefault().AddActionRequest(actionRequest);
                }
            }
            gameController.UpdateEmployeeUIList(); // Update the UI of the game controller
            yield break;
        }
        public IEnumerator RespondToUnsafeStation(Employee affectedEmployee = null)
        {
            if (affectedEmployee == null)
            {
                throw new System.ArgumentNullException(nameof(affectedEmployee), "Affected employee cannot be null.");
            }
            Disablers.Disabler unsafeStationDisabler = affectedEmployee.disablers.FirstOrDefault(d => d.name == Disablers.Employee.stationUnsafe.name);
            if (unsafeStationDisabler.Equals(default(Disablers.Disabler)))
            {
                yield break;
            }
            unsafeStationDisabler.RemoveEnablingDepartment(DepartmentTypes.Type.Safety);
            if (unsafeStationDisabler.enablingDepartments.Count == 0)
            {
                affectedEmployee.RemoveDisabler(unsafeStationDisabler);
            }
            if (unsafeStationDisabler.enablingDepartments.Count != 0)
            {
                if (unsafeStationDisabler.enablingDepartments.Contains(DepartmentTypes.Type.Safety))
                {
                    ActionRequest actionRequest = new ActionRequest(RespondToUnsafeStation(affectedEmployee), affectedEmployee);
                    Globals.departments.Where(d => d.departmentType == DepartmentTypes.Type.Safety).FirstOrDefault().AddActionRequest(actionRequest);
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
            // Generate a random first name and last name
            string firstName = firstNames[Random.Range(0, firstNames.Count)];
            string lastName = lastNames[Random.Range(0, lastNames.Count)];
            // Generate a random salary between 1000 and 1500
            int salary = Random.Range(1000, 1501);
            // Generate random stats based on salary percentage 
            // [ 0% - 50% ] = 1 - 2.5, [ 51% - 70% ] = 2.5 - 4, [ 71% - 100% ] = 4.5 - 6
            // Pick 3 random stats to recieve a bonus
            // Stat order speed, efficiency, stamina, strength, focus, experience
            float[] statValues = new float[6];
            int minStatWholeValue;
            int maxStatWholeValue;
            float salaryPercentage = (float)salary / 1500f; // Assuming max salary is 1000
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
                statValues[randomNum] = Mathf.Max(1, randomStatValue); // Ensure the stat value is between 1 and 6
                statValues[randomNum] = Mathf.Min(6, randomStatValue);
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
            newEmployee.department = null;
            newEmployee.employeeType = EmployeeType.Type.None;
            newEmployee.departmentType = DepartmentTypes.Type.None; // Set initial department type to None
            newEmployee.level = 1;
            newEmployee.actionState = ActionState.State.Idle; // Set initial action state to Idle

            // Add the new employee to the warehouse new hire list
            Globals.newHires.Add(newEmployee);
            Globals.daysSinceLastNewHire = 0; // Reset the days since last new hire
            yield break;
        }
        public IEnumerator StartRandomDisabler()
        {
            // Logic to start a random disabler
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
                            actionRequest = new ActionRequest(safetyInstance.RespondToFire(), null, department);
                            resolvingDepartment.AddActionRequest(actionRequest);
                            break;
                        case "Power Outage":
                            actionRequest = new ActionRequest(maintenanceInstance.RepairPowerOutage(), null, department);
                            resolvingDepartment.AddActionRequest(actionRequest);
                            break;
                        case "Security Breach":
                            actionRequest = new ActionRequest(securityInstance.HandleSecurityBreach(), null, department);
                            resolvingDepartment.AddActionRequest(actionRequest);
                            break;
                        case "Network Failure":
                            actionRequest = new ActionRequest(itInstance.FixNetworkFailure(), null, department);
                            resolvingDepartment.AddActionRequest(actionRequest);
                            break;
                        default:
                            break;
                    }
                }
                // create notification
                notificationController.CreateNotification($"Disabler {randomDisabler.name} has been enabled in the warehouse.");
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
                        actionRequest = new ActionRequest(safetyInstance.RespondToInjury(randomEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Employee Misconduct":
                        actionRequest = new ActionRequest(hrInstance.DocumentMisconduct(randomEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Computer Failure":
                        actionRequest = new ActionRequest(itInstance.FixComputerFailure(randomEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Equipment Malfunction":
                        actionRequest = new ActionRequest(maintenanceInstance.RepairEquipmentMalfunction(randomEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Theft":
                        actionRequest = new ActionRequest(securityInstance.HandleTheft(randomEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Dirty Station":
                        actionRequest = new ActionRequest(cleaningInstance.CleanDirtyStation(), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Unsafe Station":
                        actionRequest = new ActionRequest(safetyInstance.RespondToUnsafeStation(randomEmployee), randomEmployee, null);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    default:
                        break;
                }
                // Create notification
                notificationController.CreateNotification($"Disabler {randomDisabler.name} has been enabled for employee {randomEmployee.employeeName}.");
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
                        actionRequest = new ActionRequest(itInstance.FixDeviceFailure(), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Server Failure":
                        actionRequest = new ActionRequest(itInstance.FixServerFailure(), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Mechanical Failure":
                        actionRequest = new ActionRequest(maintenanceInstance.RepairMechanicalFailure(), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Electrical Failure":
                        actionRequest = new ActionRequest(maintenanceInstance.RepairElectricalFailure(), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Flood":
                        actionRequest = new ActionRequest(safetyInstance.RespondToFlood(randomDepartment), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Chemical Spill":
                        actionRequest = new ActionRequest(safetyInstance.RespondToChemicalSpill(randomDepartment), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Equipment Failure":
                        actionRequest = new ActionRequest(maintenanceInstance.RepairEquipmentFailure(), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Accident":
                        actionRequest = new ActionRequest(safetyInstance.RespondToAccident(randomDepartment), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    case "Hazardous Material":
                        actionRequest = new ActionRequest(safetyInstance.RespondToHazardousMaterial(randomDepartment), null, randomDepartment);
                        resolvingDepartment.AddActionRequest(actionRequest);
                        break;
                    default:
                        break;
                }
                // Create notification
                notificationController.CreateNotification($"Disabler {randomDisabler.name} has been enabled in the {randomDepartment.departmentName} department.");
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

