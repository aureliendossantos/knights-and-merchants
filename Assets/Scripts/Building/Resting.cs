public class Resting : BaseState
{
    private BuildingState buildingState;
    public Resting(BuildingState stateMachine) : base("Resting", stateMachine)
    {
        buildingState = stateMachine;
    }

    public override void Enter()
    {
        buildingState.StartRest();
    }
}