public class PlacingBuilding : BaseState
{
    private GridState gridState;
    public PlacingBuilding(GridState stateMachine) : base("PlacingBuilding", stateMachine)
    {
        gridState = stateMachine;
    }
}