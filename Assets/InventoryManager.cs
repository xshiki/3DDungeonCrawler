using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class InventoryManager : MonoBehaviour
{   

    public static InventoryManager Instance;
    [SerializeField]
    private InputActionReference hotbarSelection;
    [SerializeField] public InventorySlot[] inventorySlots;
    public GameObject inventoryItemPrefab;
    public static UnityAction<ChestInventoryManager> OnDynamicDisplayRequested;

    int selectedSlot = -1;

    private void Awake()
    {
        Instance = this;
     
    }
    private void Update()
    {
        if(hotbarSelection.action.triggered)
        {
            Debug.Log("trying to use selected item via hotbar");
            ChangeSelectedSlot((int) hotbarSelection.action.ReadValue<float>() - 1);
            
        }
    }

    void ChangeSelectedSlot(int newValue)
    {
        if(selectedSlot >= 0)
        {
            inventorySlots[selectedSlot].Deselect();
        }
        inventorySlots[newValue].Select();
        selectedSlot = newValue;
        GetSelectedItem(true);
    }
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

    public InventoryItemData GetSelectedItem(bool use)
    {

   
        InventorySlot slot = inventorySlots[selectedSlot];
        InventoryItem itemInSlot = slot.GetComponentInChildren<InventoryItem>();

        if (itemInSlot != null)
        {
            if (use == true && itemInSlot.item.consumable)
            {            
                itemInSlot.item.UseItem();
                itemInSlot.count--;
                if(itemInSlot.count <= 0)
                {
                   
                    Destroy(itemInSlot.gameObject);
                    
                }
                else
                {
                    itemInSlot.Refresh();
                }
            }
            Debug.Log("hotbar slot not used");
            return itemInSlot.item;
        }
        

        return null;
    }
    void fillInventorySlot(InventoryItemData item, InventorySlot slot)
    {
        GameObject newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGo.GetComponent<InventoryItem>();
        inventoryItem.InitialiseItem(item);

    }
}
