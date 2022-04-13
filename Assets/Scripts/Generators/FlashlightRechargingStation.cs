using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class FlashlightRechargingStation : InteractableObject
{
    public Sprite onSprite;
    public bool isOn;

    SpriteRenderer spriteRenderer;

    public override void ButtonInteract()
    {
        if (!isOn)
        {
            spriteRenderer.sprite = onSprite;
            MainManager.DecrementGeneratorCount();
            gameObject.layer = 0;

            if (showTutorial)
                tutorialText.gameObject.SetActive(false);

            isOn = true;
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void HoverInteract()
    {
    }

    public override void StopHoverInteract()
    {
    }
}
