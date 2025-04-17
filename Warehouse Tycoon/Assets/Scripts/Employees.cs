using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Employee : MonoBehaviour
{
    public int id; // Unique identifier for the employee
    public string employeeName; // Name of the employee
    public int level; // Level of the employee
    public bool levelPending; // Flag to indicate if the employee's level is pending
    public int exp; // Experience points of the employee
    public int salary; // Salary of the employee
    public int cost; // Cost of the employee
    public int departmentId; // Identifier for the department the employee belongs to
    public EmployeeType.Type employeeType; // Type of the employee (e.g., HR, IT, etc.)
    public DepartmentTypes.Type departmentType; // Type of the department the employee belongs to
    public bool departmentPending; // Flag to indicate if the department is pending
    public Department department; // Reference to the department the employee belongs to
    // Core Stats
    public float speed;      // How fast the employee moves items or travels between zones
    public float efficiency; // How well they use time/resources (less downtime, fewer mistakes)
    public float stamina;    // How long they can work before needing rest or a break
    public float strength;   // Affects how heavy of an item they can carry or how many items at once
    public float focus;      // Impacts accuracy and likelihood of making errors
    public float experience; // Affects leveling up, promotions, or unlocking new roles
    public List<TraitValues> traits = new List<TraitValues>(); // Employee traits
    public TraitValues combinedTraits; // Combined traits for the employee
    public ActionState.State actionState; // Current action state of the employee
    public float stateTimer = 0f; // Timer for current state
    public float workInterval = 0.5f; // How often to update work state
    public float restInterval = 1f; // How often to update rest state
    public int infractions = 0; // Number of infractions the employee has received
    public List<Disablers.Disabler> disablers = new List<Disablers.Disabler>(); // List of disablers affecting the employee
    public List<ActionRequest> actionRequests = new List<ActionRequest>(); // List of action requests associated with the employee

    // Department-specific stats
    // HR Department
    public float empathy;           // Increases morale and reduces turnover
    public float conflictResolution;// Ability to handle disputes
    public float recruiting;        // Increases likelihood of hiring skilled employees

    // HR Department (Manager)
    public float moraleBoost;        // Increases team performance
    public float retentionStrategy; // Improves long-term HR strategies
    public float policyEnforcement; // Enhances contract negotiations

    // IT Department
    public float techTroubleshooter; // Ability to fix technical issues quickly
    public float systemOptimization; // Enhances warehouse system efficiency
    public float security;           // Protects against cyber threats

    // IT Department (Manager)  
    public float infrastructureOversight; // Increases project completion speed
    public float incidentResponse;    // Boosts team performance
    public float techBudgeting;  // Improves long-term IT strategies

    // Operations Department
    public float logisticsPlanning;
    public float taskManagement;
    public float coordination;

    // Operations Department (Manager)
    public float processOptimization;
    public float crossDepartmentSync;
    public float kpiMonitoring;

    // Inbound Department
    public float loadMaster;        // Speeds up unloading process
    public float inventoryCheck;    // Ensures accurate receiving records
    public float speedyUnloader;    // Increases unloading speed

    // Inbound Department (Manager)
    public float dockFlowManagement;
    public float receivingAccuracy;
    public float supplierCoordination;

    // Outbound Department
    public float shippingAccuracy;  // Ensures correct destination shipping
    public float loadEfficiency;    // Increases outbound loading efficiency
    public float timeManagement;    // Reduces shipping delays

    // Outbound Department (Manager)
    public float loadScheduling;
    public float accuracyOversight;
    public float carrierCoordination;

    // Sorting Department
    public float sortingSpeed;      // Increases sorting speed
    public float sortingAccuracy;          // Reduces sorting errors
    public float patternRecognition;// Identifies efficient sorting patterns

    // Sorting Department (Manager)
    public float sortLineOversight;
    public float errorReductionPlanning;
    public float peakPrep;

    // Repacking Department
    public float packingEfficiency; // Maximizes box space usage
    public float damageControl;     // Reduces item damage risk

    // Repacking Department (Manager)
    public float qualityCheck;
    public float materialAllocation;
    public float repackFlow;

    // Palletizing Department
    public float palletEfficiency;  // Organizes items efficiently
    public float heavyLifting;      // Increases stacking speed/strength
    public float stackingPrecision; // Ensures secure stacking

    // Palletizing Department (Manager)
    public float stackingSupervision;
    public float loadForecasting;
    public float safetyChecks;

    // Water Spider Department
    public float routeEfficiency;   // Finds fastest delivery routes
    public float carryCapacity;     // Increases item carry limit
    public float supportSpeed;      // Quick restocking and assistance

    // Water Spider Department (Manager)
    public float routePlanning;
    public float supportCoordination;
    public float loadDistribution;

    // Fluid Load Department
    public float loadingSpeed;      // Increases truck loading speed
    public float hardHatProtection; // Reduces accident risk
    public float weightDistribution;// Helps distribute load weight evenly

    // Fluid Load Department (Manager)
    public float truckStaging;
    public float teamSynchronization;
    public float loadingOversight;

    // Quality Control Department
    public float attentionToDetail; // Improves defect detection
    public float inspectionSpeed;   // Speeds up inspection process
    public float productKnowledge;  // Helps handle specific products

    // Quality Control Department (Manager)
    public float inspectionProtocols;
    public float defectReporting;
    public float continuousImprovement;

    // Maintenance Department
    public float repairSpeed;       // Increases equipment repair speed
    public float preventativeMaintenance; // Reduces equipment failures
    public float toolMastery;       // Increases repair tool effectiveness

    // Maintenance Department (Manager)
    public float repairWorkflow;
    public float partInventory;
    public float maintenanceScheduling;

    // Robotics Department
    public float robotCalibration;  // Ensures peak robot efficiency
    public float speedEnhancement;  // Increases robot movement speed
    public float roboticsAccuracy;          // Reduces robot movement errors

    // Robotics Department (Manager)
    public float automationPlanning;
    public float firmwareManagement;
    public float robotUptime;

    // Security Department
    public float surveillance;      // Increases monitoring ability
    public float alertness;         // Improves breach response time
    public float patrolSpeed;       // Increases patrol coverage speed

    // Security Department (Manager)
    public float surveillanceOversight; // Increases monitoring effectiveness
    public float patrolRouting; // Optimizes patrol routes
    public float threatAssessment; // Improves threat detection

    // Cleaning Department
    public float thoroughness;      // Ensures proper cleaning
    public float routineMaintenance;// Maintains workspace cleanliness

    // Cleaning Department (Manager)
    public float zonePrioritization;  // Determines cleaning order
    public float supplyManagement; // Ensures cleaning supplies are stocked
    public float cleanlinessStandards; // Sets cleaning benchmarks

    // Learning Department
    public float trainingEffectiveness; // Improves training programs
    public float skillTransfer;        // Increases skill gain rate
    public float motivation;           // Increases training completion

    // Learning Department (Manager)
    public float curriculumDesign;
    public float progressTracking;
    public float upskillingStrategy;

    // Safety Department
    public float hazardIdentification; // Detects potential issues
    public float accidentPrevention;   // Reduces accident likelihood
    public float emergencyResponse;    // Improves emergency handling

    //  Safety Department (Manager)
    public float auditExecution; // Ensures safety audits are done
    public float trainingEnforcement; // Ensures safety training is followed
    public float incidentReview; // Reviews past incidents for improvement

    // Recruiting Department
    public float talentScouting;       // Identifies potential candidates
    public float interviewingSkills;   // Conducts effective interviews
    public float onboardingEfficieny;  // Streamlines new hire integration

    // Recruiting Department (Manager)
    public float candidatePipelineManagement; // Manages candidate flow
    public float interviewOversight; // Ensures interview quality
    public float onboardingStrategy; // Improves new hire integration



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
    public Employee(Employee employee)
    {
        id = employee.id;
        employeeName = employee.employeeName;
        level = employee.level;
        exp = employee.exp;
        salary = employee.salary;
        cost = employee.cost;
        departmentId = employee.departmentId;
        employeeType = employee.employeeType;
        departmentType = employee.departmentType;
        departmentPending = employee.departmentPending;
        department = employee.department;

        // Copy core stats
        speed = employee.speed;
        efficiency = employee.efficiency;
        stamina = employee.stamina;
        strength = employee.strength;
        focus = employee.focus;
        experience = employee.experience;

        // Copy traits
        traits = new List<TraitValues>(employee.traits);
        combinedTraits = employee.combinedTraits; // Copy combined traits
        actionState = employee.actionState; // Copy action state
        stateTimer = employee.stateTimer; // Copy state timer
        workInterval = employee.workInterval; // Copy work interval
        restInterval = employee.restInterval; // Copy rest interval

        // Copy department-specific stats
        empathy = employee.empathy;
        conflictResolution = employee.conflictResolution;
        recruiting = employee.recruiting;
        techTroubleshooter = employee.techTroubleshooter;
        systemOptimization = employee.systemOptimization;
        security = employee.security;
        logisticsPlanning = employee.logisticsPlanning;
        taskManagement = employee.taskManagement;
        coordination = employee.coordination;
        loadMaster = employee.loadMaster;
        inventoryCheck = employee.inventoryCheck;
        speedyUnloader = employee.speedyUnloader;
        shippingAccuracy = employee.shippingAccuracy;
        loadEfficiency = employee.loadEfficiency;
        timeManagement = employee.timeManagement;
        sortingSpeed = employee.sortingSpeed;
        sortingAccuracy = employee.sortingAccuracy;
        patternRecognition = employee.patternRecognition;
        packingEfficiency = employee.packingEfficiency;
        damageControl = employee.damageControl;
        palletEfficiency = employee.palletEfficiency;
        heavyLifting = employee.heavyLifting;
        stackingPrecision = employee.stackingPrecision;
        routeEfficiency = employee.routeEfficiency;
        carryCapacity = employee.carryCapacity;
        supportSpeed = employee.supportSpeed;
        loadingSpeed = employee.loadingSpeed;
        hardHatProtection = employee.hardHatProtection;
        weightDistribution = employee.weightDistribution;
        attentionToDetail = employee.attentionToDetail;
        inspectionSpeed = employee.inspectionSpeed;
        productKnowledge = employee.productKnowledge;
        repairSpeed = employee.repairSpeed;
        preventativeMaintenance = employee.preventativeMaintenance;
        toolMastery = employee.toolMastery;
        robotCalibration = employee.robotCalibration;
        speedEnhancement = employee.speedEnhancement;
        roboticsAccuracy = employee.roboticsAccuracy;
        surveillance = employee.surveillance;
        alertness = employee.alertness;
        patrolSpeed = employee.patrolSpeed;
        thoroughness = employee.thoroughness;
        routineMaintenance = employee.routineMaintenance;
        trainingEffectiveness = employee.trainingEffectiveness;
        skillTransfer = employee.skillTransfer;
        motivation = employee.motivation;
        hazardIdentification = employee.hazardIdentification;
        accidentPrevention = employee.accidentPrevention;
        emergencyResponse = employee.emergencyResponse;
        talentScouting = employee.talentScouting;
        interviewingSkills = employee.interviewingSkills;
        onboardingEfficieny = employee.onboardingEfficieny;
        // Copy manager-specific stats
        moraleBoost = employee.moraleBoost;
        retentionStrategy = employee.retentionStrategy;
        policyEnforcement = employee.policyEnforcement;
        infrastructureOversight = employee.infrastructureOversight;
        incidentResponse = employee.incidentResponse;
        techBudgeting = employee.techBudgeting;
        processOptimization = employee.processOptimization;
        crossDepartmentSync = employee.crossDepartmentSync;
        kpiMonitoring = employee.kpiMonitoring;
        dockFlowManagement = employee.dockFlowManagement;
        receivingAccuracy = employee.receivingAccuracy;
        supplierCoordination = employee.supplierCoordination;
        loadScheduling = employee.loadScheduling;
        accuracyOversight = employee.accuracyOversight;
        carrierCoordination = employee.carrierCoordination;
        sortLineOversight = employee.sortLineOversight;
        errorReductionPlanning = employee.errorReductionPlanning;
        peakPrep = employee.peakPrep;
        stackingSupervision = employee.stackingSupervision;
        loadForecasting = employee.loadForecasting;
        safetyChecks = employee.safetyChecks;
        routePlanning = employee.routePlanning;
        supportCoordination = employee.supportCoordination;
        loadDistribution = employee.loadDistribution;
        truckStaging = employee.truckStaging;
        teamSynchronization = employee.teamSynchronization;
        loadingOversight = employee.loadingOversight;
        inspectionProtocols = employee.inspectionProtocols;
        defectReporting = employee.defectReporting;
        continuousImprovement = employee.continuousImprovement;
        repairWorkflow = employee.repairWorkflow;
        partInventory = employee.partInventory;
        maintenanceScheduling = employee.maintenanceScheduling;
        automationPlanning = employee.automationPlanning;
        firmwareManagement = employee.firmwareManagement;
        robotUptime = employee.robotUptime;
        surveillanceOversight = employee.surveillanceOversight;
        patrolRouting = employee.patrolRouting;
        threatAssessment = employee.threatAssessment;
        zonePrioritization = employee.zonePrioritization;
        supplyManagement = employee.supplyManagement;
        cleanlinessStandards = employee.cleanlinessStandards;
        curriculumDesign = employee.curriculumDesign;
        progressTracking = employee.progressTracking;
        upskillingStrategy = employee.upskillingStrategy;
        auditExecution = employee.auditExecution;
        trainingEnforcement = employee.trainingEnforcement;
        incidentReview = employee.incidentReview;
        candidatePipelineManagement = employee.candidatePipelineManagement;
        interviewOversight = employee.interviewOversight;
        onboardingStrategy = employee.onboardingStrategy;
    }
    public Employee(string name, int level, int exp, int salary)
    {
        employeeName = name;
        this.level = level;
        this.exp = exp;
        this.salary = salary;

        // Initialize core stats with default values
        speed = 1f;
        efficiency = 1f;
        stamina = 1f;
        strength = 1f;
        focus = 1f;
        experience = 1f;
    }
    public float GetStatValue(StatTypes.Type statType)
    {
        return statType switch
        {
            StatTypes.Type.Speed => speed,
            StatTypes.Type.Efficiency => efficiency,
            StatTypes.Type.Stamina => stamina,
            StatTypes.Type.Strength => strength,
            StatTypes.Type.Focus => focus,
            StatTypes.Type.Experience => experience,
            _ => throw new System.ArgumentOutOfRangeException(nameof(statType), "Invalid stat type.")
        };
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
        ActionRequest actionRequest = department.newActionRequests[0];
        StartCoroutine(HandleRequestSequence(actionRequest));
    } // Placeholder for primary action
    public void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Employee {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Employee {employeeName} is performing a secondary action.");
        // Check for claimedActionRequests in the department
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Employee {employeeName} has no claimed action requests to review.");
            return;
        }
        // Get all claimedActionRequests from the department that have the status of "completed"
        List<ActionRequest> completedRequests = department.claimedActionRequests.Where(request => request.status == ActionRequest.StatusType.Type.Completed).ToList();
        // Give experience points based on completedRequests count
        int experiencePoints = completedRequests.Count * 5;
        // Update employee experience and check for level up
        AddExperience(experiencePoints);
        // Delete the completed requests from the department
        foreach (ActionRequest request in completedRequests)
        {
            department.claimedActionRequests.Remove(request);
        }
        Debug.Log($"Employee {employeeName} has reviewed and cleared {completedRequests.Count} completed action requests.");
    }
    public void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Employee {employeeName} has canceled the current action.");
            // Check for a action request
            ActionRequest actionRequest = actionRequests.FirstOrDefault();
            actionRequest.status = ActionRequest.StatusType.Type.Pending; // Set status to canceled
            StopAllCoroutines(); // Stop all ongoing actions
            actionState = ActionState.State.Idle; // Set state to idle
        }
        else
        {
            Debug.Log($"Employee {employeeName} is not currently working on any action.");
        }
    }
    IEnumerator HandleRequestSequence(ActionRequest actionRequest)
    {
        actionState = ActionState.State.Working;
        actionRequest.status = ActionRequest.StatusType.Type.InProgress;
        yield return StartCoroutine(actionRequest.action);
        // generate random number for success rate
        bool successful = Random.Range(1, 101) <= GetStatAverage() * 100;
        if (successful)
        {
            actionRequest.status = ActionRequest.StatusType.Type.Completed;
            actionRequests.Remove(actionRequest); // Remove the action request from the employee's list
            department.claimedActionRequests.Add(actionRequest); // Add the action request to the department's claimed list
            Debug.Log($"Employee {employeeName} successfully completed the action request.");
            // Update employee experience based on the action request
            AddExperience(10); // Example experience points for completing an action request
        }
        else
        {
            actionRequest.status = ActionRequest.StatusType.Type.Failed;
            Debug.Log($"Employee {employeeName} failed the action request.");
            // Update employee experience based on the action request
            AddExperience(5); // Example experience points for completing an action request
        }
        actionState = ActionState.State.Idle;
    }
    public void AddDisabler(Disablers.Disabler disabler)
    {
        if (!disablers.Contains(disabler))
        {
            disablers.Add(disabler);
        }
    }
    public void RemoveDisabler(Disablers.Disabler disabler)
    {
        if (disablers.Contains(disabler))
        {
            disablers.Remove(disabler);
        }
    }
}
public class Manager : Employee
{
    public Manager() { } // Default constructor
    public Manager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetStamina()
    {
        return Mathf.Min(2f, stamina + combinedTraits.stamina);
    }
    public new float GetEfficiency()
    {
        return Mathf.Min(2f, efficiency + combinedTraits.efficiency);
    }
    public new float GetFocus()
    {
        return Mathf.Min(2f, focus + combinedTraits.focus);
    }
    public new float GetExperience()
    {
        return Mathf.Min(2f, experience + combinedTraits.experience);
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
        Debug.Log($"Manager {employeeName} is performing a secondary action.");
        StartCoroutine(HandleAssigningSequence());
    }

    public new void SecondaryAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Manager {employeeName} is busy with another action.");
            return;
        }
        Debug.Log($"Manager {employeeName} is performing a secondary action.");
        // Check for claimedActionRequests in the department
        if (department.claimedActionRequests.Count == 0)
        {
            Debug.Log($"Manager {employeeName} has no claimed action requests to review.");
            return;
        }
        // Get all claimedActionRequests from the department that have the status of "failed"
        List<ActionRequest> failedRequests = department.claimedActionRequests.Where(request => request.status == ActionRequest.StatusType.Type.Failed).ToList();
        // Reset all failed requests to "pending" status
        foreach (ActionRequest request in failedRequests)
        {
            request.status = ActionRequest.StatusType.Type.Pending;
            Debug.Log($"Manager {employeeName} has reset the status of an action request to pending.");
        }
        // Clear the failed requests from the department
        foreach (ActionRequest request in failedRequests)
        {
            department.claimedActionRequests.Remove(request);
        }
        // Add failed requests back to newActionRequests
        foreach (ActionRequest request in failedRequests)
        {
            department.newActionRequests.Add(request);
        }
        Debug.Log($"Manager {employeeName} has reviewed and reset {failedRequests.Count} failed action requests.");
    }
    public new void CancelAction()
    {
        if (actionState != ActionState.State.Idle)
        {
            Debug.Log($"Manager {employeeName} has canceled the current action.");
            StopAllCoroutines(); // Stop all ongoing actions
            actionState = ActionState.State.Idle; // Set state to idle
        }
        else
        {
            Debug.Log($"Manager {employeeName} is not currently working on any action.");
        }
    }

    IEnumerator HandleAssigningSequence()
    {
        actionState = ActionState.State.Working;
        // Get the first action request from the department
        ActionRequest actionRequest = department.newActionRequests[0];
        // Get the employee with the least action requests
        Employee employee = department.employees.OrderBy(e => e.actionRequests.Count).FirstOrDefault();
        if (employee == null)
        {
            yield break;
        }
        yield return StartCoroutine(AssignRequestToEmployee(actionRequest, employee));
        // Check if the action request was successful
        AddExperience(10); // Example experience points for completing an action request
        actionState = ActionState.State.Idle;
    }
    public IEnumerator AssignRequestToEmployee(ActionRequest actionRequest, Employee employee)
    {
        // Check if the employee is available for the action request
        if (employee.actionState != ActionState.State.Idle)
        {
            Debug.Log($"Employee {employee.employeeName} is busy with another action.");
            yield break;
        }
        // Assign the action request to the employee
        actionRequest.employee = employee; // Set the employee for the action request
        employee.actionRequests.Add(actionRequest);
        department.newActionRequests.Remove(actionRequest); // Remove the action request from the department's newActionRequests
        Debug.Log($"Manager {employeeName} has assigned action request to employee {employee.employeeName}.");
    }
}
// Dept. Employees
public class HREmployee : Employee
{
    public HREmployee() { } // Default constructor
    public HREmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetStamina() { return Mathf.Min(2f, stamina + empathy + combinedTraits.stamina); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + conflictResolution + empathy + recruiting + combinedTraits.efficiency); }
    public new float GetFocus() { return Mathf.Min(2f, focus + conflictResolution + combinedTraits.focus); }
    public new float GetExperience() { return Mathf.Min(2f, experience + recruiting + combinedTraits.experience); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + combinedTraits.speed); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
}

public class HRManager : HREmployee
{
    public HRManager() { } // Default constructor
    public HRManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + policyEnforcement + retentionStrategy + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + policyEnforcement + moraleBoost + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + moraleBoost + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + retentionStrategy + combinedTraits.experience); }
}

public class ITEmployee : Employee
{
    public ITEmployee() { } // Default constructor
    public ITEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + techTroubleshooter + systemOptimization + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + techTroubleshooter + security + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + systemOptimization + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + security + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class ITManager : ITEmployee
{
    public ITManager() { } // Default constructor
    public ITManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + incidentResponse + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + incidentResponse + infrastructureOversight + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + infrastructureOversight + techBudgeting + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + techBudgeting + combinedTraits.experience); }
}

public class OperationsEmployee : Employee
{
    public OperationsEmployee() { } // Default constructor
    public OperationsEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + logisticsPlanning + taskManagement + coordination + combinedTraits.efficiency); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + logisticsPlanning + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + taskManagement + combinedTraits.focus); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + coordination + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class OperationsManager : OperationsEmployee
{
    public OperationsManager() { } // Default constructor
    public OperationsManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + processOptimization + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + crossDepartmentSync + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + kpiMonitoring + crossDepartmentSync + processOptimization + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + kpiMonitoring + combinedTraits.experience); }
}

public class InboundEmployee : Employee
{
    public InboundEmployee() { } // Default constructor
    public InboundEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + loadMaster + speedyUnloader + combinedTraits.speed); }
    public new float GetStrength() { return Mathf.Min(2f, strength + loadMaster + combinedTraits.strength); }
    public new float GetFocus() { return Mathf.Min(2f, focus + inventoryCheck + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + inventoryCheck + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + speedyUnloader + combinedTraits.stamina); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class InboundManager : InboundEmployee
{
    public InboundManager() { } // Default constructor
    public InboundManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + dockFlowManagement + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + receivingAccuracy + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + supplierCoordination + receivingAccuracy + dockFlowManagement + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + supplierCoordination + combinedTraits.experience); }
}

public class OutboundEmployee : Employee
{
    public OutboundEmployee() { } // Default constructor
    public OutboundEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetFocus() { return Mathf.Min(2f, focus + shippingAccuracy + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + shippingAccuracy + loadEfficiency + timeManagement + combinedTraits.efficiency); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + loadEfficiency + timeManagement + combinedTraits.speed); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class OutboundManager : OutboundEmployee
{
    public OutboundManager() { } // Default constructor
    public OutboundManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + loadScheduling + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + accuracyOversight + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + carrierCoordination + loadScheduling + accuracyOversight + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + carrierCoordination + combinedTraits.experience); }
}

public class SortingEmployee : Employee
{
    public SortingEmployee() { } // Default constructor
    public SortingEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + sortingSpeed + patternRecognition + combinedTraits.speed); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + sortingSpeed + sortingAccuracy + patternRecognition + combinedTraits.efficiency); }
    public new float GetFocus() { return Mathf.Min(2f, focus + sortingAccuracy + combinedTraits.focus); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class SortingManager : SortingEmployee
{
    public SortingManager() { } // Default constructor
    public SortingManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + sortLineOversight + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + errorReductionPlanning + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + sortLineOversight + errorReductionPlanning + peakPrep + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + peakPrep + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + +combinedTraits.experience); }
}

public class RepackingEmployee : Employee
{
    public RepackingEmployee() { } // Default constructor
    public RepackingEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + packingEfficiency + damageControl + combinedTraits.efficiency); }
    public new float GetFocus() { return Mathf.Min(2f, focus + packingEfficiency + damageControl + combinedTraits.focus); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + combinedTraits.speed); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class RepackingManager : RepackingEmployee
{
    public RepackingManager() { } // Default constructor
    public RepackingManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + repackFlow + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + qualityCheck + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + qualityCheck + materialAllocation + repackFlow + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + materialAllocation + combinedTraits.experience); }
}

public class PalletizingEmployee : Employee
{
    public PalletizingEmployee() { } // Default constructor
    public PalletizingEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + palletEfficiency + stackingPrecision + combinedTraits.efficiency); }
    public new float GetStrength() { return Mathf.Min(2f, strength + palletEfficiency + heavyLifting + combinedTraits.strength); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + heavyLifting + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + stackingPrecision + combinedTraits.focus); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class PalletizingManager : PalletizingEmployee
{
    public PalletizingManager() { } // Default constructor
    public PalletizingManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + stackingSupervision + safetyChecks + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + stackingSupervision + loadForecasting + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + safetyChecks + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + loadForecasting + combinedTraits.experience); }
}

public class WaterSpiderEmployee : Employee
{
    public WaterSpiderEmployee() { } // Default constructor
    public WaterSpiderEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + routeEfficiency + supportSpeed + combinedTraits.speed); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + routeEfficiency + carryCapacity + combinedTraits.efficiency); }
    public new float GetStrength() { return Mathf.Min(2f, strength + carryCapacity + combinedTraits.strength); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + supportSpeed + combinedTraits.stamina); }
    public new float GetFocus() { return Mathf.Min(2f, focus + combinedTraits.focus); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class WaterSpiderManager : WaterSpiderEmployee
{
    public WaterSpiderManager() { } // Default constructor
    public WaterSpiderManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + routePlanning + combinedTraits.speed); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + routePlanning + supportCoordination + loadDistribution + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + loadDistribution + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetFocus() { return Mathf.Min(2f, focus + supportCoordination + combinedTraits.focus); }
    public new float GetExperience() { return Mathf.Min(2f, experience + +combinedTraits.experience); }
}

public class FluidLoadEmployee : Employee
{
    public FluidLoadEmployee() { } // Default constructor
    public FluidLoadEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + loadingSpeed + combinedTraits.speed); }
    public new float GetStrength() { return Mathf.Min(2f, strength + loadingSpeed + combinedTraits.strength); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + hardHatProtection + combinedTraits.stamina); }
    public new float GetFocus() { return Mathf.Min(2f, focus + hardHatProtection + weightDistribution + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + weightDistribution + combinedTraits.efficiency); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class FluidLoadManager : FluidLoadEmployee
{
    public FluidLoadManager() { } // Default constructor
    public FluidLoadManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + truckStaging + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + teamSynchronization + loadingOversight + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + truckStaging + teamSynchronization + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + loadingOversight + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + +combinedTraits.experience); }
}

public class QualityControlEmployee : Employee
{
    public QualityControlEmployee() { } // Default constructor
    public QualityControlEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetFocus() { return Mathf.Min(2f, focus + attentionToDetail + inspectionSpeed + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + attentionToDetail + productKnowledge + combinedTraits.efficiency); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + inspectionSpeed + combinedTraits.speed); }
    public new float GetExperience() { return Mathf.Min(2f, experience + productKnowledge + combinedTraits.experience); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
}

public class QualityControlManager : QualityControlEmployee
{
    public QualityControlManager() { } // Default constructor
    public QualityControlManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + +combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + inspectionProtocols + defectReporting + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + defectReporting + continuousImprovement + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + +combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + inspectionProtocols + continuousImprovement + combinedTraits.experience); }
}

public class MaintenanceEmployee : Employee
{
    public MaintenanceEmployee() { } // Default constructor
    public MaintenanceEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + repairSpeed + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + repairSpeed + combinedTraits.focus); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + preventativeMaintenance + combinedTraits.stamina); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + preventativeMaintenance + toolMastery + combinedTraits.efficiency); }
    public new float GetExperience() { return Mathf.Min(2f, experience + toolMastery + combinedTraits.experience); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
}

public class MaintenanceManager : MaintenanceEmployee
{
    public MaintenanceManager() { } // Default constructor
    public MaintenanceManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + repairWorkflow + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + partInventory + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + repairWorkflow + maintenanceScheduling + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + maintenanceScheduling + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + +combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + partInventory + combinedTraits.experience); }
}

public class RoboticsEmployee : Employee
{
    public RoboticsEmployee() { } // Default constructor
    public RoboticsEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + robotCalibration + roboticsAccuracy + combinedTraits.efficiency); }
    public new float GetFocus() { return Mathf.Min(2f, focus + robotCalibration + roboticsAccuracy + combinedTraits.focus); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + speedEnhancement + combinedTraits.speed); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class RoboticsManager : RoboticsEmployee
{
    public RoboticsManager() { } // Default constructor
    public RoboticsManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + robotUptime + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + firmwareManagement + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + automationPlanning + firmwareManagement + robotUptime + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + automationPlanning + combinedTraits.experience); }
}

public class SecurityEmployee : Employee
{
    public SecurityEmployee() { } // Default constructor
    public SecurityEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetFocus() { return Mathf.Min(2f, focus + surveillance + alertness + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + surveillance + patrolSpeed + combinedTraits.efficiency); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + alertness + patrolSpeed + combinedTraits.speed); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class SecurityManager : SecurityEmployee
{
    public SecurityManager() { } // Default constructor
    public SecurityManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + patrolRouting + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + surveillanceOversight + threatAssessment + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + patrolRouting + threatAssessment + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + surveillanceOversight + combinedTraits.experience); }
}

public class CleaningEmployee : Employee
{
    public CleaningEmployee() { } // Default constructor
    public CleaningEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + combinedTraits.speed); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + routineMaintenance + combinedTraits.stamina); }
    public new float GetFocus() { return Mathf.Min(2f, focus + thoroughness + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + thoroughness + routineMaintenance + combinedTraits.efficiency); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class CleaningManager : CleaningEmployee
{
    public CleaningManager() { } // Default constructor
    public CleaningManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetSpeed() { return Mathf.Min(2f, speed + +combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + zonePrioritization + cleanlinessStandards + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + zonePrioritization + supplyManagement + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + cleanlinessStandards + combinedTraits.stamina); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + supplyManagement + combinedTraits.experience); }
}

public class LearningEmployee : Employee
{
    public LearningEmployee() { } // Default constructor
    public LearningEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetExperience() { return Mathf.Min(2f, experience + trainingEffectiveness + skillTransfer + combinedTraits.experience); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + trainingEffectiveness + skillTransfer + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + motivation + combinedTraits.stamina); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + combinedTraits.focus); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
}

public class LearningManager : LearningEmployee
{
    public LearningManager() { } // Default constructor
    public LearningManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetExperience() { return Mathf.Min(2f, experience + curriculumDesign + progressTracking + upskillingStrategy + combinedTraits.experience); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + curriculumDesign + upskillingStrategy + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + +combinedTraits.stamina); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + progressTracking + combinedTraits.focus); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
}

public class SafetyEmployee : Employee
{
    public SafetyEmployee() { } // Default constructor
    public SafetyEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetFocus() { return Mathf.Min(2f, focus + hazardIdentification + emergencyResponse + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + hazardIdentification + accidentPrevention + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + accidentPrevention + combinedTraits.stamina); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + emergencyResponse + combinedTraits.speed); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + combinedTraits.experience); }
}

public class SafetyManager : SafetyEmployee
{
    public SafetyManager() { } // Default constructor
    public SafetyManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetFocus() { return Mathf.Min(2f, focus + auditExecution + incidentReview + combinedTraits.focus); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + trainingEnforcement + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + incidentReview + combinedTraits.speed); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
    public new float GetExperience() { return Mathf.Min(2f, experience + trainingEnforcement + auditExecution + combinedTraits.experience); }
}

public class RecruitingEmployee : Employee
{
    public RecruitingEmployee() { } // Default constructor
    public RecruitingEmployee(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetExperience() { return Mathf.Min(2f, experience + interviewingSkills + combinedTraits.experience); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + talentScouting + onboardingEfficieny + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + onboardingEfficieny + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + interviewingSkills + talentScouting + combinedTraits.focus); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
}

public class RecruitingManager : RecruitingEmployee
{
    public RecruitingManager() { } // Default constructor
    public RecruitingManager(Employee existingEmployee) : base(existingEmployee) { }
    public new float GetExperience() { return Mathf.Min(2f, experience + interviewOversight + combinedTraits.experience); }
    public new float GetEfficiency() { return Mathf.Min(2f, efficiency + candidatePipelineManagement + onboardingStrategy + combinedTraits.efficiency); }
    public new float GetStamina() { return Mathf.Min(2f, stamina + combinedTraits.stamina); }
    public new float GetSpeed() { return Mathf.Min(2f, speed + onboardingStrategy + combinedTraits.speed); }
    public new float GetFocus() { return Mathf.Min(2f, focus + candidatePipelineManagement + interviewOversight + combinedTraits.focus); }
    public new float GetStrength() { return Mathf.Min(2f, strength + combinedTraits.strength); }
}
