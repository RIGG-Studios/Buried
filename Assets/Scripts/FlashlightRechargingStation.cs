using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class FlashlightRechargingStation : InteractableObject
{
    public override void ButtonInteract()
    {

    }

    public override void HoverInteract()
    {
    }

    public override void StopHoverInteract()
    {
    }


    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (CheckCollision(collision))
            GameEvents.OnToggleRechargingStation?.Invoke(true);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (CheckCollision(collision))
            GameEvents.OnToggleRechargingStation?.Invoke(false);
    }


    private bool CheckCollision(Collider2D collision)
    {
        Player player;
        collision.TryGetComponent(out player);

        return player != null;
    }

}