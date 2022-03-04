using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Vector3 startingPosition;
    public Vector3 startingRotation;

    public ItemProperties baseItem;

    public virtual void SetupController(Player player) { }
    public virtual void UseItem() { }
}
