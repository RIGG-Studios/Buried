using UnityEngine;

public class FlareController : ItemController
{
    private Transform throwTransform;
    private FlareSettings settings;

    public override void SetupController(Player player, Item itemInInventory)
    {
        base.SetupController(player, itemInInventory);
        throwTransform = player.defaultLight.transform;
        settings = player.flareSettings;
    }

    public override void UseItem()
    {
        int stack = player.inventory.currentControllableItem.itemInInventory.stack;

        if (stack > 0)
        {
            Vector2 dir = Camera.main.ScreenToWorldPoint(Utilites.GetMousePosition()) - transform.position;
            ItemThrower.ThrowItem(throwTransform, settings.flarePrefab, dir.normalized, settings.throwForce, settings.decayRate, settings.baseIntensity);

            player.inventory.UseItem(player.inventory.currentControllableItem.baseItem);
        }
    }
}
