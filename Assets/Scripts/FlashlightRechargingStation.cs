using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FlashlightRechargingStation : InteractableObject
{
    public float maxUses;
    public Sprite onSprite;
    public Sprite depletedSprite;
    public Sprite chargingSprite;

    bool isOn;
    bool playerCharging;

    float uses;
    SpriteRenderer spriteRenderer;

    public override void ButtonInteract()
    {
        if (!isOn)
        {
            isOn = true;
            spriteRenderer.sprite = onSprite;
        }
    }

    public override void HoverInteract()
    {
    }

    public override void StopHoverInteract()
    {
    }

    private void Awake()
    {
        uses = maxUses;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isOn && playerCharging)
        {
            uses -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered");
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("Something is in trigger");

        if (CheckCollision(collision))
        {
            Debug.Log(isOn + " " + uses);

            if (isOn && uses > 0)
            {
                playerCharging = true;
                GameEvents.OnToggleRechargingStation?.Invoke(true);
                spriteRenderer.sprite = chargingSprite;
            }
            else
            {
                GameEvents.OnToggleRechargingStation?.Invoke(false);
                playerCharging = false;
                spriteRenderer.sprite = depletedSprite;
            }
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (isOn)
        {
            if (CheckCollision(collision))
            {
                GameEvents.OnToggleRechargingStation?.Invoke(false);
                playerCharging = false;
            }
        }
    }


    private bool CheckCollision(Collider2D collision)
    {
        Player player;
        collision.TryGetComponent(out player);

        return player != null;
    }

}
