using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] slots;
    
    public StaticInventoryDisplay()
    {
    }
    protected virtual void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }
    protected virtual void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }
    private void RefreshStaticDisplay()
    {
        if(inventoryHolder != null) {
        inventorySystem = inventoryHolder.PrimaryInventorySystem;
        inventorySystem.OnInventorySlotChanged += UpdateSlot;
        }else 
        { Debug.Log($"No inventory assignted to {this.gameObject}");}
        AssignSlot(inventorySystem, 0);
    }
    protected override void Start()
    {
        base.Start();
        //if(inventoryHolder != null) {
        //inventorySystem = inventoryHolder.PrimaryInventorySystem;
        //inventorySystem.OnInventorySlotChanged += UpdateSlot;
        //}
        //else { Debug.Log($"No inventory assignted to {this.gameObject}");}
        RefreshStaticDisplay();



    }

    public override void AssignSlot(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();
        //if(slots.Length != inventorySystem.InventorySize) Debug.Log("Inventory slots out of sync")
        for(int i = 0; i< inventoryHolder.Offset; i++) {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }

}
