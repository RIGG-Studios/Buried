using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : InteractableObject
{
    [SerializeField] private Sprite emptySprite = null;
    [SerializeField] private ItemProperties itemProperties = null;
    [SerializeField, Range(1, 5)] private int pickupAmount;

    private PlayerInventory inventory
    {
        get
        {
            return FindObjectOfType<PlayerInventory>();
        }
    }

    private SpriteRenderer render = null;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    public override void ButtonInteract()
    {
        bool itm = inventory.AddItem(itemProperties, pickupAmount);

        if (itm)
        {
            render.sprite = emptySprite;
            interactable = false;
        }
    }

    public override void HoverInteract()
    {
    }

    public override void StopHoverInteract()
    {
    }
}
