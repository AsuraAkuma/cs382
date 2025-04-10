using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Employee : MonoBehaviour
{
    public int id; // Unique identifier for the employee
    public string employeeName; // Name of the employee
    public int level; // Level of the employee
    public int exp; // Experience points of the employee
    public int salary; // Salary of the employee
    public int cost; // Cost of the employee
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
    public List<TraitValues> traits = new List<TraitValues>(); // Employee traits
    public TraitValues combinedTraits; // Combined traits for the employee
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
        return Mathf.Min(2f, speed + combinedTraits.speed);
    }
    public void SetSpeed(float value)
    {
        speed = value;
    }
    public float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + combinedTraits.efficiency);
    }
    public void SetEfficiency(float value)
    {
        efficiency = value;
    }
    public float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public void SetStamina(float value)
    {
        stamina = value;
    }
    public float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public void SetStrength(float value)
    {
        strength = value;
    }
    public float GetFocus()
    {
        return Mathf.Min(2f, focus + combinedTraits.focus);
    }
    public void SetFocus(float value)
    {
        focus = value;
    }
    public float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }
    public void SetExperience(float value)
    {
        experience = value;
    }
    public float GetLevel()
    {
        return level;
    }
    public void SetLevel(int value)
    {
        level = value;
    }
    public int GetExp()
    {
        return exp;
    }
    public void SetExp(int value)
    {
        exp = value;
    }
    public int GetSalary()
    {
        return salary;
    }
    public void SetSalary(int value)
    {
        salary = value;
    }
    public void AddExperience(int value)
    {
        exp += value;
        // Check if the employee has enough experience to level up
        int expNeeded = level * 250; // Example formula for experience needed to level up
        if (exp >= expNeeded)
        {
            level++;
            exp = 0; // Reset experience after leveling up
            Debug.Log($"{employeeName} has leveled up to level {level}!");
        }
    }
    public float GetStatAverage()
    {
        return Mathf.Min(2f, (GetSpeed() + GetEfficiency() + GetStamina() + GetStrength() + GetFocus() + GetExperience()) / 6);
    }
    public void SyncTraits()
    {
        // Combine all traits into one for the employee
        combinedTraits = TraitValues.CombineTraits(traits);
    }
    public void PrimaryAction()
    {
        Debug.Log("No primary action defined for this employee type.");
    } // Placeholder for primary action
    public void SecondaryAction()
    {
        Debug.Log("No secondary action defined for this employee type.");
    } // Placeholder for secondary action
    public void CancelAction()
    {
        Debug.Log("No action to cancel for this employee type.");
    } // Placeholder for cancel action
}

#region Dept. Employees
public class HREmployee : Employee
{
    protected float empathy;           // Increases morale and reduces turnover
    protected float conflictResolution;// Ability to handle disputes
    protected float recruiting;        // Increases likelihood of hiring skilled employees

    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + empathy + combinedTraits.stamina);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + conflictResolution + empathy + recruiting + combinedTraits.efficiency);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + conflictResolution + combinedTraits.focus);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + recruiting + combinedTraits.experience);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + combinedTraits.speed);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"HR Employee {employeeName} is busy with another action.");
            return;
        }
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

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"HR Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"HR Employee {employeeName} is performing a secondary action.");
        // Check for claimedActionRequests in the department
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"HR Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        // Give experience points based on claimedActionRequests count
        int experiencePoints = department.claimedActionRequests.Count * 5;
        // Update employee experience and check for level up
        AddExperience(experiencePoints);
        // Clear claimedActionRequests after review
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"HR Employee {employeeName} has canceled the current action.");
            StopAllCoroutines(); // Stop all ongoing actions
            actionState = ActionState.State.Idle; // Set state to idle
        }
        else
        {
            Debug.Log($"HR Employee {employeeName} is not currently working on any action.");
        }
    }
    IEnumerator HandleTicketSequence(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleTicket(actionRequest));
        actionState = ActionState.State.Idle;
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
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
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
        AddExperience(10); // Add experience for handling the ticket
    }

}

public class HRManager : HREmployee
{
    protected float moraleBoost;        // Increases team performance
    protected float retentionStrategy; // Improves long-term HR strategies
    protected float policyEnforcement; // Enhances contract negotiations


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + policyEnforcement + retentionStrategy + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + policyEnforcement + moraleBoost + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + moraleBoost + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + retentionStrategy + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"HR Manager {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"HR Manager {employeeName} is performing a primary action.");
        // Check if there are any action requests available in the department
        if (Globals.newHires.Count == 0)
        {
            Debug.Log($"HR Manager {employeeName} has no new hires to process.");
            return;
        }
        // Check if warehouse employees count is less than max employees
        if (Globals.warehouseEmployees.Count >= Globals.warehouseMaxEmployees)
        {
            Debug.Log($"HR Manager {employeeName} cannot hire more employees. Warehouse is at maximum capacity.");
            return;
        }
        // Check if departments are in need of new hires
        if (Globals.departments.Count == 0)
        {
            Debug.Log($"HR Manager {employeeName} has no departments to assign new hires to.");
            return;
        }

        StartCoroutine(HandleHireProcess());
    }
    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"HR Manager {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"HR Manager {employeeName} is performing a secondary action.");
        // Check for claimedActionRequests in the department
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"HR Manager {employeeName} has no claimed action requests to review.");
            return;
        }
        // Give experience points based on claimedActionRequests count
        int experiencePoints = department.claimedActionRequests.Count * 5;
        // Update employee experience and check for level up
        AddExperience(experiencePoints);
        // Clear claimedActionRequests after review
        department.claimedActionRequests.Clear();
    }
    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"HR Manager {employeeName} has canceled the current action.");
            StopAllCoroutines(); // Stop all ongoing actions
            actionState = ActionState.State.Idle; // Set state to idle
        }
        else
        {
            Debug.Log($"HR Manager {employeeName} is not currently working on any action.");
        }
    }
    IEnumerator HandleHireProcess()
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleHiring());
        actionState = ActionState.State.Idle;
    }
    struct DepartmentIndex
    {
        public DepartmentTypes.Type departmentType;
        public int count;
    }

    IEnumerator HandleHiring()
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
            Debug.Log($"HR Manager {employeeName} found no new hires to process.");
            yield break;
        }
        Employee newHire = Globals.newHires[0];
        if (newHire.cost > Globals.playerMoney)
        {
            Debug.Log($"HR Manager {employeeName} cannot afford to hire {newHire.employeeName}. Cost: {newHire.cost}, Available Money: {Globals.playerMoney}.");
            yield break;
        }
        Debug.Log($"HR Manager {employeeName} is processing a new hire: {newHire.employeeName}.");
        // Check if the new hire can be assigned to a department
        if (departmentsInNeed.Count == 0)
        {
            Debug.Log($"HR Manager {employeeName} found no departments in need of new hires.");
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
            Debug.Log($"HR Manager {employeeName} could not find a suitable department for {newHire.employeeName}.");
            yield break;
        }
        selectedDepartment.employees.Add(newHire);
        Globals.warehouseEmployees.Add(newHire);
        Globals.newHires.Remove(newHire);
        Globals.playerMoney -= newHire.cost; // Deduct the cost of hiring from player's money
        Debug.Log($"HR Manager {employeeName} has successfully assigned {newHire.employeeName} to the {selectedDepartment.departmentType} department.");
        yield break;
    }
}

public class ITEmployee : Employee
{
    protected float techTroubleshooter;// Ability to fix technical issues quickly
    protected float systemOptimization;// Enhances warehouse system efficiency
    protected float security;          // Protects against cyber threats


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + techTroubleshooter + systemOptimization + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + techTroubleshooter + security + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + systemOptimization + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + security + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"IT Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"IT Employee {employeeName} is performing a primary action.");
        // Check if there are any action requests available in the department
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"IT Employee {employeeName} has no action requests to handle.");
            return;
        }
        // Get the first action request from the department
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }
    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"IT Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"IT Employee {employeeName} is performing a secondary action.");
        // Check for claimedActionRequests in the department
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"IT Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        // Give experience points based on claimedActionRequests count
        int experiencePoints = department.claimedActionRequests.Count * 5;
        // Update employee experience and check for level up
        AddExperience(experiencePoints);
        // Clear claimedActionRequests after review
        department.claimedActionRequests.Clear();
    }
    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"IT Employee {employeeName} has canceled the current action.");
            StopAllCoroutines(); // Stop all ongoing actions
            actionState = ActionState.State.Idle; // Set state to idle
        }
        else
        {
            Debug.Log($"IT Employee {employeeName} is not currently working on any action.");
        }
    }
    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }
    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        // Simulate action request handling process
        Debug.Log($"IT Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            // Perform action based on department statTimes
            Debug.Log($"IT Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        // Simulate chance of success based on focus
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"IT Employee {employeeName} successfully handled the action request.");
            // Update action request status to completed
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"IT Employee {employeeName} failed to handle the action request.");
            // Update action request status to failed
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10); // Add experience for handling the action request
    }
}

public class ITManager : ITEmployee
{
    protected float infrastructureOversight; // Increases project completion speed
    protected float incidentResponse;    // Boosts team performance
    protected float techBudgeting;  // Improves long-term IT strategies


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + incidentResponse + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + incidentResponse + infrastructureOversight + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + infrastructureOversight + techBudgeting + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + techBudgeting + combinedTraits.experience);
    }
    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"IT Manager {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"IT Manager {employeeName} is performing a primary action.");
        // Check if there are any action requests available in the department
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"IT Manager {employeeName} has no action requests to handle.");
            return;
        }
        // Get the first action request from the department
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }
    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"IT Manager {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"IT Manager {employeeName} is performing a secondary action.");
        // Check for claimedActionRequests in the department
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"IT Manager {employeeName} has no claimed action requests to review.");
            return;
        }
        // Give experience points based on claimedActionRequests count
        int experiencePoints = department.claimedActionRequests.Count * 5;
        // Update employee experience and check for level up
        AddExperience(experiencePoints);
        // Clear claimedActionRequests after review
        department.claimedActionRequests.Clear();
    }
    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"IT Manager {employeeName} has canceled the current action.");
            StopAllCoroutines(); // Stop all ongoing actions
            actionState = ActionState.State.Idle; // Set state to idle
        }
        else
        {
            Debug.Log($"IT Manager {employeeName} is not currently working on any action.");
        }
    }
    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }
    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        // Simulate action request handling process
        Debug.Log($"IT Manager {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            // Perform action based on department statTimes
            Debug.Log($"IT Manager {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        // Simulate chance of success based on focus
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"IT Manager {employeeName} successfully handled the action request.");
            // Update action request status to completed
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"IT Manager {employeeName} failed to handle the action request.");
            // Update action request status to failed
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10); // Add experience for handling the action request
    }
}
public class OperationsEmployee : Employee
{
    protected float logisticsPlanning;
    protected float taskManagement;
    protected float coordination;


    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + logisticsPlanning + taskManagement + coordination + combinedTraits.efficiency);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + logisticsPlanning + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + taskManagement + combinedTraits.focus);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + coordination + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Operations Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Operations Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Operations Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Operations Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Operations Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Operations Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Operations Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Operations Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Operations Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Operations Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Operations Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Operations Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class OperationsManager : OperationsEmployee
{
    protected float processOptimization;
    protected float crossDepartmentSync;
    protected float kpiMonitoring;


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + processOptimization + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + crossDepartmentSync + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + kpiMonitoring + crossDepartmentSync + processOptimization + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + kpiMonitoring + combinedTraits.experience);
    }

}

public class InboundEmployee : Employee
{
    protected float loadMaster;        // Speeds up unloading process
    protected float inventoryCheck;    // Ensures accurate receiving records
    protected float speedyUnloader;    // Increases unloading speed


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + loadMaster + speedyUnloader + combinedTraits.speed);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + loadMaster + combinedTraits.strength);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + inventoryCheck + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + inventoryCheck + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + speedyUnloader + combinedTraits.stamina);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Inbound Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Inbound Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Inbound Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Inbound Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Inbound Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Inbound Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Inbound Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Inbound Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Inbound Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Inbound Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Inbound Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Inbound Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class InboundManager : InboundEmployee
{
    protected float dockFlowManagement;
    protected float receivingAccuracy;
    protected float supplierCoordination;


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + dockFlowManagement + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + receivingAccuracy + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + supplierCoordination + receivingAccuracy + dockFlowManagement + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + supplierCoordination + combinedTraits.experience);
    }

}
public class OutboundEmployee : Employee
{
    protected float shippingAccuracy;  // Ensures correct destination shipping
    protected float loadEfficiency;    // Increases outbound loading efficiency
    protected float timeManagement;    // Reduces shipping delays


    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + shippingAccuracy + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + shippingAccuracy + loadEfficiency + timeManagement + combinedTraits.efficiency);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + loadEfficiency + timeManagement + combinedTraits.speed);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Outbound Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Outbound Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Outbound Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Outbound Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Outbound Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Outbound Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Outbound Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Outbound Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Outbound Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Outbound Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Outbound Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Outbound Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class OutboundManager : OutboundEmployee
{
    protected float loadScheduling;
    protected float accuracyOversight;
    protected float carrierCoordination;


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + loadScheduling + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + accuracyOversight + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + carrierCoordination + loadScheduling + accuracyOversight + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + carrierCoordination + combinedTraits.experience);
    }

}
public class SortingEmployee : Employee
{
    protected float sortingSpeed;      // Increases sorting speed
    protected float accuracy;          // Reduces sorting errors
    protected float patternRecognition;// Identifies efficient sorting patterns


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + sortingSpeed + patternRecognition + combinedTraits.speed);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + sortingSpeed + accuracy + patternRecognition + combinedTraits.efficiency);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + accuracy + combinedTraits.focus);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Sorting Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Sorting Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Sorting Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Sorting Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Sorting Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Sorting Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Sorting Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Sorting Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Sorting Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Sorting Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Sorting Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Sorting Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class SortingManager : SortingEmployee
{
    protected float sortLineOversight;
    protected float errorReductionPlanning;
    protected float peakPrep;


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + sortLineOversight + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + errorReductionPlanning + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + sortLineOversight + errorReductionPlanning + peakPrep + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + peakPrep + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + +combinedTraits.experience);
    }

}
public class RepackingEmployee : Employee
{
    protected float packingEfficiency; // Maximizes box space usage
    protected float damageControl;    // Reduces item damage risk


    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + packingEfficiency + damageControl + combinedTraits.efficiency);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + packingEfficiency + damageControl + combinedTraits.focus);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + combinedTraits.speed);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Repacking Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Repacking Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Repacking Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Repacking Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Repacking Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Repacking Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Repacking Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Repacking Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Repacking Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Repacking Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Repacking Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Repacking Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class RepackingManager : RepackingEmployee
{
    protected float qualityCheck;
    protected float materialAllocation;
    protected float repackFlow;


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + repackFlow + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + qualityCheck + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + qualityCheck + materialAllocation + repackFlow + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + materialAllocation + combinedTraits.experience);
    }

}
public class PalletizingEmployee : Employee
{
    protected float palletEfficiency;  // Organizes items efficiently
    protected float heavyLifting;      // Increases stacking speed/strength
    protected float stackingPrecision; // Ensures secure stacking


    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + palletEfficiency + stackingPrecision + combinedTraits.efficiency);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + palletEfficiency + heavyLifting + combinedTraits.strength);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + heavyLifting + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + stackingPrecision + combinedTraits.focus);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Palletizing Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Palletizing Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Palletizing Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Palletizing Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Palletizing Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Palletizing Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Palletizing Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Palletizing Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Palletizing Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Palletizing Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Palletizing Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Palletizing Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class PalletizingManager : PalletizingEmployee
{
    protected float stackingSupervision;
    protected float loadForecasting;
    protected float safetyChecks;

    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + stackingSupervision + safetyChecks + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + stackingSupervision + loadForecasting + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + safetyChecks + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + loadForecasting + combinedTraits.experience);
    }

}
public class WaterSpiderEmployee : Employee
{
    protected float routeEfficiency;   // Finds fastest delivery routes
    protected float carryCapacity;     // Increases item carry limit
    protected float supportSpeed;      // Quick restocking and assistance


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + routeEfficiency + supportSpeed + combinedTraits.speed);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + routeEfficiency + carryCapacity + combinedTraits.efficiency);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + carryCapacity + combinedTraits.strength);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + supportSpeed + combinedTraits.stamina);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + combinedTraits.focus);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Water Spider Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Water Spider Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Water Spider Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Water Spider Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Water Spider Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Water Spider Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Water Spider Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Water Spider Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Water Spider Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Water Spider Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Water Spider Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Water Spider Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class WaterSpiderManager : WaterSpiderEmployee
{
    protected float routePlanning;
    protected float supportCoordination;
    protected float loadDistribution;


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + routePlanning + combinedTraits.speed);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + routePlanning + supportCoordination + loadDistribution + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + loadDistribution + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + supportCoordination + combinedTraits.focus);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + +combinedTraits.experience);
    }

}
public class FluidLoadEmployee : Employee
{
    protected float loadingSpeed;      // Increases truck loading speed
    protected float hardHatProtection;// Reduces accident risk
    protected float weightDistribution;// Helps distribute load weight evenly


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + loadingSpeed + combinedTraits.speed);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + loadingSpeed + combinedTraits.strength);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + hardHatProtection + combinedTraits.stamina);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + hardHatProtection + weightDistribution + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + weightDistribution + combinedTraits.efficiency);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Fluid Load Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Fluid Load Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Fluid Load Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Fluid Load Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Fluid Load Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Fluid Load Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Fluid Load Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Fluid Load Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Fluid Load Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Fluid Load Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Fluid Load Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Fluid Load Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class FluidLoadManager : FluidLoadEmployee
{
    protected float truckStaging;
    protected float teamSynchronization;
    protected float loadingOversight;

    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + truckStaging + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + teamSynchronization + loadingOversight + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + truckStaging + teamSynchronization + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + loadingOversight + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + +combinedTraits.experience);
    }

}
public class QualityControlEmployee : Employee
{
    protected float attentionToDetail; // Improves defect detection
    protected float inspectionSpeed;   // Speeds up inspection process
    protected float productKnowledge;  // Helps handle specific products


    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + attentionToDetail + inspectionSpeed + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + attentionToDetail + productKnowledge + combinedTraits.efficiency);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + inspectionSpeed + combinedTraits.speed);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + productKnowledge + combinedTraits.experience);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Quality Control Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Quality Control Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Quality Control Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Quality Control Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Quality Control Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Quality Control Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Quality Control Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Quality Control Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Quality Control Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Quality Control Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Quality Control Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Quality Control Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class QualityControlManager : QualityControlEmployee
{
    protected float inspectionProtocols;
    protected float defectReporting;
    protected float continuousImprovement;


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + +combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + inspectionProtocols + defectReporting + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + defectReporting + continuousImprovement + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + +combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + inspectionProtocols + continuousImprovement + combinedTraits.experience);
    }

}
public class MaintenanceEmployee : Employee
{
    protected float repairSpeed;       // Increases equipment repair speed
    protected float preventativeMaintenance; // Reduces equipment failures
    protected float toolMastery;      // Increases repair tool effectiveness


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + repairSpeed + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + repairSpeed + combinedTraits.focus);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + preventativeMaintenance + combinedTraits.stamina);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + preventativeMaintenance + toolMastery + combinedTraits.efficiency);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + toolMastery + combinedTraits.experience);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Maintenance Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Maintenance Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Maintenance Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Maintenance Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Maintenance Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Maintenance Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Maintenance Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Maintenance Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Maintenance Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Maintenance Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Maintenance Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Maintenance Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class MaintenanceManager : MaintenanceEmployee
{
    protected float repairWorkflow;
    protected float partInventory;
    protected float maintenanceScheduling;

    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + repairWorkflow + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + partInventory + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + repairWorkflow + maintenanceScheduling + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + maintenanceScheduling + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + +combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + partInventory + combinedTraits.experience);
    }

}
public class RoboticsEmployee : Employee
{
    protected float robotCalibration;  // Ensures peak robot efficiency
    protected float speedEnhancement;  // Increases robot movement speed
    protected float accuracy;          // Reduces robot movement errors


    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + robotCalibration + accuracy + combinedTraits.efficiency);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + robotCalibration + accuracy + combinedTraits.focus);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + speedEnhancement + combinedTraits.speed);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Robotics Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Robotics Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Robotics Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Robotics Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Robotics Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Robotics Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Robotics Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Robotics Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Robotics Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Robotics Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Robotics Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Robotics Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class RoboticsManager : RoboticsEmployee
{
    protected float automationPlanning;
    protected float firmwareManagement;
    protected float robotUptime;

    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + robotUptime + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + firmwareManagement + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + automationPlanning + firmwareManagement + robotUptime + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + automationPlanning + combinedTraits.experience);
    }

}
public class SecurityEmployee : Employee
{
    protected float surveillance;      // Increases monitoring ability
    protected float alertness;        // Improves breach response time
    protected float patrolSpeed;      // Increases patrol coverage speed


    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + surveillance + alertness + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + surveillance + patrolSpeed + combinedTraits.efficiency);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + alertness + patrolSpeed + combinedTraits.speed);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Security Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Security Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Security Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Security Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Security Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Security Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Security Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Security Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Security Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Security Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Security Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Security Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}

public class SecurityManager : SecurityEmployee
{
    protected float surveillanceOversight; // Increases monitoring effectiveness
    protected float patrolRouting; // Optimizes patrol routes
    protected float threatAssessment; // Improves threat detection


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + patrolRouting + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + surveillanceOversight + threatAssessment + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + patrolRouting + threatAssessment + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + surveillanceOversight + combinedTraits.experience);
    }

}
public class CleaningEmployee : Employee
{
    protected float thoroughness;     // Ensures proper cleaning
    protected float routineMaintenance;// Maintains workspace cleanliness


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + combinedTraits.speed);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + routineMaintenance + combinedTraits.stamina);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + thoroughness + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + thoroughness + routineMaintenance + combinedTraits.efficiency);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Cleaning Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Cleaning Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Cleaning Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Cleaning Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Cleaning Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Cleaning Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Cleaning Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Cleaning Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Cleaning Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Cleaning Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Cleaning Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Cleaning Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}
public class CleaningManager : CleaningEmployee
{
    protected float zonePrioritization;  // Determines cleaning order
    protected float supplyManagement; // Ensures cleaning supplies are stocked
    protected float cleanlinessStandards; // Sets cleaning benchmarks


    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + +combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + zonePrioritization + cleanlinessStandards + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + zonePrioritization + supplyManagement + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + cleanlinessStandards + combinedTraits.stamina);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + supplyManagement + combinedTraits.experience);
    }

}
public class LearningEmployee : Employee
{
    protected float trainingEffectiveness; // Improves training programs
    protected float skillTransfer;        // Increases skill gain rate
    protected float motivation;           // Increases training completion


    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + trainingEffectiveness + skillTransfer + combinedTraits.experience);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + trainingEffectiveness + skillTransfer + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + motivation + combinedTraits.stamina);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + combinedTraits.focus);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Learning Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Learning Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Learning Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Learning Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Learning Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Learning Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Learning Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Learning Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Learning Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Learning Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Learning Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Learning Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}
public class LearningManager : LearningEmployee
{
    protected float curriculumDesign;
    protected float progressTracking;
    protected float upskillingStrategy;

    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + curriculumDesign + progressTracking + upskillingStrategy + combinedTraits.experience);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + curriculumDesign + upskillingStrategy + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + +combinedTraits.stamina);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + combinedTraits.speed);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + progressTracking + combinedTraits.focus);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }

}
public class SafetyEmployee : Employee
{
    protected float hazardIdentification; // Detects potential issues
    protected float accidentPrevention;   // Reduces accident likelihood
    protected float emergencyResponse;    // Improves emergency handling

    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + hazardIdentification + emergencyResponse + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + hazardIdentification + accidentPrevention + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + accidentPrevention + combinedTraits.stamina);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + emergencyResponse + combinedTraits.speed);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
    }

    public new void PrimaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Safety Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Safety Employee {employeeName} is performing a primary action.");
        if (department.newActionRequests.Count == 0)
        {
            Debug.Log($"Safety Employee {employeeName} has no action requests to handle.");
            return;
        }
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleActionProcess(actionRequest));
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Safety Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Safety Employee {employeeName} is performing a secondary action.");
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Safety Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        int experiencePoints = department.claimedActionRequests.Count * 5;
        AddExperience(experiencePoints);
        department.claimedActionRequests.Clear();
    }

    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Safety Employee {employeeName} has canceled the current action.");
            StopAllCoroutines();
            actionState = ActionState.State.Idle;
        }
        else
        {
            Debug.Log($"Safety Employee {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleActionProcess(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        yield return StartCoroutine(HandleActionRequest(actionRequest));
        actionState = ActionState.State.Idle;
    }

    IEnumerator HandleActionRequest(ActionRequest actionRequest)
    {
        Debug.Log($"Safety Employee {employeeName} is handling an action request.");
        for (int i = 0; i < department.statTimes.Count; i++)
        {
            Debug.Log($"Safety Employee {employeeName} is performing action {department.statTimes[i].Key}({i + 1}) for {department.statTimes[i].Value} seconds.");
            yield return new WaitForSeconds(department.statTimes[i].Value);
        }
        float successChance = Random.Range(0f, 2f);
        if (successChance <= GetStatAverage())
        {
            Debug.Log($"Safety Employee {employeeName} successfully handled the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
        }
        else
        {
            Debug.Log($"Safety Employee {employeeName} failed to handle the action request.");
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
        }
        department.claimedActionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest);
        AddExperience(10);
    }
}
public class SafetyManager : SafetyEmployee
{
    protected float auditExecution; // Ensures safety audits are done
    protected float trainingEnforcement; // Ensures safety training is followed
    protected float incidentReview; // Reviews past incidents for improvement


    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + auditExecution + incidentReview + combinedTraits.focus);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + trainingEnforcement + combinedTraits.efficiency);
    }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetSpeed()
    {
        return Mathf.Min(2f, speed + incidentReview + combinedTraits.speed);
    }
    public new float GetStrength()
    {
        return Mathf.Min(2f, strength + combinedTraits.strength);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + trainingEnforcement + auditExecution + combinedTraits.experience);
    }

}
#endregion