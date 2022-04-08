using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneratorRecharging : MonoBehaviour
{
    public float maxUses;
    public Sprite depletedSprite;
    public Sprite chargingSprite;

    public Color hasChargeUIColour;
    public Color depletedUIColour;

    public Image generatorUsesImage;

    FlashlightController playerFlashlight;

    bool playerInTrigger;

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
        isOn = transform.parent.GetComponent<FlashlightRechargingStation>().isOn;

        if (isOn)
        {
            if (playerInTrigger && uses > -1 && !playerFlashlight.GetIsFullyCharged())
            {
                uses -= Time.deltaTime;
            }

            if (uses > 0 && playerInTrigger)
            {
                GameEvents.OnToggleRechargingStation?.Invoke(true);
                spriteRenderer.sprite = chargingSprite;
                generatorUsesImage.color = hasChargeUIColour;

                generatorUsesImage.fillAmount = uses / maxUses;
            }
            else if (uses <= 0 || !playerInTrigger)
            {
                GameEvents.OnToggleRechargingStation?.Invoke(false);

                if(uses <= 0)
                {
                    spriteRenderer.sprite = depletedSprite;
                    generatorUsesImage.color = depletedUIColour;

                    generatorUsesImage.fillAmount = 1;
                }
            }
        }
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        playerFlashlight = collision.GetComponentInChildren<FlashlightController>();

        if (CheckCollision(collision))
        {
            if (isOn)
            {
                generatorUsesImage.gameObject.SetActive(true);
            }

            playerInTrigger = true;
        }
    }


    public void OnTriggerExit2D(Collider2D collision)
    {
        if (CheckCollision(collision))
        {
            playerInTrigger = false;
            generatorUsesImage.gameObject.SetActive(false);
        }
    }


    private bool CheckCollision(Collider2D collision)
    {
        Player player;
        collision.TryGetComponent(out player);

        return player != null;
    }
}
