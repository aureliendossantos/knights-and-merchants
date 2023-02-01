public class PlacingTile : BaseState
{
    private GridState gridState;
    public PlacingTile(GridState stateMachine) : base("PlacingTile", stateMachine)
    {
        gridState = stateMachine;
    }
}