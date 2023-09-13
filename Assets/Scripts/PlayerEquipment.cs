using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    public event EventHandler<OnEquipChangedArgs> OnEquipmentChanged;

    public class OnEquipChangedArgs : EventArgs
    {
        public InventoryItemData newItem;
        public InventoryItemData oldItem;
    }
    [SerializeField]
    private InventoryItemData current_weaponItem;
    [SerializeField]
    private InventoryItemData current_helmetItem;
    [SerializeField]
    private InventoryItemData current_chestItem;
    [SerializeField]
    private InventoryItemData current_pantsItem;
    [SerializeField]
    private InventoryItemData current_bootsItem;
   

    public InventoryItemData GetWeaponItem() { return current_weaponItem; }
    public InventoryItemData GetHelmetItem() { return current_helmetItem; }   
    public InventoryItemData GetChestItem() { return current_chestItem; }  
    public InventoryItemData GetPantsItem() { return current_pantsItem; }  
    public InventoryItemData GetBootsItem() { return current_bootsItem; }

    InventoryItemData previosItem;
    public void SetWeaponItem(InventoryItemData weaponItem) {

        previosItem = current_weaponItem;
        current_weaponItem = weaponItem;
        OnEquipmentChanged?.Invoke(this, new OnEquipChangedArgs { newItem = weaponItem, oldItem = previosItem });
    }
    public void SetHelmetItem(InventoryItemData helmetItem) {

       
        
        previosItem = current_helmetItem;
        current_helmetItem = helmetItem;
        OnEquipmentChanged?.Invoke(this, new OnEquipChangedArgs { newItem = helmetItem, oldItem = previosItem });
    }
    public void SetChestItem(InventoryItemData chestItem)
    {

        previosItem = current_chestItem;
        current_chestItem = chestItem;
        OnEquipmentChanged?.Invoke(this, new OnEquipChangedArgs { newItem = chestItem, oldItem = previosItem });
    }
    public void SetPantsItem(InventoryItemData pantsItem)
    {

        previosItem = current_pantsItem;
        current_pantsItem = pantsItem;
        OnEquipmentChanged?.Invoke(this, new OnEquipChangedArgs { newItem = pantsItem, oldItem = previosItem });
    }
    public void SetBootsItem(InventoryItemData bootsItem)
    {

        previosItem = current_bootsItem;
        current_bootsItem = bootsItem;
        OnEquipmentChanged?.Invoke(this, new OnEquipChangedArgs { newItem = bootsItem, oldItem = previosItem });
    }
}
