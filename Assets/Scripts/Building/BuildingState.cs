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
    AudioSource audioSource;
    public Building building;
    [SerializeField] Transform animationTransform;
    [SerializeField] Animator animator;
    [SerializeField] SpriteRenderer productionSprite;

    public Inventory inventory = new Inventory();

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

    public void SetParameters(Building building, Vector3 entrancePos, GameState gameState)
    {
        this.gameState = gameState;
        this.building = building;
        if (building.animator != null)
            this.animator.runtimeAnimatorController = building.animator;
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        spriteRenderer.sprite = building.sprite;
        audioSource = GetComponentInChildren<AudioSource>();
        if (building.animationSound != null)
            audioSource.clip = building.animationSound;
        animationTransform.position = entrancePos;
        inventory.contents.Add(new InventoryEntry(building.production));
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
        yield return new WaitForSeconds(5.0f);
        ChangeState(workingState);
    }

    IEnumerator Production()
    {
        yield return new WaitForSeconds(5.0f);
        inventory.AddResource(building.production, building.productionQuantity);
        if (building.productionSprites != null)
        {
            int amount = inventory.contents[0].quantity > 5 ? 4 : inventory.contents[0].quantity - 1;
            productionSprite.sprite = building.productionSprites[amount];
            productionSprite.transform.localPosition = new Vector3(building.productionOffsets[amount].x / 40f, -building.productionOffsets[amount].y / 40f);
        }
        gameState.inventory.AddResource(building.production, building.productionQuantity);
        ChangeState(restingState);
    }
}
