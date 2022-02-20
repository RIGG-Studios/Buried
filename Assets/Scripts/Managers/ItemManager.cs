using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public ItemObjects itemVariables;
    public GameObject itemPrefab;
    public bool destroyOnUse;
    public bool used;

    public ItemObjects preRequisite;
}
