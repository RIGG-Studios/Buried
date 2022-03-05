using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    public Vector3 startingPosition;
    public Vector3 startingRotation;

    public Item baseItem;

    protected Player player;

    public virtual void SetupController(Player player) => this.player = player;
    public virtual void UseItem() { }

    public virtual void ResetItem() { }
}
