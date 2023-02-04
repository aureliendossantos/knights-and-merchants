public class PlacingBuilding : BaseState
{
    private GridState gridState;
    public PlacingBuilding(GridState stateMachine) : base("PlacingBuilding", stateMachine)
    {
        gridState = stateMachine;
    }

    public override void Exit()
    {
        gridState.uiLayer.ClearAllTiles();
    }
}