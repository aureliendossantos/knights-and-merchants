using UnityEngine;

/// <summary>
/// Finite State Machine
/// https://github.com/MinaPecheux/UnityTutorials-FiniteStateMachines
/// </summary>
public class StateMachine : MonoBehaviour
{
    public BaseState currentState;

    protected void Start()
    {
        currentState = GetInitialState();
        if (currentState != null)
            currentState.Enter();
    }

    protected virtual void Update()
    {
        if (currentState != null)
            currentState.UpdateLogic();
    }

    void LateUpdate()
    {
        if (currentState != null)
            currentState.UpdatePhysics();
    }

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();
        currentState = newState;
        currentState.Enter();
    }

    /// <summary>
    /// Defines the initial state of the State Machine.
    /// </summary>
    /// <returns>Initial state</returns>
    protected virtual BaseState GetInitialState()
    {
        return null;
    }
}
