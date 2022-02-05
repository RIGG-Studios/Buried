using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Inventory : MonoBehaviour
{
    public SlotManager slotManager;
    public Item flashlight;

    private void Start()
    {
        for (int i = 0; i < 4; i++)
            slotManager.SetupSlots();
    }

    private void Update()
    {
        if (Keyboard.current.uKey.wasPressedThisFrame)
            slotManager.AddSlot(flashlight);
    }
}
