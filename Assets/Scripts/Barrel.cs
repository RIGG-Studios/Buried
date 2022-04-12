using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barrel : InteractableObject
{
    [SerializeField] private bool showTutorial;
    [SerializeField] private Sprite emptySprite = null;
    [SerializeField] private Text text;
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

      if(text && showTutorial)
            text.gameObject.SetActive(true);
    }

    public override void ButtonInteract()
    {
        bool itm = inventory.AddItem(itemProperties, pickupAmount);

        if (itm)
        {
            render.sprite = emptySprite;
            interactable = false;
            text.gameObject.SetActive(false);
        }
    }

    public override void HoverInteract()
    {
    }

    public override void StopHoverInteract()
    {
    }
}
