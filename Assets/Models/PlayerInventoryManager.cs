using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryManager : InventoryManager
{

    [SerializeField]
    private InputActionReference hotbarSelection;

    [HideInInspector]
    public InventorySlot weaponSlot;
    int selectedSlot = -1;
    private void Awake()
    {
        Instance = this;
        if(weaponSlot == null)
        {
            weaponSlot = GameObject.Find("Weapon Slot").GetComponent<InventorySlot>();
        }

    }

    private void Update()
    {
        if (hotbarSelection.action.triggered)
        {
            ChangeSelectedSlot((int)hotbarSelection.action.ReadValue<float>() - 1);

        }
    }



    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        if(newValue == 0)
        {
            newValue = 9;
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        GetSelectedItem(true);
        

    }



    public InventoryItemData GetSelectedItem(bool use)
    {


        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            if (itemInSlot.myType == InventoryItemType.Weapon)
            {

                if (weaponSlot.transform.childCount > 0)
                {
                    Transform child = weaponSlot.transform.GetChild(0);
                    itemInSlot.transform.SetParent(weaponSlot.transform, false);
                    weaponSlot.SlotChanged(itemInSlot);
                    child.SetParent(slot.transform, false);
                }
                else
                {
                    itemInSlot.transform.SetParent(weaponSlot.transform, false);
                    weaponSlot.SlotChanged(itemInSlot);
                }
                inventorySlots[selectedSlot].Deselect();
            }
            else
            if (use && itemInSlot.item.consumable)
            {
                Debug.Log("consumed item");

                itemInSlot.item.UseItem();
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);

                }
                else
                {
                    itemInSlot.Refresh();
                }
            }



            return itemInSlot.item;
        }


        return null;
    }
}
