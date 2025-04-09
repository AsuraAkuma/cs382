using System.Collections;
using UnityEngine;

public class Employee : MonoBehaviour
{
    public int id; // Unique identifier for the employee
    public string employeeName; // Name of the employee
    public int level; // Level of the employee
    public int exp; // Experience points of the employee
    public int salary; // Salary of the employee
    public int departmentId; // Identifier for the department the employee belongs to
    public EmployeeType.Type employeeType; // Type of the employee (e.g., HR, IT, etc.)
    public DepartmentTypes.Type departmentType; // Type of the department the employee belongs to
    public Department department; // Reference to the department the employee belongs to
    // Core Stats
    protected float speed;      // How fast the employee moves items or travels between zones
    protected float efficiency; // How well they use time/resources (less downtime, fewer mistakes)
    protected float stamina;    // How long they can work before needing rest or a break
    protected float strength;   // Affects how heavy of an item they can carry or how many items at once
    protected float focus;      // Impacts accuracy and likelihood of making errors
    protected float experience; // Affects leveling up, promotions, or unlocking new roles
    private TraitValues[] traits = { }; // Employee traits
    private TraitValues combinedTraits; // Combined traits for the employee
    public ActionState.State actionState; // Current action state of the employee
    protected float stateTimer = 0f; // Timer for current state
    protected float workInterval = 0.5f; // How often to update work state
    protected float restInterval = 1f; // How often to update rest state
    // Constructor to initialize an Employee object
    public Employee()
    {
        // Initialize core stats with default values
        speed = 1f;
        efficiency = 1f;
        stamina = 1f;
        strength = 1f;
        focus = 1f;
        experience = 1f;
    }

    public Employee(int id, string name, int level, int exp, int salary, int departmentId)
    {
        this.id = id;
        employeeName = name;
        this.level = level;
        this.exp = exp;
        this.salary = salary;
        this.departmentId = departmentId;

        // Initialize core stats with default values
        speed = 1f;
        efficiency = 1f;
        stamina = 1f;
        strength = 1f;
        focus = 1f;
        experience = 1f;
    }
    void FixedUpdate()
    {
        if (Globals.gameState != State.Playing)
        {
            return;
        }

        stateTimer += Time.fixedDeltaTime;

        // Update employee state based on actionState
        switch (actionState)
        {
            case ActionState.State.Working:
                if (stateTimer >= workInterval)
                {
                    stamina -= 0.01f * (1f / efficiency);
                    experience += 0.005f * efficiency;
                    efficiency += 0.002f;
                    focus -= 0.005f;
                    speed -= 0.003f;
                    strength -= 0.002f;
                    stateTimer = 0f;
                }
                break;

            case ActionState.State.Resting:
                if (stateTimer >= restInterval)
                {
                    stamina += 0.02f;
                    focus += 0.01f;
                    speed += 0.005f;
                    efficiency += 0.001f;
                    strength -= 0.003f;
                    experience -= 0.001f;
                    stateTimer = 0f;
                }
                break;

            case ActionState.State.Training:
                if (stateTimer >= workInterval)
                {
                    experience += 0.015f * efficiency;
                    focus += 0.01f;
                    efficiency += 0.005f;
                    stamina -= 0.01f;
                    speed += 0.005f;
                    strength += 0.003f;
                    stateTimer = 0f;
                }
                break;

            case ActionState.State.Emergency:
                if (stateTimer >= workInterval)
                {
                    stamina -= 0.2f;
                    speed += 0.02f;
                    focus += 0.01f;
                    strength += 0.015f;
                    efficiency -= 0.005f;
                    experience -= 0.01f;
                    stateTimer = 0f;
                }
                break;

            case ActionState.State.Break:
                if (stateTimer >= restInterval)
                {
                    stamina += 0.01f;
                    focus += 0.005f;
                    experience += 0.001f;
                    speed -= 0.001f;
                    strength -= 0.001f;
                    efficiency -= 0.002f;
                    stateTimer = 0f;
                }
                break;

            case ActionState.State.Idle:
                if (stateTimer >= restInterval)
                {
                    stamina += 0.005f;
                    experience -= 0.002f;
                    focus -= 0.005f;
                    efficiency -= 0.003f;
                    speed -= 0.002f;
                    strength -= 0.001f;
                    stateTimer = 0f;
                }
                break;
        }

        // Clamp all stats to ensure none go below 0
        stamina = Mathf.Max(0f, stamina);
        speed = Mathf.Max(0f, speed);
        efficiency = Mathf.Max(0f, efficiency);
        focus = Mathf.Max(0f, focus);
        strength = Mathf.Max(0f, strength);
        experience = Mathf.Max(0f, experience);
    }
    public float GetSpeed()
    {
        return speed;
    }
    public void SetSpeed(float value)
    {
        speed = value;
    }
    public float GetEfficiency()
    {
        return efficiency;
    }
    public void SetEfficiency(float value)
    {
        efficiency = value;
    }
    public float GetStamina()
    {
        return stamina;
    }
    public void SetStamina(float value)
    {
        stamina = value;
    }
    public float GetStrength()
    {
        return strength;
    }
    public void SetStrength(float value)
    {
        strength = value;
    }
    public float GetFocus()
    {
        return focus;
    }
    public void SetFocus(float value)
    {
        focus = value;
    }
    public float GetExperience()
    {
        return experience;
    }
    public void SetExperience(float value)
    {
        experience = value;
    }
    public void SyncTraits()
    {
        // Combine all traits into one for the employee
        combinedTraits = TraitValues.CombineTraits(traits);
        // Set the department type based on the combined traits
        departmentType = combinedTraits.departmentType;
    }
    public void PrimaryAction()
    {
        Debug.Log("No primary action defined for this employee type.");
    } // Placeholder for primary action
    public void SecondaryAction()
    {
        Debug.Log("No secondary action defined for this employee type.");
    } // Placeholder for secondary action
}

#region Dept. Employees
public class HREmployee : Employee
{
    protected float empathy;           // Increases morale and reduces turnover
    protected float conflictResolution;// Ability to handle disputes
    protected float recruiting;        // Increases likelihood of hiring skilled employees
    private TraitValues traits; // Employee traits

    public new float GetStamina()
    {
        return stamina + empathy + traits.stamina;
    }
    public new float GetEfficiency()
    {
        return efficiency + conflictResolution + empathy + recruiting + traits.efficiency;
    }
    public new float GetFocus()
    {
        return focus + conflictResolution + traits.focus;
    }
    public new float GetExperience()
    {
        return experience + recruiting + traits.experience;
    }
    public new float GetSpeed()
    {
        return speed + traits.speed;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new void PrimaryAction()
    {
        Debug.Log($"HR Employee {employeeName} is performing a primary action.");
        // Check if there are any action requests available in the department
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"HR Employee {employeeName} has no action requests to handle.");
            return;
        }
        // Get the first action request from the department
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleTicketSequence(actionRequest));
    }

    IEnumerator HandleTicketSequence(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleTicket(actionRequest));
        actionState = ActionState.State.Idle;
    }
    public new void SecondaryAction()
    {
        Debug.Log($"HR Employee {employeeName} is performing a secondary action.");
    }

    IEnumerator HandleTicket(ActionRequest actionRequest)
    {
        // Simulate ticket handling process
        Debug.Log($"HR Employee {employeeName} is handling a ticket.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            // Perform action based on department statTimes
            Debug.Log($"HR Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        // Simulate chance of success based on focus
        float successChance = Random.Range(0f, 1f);
        if (successChance <= focus)
        {
            Debug.Log($"HR Employee {employeeName} successfully handled the ticket.");
            // Update action request status to completed
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"HR Employee {employeeName} failed to handle the ticket.");
            // Update action request status to failed
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
    }
}

public class HRManager : HREmployee
{
    protected float moraleBoost;        // Increases team performance
    protected float retentionStrategy; // Improves long-term HR strategies
    protected float policyEnforcement; // Enhances contract negotiations
    private TraitValues traits;

    public new float GetSpeed()
    {
        return speed + traits.speed;
    }
    public new float GetFocus()
    {
        return focus + policyEnforcement + retentionStrategy + traits.focus;
    }
    public new float GetEfficiency()
    {
        return efficiency + policyEnforcement + moraleBoost + traits.efficiency;
    }
    public new float GetStamina()
    {
        return stamina + moraleBoost + traits.stamina;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new float GetExperience()
    {
        return experience + retentionStrategy + traits.experience;
    }
}

public class ITEmployee : Employee
{
    protected float techTroubleshooter;// Ability to fix technical issues quickly
    protected float systemOptimization;// Enhances warehouse system efficiency
    protected float security;          // Protects against cyber threats
    private TraitValues traits;

    public new float GetSpeed()
    {
        return speed + techTroubleshooter + systemOptimization + traits.speed;
    }
    public new float GetFocus()
    {
        return focus + techTroubleshooter + security + traits.focus;
    }
    public new float GetEfficiency()
    {
        return efficiency + systemOptimization + traits.efficiency;
    }
    public new float GetStamina()
    {
        return stamina + security + traits.stamina;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}

public class OperationsEmployee : Employee
{
    protected float logisticsPlanning; // Increases workflow efficiency
    protected float taskManagement;    // Organizes and prioritizes tasks
    protected float coordination;      // Boosts teamwork between departments
    private TraitValues traits;

    public new float GetEfficiency()
    {
        return efficiency + logisticsPlanning + taskManagement + coordination + traits.efficiency;
    }
    public new float GetSpeed()
    {
        return speed + logisticsPlanning + traits.speed;
    }
    public new float GetFocus()
    {
        return focus + taskManagement + traits.focus;
    }
    public new float GetStamina()
    {
        return stamina + coordination + traits.stamina;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}

public class InboundEmployee : Employee
{
    protected float loadMaster;        // Speeds up unloading process
    protected float inventoryCheck;    // Ensures accurate receiving records
    protected float speedyUnloader;    // Increases unloading speed
    private TraitValues traits;

    public new float GetSpeed()
    {
        return speed + loadMaster + speedyUnloader + traits.speed;
    }
    public new float GetStrength()
    {
        return strength + loadMaster + traits.strength;
    }
    public new float GetFocus()
    {
        return focus + inventoryCheck + traits.focus;
    }
    public new float GetEfficiency()
    {
        return efficiency + inventoryCheck + traits.efficiency;
    }
    public new float GetStamina()
    {
        return stamina + speedyUnloader + traits.stamina;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}

public class OutboundEmployee : Employee
{
    protected float shippingAccuracy;  // Ensures correct destination shipping
    protected float loadEfficiency;    // Increases outbound loading efficiency
    protected float timeManagement;    // Reduces shipping delays
    private TraitValues traits;

    public new float GetFocus()
    {
        return focus + shippingAccuracy + traits.focus;
    }
    public new float GetEfficiency()
    {
        return efficiency + shippingAccuracy + loadEfficiency + timeManagement + traits.efficiency;
    }
    public new float GetSpeed()
    {
        return speed + loadEfficiency + timeManagement + traits.speed;
    }
    public new float GetStamina()
    {
        return stamina + traits.stamina;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}

public class SortingEmployee : Employee
{
    protected float sortingSpeed;      // Increases sorting speed
    protected float accuracy;          // Reduces sorting errors
    protected float patternRecognition;// Identifies efficient sorting patterns
    private TraitValues traits;

    public new float GetSpeed()
    {
        return speed + sortingSpeed + patternRecognition + traits.speed;
    }
    public new float GetEfficiency()
    {
        return efficiency + sortingSpeed + accuracy + patternRecognition + traits.efficiency;
    }
    public new float GetFocus()
    {
        return focus + accuracy + traits.focus;
    }
    public new float GetStamina()
    {
        return stamina + traits.stamina;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}

public class RepackingEmployee : Employee
{
    protected float packingEfficiency; // Maximizes box space usage
    protected float damageControl;    // Reduces item damage risk
    private TraitValues traits;

    public new float GetEfficiency()
    {
        return efficiency + packingEfficiency + damageControl + traits.efficiency;
    }
    public new float GetFocus()
    {
        return focus + packingEfficiency + damageControl + traits.focus;
    }
    public new float GetSpeed()
    {
        return speed + traits.speed;
    }
    public new float GetStamina()
    {
        return stamina + traits.stamina;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}

public class PalletizingEmployee : Employee
{
    protected float palletEfficiency;  // Organizes items efficiently
    protected float heavyLifting;      // Increases stacking speed/strength
    protected float stackingPrecision; // Ensures secure stacking
    private TraitValues traits;

    public new float GetEfficiency()
    {
        return efficiency + palletEfficiency + stackingPrecision + traits.efficiency;
    }
    public new float GetStrength()
    {
        return strength + palletEfficiency + heavyLifting + traits.strength;
    }
    public new float GetSpeed()
    {
        return speed + heavyLifting + traits.speed;
    }
    public new float GetFocus()
    {
        return focus + stackingPrecision + traits.focus;
    }
    public new float GetStamina()
    {
        return stamina + traits.stamina;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}
public class WaterSpiderEmployee : Employee
{
    protected float routeEfficiency;   // Finds fastest delivery routes
    protected float carryCapacity;     // Increases item carry limit
    protected float supportSpeed;      // Quick restocking and assistance
    private TraitValues traits;

    public new float GetSpeed()
    {
        return speed + routeEfficiency + supportSpeed + traits.speed;
    }
    public new float GetEfficiency()
    {
        return efficiency + routeEfficiency + carryCapacity + traits.efficiency;
    }
    public new float GetStrength()
    {
        return strength + carryCapacity + traits.strength;
    }
    public new float GetStamina()
    {
        return stamina + supportSpeed + traits.stamina;
    }
    public new float GetFocus()
    {
        return focus + traits.focus;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}

public class FluidLoadEmployee : Employee
{
    protected float loadingSpeed;      // Increases truck loading speed
    protected float hardHatProtection;// Reduces accident risk
    protected float weightDistribution;// Helps distribute load weight evenly
    private TraitValues traits;

    public new float GetSpeed()
    {
        return speed + loadingSpeed + traits.speed;
    }
    public new float GetStrength()
    {
        return strength + loadingSpeed + traits.strength;
    }
    public new float GetStamina()
    {
        return stamina + hardHatProtection + traits.stamina;
    }
    public new float GetFocus()
    {
        return focus + hardHatProtection + weightDistribution + traits.focus;
    }
    public new float GetEfficiency()
    {
        return efficiency + weightDistribution + traits.efficiency;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}

public class QualityControlEmployee : Employee
{
    protected float attentionToDetail; // Improves defect detection
    protected float inspectionSpeed;   // Speeds up inspection process
    protected float productKnowledge;  // Helps handle specific products
    private TraitValues traits;

    public new float GetFocus()
    {
        return focus + attentionToDetail + inspectionSpeed + traits.focus;
    }
    public new float GetEfficiency()
    {
        return efficiency + attentionToDetail + productKnowledge + traits.efficiency;
    }
    public new float GetSpeed()
    {
        return speed + inspectionSpeed + traits.speed;
    }
    public new float GetExperience()
    {
        return experience + productKnowledge + traits.experience;
    }
    public new float GetStamina()
    {
        return stamina + traits.stamina;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
}

public class MaintenanceEmployee : Employee
{
    protected float repairSpeed;       // Increases equipment repair speed
    protected float preventativeMaintenance; // Reduces equipment failures
    protected float toolMastery;      // Increases repair tool effectiveness
    private TraitValues traits;

    public new float GetSpeed()
    {
        return speed + repairSpeed + traits.speed;
    }
    public new float GetFocus()
    {
        return focus + repairSpeed + traits.focus;
    }
    public new float GetStamina()
    {
        return stamina + preventativeMaintenance + traits.stamina;
    }
    public new float GetEfficiency()
    {
        return efficiency + preventativeMaintenance + toolMastery + traits.efficiency;
    }
    public new float GetExperience()
    {
        return experience + toolMastery + traits.experience;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
}

public class RoboticsEmployee : Employee
{
    protected float robotCalibration;  // Ensures peak robot efficiency
    protected float speedEnhancement;  // Increases robot movement speed
    protected float accuracy;          // Reduces robot movement errors
    private TraitValues traits;

    public new float GetEfficiency()
    {
        return efficiency + robotCalibration + accuracy + traits.efficiency;
    }
    public new float GetFocus()
    {
        return focus + robotCalibration + accuracy + traits.focus;
    }
    public new float GetSpeed()
    {
        return speed + speedEnhancement + traits.speed;
    }
    public new float GetStamina()
    {
        return stamina + traits.stamina;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}

public class SecurityEmployee : Employee
{
    protected float surveillance;      // Increases monitoring ability
    protected float alertness;        // Improves breach response time
    protected float patrolSpeed;      // Increases patrol coverage speed
    private TraitValues traits;

    public new float GetFocus()
    {
        return focus + surveillance + alertness + traits.focus;
    }
    public new float GetEfficiency()
    {
        return efficiency + surveillance + patrolSpeed + traits.efficiency;
    }
    public new float GetSpeed()
    {
        return speed + alertness + patrolSpeed + traits.speed;
    }
    public new float GetStamina()
    {
        return stamina + traits.stamina;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}

public class CleaningEmployee : Employee
{
    protected float thoroughness;     // Ensures proper cleaning
    protected float routineMaintenance;// Maintains workspace cleanliness
    private TraitValues traits;

    public new float GetSpeed()
    {
        return speed + traits.speed;
    }
    public new float GetStamina()
    {
        return stamina + routineMaintenance + traits.stamina;
    }
    public new float GetFocus()
    {
        return focus + thoroughness + traits.focus;
    }
    public new float GetEfficiency()
    {
        return efficiency + thoroughness + routineMaintenance + traits.efficiency;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}
public class LearningEmployee : Employee
{
    protected float trainingEffectiveness; // Improves training programs
    protected float skillTransfer;        // Increases skill gain rate
    protected float motivation;           // Increases training completion
    private TraitValues traits;

    public new float GetExperience()
    {
        return experience + trainingEffectiveness + skillTransfer + traits.experience;
    }
    public new float GetEfficiency()
    {
        return efficiency + trainingEffectiveness + skillTransfer + traits.efficiency;
    }
    public new float GetStamina()
    {
        return stamina + motivation + traits.stamina;
    }
    public new float GetSpeed()
    {
        return speed + traits.speed;
    }
    public new float GetFocus()
    {
        return focus + traits.focus;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
}

public class SafetyEmployee : Employee
{
    protected float hazardIdentification; // Detects potential issues
    protected float accidentPrevention;   // Reduces accident likelihood
    protected float emergencyResponse;    // Improves emergency handling
    private TraitValues traits;

    public new float GetFocus()
    {
        return focus + hazardIdentification + emergencyResponse + traits.focus;
    }
    public new float GetEfficiency()
    {
        return efficiency + hazardIdentification + accidentPrevention + traits.efficiency;
    }
    public new float GetStamina()
    {
        return stamina + accidentPrevention + traits.stamina;
    }
    public new float GetSpeed()
    {
        return speed + emergencyResponse + traits.speed;
    }
    public new float GetStrength()
    {
        return strength + traits.strength;
    }
    public new float GetExperience()
    {
        return experience + traits.experience;
    }
}
#endregion