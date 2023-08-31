using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class UI_CharacterEquipment : MonoBehaviour
{
    [Header("InventorySlot of Equipment")]
    public InventorySlot weaponSlot;
    public InventorySlot helmetSlot;
    public InventorySlot chestSlot;
    public InventorySlot pantsSlot;
    public InventorySlot bootsSlot;

    public PlayerController playerController;
    public PlayerEquipment playerEquipment;

    [Header("Sockets for Equipment")] //Places equipment on the correct position
    public Transform weaponSocket;
    public Transform helmetSocket;
    public Transform chestSocket;
    public Transform pantsSocket;
    public Transform bootsSocket;

    private ChangeNotifier weaponNotifier;
    private ChangeNotifier helmetNotifier;
    private ChangeNotifier chestNotifier;
    private ChangeNotifier pantsNotifier;
    private ChangeNotifier bootsNotifier;
    


    public void ClearSocket(Transform socketTransform)
    {
        if(socketTransform.childCount == 0) { return; }
        foreach(Transform child in socketTransform)
        {
            Destroy(child.gameObject);
        }

    }
    private void Awake()
    {
        weaponSlot.OnItemDropped += WeaponSlot_OnItemDropped;
        helmetSlot.OnItemDropped += HelmetSlot_OnItemDropped;
        chestSlot.OnItemDropped += ChestSlot_OnItemDropped;
        pantsSlot.OnItemDropped += PantsSlot_OnItemDropped;
        bootsSlot.OnItemDropped += BootsSlot_OnItemDropped;


        weaponNotifier = weaponSlot.GetComponent<ChangeNotifier>();
        helmetNotifier = helmetSlot.GetComponent<ChangeNotifier>();
        chestNotifier = chestSlot.GetComponent<ChangeNotifier>();
        pantsNotifier = pantsSlot.GetComponent<ChangeNotifier>();
        bootsNotifier = bootsSlot.GetComponent<ChangeNotifier>();

        weaponNotifier.OnChildRemoved += PlayerEquipment_OnWeaponRemoved;
        helmetNotifier.OnChildRemoved += PlayerEquipment_OnHelmetRemoved;
        chestNotifier.OnChildRemoved += PlayerEquipment_OnChestRemoved;
        pantsNotifier.OnChildRemoved += PlayerEquipment_OnPantsRemoved;
        bootsNotifier.OnChildRemoved += PlayerEquipment_OnBootsRemoved;

        playerEquipment.OnEquipmentChanged += PlayerEquipment_OnEquipmentChanged;
    }

    private void PlayerEquipment_OnBootsRemoved(object sender, EventArgs e)
    {
        ClearSocket(bootsSocket);
        playerEquipment.SetBootsItem(null);
    }

    private void PlayerEquipment_OnPantsRemoved(object sender, EventArgs e)
    {
        ClearSocket(pantsSocket);
        playerEquipment.SetPantsItem(null);
    }

    private void PlayerEquipment_OnChestRemoved(object sender, EventArgs e)
    {
        ClearSocket(chestSocket);
        playerEquipment.SetChestItem(null);
    }

    private void PlayerEquipment_OnHelmetRemoved(object sender, EventArgs e)
    {
        ClearSocket(helmetSocket);
        playerEquipment.SetHelmetItem(null);
    }

    private void PlayerEquipment_OnWeaponRemoved(object sender, EventArgs e)
    {
        ClearSocket(weaponSocket);
        playerEquipment.SetWeaponItem(null);
    }

    private void BootsSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        playerEquipment.SetBootsItem(e.item.item);
    }

    private void PantsSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        playerEquipment.SetPantsItem(e.item.item);
    }

    private void ChestSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        playerEquipment.SetChestItem(e.item.item);
    }

    private void HelmetSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e)
    {
        playerEquipment.SetHelmetItem(e.item.item);
    }

    private void WeaponSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e) {
        playerEquipment.SetWeaponItem(e.item.item);
     

    }

    public void SetPlayerEquipment(PlayerEquipment playerEquipment)
    {
        this.playerEquipment = playerEquipment;
       
    }

    private void PlayerEquipment_OnEquipmentChanged(object sender, EventArgs e)
    {
        InitializeWeapon();
        InitializeHelmet();
        InitializeChest();
        InitializePants();
        InitializeBoots();
    }

    public void InitializeWeapon()
    {   
       

        foreach (Transform child in weaponSocket)
        {
            Destroy(child.gameObject);
          
        }

        InventoryItemData weaponItem = playerEquipment.GetWeaponItem();
        if(weaponItem!=null) { 
            GameObject weaponObject = Instantiate(weaponItem.ItemPrefab, weaponSocket);
            weaponObject.layer = 6;
            ItemPickUp itemPickUp= weaponObject.GetComponent<ItemPickUp>();
            itemPickUp.enabled = false;
            playerController.SetCurrentWeapon(weaponObject.GetComponent<WeaponController>());
            Debug.Log("weapon instantiated");
        }
    }


    public void InitializeHelmet()
    {

        foreach (Transform child in helmetSocket)
        {
            Destroy(child.gameObject);

        }

        InventoryItemData helmetItem = playerEquipment.GetHelmetItem();
        if (helmetItem != null)
        {
            GameObject helmetObject = Instantiate(helmetItem.ItemPrefab, helmetSocket);
            
           
        }
    }

    public void InitializeChest()
    {
        foreach (Transform child in chestSocket)
        {
            Destroy(child.gameObject);

        }



        InventoryItemData chestItem = playerEquipment.GetChestItem();
        if (chestItem != null)
        {
            GameObject chestObject = Instantiate(chestItem.ItemPrefab, chestSocket);
          
        }
    }
    public void InitializePants()
    {
        foreach (Transform child in pantsSocket)
        {
            Destroy(child.gameObject);

        }


        InventoryItemData pantsItem = playerEquipment.GetPantsItem();
        if (pantsItem != null)
        {
            GameObject pantsObject = Instantiate(pantsItem.ItemPrefab, pantsSocket);

        }
    }

    public void InitializeBoots()
    {
        foreach (Transform child in bootsSocket)
        {
            Destroy(child.gameObject);

        }



        InventoryItemData bootsItem = playerEquipment.GetBootsItem();
        if (bootsItem != null)
        {
            GameObject bootsObject = Instantiate(bootsItem.ItemPrefab, bootsSocket);

        }
    }
}
