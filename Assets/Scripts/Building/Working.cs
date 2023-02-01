public class Working : BaseState
{
    private BuildingState buildingState;
    public Working(BuildingState stateMachine) : base("Working", stateMachine)
    {
        buildingState = stateMachine;
    }

    public override void Enter()
    {
        buildingState.StartProduction();
    }
}