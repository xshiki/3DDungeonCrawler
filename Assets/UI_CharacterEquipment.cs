using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UI_CharacterEquipment : MonoBehaviour
{
    
    public InventorySlot weaponSlot;
    public InventorySlot helmetSlot;
    public InventorySlot chestSlot;
    public InventorySlot pantsSlot;
    public InventorySlot bootsSlot;
    public PlayerEquipment playerEquipment;
    public Transform socket; //


    private void Update()
    {
        if (!weaponSlot.GetComponentInChildren<InventoryItem>())
        {
            foreach (Transform child in socket)
            {
                Destroy(child.gameObject);
                playerEquipment.SetWeaponItem(null);
            }
        }
    }
    private void Awake()
    {

       
        weaponSlot.OnItemDropped += WeaponSlot_OnItemDropped;
        playerEquipment.OnEquipmentChanged += PlayerEquipment_OnEquipmentChanged;
    }

    private void WeaponSlot_OnItemDropped(object sender, InventorySlot.OnItemDroppedEventArgs e) {
        Debug.Log("weapon dropped in weapon slot");
        playerEquipment.SetWeaponItem(e.item.item);
     

    }

    public void SetPlayerEquipment(PlayerEquipment playerEquipment)
    {
        this.playerEquipment = playerEquipment;
       
    }

    private void PlayerEquipment_OnEquipmentChanged(object sender, EventArgs e)
    {
        InitializeEquipment();
    }

    public void InitializeEquipment()
    {   
        foreach(Transform child in socket)
        {
            Destroy(child.gameObject);
        }
        InventoryItemData weaponItem = playerEquipment.GetWeaponItem();
        if(weaponItem!=null) { 
            GameObject weaponObject = Instantiate(weaponItem.ItemPrefab, socket);
            weaponObject.layer = 6;
            Debug.Log("weapon instantiated");
        }
    }
}
