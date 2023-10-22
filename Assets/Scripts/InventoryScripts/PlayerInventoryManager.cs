using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventoryManager : InventoryManager
{

    [SerializeField]
    private InputActionReference hotbarSelection;
    [SerializeField]
    private InputActionReference throwAwayItem;

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
            Debug.Log((int)hotbarSelection.action.ReadValue<float>());
            ChangeSelectedSlot((int)hotbarSelection.action.ReadValue<float>()-1);

        }

        if(throwAwayItem.action.triggered && selectedSlot != -1)
        {
            ThrowAwaySelectedItem();
        }
    }

    void ThrowAwaySelectedItem()
    {
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            // Remove the item from the slot
            itemInSlot.count--;
            GameObject prefabInstance = Instantiate(itemInSlot.item.ItemPrefab,transform.position + transform.forward * 2f, Quaternion.identity);
            prefabInstance.layer = 7;
            prefabInstance.AddComponent<Rigidbody>();
            if (itemInSlot.count <= 0)
            {
                // Destroy the item if the count is zero or less
                Destroy(itemInSlot.gameObject);
            }
            else
            {
                // Refresh the item to update the UI with the new count
                itemInSlot.Refresh();
            }
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if (selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        if(newValue == -1)
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

                var consum = itemInSlot.item as ConsumableItemData;

                var health = consum._healthToReplenish;
                var mana = consum._manaToReplenish;

             

                GameObject.Find("Player").GetComponent<PlayerRessource>().ReplenshHealthMana(health, mana);
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);

                }
                else
                {
                    itemInSlot.Refresh();
                }
            }else if(use && itemInSlot.item.isAbility)
            {
                var consum = itemInSlot.item as SupportSpellSO;
                GameObject.Find("Player").GetComponent<PlayerRessource>().ApplySupport(consum);
            }



            return itemInSlot.item;
        }


        return null;
    }
}
