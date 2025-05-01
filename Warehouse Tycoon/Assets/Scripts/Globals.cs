using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public struct SerializableEmployee
{
    public int id;
    public string employeeName;
    public int level;
    public bool levelPending;
    public int exp;
    public string spriteName; // Store sprite name instead of Sprite reference
    public int salary;
    public int cost;
    public int departmentId;
    public bool isHired;
    public bool isFired;
    public EmployeeType.Type employeeType;
    public DepartmentTypes.Type departmentType;
    public bool departmentPending;
    // Core Stats
    public float speed;
    public float efficiency;
    public float stamina;
    public float strength;
    public float focus;
    public float experience;
    // List of traits
    public List<TraitValues> traits;
    // Combined traits
    public TraitValues combinedTraits;
    public ActionState.State actionState;
    public float stateTimer;
    public float workInterval;
    public float restInterval;
    public int infractions;
    // Department-specific stats
    // HR Department
    public float empathy;
    public float conflictResolution;
    public float recruiting;
    // HR Department (Manager)
    public float moraleBoost;
    public float retentionStrategy;
    public float policyEnforcement;
    // IT Department
    public float techTroubleshooter;
    public float systemOptimization;
    public float security;
    // IT Department (Manager)  
    public float infrastructureOversight;
    public float incidentResponse;
    public float techBudgeting;
    // Operations Department
    public float logisticsPlanning;
    public float taskManagement;
    public float coordination;
    // Operations Department (Manager)
    public float processOptimization;
    public float crossDepartmentSync;
    public float kpiMonitoring;
    // Inbound Department
    public float loadMaster;
    public float inventoryCheck;
    public float speedyUnloader;
    // Inbound Department (Manager)
    public float dockFlowManagement;
    public float receivingAccuracy;
    public float supplierCoordination;
    // Outbound Department
    public float shippingAccuracy;
    public float loadEfficiency;
    public float timeManagement;
    // Outbound Department (Manager)
    public float loadScheduling;
    public float accuracyOversight;
    public float carrierCoordination;
    // Sorting Department
    public float sortingSpeed;
    public float sortingAccuracy;
    public float patternRecognition;
    // Sorting Department (Manager)
    public float sortLineOversight;
    public float errorReductionPlanning;
    public float peakPrep;
    // Repacking Department
    public float packingEfficiency;
    public float damageControl;
    // Repacking Department (Manager)
    public float qualityCheck;
    public float materialAllocation;
    public float repackFlow;
    // Palletizing Department
    public float palletEfficiency;
    public float heavyLifting;
    public float stackingPrecision;
    // Palletizing Department (Manager)
    public float stackingSupervision;
    public float loadForecasting;
    public float safetyChecks;
    // Water Spider Department
    public float routeEfficiency;
    public float carryCapacity;
    public float supportSpeed;
    // Water Spider Department (Manager)
    public float routePlanning;
    public float supportCoordination;
    public float loadDistribution;
    // Fluid Load Department
    public float loadingSpeed;
    public float hardHatProtection;
    public float weightDistribution;
    // Fluid Load Department (Manager)
    public float truckStaging;
    public float teamSynchronization;
    public float loadingOversight;
    // Quality Control Department
    public float attentionToDetail;
    public float inspectionSpeed;
    public float productKnowledge;
    // Quality Control Department (Manager)
    public float inspectionProtocols;
    public float defectReporting;
    public float continuousImprovement;
    // Maintenance Department
    public float repairSpeed;
    public float preventativeMaintenance;
    public float toolMastery;
    // Maintenance Department (Manager)
    public float repairWorkflow;
    public float partInventory;
    public float maintenanceScheduling;
    // Robotics Department
    public float robotCalibration;
    public float speedEnhancement;
    public float roboticsAccuracy;
    // Robotics Department (Manager)
    public float automationPlanning;
    public float firmwareManagement;
    public float robotUptime;
    // Security Department
    public float surveillance;
    public float alertness;
    public float patrolSpeed;
    // Security Department (Manager)
    public float surveillanceOversight;
    public float patrolRouting;
    public float threatAssessment;
    // Cleaning Department
    public float thoroughness;
    public float routineMaintenance;
    // Cleaning Department (Manager)
    public float zonePrioritization;
    public float supplyManagement;
    public float cleanlinessStandards;
    // Learning Department
    public float trainingEffectiveness;
    public float skillTransfer;
    public float motivation;
    // Learning Department (Manager)
    public float curriculumDesign;
    public float progressTracking;
    public float upskillingStrategy;
    // Safety Department
    public float hazardIdentification;
    public float accidentPrevention;
    public float emergencyResponse;
    // Safety Department (Manager)
    public float auditExecution;
    public float trainingEnforcement;
    public float incidentReview;
    // Recruiting Department
    public float talentScouting;
    public float interviewingSkills;
    public float onboardingEfficieny;
    // Recruiting Department (Manager)
    public float candidatePipelineManagement;
    public float interviewOversight;
    public float onboardingStrategy;

    public static SerializableEmployee FromEmployee(Employee employee)
    {
        return new SerializableEmployee
        {
            id = employee.id,
            employeeName = employee.employeeName,
            level = employee.level,
            levelPending = employee.levelPending,
            exp = employee.exp,
            spriteName = employee.employeeSprite != null ? employee.employeeSprite.name : "",
            salary = employee.salary,
            cost = employee.cost,
            departmentId = employee.department != null ? employee.department.departmentId : -1,
            isHired = employee.isHired,
            isFired = employee.isFired,
            employeeType = employee.employeeType,
            departmentType = employee.departmentType,
            departmentPending = employee.departmentPending,
            // Core Stats
            speed = employee.speed,
            efficiency = employee.efficiency,
            stamina = employee.stamina,
            strength = employee.strength,
            focus = employee.focus,
            experience = employee.experience,
            // Traits
            traits = employee.traits,
            combinedTraits = employee.combinedTraits,
            actionState = employee.actionState,
            stateTimer = employee.stateTimer,
            workInterval = employee.workInterval,
            restInterval = employee.restInterval,
            infractions = employee.infractions,
            // Department-specific stats
            empathy = employee.empathy,
            conflictResolution = employee.conflictResolution,
            recruiting = employee.recruiting,
            moraleBoost = employee.moraleBoost,
            retentionStrategy = employee.retentionStrategy,
            policyEnforcement = employee.policyEnforcement,
            techTroubleshooter = employee.techTroubleshooter,
            systemOptimization = employee.systemOptimization,
            security = employee.security,
            infrastructureOversight = employee.infrastructureOversight,
            incidentResponse = employee.incidentResponse,
            techBudgeting = employee.techBudgeting,
            logisticsPlanning = employee.logisticsPlanning,
            taskManagement = employee.taskManagement,
            coordination = employee.coordination,
            processOptimization = employee.processOptimization,
            crossDepartmentSync = employee.crossDepartmentSync,
            kpiMonitoring = employee.kpiMonitoring,
            loadMaster = employee.loadMaster,
            inventoryCheck = employee.inventoryCheck,
            speedyUnloader = employee.speedyUnloader,
            dockFlowManagement = employee.dockFlowManagement,
            receivingAccuracy = employee.receivingAccuracy,
            supplierCoordination = employee.supplierCoordination,
            shippingAccuracy = employee.shippingAccuracy,
            loadEfficiency = employee.loadEfficiency,
            timeManagement = employee.timeManagement,
            loadScheduling = employee.loadScheduling,
            accuracyOversight = employee.accuracyOversight,
            carrierCoordination = employee.carrierCoordination,
            sortingSpeed = employee.sortingSpeed,
            sortingAccuracy = employee.sortingAccuracy,
            patternRecognition = employee.patternRecognition,
            sortLineOversight = employee.sortLineOversight,
            errorReductionPlanning = employee.errorReductionPlanning,
            peakPrep = employee.peakPrep,
            packingEfficiency = employee.packingEfficiency,
            damageControl = employee.damageControl,
            qualityCheck = employee.qualityCheck,
            materialAllocation = employee.materialAllocation,
            repackFlow = employee.repackFlow,
            palletEfficiency = employee.palletEfficiency,
            heavyLifting = employee.heavyLifting,
            stackingPrecision = employee.stackingPrecision,
            stackingSupervision = employee.stackingSupervision,
            loadForecasting = employee.loadForecasting,
            safetyChecks = employee.safetyChecks,
            routeEfficiency = employee.routeEfficiency,
            carryCapacity = employee.carryCapacity,
            supportSpeed = employee.supportSpeed,
            routePlanning = employee.routePlanning,
            supportCoordination = employee.supportCoordination,
            loadDistribution = employee.loadDistribution,
            loadingSpeed = employee.loadingSpeed,
            hardHatProtection = employee.hardHatProtection,
            weightDistribution = employee.weightDistribution,
            truckStaging = employee.truckStaging,
            teamSynchronization = employee.teamSynchronization,
            loadingOversight = employee.loadingOversight,
            attentionToDetail = employee.attentionToDetail,
            inspectionSpeed = employee.inspectionSpeed,
            productKnowledge = employee.productKnowledge,
            inspectionProtocols = employee.inspectionProtocols,
            defectReporting = employee.defectReporting,
            continuousImprovement = employee.continuousImprovement,
            repairSpeed = employee.repairSpeed,
            preventativeMaintenance = employee.preventativeMaintenance,
            toolMastery = employee.toolMastery,
            repairWorkflow = employee.repairWorkflow,
            partInventory = employee.partInventory,
            maintenanceScheduling = employee.maintenanceScheduling,
            robotCalibration = employee.robotCalibration,
            speedEnhancement = employee.speedEnhancement,
            roboticsAccuracy = employee.roboticsAccuracy,
            automationPlanning = employee.automationPlanning,
            firmwareManagement = employee.firmwareManagement,
            robotUptime = employee.robotUptime,
            surveillance = employee.surveillance,
            alertness = employee.alertness,
            patrolSpeed = employee.patrolSpeed,
            surveillanceOversight = employee.surveillanceOversight,
            patrolRouting = employee.patrolRouting,
            threatAssessment = employee.threatAssessment,
            thoroughness = employee.thoroughness,
            routineMaintenance = employee.routineMaintenance,
            zonePrioritization = employee.zonePrioritization,
            supplyManagement = employee.supplyManagement,
            cleanlinessStandards = employee.cleanlinessStandards,
            trainingEffectiveness = employee.trainingEffectiveness,
            skillTransfer = employee.skillTransfer,
            motivation = employee.motivation,
            curriculumDesign = employee.curriculumDesign,
            progressTracking = employee.progressTracking,
            upskillingStrategy = employee.upskillingStrategy,
            hazardIdentification = employee.hazardIdentification,
            accidentPrevention = employee.accidentPrevention,
            emergencyResponse = employee.emergencyResponse,
            auditExecution = employee.auditExecution,
            trainingEnforcement = employee.trainingEnforcement,
            incidentReview = employee.incidentReview,
            talentScouting = employee.talentScouting,
            interviewingSkills = employee.interviewingSkills,
            onboardingEfficieny = employee.onboardingEfficieny,
            candidatePipelineManagement = employee.candidatePipelineManagement,
            interviewOversight = employee.interviewOversight,
            onboardingStrategy = employee.onboardingStrategy
        };
    }

    public Employee ToEmployee()
    {
        Employee employee = new Employee();
        employee.id = this.id;
        employee.employeeName = this.employeeName;
        employee.level = this.level;
        employee.levelPending = this.levelPending;
        employee.exp = this.exp;
        // Load sprite by name if available
        if (!string.IsNullOrEmpty(this.spriteName))
        {
            employee.employeeSprite = Resources.Load<Sprite>("EmployeeSprites/" + this.spriteName);
        }
        employee.salary = this.salary;
        employee.cost = this.cost;
        employee.isHired = this.isHired;
        employee.isFired = this.isFired;
        employee.employeeType = this.employeeType;
        employee.departmentType = this.departmentType;
        employee.departmentPending = this.departmentPending;
        // Core Stats
        employee.speed = this.speed;
        employee.efficiency = this.efficiency;
        employee.stamina = this.stamina;
        employee.strength = this.strength;
        employee.focus = this.focus;
        employee.experience = this.experience;
        // Traits
        employee.traits = this.traits;
        employee.combinedTraits = this.combinedTraits;
        employee.actionState = this.actionState;
        employee.stateTimer = this.stateTimer;
        employee.workInterval = this.workInterval;
        employee.restInterval = this.restInterval;
        employee.infractions = this.infractions;
        // Department-specific stats
        employee.empathy = this.empathy;
        employee.conflictResolution = this.conflictResolution;
        employee.recruiting = this.recruiting;
        employee.moraleBoost = this.moraleBoost;
        employee.retentionStrategy = this.retentionStrategy;
        employee.policyEnforcement = this.policyEnforcement;
        employee.techTroubleshooter = this.techTroubleshooter;
        employee.systemOptimization = this.systemOptimization;
        employee.security = this.security;
        employee.infrastructureOversight = this.infrastructureOversight;
        employee.incidentResponse = this.incidentResponse;
        employee.techBudgeting = this.techBudgeting;
        employee.logisticsPlanning = this.logisticsPlanning;
        employee.taskManagement = this.taskManagement;
        employee.coordination = this.coordination;
        employee.processOptimization = this.processOptimization;
        employee.crossDepartmentSync = this.crossDepartmentSync;
        employee.kpiMonitoring = this.kpiMonitoring;
        employee.loadMaster = this.loadMaster;
        employee.inventoryCheck = this.inventoryCheck;
        employee.speedyUnloader = this.speedyUnloader;
        employee.dockFlowManagement = this.dockFlowManagement;
        employee.receivingAccuracy = this.receivingAccuracy;
        employee.supplierCoordination = this.supplierCoordination;
        employee.shippingAccuracy = this.shippingAccuracy;
        employee.loadEfficiency = this.loadEfficiency;
        employee.timeManagement = this.timeManagement;
        employee.loadScheduling = this.loadScheduling;
        employee.accuracyOversight = this.accuracyOversight;
        employee.carrierCoordination = this.carrierCoordination;
        employee.sortingSpeed = this.sortingSpeed;
        employee.sortingAccuracy = this.sortingAccuracy;
        employee.patternRecognition = this.patternRecognition;
        employee.sortLineOversight = this.sortLineOversight;
        employee.errorReductionPlanning = this.errorReductionPlanning;
        employee.peakPrep = this.peakPrep;
        employee.packingEfficiency = this.packingEfficiency;
        employee.damageControl = this.damageControl;
        employee.qualityCheck = this.qualityCheck;
        employee.materialAllocation = this.materialAllocation;
        employee.repackFlow = this.repackFlow;
        employee.palletEfficiency = this.palletEfficiency;
        employee.heavyLifting = this.heavyLifting;
        employee.stackingPrecision = this.stackingPrecision;
        employee.stackingSupervision = this.stackingSupervision;
        employee.loadForecasting = this.loadForecasting;
        employee.safetyChecks = this.safetyChecks;
        employee.routeEfficiency = this.routeEfficiency;
        employee.carryCapacity = this.carryCapacity;
        employee.supportSpeed = this.supportSpeed;
        employee.routePlanning = this.routePlanning;
        employee.supportCoordination = this.supportCoordination;
        employee.loadDistribution = this.loadDistribution;
        employee.loadingSpeed = this.loadingSpeed;
        employee.hardHatProtection = this.hardHatProtection;
        employee.weightDistribution = this.weightDistribution;
        employee.truckStaging = this.truckStaging;
        employee.teamSynchronization = this.teamSynchronization;
        employee.loadingOversight = this.loadingOversight;
        employee.attentionToDetail = this.attentionToDetail;
        employee.inspectionSpeed = this.inspectionSpeed;
        employee.productKnowledge = this.productKnowledge;
        employee.inspectionProtocols = this.inspectionProtocols;
        employee.defectReporting = this.defectReporting;
        employee.continuousImprovement = this.continuousImprovement;
        employee.repairSpeed = this.repairSpeed;
        employee.preventativeMaintenance = this.preventativeMaintenance;
        employee.toolMastery = this.toolMastery;
        employee.repairWorkflow = this.repairWorkflow;
        employee.partInventory = this.partInventory;
        employee.maintenanceScheduling = this.maintenanceScheduling;
        employee.robotCalibration = this.robotCalibration;
        employee.speedEnhancement = this.speedEnhancement;
        employee.roboticsAccuracy = this.roboticsAccuracy;
        employee.automationPlanning = this.automationPlanning;
        employee.firmwareManagement = this.firmwareManagement;
        employee.robotUptime = this.robotUptime;
        employee.surveillance = this.surveillance;
        employee.alertness = this.alertness;
        employee.patrolSpeed = this.patrolSpeed;
        employee.surveillanceOversight = this.surveillanceOversight;
        employee.patrolRouting = this.patrolRouting;
        employee.threatAssessment = this.threatAssessment;
        employee.thoroughness = this.thoroughness;
        employee.routineMaintenance = this.routineMaintenance;
        employee.zonePrioritization = this.zonePrioritization;
        employee.supplyManagement = this.supplyManagement;
        employee.cleanlinessStandards = this.cleanlinessStandards;
        employee.trainingEffectiveness = this.trainingEffectiveness;
        employee.skillTransfer = this.skillTransfer;
        employee.motivation = this.motivation;
        employee.curriculumDesign = this.curriculumDesign;
        employee.progressTracking = this.progressTracking;
        employee.upskillingStrategy = this.upskillingStrategy;
        employee.hazardIdentification = this.hazardIdentification;
        employee.accidentPrevention = this.accidentPrevention;
        employee.emergencyResponse = this.emergencyResponse;
        employee.auditExecution = this.auditExecution;
        employee.trainingEnforcement = this.trainingEnforcement;
        employee.incidentReview = this.incidentReview;
        employee.talentScouting = this.talentScouting;
        employee.interviewingSkills = this.interviewingSkills;
        employee.onboardingEfficieny = this.onboardingEfficieny;
        employee.candidatePipelineManagement = this.candidatePipelineManagement;
        employee.interviewOversight = this.interviewOversight;
        employee.onboardingStrategy = this.onboardingStrategy;

        // Initialize empty lists
        employee.disablers = new List<Disablers.Disabler>();
        employee.actionRequests = new List<ActionRequest>();

        return employee;
    }
}

[System.Serializable]
public struct SerializableDepartment
{
    public int capacity; // Maximum number of employees that can work in this department
    public string departmentName; // Name of the department
    public List<ActionRequest> newActionRequests; // Array of new action requests associated with this department
    public List<ActionRequest> claimedActionRequests; // Array of claimed action requests associated with this department
    public GameController gameController; // Reference to the GameController script
    public int departmentId; // Unique identifier for the department
    public int departmentLevel; // Level of the department
    public int departmentExp; // Experience points of the department
    public DepartmentTypes.Type departmentType; // Type of the department (e.g., HR, IT, etc.)
    public int managerCapacity; // Maximum number of managers that can work in this department
    public List<Employee> employees; // Array of employees in this department
    public List<Disablers.Disabler> disablers; // Array of disablers associated with this department
    private int managerIndex; // Index of the current manager in the department
    public List<Employee> managers; // Array of managers in this department

    // Add any other Department properties that need to be saved

    public static SerializableDepartment FromDepartment(Department department)
    {
        return new SerializableDepartment
        {
            capacity = department.capacity,
            departmentName = department.departmentName,
            newActionRequests = department.newActionRequests,
            claimedActionRequests = department.claimedActionRequests,
            gameController = department.gameController,
            departmentId = department.departmentId,
            departmentLevel = department.departmentLevel,
            departmentExp = department.departmentExp,
            departmentType = department.departmentType,
            managerCapacity = department.managerCapacity,
            employees = new List<Employee>(department.employees),
            disablers = new List<Disablers.Disabler>(department.disablers),
            managerIndex = department.managerIndex,
            managers = new List<Employee>(department.managers)
            // Copy other needed properties
        };
    }

    public Department ToDepartment()
    {
        Department department;

        // Create a temporary GameObject to hold our department component
        GameObject tempGameObject = new GameObject("TempDepartment");

        // Add the appropriate department component based on departmentType
        switch (departmentType)
        {
            case DepartmentTypes.Type.HR:
                department = tempGameObject.AddComponent<HR>();
                break;
            case DepartmentTypes.Type.IT:
                department = tempGameObject.AddComponent<IT>();
                break;
            case DepartmentTypes.Type.Operations:
                department = tempGameObject.AddComponent<Operations>();
                break;
            case DepartmentTypes.Type.Inbound:
                department = tempGameObject.AddComponent<Inbound>();
                break;
            case DepartmentTypes.Type.Sorting:
                department = tempGameObject.AddComponent<Sorting>();
                break;
            case DepartmentTypes.Type.Repacking:
                department = tempGameObject.AddComponent<Repacking>();
                break;
            case DepartmentTypes.Type.Palletizing:
                department = tempGameObject.AddComponent<Palletizing>();
                break;
            case DepartmentTypes.Type.WaterSpidering:
                department = tempGameObject.AddComponent<WaterSpidering>();
                break;
            case DepartmentTypes.Type.FluidLoad:
                department = tempGameObject.AddComponent<FluidLoad>();
                break;
            case DepartmentTypes.Type.QualityControl:
                department = tempGameObject.AddComponent<QualityControl>();
                break;
            case DepartmentTypes.Type.Outbound:
                department = tempGameObject.AddComponent<Outbound>();
                break;
            case DepartmentTypes.Type.Maintenance:
                department = tempGameObject.AddComponent<Maintenance>();
                break;
            case DepartmentTypes.Type.Robotics:
                department = tempGameObject.AddComponent<Robotics>();
                break;
            case DepartmentTypes.Type.Safety:
                department = tempGameObject.AddComponent<Safety>();
                break;
            case DepartmentTypes.Type.Cleaning:
                department = tempGameObject.AddComponent<Cleaning>();
                break;
            case DepartmentTypes.Type.Security:
                department = tempGameObject.AddComponent<Security>();
                break;
            case DepartmentTypes.Type.Learning:
                department = tempGameObject.AddComponent<Learning>();
                break;
            case DepartmentTypes.Type.Recruiting:
                department = tempGameObject.AddComponent<Recruiting>();
                break;
            default:
                department = tempGameObject.AddComponent<Department>();
                break;
        }

        // Populate the department properties
        department.capacity = capacity;
        department.departmentName = departmentName;
        department.newActionRequests = newActionRequests;
        department.claimedActionRequests = claimedActionRequests;
        department.gameController = gameController;
        department.departmentId = departmentId;
        department.departmentLevel = departmentLevel;
        department.departmentExp = departmentExp;
        department.departmentType = departmentType;
        department.managerCapacity = managerCapacity;
        department.employees = new List<Employee>(employees);
        department.disablers = new List<Disablers.Disabler>(disablers);
        department.managerIndex = managerIndex;
        department.managers = new List<Employee>(managers);

        // Don't destroy the GameObject when loading new scenes
        GameObject.DontDestroyOnLoad(tempGameObject);

        return department;
    }
}

[System.Serializable]
public struct GlobalVariables
{
    public string warehouseName;
    public int warehouseId;
    public int warehouselevel;
    public int warehouseValue;
    public int warehouseExp;
    public List<SerializableEmployee> warehouseEmployees;
    public int warehouseMaxEmployees;
    public List<SerializableEmployee> newHires;
    public List<SerializableDepartment> departments;
    public int departmentCount;
    public List<SerializableDepartment> disabledDepartments;
    public int playerId;
    public string playerName;
    public int playerLevel;
    public double playerExp;
    public int playerExpMultiplier;
    public double playerMoney;
    public double playerMaxMoney;
    public int playerMaxLevel;
    public float employeeStatMax;
    public float employeeStatMin;
    public float employeeStatUpgradeValue;
    public float employeeStatUpgradeCost;
    public float employeeMaxLevel;
    public int employeeInfractionMax;
    public StatusType.Type tutorialStatus;
    public int tutorialStep;
    public int gameState;
    public List<Notification> notifications;
    public int boxesInStorage;
    public int palletsInStorage;
    public int boxValue;
    public int palletBoxLimit;
    public int palletValue;
    public int truckPalletLimit;
    public int truckValue;
    public float gameTimeElapsed;
    public float gameSpeed;
    public float daysSinceLastNewHire;
}

public class Globals
{
    public static string apiURL = "http://127.0.0.1:5505/api/v1"; // Test API URL for local development
    // public static string apiURL = "https://api.warehousetycoon.com/api/v1"; // Production API URL for live deployment

    // Added static field to store temporary save data
    public static GlobalVariables tempSaveData;

    public static int gameState = State.NotPlaying; // Current state of the game, initialized to NotPlaying
    // Warehouse data
    public static string warehouseName = "My Warehouse"; // Default name for the warehouse
    public static int warehouseId; // Unique identifier for the warehouse
    public static int warehouselevel = 1;
    public static int warehouseValue = 0;
    public static int warehouseExp = 0;
    public static List<Employee> warehouseEmployees = new List<Employee>(); // Array of employees in the warehouse
    public static int warehouseMaxEmployees; // Maximum number of employees allowed in the warehouse
    public static int boxesInStorage = 0; // Number of boxes currently in storage
    public static int palletsInStorage = 0; // Number of pallets currently in storage
    public static int boxValue = 5;
    public static int palletBoxLimit = 5;
    public static int palletValue = palletBoxLimit * boxValue; // Value of a pallet based on the number of boxes it can hold
    public static int truckPalletLimit = 1;
    public static int truckValue = truckPalletLimit * palletValue; // Value of a truck based on the number of pallets it can hold
    // HR data
    public static List<Employee> newHires = new List<Employee>(); // Array of new hires in the warehouse
    // Department data
    public static List<Department> departments = new List<Department>(); // Array of departments in the warehouse
    public static int departmentCount; // Number of departments in the warehouse
    public static List<Department> disabledDepartments = new List<Department>(); // Departments that are currently disabled
    public static int departmentCost = 50000; // Cost to purchase a new department
    // Player data
    public static int playerId;
    public static string playerName = "Guest"; // Default player name
    public static int playerLevel = 1; // Player's current level
    public static double playerExp = 0; // Player's current experience points
    public static int playerExpMultiplier = 250; // Multiplier for experience points gained
    public static double playerMoney = 0; // Amount of money the player has
    public static double playerMaxMoney = 99999999999; // Maximum amount of money the player can have
    public static int playerMaxLevel = 100; // Maximum level the player can reach
    public static float employeeStatMax = 6f; // Maximum value for employee stats
    public static float employeeStatMin = 1f; // Minimum value for employee stats
    public static float employeeStatUpgradeValue = 0.5f; // Multiplier for employee stats
    public static float employeeStatUpgradeCost = 1000f; // Cost to upgrade employee stats
    public static float employeeStatUpgradeExpBase = 100f; // Base experience required for warehouse upgrades
    public static float employeeMaxLevel = 10f; // Maximum level for employees
    public static int employeeInfractionMax = 3; // Maximum number of infractions for employees
    // Tutorial data
    public static StatusType.Type tutorialStatus = StatusType.Type.Completed; // Status of the tutorial
    public static int tutorialStep = 0; // Index for the current step in the tutorial
    // Notification data
    public static NotificationController notificationController; // Reference to the NotificationController
    public static List<Notification> notifications = new List<Notification>(); // Array of notifications for the player
    // Game state data
    public static float gameTimeElapsed = 0; // Time elapsed in the game
    public static float gameSpeed = 1; // Speed of the game (1x speed by default)
    public static float daysSinceLastNewHire = 0; // Days since the last new hire was made

    // File path for local save
    public static string saveFilePath = Path.Combine(Application.persistentDataPath, "warehouseTycoonSave.json");

    public static IEnumerator Save()
    {
        try
        {
            // Convert complex objects to serializable versions
            List<SerializableEmployee> serializableEmployees = new List<SerializableEmployee>();
            foreach (Employee emp in warehouseEmployees)
            {
                serializableEmployees.Add(SerializableEmployee.FromEmployee(emp));
            }

            List<SerializableEmployee> serializableNewHires = new List<SerializableEmployee>();
            foreach (Employee hire in newHires)
            {
                serializableNewHires.Add(SerializableEmployee.FromEmployee(hire));
            }

            List<SerializableDepartment> serializableDepartments = new List<SerializableDepartment>();
            foreach (Department dept in departments)
            {
                serializableDepartments.Add(SerializableDepartment.FromDepartment(dept));
            }

            List<SerializableDepartment> serializableDisabledDepartments = new List<SerializableDepartment>();
            foreach (Department dept in disabledDepartments)
            {
                serializableDisabledDepartments.Add(SerializableDepartment.FromDepartment(dept));
            }

            // Create a GlobalVariables instance with current values
            GlobalVariables saveData = new GlobalVariables
            {
                warehouseName = warehouseName,
                warehouseId = warehouseId,
                warehouselevel = warehouselevel,
                warehouseValue = warehouseValue,
                warehouseExp = warehouseExp,
                warehouseEmployees = serializableEmployees,
                warehouseMaxEmployees = warehouseMaxEmployees,
                newHires = serializableNewHires,
                departments = serializableDepartments,
                departmentCount = departmentCount,
                disabledDepartments = serializableDisabledDepartments,
                playerId = playerId,
                playerName = playerName,
                playerLevel = playerLevel,
                playerExp = playerExp,
                playerExpMultiplier = playerExpMultiplier,
                playerMoney = playerMoney,
                playerMaxMoney = playerMaxMoney,
                playerMaxLevel = playerMaxLevel,
                employeeStatMax = employeeStatMax,
                employeeStatMin = employeeStatMin,
                employeeStatUpgradeValue = employeeStatUpgradeValue,
                employeeStatUpgradeCost = employeeStatUpgradeCost,
                employeeMaxLevel = employeeMaxLevel,
                employeeInfractionMax = employeeInfractionMax,
                tutorialStatus = tutorialStatus,
                tutorialStep = tutorialStep,
                gameState = gameState,
                notifications = notifications,
                boxesInStorage = boxesInStorage,
                palletsInStorage = palletsInStorage,
                boxValue = boxValue,
                palletBoxLimit = palletBoxLimit,
                palletValue = palletValue,
                truckPalletLimit = truckPalletLimit,
                truckValue = truckValue,
                gameTimeElapsed = gameTimeElapsed,
                gameSpeed = gameSpeed,
                daysSinceLastNewHire = daysSinceLastNewHire
            };

            // Convert the GlobalVariables struct to JSON
            string jsonData = JsonUtility.ToJson(saveData, true);

            // Write the JSON to a local file
            File.WriteAllText(saveFilePath, jsonData);

            Debug.Log($"Game saved successfully to {saveFilePath}");
            Debug.Log($"JSON data: {jsonData}"); // Add this to verify data is being serialized correctly
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error saving game: {e.Message}");
        }

        yield return null;
    }

    public static IEnumerator Load()
    {
        try
        {
            // Check if save file exists
            if (File.Exists(saveFilePath))
            {
                // Read the JSON from the local file
                string jsonData = File.ReadAllText(saveFilePath);

                // Parse the JSON and update the game state
                var data = JsonUtility.FromJson<GlobalVariables>(jsonData);

                // Simple properties
                warehouseName = data.warehouseName;
                warehouseId = data.warehouseId;
                warehouselevel = data.warehouselevel;
                warehouseValue = data.warehouseValue;
                warehouseExp = data.warehouseExp;
                warehouseMaxEmployees = data.warehouseMaxEmployees;
                playerId = data.playerId;
                playerName = data.playerName;
                playerLevel = data.playerLevel;
                playerExp = data.playerExp;
                playerExpMultiplier = data.playerExpMultiplier;
                playerMoney = data.playerMoney;
                playerMaxMoney = data.playerMaxMoney;
                playerMaxLevel = data.playerMaxLevel;
                employeeStatMax = data.employeeStatMax;
                employeeStatMin = data.employeeStatMin;
                employeeStatUpgradeValue = data.employeeStatUpgradeValue;
                employeeStatUpgradeCost = data.employeeStatUpgradeCost;
                employeeMaxLevel = data.employeeMaxLevel;
                employeeInfractionMax = data.employeeInfractionMax;
                tutorialStatus = data.tutorialStatus;
                tutorialStep = data.tutorialStep;
                gameState = data.gameState;
                notifications = data.notifications;
                boxesInStorage = data.boxesInStorage;
                palletsInStorage = data.palletsInStorage;
                boxValue = data.boxValue;
                palletBoxLimit = data.palletBoxLimit;
                palletValue = data.palletValue;
                truckPalletLimit = data.truckPalletLimit;
                truckValue = data.truckValue;
                gameTimeElapsed = data.gameTimeElapsed;
                gameSpeed = data.gameSpeed;
                daysSinceLastNewHire = data.daysSinceLastNewHire;

                // Recreate departments first
                departments.Clear();
                foreach (SerializableDepartment deptData in data.departments)
                {
                    departments.Add(deptData.ToDepartment());
                }

                disabledDepartments.Clear();
                foreach (SerializableDepartment deptData in data.disabledDepartments)
                {
                    disabledDepartments.Add(deptData.ToDepartment());
                }

                // Recreate employees and assign departments
                warehouseEmployees.Clear();
                foreach (SerializableEmployee empData in data.warehouseEmployees)
                {
                    Employee emp = empData.ToEmployee();
                    // Reconnect department reference
                    if (empData.departmentId >= 0)
                    {
                        emp.department = departments.Find(d => d.departmentId == empData.departmentId);
                    }
                    warehouseEmployees.Add(emp);
                }

                newHires.Clear();
                foreach (SerializableEmployee hireData in data.newHires)
                {
                    Employee hire = hireData.ToEmployee();
                    // Reconnect department reference if needed
                    if (hireData.departmentId >= 0)
                    {
                        hire.department = departments.Find(d => d.departmentId == hireData.departmentId);
                    }
                    newHires.Add(hire);
                }

                Debug.Log($"Game loaded successfully from {saveFilePath}");
            }
            else
            {
                Debug.LogWarning("No save file found. Starting new game.");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading game: {e.Message}");
        }

        yield return null;
    }

    public static IEnumerator LoadFromSpecificFile(string filePath)
    {
        try
        {
            // Check if file exists
            if (File.Exists(filePath))
            {
                // Read the JSON from the specified file
                string jsonData = File.ReadAllText(filePath);

                // Parse the JSON and update the game state
                var data = JsonUtility.FromJson<GlobalVariables>(jsonData);

                // Store in tempSaveData for preview purposes
                tempSaveData = data;

                Debug.Log($"Game data loaded successfully from {filePath} into temporary storage");
                Debug.Log($"To apply this save, call a method to copy from tempSaveData to the actual game state");
            }
            else
            {
                Debug.LogWarning($"No save file found at path: {filePath}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error loading game from specific file: {e.Message}");
        }

        yield return null;
    }
}

public static class StatusType
{
    public enum Type
    {
        InComplete,
        InProgress,
        Completed
    }

    public static string GetStatusName(Type statusType)
    {
        return statusType.ToString();
    }
}