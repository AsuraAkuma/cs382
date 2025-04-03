public class Upgrade
{
    public string upgradeName; // Name of the upgrade
    public string description; // Description of the upgrade
    public DepartmentTypes.Type departmentType; // The department this upgrade applies to
    public int level; // Current level of the upgrade
    public int cost; // Cost to upgrade to the next level
    public int expRequired; // Experience required to reach the next level
    public int maxLevel; // Maximum level for this upgrade
    public string affectedStat;
    public int affectedValue; // The value by which the affected stat is increased at this level
    public CalcType affectCalcType; // Type of calculation for the affected value ex: +,-,*,/
}