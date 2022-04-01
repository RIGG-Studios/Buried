using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorRecharging : MonoBehaviour
{
    public float maxUses;
    public Sprite depletedSprite;
    public Sprite chargingSprite;

    bool playerCharging;

    float uses;
    SpriteRenderer spriteRenderer;

    bool isOn;

    private void Awake()
    {
        uses = maxUses;
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isOn && playerCharging)
        {
            uses -= Time.deltaTime;
        }

        isOn = transform.parent.GetComponent<FlashlightRechargingStation>().isOn;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (CheckCollision(collision))
        {
            if (isOn && uses > 0)
            {
                if(playerCharging != true)
                {
                    playerCharging = true;
                    GameEvents.OnToggleRechargingStation?.Invoke(true);
                    spriteRenderer.sprite = chargingSprite;
                }
            }
            else if (isOn && uses <= 0)
            {
                if(playerCharging == true)
                {
                    GameEvents.OnToggleRechargingStation?.Invoke(false);
                    playerCharging = false;
                    spriteRenderer.sprite = depletedSprite;
                }
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
