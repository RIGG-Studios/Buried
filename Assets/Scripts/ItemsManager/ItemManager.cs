using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemObjects itemVariables;
    
    public void OnPickUp(Vector2 mousePosition, Transform player)
    {
        transform.GetComponent<Collider2D>().enabled = false;
        transform.GetComponent<SpriteRenderer>().sortingOrder = 3;
    }

    public void OnDrop(Vector2 mousePosition, Transform player, float interactRange)
    {
        Vector2 playerPosition = new Vector2(player.position.x, player.position.y);

        if ((playerPosition - mousePosition).magnitude > interactRange)
        {
            transform.position = playerPosition + (mousePosition - playerPosition).normalized * interactRange/2;
        }

        transform.GetComponent<Collider2D>().enabled = true;
        transform.GetComponent<SpriteRenderer>().sortingOrder = 1;
    }

    public void Tick(Vector2 mousePosition, Transform player)
    {
        transform.position = new Vector2(mousePosition.x, mousePosition.y);
    }
}
