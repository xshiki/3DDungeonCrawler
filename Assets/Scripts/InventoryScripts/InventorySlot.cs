using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Printing;
using UnityEngine;


public enum InventorySlotTag { None, Head, Chest, Pants, Boots, Weapon}
[System.Serializable]
public class InventorySlot: ISerializationCallbackReceiver
{
    [NonSerialized] private InventoryItemData itemData;
    [SerializeField] private int stackSize; //Current stack size
    [SerializeField] private int _itemID = -1;
    [SerializeField] public InventorySlotTag slotTag; 
    public InventoryItemData ItemData => itemData;
    public int StackSize => stackSize;  


    
    public InventorySlot(InventoryItemData source, int amount)
    {
        itemData= source;
        Debug.Log(itemData.ID); 
        _itemID = itemData.ID;
        slotTag = InventorySlotTag.None;
        stackSize= amount;  

    }

    public InventorySlot(InventoryItemData source, int amount, InventorySlotTag tag)
    {
        itemData = source;
        Debug.Log(itemData.ID);
        _itemID = itemData.ID;
        slotTag = tag;
        stackSize = amount;

    }

    public InventorySlot() { //Constructor empty inventory slot
        ClearSlot();
    }

    public InventorySlot(InventorySlotTag tag)
    { //Constructor empty inventory slot
        ClearSlot();
        slotTag = tag;
    }

    public void ClearSlot()
    {
        itemData = null;
        _itemID = -1;   
        stackSize = -1;
    }


    public void AssignItem(InventorySlot invSlot) //Assign Item to the slot
    {
        if(slotTag == InventorySlotTag.None)
        {
            Debug.Log("true");
            if (itemData == invSlot.ItemData) //does the slot contain the same item? then add to stack
            {
                AddToStack(invSlot.stackSize);
            }
            else //Overwrite the slot with ivnentory slot that're passing in
            {
                itemData = invSlot.ItemData;
                _itemID = itemData.ID;
                stackSize = 0;
                AddToStack(invSlot.stackSize);
            }
        }
        else
        {
           
          
        }
       
    }
  
    public void UpdateInventorySlot(InventoryItemData data, int amount)//Update slot directly
    {
        itemData= data;
        _itemID = itemData.ID;
        stackSize= amount;
    }
    public bool EnoughRoomLeftInStack(int amountToAdd, out int amountRemaining) //Is there enough room for stack, for amount we trying to add
    {
        amountRemaining = itemData.MaxStackSize - stackSize;
        return EnoughRoomLeftInStack(amountToAdd);
    }
    public bool EnoughRoomLeftInStack(int amountToAdd)
    {
        if (stackSize + amountToAdd <= itemData.MaxStackSize) { 
            return true; 
        }else return false;
    }
    public void AddToStack(int amount)
    {
        stackSize += amount;
    }
    public void RemoveFromStack(int amount) {
        stackSize -= amount;
    }

    public bool SplitStack(out InventorySlot splitStack)
    {

        if(stackSize <= 1)
        {
            splitStack = null;
            return false;
        }

        int halfStack = Mathf.RoundToInt(stackSize / 2);
        RemoveFromStack(halfStack);

        splitStack = new InventorySlot(itemData, halfStack);

        return true;
    }

    public void OnBeforeSerialize()
    {
        
    }

    public void OnAfterDeserialize()
    {
        if (_itemID == -1) return;
        //TODO
        var db = Resources.Load<Database>("Database"); //Access db later with a singleton 
        itemData = db.GetItem(_itemID);

    }
}
