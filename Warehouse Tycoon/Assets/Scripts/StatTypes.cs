public class StatTypes
{
    public enum Type
    {
        Speed,
        Efficiency,
        Stamina,
        Strength,
        Focus,
        Experience
    }

    public static string GetStatName(Type statType)
    {
        return statType.ToString();
    }
}