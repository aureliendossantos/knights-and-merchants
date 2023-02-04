public class PlacingTile : BaseState
{
    private GridState gridState;
    public PlacingTile(GridState stateMachine) : base("PlacingTile", stateMachine)
    {
        gridState = stateMachine;
    }

    public override void Exit()
    {
        gridState.uiLayer.ClearAllTiles();
    }
}