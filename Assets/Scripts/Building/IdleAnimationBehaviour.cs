using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimationBehaviour : StateMachineBehaviour
{
    BuildingState buildingState;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!buildingState) buildingState = animator.GetComponentInParent<BuildingState>();
        buildingState.UpdateProductionSprite();
    }
}
