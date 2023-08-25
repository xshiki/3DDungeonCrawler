using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class DynamicInventoryScript : InventoryManager
{

    void createInventory(ChestInventoryManager invToDisplay)
    {
        
        Debug.Log("destroy chidlren");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        inventorySlots = invToDisplay.inventorySlots;
        for(int i =0; i<inventorySlots.Length; i++)
        {
            Instantiate(inventorySlots[i], transform);
        }
        ///READD ITEMS TO THE inventorySlotslist
    }

    private void OnEnable()
    {
        Debug.Log("activated");
        OnDynamicDisplayRequested += createInventory;
    }

    private void OnDisable()
    {

        OnDynamicDisplayRequested -= createInventory;
       
    }
}
