using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{


    public event EventHandler OnEquipmentChanged;
    [SerializeField]
    private InventoryItemData weaponItem;
    [SerializeField]
    private InventoryItemData helmetItem;
    [SerializeField]
    private InventoryItemData chestItem;
    [SerializeField]
    private InventoryItemData pantsItem;
    [SerializeField]
    private InventoryItemData bootsItem;
   

    public InventoryItemData GetWeaponItem() { return weaponItem; }
    public InventoryItemData GetHelmetItem() {  return helmetItem; }   
    public InventoryItemData GetChestItem() {  return chestItem; }  
    public InventoryItemData GetPantsItem() {  return pantsItem; }  
    public InventoryItemData GetBootsItem() {  return bootsItem; }  
    

    public void SetWeaponItem(InventoryItemData weaponItem) {
    
        this.weaponItem = weaponItem;
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("weapon changed");
    }
    public void SetHelmetItem(InventoryItemData helmetItem) {
        this.helmetItem = helmetItem;
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
    }
    public void SetChestItem(InventoryItemData chestItem)
    {
        this.chestItem = chestItem;
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
    }
    public void SetPantsItem(InventoryItemData pantsItem)
    {
        this.pantsItem  = pantsItem;
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
    }
    public void SetBootsItem(InventoryItemData bootsItem)
    {
        this.bootsItem = bootsItem;
        OnEquipmentChanged?.Invoke(this, EventArgs.Empty);
    }
}
