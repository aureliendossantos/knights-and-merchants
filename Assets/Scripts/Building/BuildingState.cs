using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingState : StateMachine
{
    // Declaring states
    public Resting restingState;
    public Working workingState;

    GameState gameState;
    SpriteRenderer spriteRenderer;
    public Building building;
    public Transform animationTransform;
    public Animator animator;

    /// <summary>
    /// StateMachine boilerplate: create states
    /// </summary>
    void Awake()
    {
        restingState = new Resting(this);
        workingState = new Working(this);
    }

    protected override BaseState GetInitialState()
    {
        return restingState;
    }

    public void SetParameters(Building building, GameState gameState)
    {
        this.gameState = gameState;
        this.building = building;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = building.sprite;
    }

    public void StartRest()
    {
        animator.SetBool("Work", false);
        StartCoroutine(Rest());
    }

    public void StartProduction()
    {
        animator.SetBool("Work", true);
        StartCoroutine(Production());
    }

    IEnumerator Rest()
    {
        yield return new WaitForSeconds(3.0f);
        ChangeState(workingState);
    }

    IEnumerator Production()
    {
        yield return new WaitForSeconds(3.0f);
        gameState.inventory.AddResource(building.production, building.productionQuantity);
        ChangeState(restingState);
    }
}
