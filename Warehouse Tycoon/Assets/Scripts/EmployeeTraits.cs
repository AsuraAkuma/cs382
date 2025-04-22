
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public static class EmployeeTraits
{

    // HR Department
    public static readonly TraitValues EmpatheticListener = new TraitValues { stamina = 0.1f, departmentType = DepartmentTypes.Type.HR };
    public static readonly TraitValues PolicyEnforcer = new TraitValues { efficiency = 0.15f, departmentType = DepartmentTypes.Type.HR };
    public static readonly TraitValues EmployeeAdvocate = new TraitValues { focus = 0.1f, departmentType = DepartmentTypes.Type.HR };

    // IT Department
    public static readonly TraitValues TechSavvy = new TraitValues { speed = 0.2f, departmentType = DepartmentTypes.Type.IT };
    public static readonly TraitValues CyberGuardian = new TraitValues { efficiency = 0.15f, departmentType = DepartmentTypes.Type.IT };
    public static readonly TraitValues AutomationExpert = new TraitValues { speed = 0.1f, efficiency = 0.1f, departmentType = DepartmentTypes.Type.IT };

    // Operations
    public static readonly TraitValues TaskMaster = new TraitValues { speed = 0.15f, departmentType = DepartmentTypes.Type.Operations };
    public static readonly TraitValues OrganizedLeader = new TraitValues { efficiency = 0.1f, stamina = 0.05f, departmentType = DepartmentTypes.Type.Operations };
    public static readonly TraitValues ProblemSolver = new TraitValues { focus = 0.1f, departmentType = DepartmentTypes.Type.Operations };

    // Inbound (Logistics)
    public static readonly TraitValues QuickUnloader = new TraitValues { speed = 0.2f, departmentType = DepartmentTypes.Type.Inbound };
    public static readonly TraitValues InventoryGenius = new TraitValues { focus = 0.15f, departmentType = DepartmentTypes.Type.Inbound };
    public static readonly TraitValues CargoExpert = new TraitValues { efficiency = 0.1f, departmentType = DepartmentTypes.Type.Inbound };

    // Sorting
    public static readonly TraitValues FastSorter = new TraitValues { speed = 0.2f, departmentType = DepartmentTypes.Type.Sorting };
    public static readonly TraitValues AccuracyGuru = new TraitValues { focus = 0.15f, departmentType = DepartmentTypes.Type.Sorting };
    public static readonly TraitValues PatternFinder = new TraitValues { efficiency = 0.1f, departmentType = DepartmentTypes.Type.Sorting };

    // Repacking
    public static readonly TraitValues EfficientPacker = new TraitValues { speed = 0.15f, efficiency = 0.05f, departmentType = DepartmentTypes.Type.Repacking };
    public static readonly TraitValues QualityPackager = new TraitValues { focus = 0.1f, departmentType = DepartmentTypes.Type.Repacking };
    public static readonly TraitValues SpaceSaver = new TraitValues { efficiency = 0.1f, departmentType = DepartmentTypes.Type.Repacking };

    // Palletizing
    public static readonly TraitValues HeavyLifter = new TraitValues { strength = 0.2f, departmentType = DepartmentTypes.Type.Palletizing };
    public static readonly TraitValues StackMaster = new TraitValues { efficiency = 0.15f, departmentType = DepartmentTypes.Type.Palletizing };
    public static readonly TraitValues QuickStacker = new TraitValues { speed = 0.1f, departmentType = DepartmentTypes.Type.Palletizing };

    // WaterSpidering
    public static readonly TraitValues SwiftRunner = new TraitValues { speed = 0.2f, departmentType = DepartmentTypes.Type.WaterSpidering };
    public static readonly TraitValues MultiTasker = new TraitValues { efficiency = 0.15f, departmentType = DepartmentTypes.Type.WaterSpidering };
    public static readonly TraitValues QuickResponder = new TraitValues { speed = 0.1f, focus = 0.05f, departmentType = DepartmentTypes.Type.WaterSpidering };

    // FluidLoad
    public static readonly TraitValues LoadMaster = new TraitValues { speed = 0.15f, strength = 0.05f, departmentType = DepartmentTypes.Type.FluidLoad };
    public static readonly TraitValues SafetyFirst = new TraitValues { stamina = 0.1f, departmentType = DepartmentTypes.Type.FluidLoad };
    public static readonly TraitValues WeightDistributor = new TraitValues { focus = 0.1f, departmentType = DepartmentTypes.Type.FluidLoad };

    // Quality Control
    public static readonly TraitValues EagleEye = new TraitValues { focus = 0.2f, departmentType = DepartmentTypes.Type.QualityControl };
    public static readonly TraitValues ThoroughInspector = new TraitValues { speed = 0.15f, departmentType = DepartmentTypes.Type.QualityControl };
    public static readonly TraitValues Perfectionist = new TraitValues { focus = 0.25f, speed = -0.05f, departmentType = DepartmentTypes.Type.QualityControl };

    // Outbound
    public static readonly TraitValues RoutePlanner = new TraitValues { efficiency = 0.15f, departmentType = DepartmentTypes.Type.Outbound };
    public static readonly TraitValues OnTimeShipper = new TraitValues { speed = 0.1f, departmentType = DepartmentTypes.Type.Outbound };
    public static readonly TraitValues AccurateShipper = new TraitValues { focus = 0.1f, departmentType = DepartmentTypes.Type.Outbound };

    // Maintenance
    public static readonly TraitValues FixItFast = new TraitValues { speed = 0.2f, departmentType = DepartmentTypes.Type.Maintenance };
    public static readonly TraitValues ToolMaster = new TraitValues { efficiency = 0.15f, departmentType = DepartmentTypes.Type.Maintenance };
    public static readonly TraitValues PreventivePro = new TraitValues { stamina = 0.1f, departmentType = DepartmentTypes.Type.Maintenance };

    // Robotics
    public static readonly TraitValues CalibratedPrecision = new TraitValues { focus = 0.15f, departmentType = DepartmentTypes.Type.Robotics };
    public static readonly TraitValues RobotTuner = new TraitValues { speed = 0.1f, efficiency = 0.05f, departmentType = DepartmentTypes.Type.Robotics };
    public static readonly TraitValues MechanicalWizard = new TraitValues { stamina = 0.1f, departmentType = DepartmentTypes.Type.Robotics };

    // Safety
    public static readonly TraitValues Alert = new TraitValues { focus = 0.15f, departmentType = DepartmentTypes.Type.Safety };
    public static readonly TraitValues CalmUnderPressure = new TraitValues { stamina = 0.1f, departmentType = DepartmentTypes.Type.Safety };
    public static readonly TraitValues SafetyGuru = new TraitValues { efficiency = 0.1f, departmentType = DepartmentTypes.Type.Safety };

    // Cleaning
    public static readonly TraitValues SpeedyScrubber = new TraitValues { speed = 0.2f, departmentType = DepartmentTypes.Type.Cleaning };
    public static readonly TraitValues DeepClean = new TraitValues { focus = 0.15f, departmentType = DepartmentTypes.Type.Cleaning };
    public static readonly TraitValues RoutineMaster = new TraitValues { efficiency = 0.1f, departmentType = DepartmentTypes.Type.Cleaning };

    // Security
    public static readonly TraitValues WatchfulEye = new TraitValues { focus = 0.15f, departmentType = DepartmentTypes.Type.Security };
    public static readonly TraitValues SecurityResponder = new TraitValues { speed = 0.1f, departmentType = DepartmentTypes.Type.Security };
    public static readonly TraitValues PatrolEfficiency = new TraitValues { efficiency = 0.1f, departmentType = DepartmentTypes.Type.Security };

    // Learning & Development
    public static readonly TraitValues TrainingExpert = new TraitValues { experience = 0.15f, departmentType = DepartmentTypes.Type.Learning };
    public static readonly TraitValues SkillTransfer = new TraitValues { efficiency = 0.1f, departmentType = DepartmentTypes.Type.Learning };
    public static readonly TraitValues MotivationalSpeaker = new TraitValues { stamina = 0.1f, departmentType = DepartmentTypes.Type.Learning };
}

public class TraitValues
{
    // Trait values for each trait
    public float speed = 0f;
    public float efficiency = 0f;
    public float stamina = 0f;
    public float strength = 0f;
    public float focus = 0f;
    public float experience = 0f;
    public DepartmentTypes.Type departmentType;

    // Method to combine traits
    public static TraitValues CombineTraits(List<TraitValues> traits)
    {
        TraitValues combined = new TraitValues();
        foreach (var trait in traits)
        {
            combined.speed += trait.speed;
            combined.efficiency += trait.efficiency;
            combined.stamina += trait.stamina;
            combined.strength += trait.strength;
            combined.focus += trait.focus;
            combined.experience += trait.experience;

            // Set department type from the first trait
            if (combined.departmentType == default(DepartmentTypes.Type))
            {
                combined.departmentType = trait.departmentType;
            }
        }
        return combined;
    }

    public static string GetTraitName(TraitValues traitValue)
    {
        var fields = typeof(EmployeeTraits)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(TraitValues));

        foreach (var field in fields)
        {
            if (field.GetValue(null) == traitValue)
            {
                return field.Name;
            }
        }

        return "Unknown Trait";
    }
}
