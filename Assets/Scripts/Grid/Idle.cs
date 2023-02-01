public class Idle : BaseState
{
    private GridState gridState;
    public Idle(GridState stateMachine) : base("Idle", stateMachine)
    {
        gridState = stateMachine;
    }
}