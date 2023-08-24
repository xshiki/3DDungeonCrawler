using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public bool AddItem(InventoryItemData item)
    {

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

            if (itemInSlot != null && itemInSlot.item == item && itemInSlot.count < itemInSlot.maxStacks)
            {
                itemInSlot.count++;
                itemInSlot.Refresh();
                return true;
            }
        }




            for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot = inventorySlots[i];
            InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();
          
            if(itemInSlot == null)
            {
                fillInventorySlot(item,slot);
                Debug.Log("added" + item.name);
                return true;
            }
        }
        return false;

    }

    public void RemoveFromStack()
    {

    }


    void fillInventorySlot(InventoryItemData item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);

    }
}
