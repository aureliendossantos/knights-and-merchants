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
    [SerializeField] SpriteRenderer stockSprite;

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
        inventory.contents.Add(new InventoryEntry(building.production, 5));
        if (building.requiredResource != null)
            inventory.contents.Add(new InventoryEntry(building.requiredResource, 5, 5));
        UpdateStockSprite();
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
        while (inventory.contents[0].IsFull())
            yield return new WaitForSeconds(5.0f);
        ChangeState(workingState);
    }

    IEnumerator Production()
    {
        if (building.requiredResource)
            inventory.RemoveResource(building.requiredResource, 1);
        inventory.AddResource(building.production, building.productionQuantity);
        yield return new WaitForSeconds(5.0f);
        // UpdateProductionSprite();
        // UpdateStockSprite();
        gameState.inventory.AddResource(building.production, building.productionQuantity);
        ChangeState(restingState);
    }

    public void UpdateProductionSprite()
    {
        if (inventory.contents[0].quantity == 0)
        {
            productionSprite.sprite = null;
            return;
        }
        if (building.productionSprites.Count == 5)
        {
            int amount = inventory.contents[0].quantity - 1;
            productionSprite.sprite = building.productionSprites[amount];
            productionSprite.transform.localPosition = new Vector3(building.productionOffsets[amount].x / 40f, -building.productionOffsets[amount].y / 40f);
        }
    }

    public void UpdateStockSprite()
    {
        if (inventory.contents.Count > 1 && inventory.contents[1].quantity == 0)
        {
            stockSprite.sprite = null;
            return;
        }
        if (inventory.contents.Count > 1 && building.stockSprites.Count == 5)
        {
            int amount = inventory.contents[1].quantity - 1;
            stockSprite.sprite = building.stockSprites[amount];
            stockSprite.transform.localPosition = new Vector3(building.stockOffsets[amount].x / 40f, -building.stockOffsets[amount].y / 40f);
        }
    }
}
