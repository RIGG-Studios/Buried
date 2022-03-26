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
        /*/
        if (!player.inventory.HasItem(baseItem))
            return;

        Vector2 dir = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition()) - transform.position;
        ItemThrower.ThrowItem(throwTransform, settings.flarePrefab, dir.normalized, settings.throwForce, settings.decayRate, settings.baseIntensity);

        player.inventory.UseItem(player.inventory.currentControllableItem.baseItem);
        /*/
    }
}
