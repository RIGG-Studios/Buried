using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ItemManagement : MonoBehaviour
{

    [SerializeField] private Transform itemParent = null;

    private List<ItemProperties> controllableItems = new List<ItemProperties>();
    private int currentItemIndex;
    private Player player = null;

    public ItemProperties activeItem { get; private set; }

    private void Start()
    {
        player = GetComponent<Player>();
    }

    public void AddControllableItems(ItemProperties[] props)
    {
        if (props == null || props.Length <= 0)
            return;

        for(int i =0; i < props.Length; i++)
        {
            ItemController obj = Instantiate(props[i].itemPrefab, itemParent).GetComponent<ItemController>();

            if (obj != null)
            {
                obj.transform.position = obj.startingPosition;
                obj.transform.rotation = Quaternion.Euler(obj.startingRotation);

                controllableItems.Add(props[i]);
            }
        }
    }



}
