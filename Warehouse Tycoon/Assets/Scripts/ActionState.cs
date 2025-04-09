public class ActionState
{
    public enum State
    {
        Idle,
        Working,
        Resting,
        Training,
        Emergency,
        Break,
        None
    }

    public static string GetActionStateName(State actionState)
    {
        return actionState.ToString();
    }
    public static string GetActionStateDescription(State actionState)
    {
        switch (actionState)
        {
            case State.Idle:
                return "The employee is currently idle and not performing any tasks.";
            case State.Working:
                return "The employee is actively working on their assigned tasks.";
            case State.Resting:
                return "The employee is taking a break to recover energy.";
            case State.Training:
                return "The employee is undergoing training to improve their skills.";
            case State.Emergency:
                return "The employee is responding to an emergency situation.";
            case State.Break:
                return "The employee is on a scheduled break.";
            default:
                return "Unknown action state.";
        }
    }
}