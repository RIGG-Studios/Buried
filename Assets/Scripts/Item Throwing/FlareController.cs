using System.Collections;
using UnityEngine;


public class FlareController : ItemController
{
    private Transform throwTransform;
    private FlareSettings settings;


    public override void SetupController(Player player, ItemProperties properties)
    {
        base.SetupController(player, properties);
        throwTransform = player.defaultLight.transform;
        settings = player.flareSettings;
    }

    public override void UseItem()
    {
        if (!CanUseItem())
            return;

        Item flares = null;
        player.inventory.HasItem(properties.itemType, out flares);

        if (flares != null)
        {
            if (flares.stack <= 0)
            {
                StartCoroutine(ShowAmmoNeededUI("NO FLARES"));
                return;
            }

            Vector2 dir = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition()) - transform.position;
            ItemThrower.ThrowItem(throwTransform, settings.flarePrefab, dir.normalized, settings.throwForce, settings.decayRate, settings.baseIntensity);

            player.inventory.UseItem(properties.itemType);
        }
        else
        {
            StartCoroutine(ShowAmmoNeededUI("NO FLARES"));
        }
    }
}
