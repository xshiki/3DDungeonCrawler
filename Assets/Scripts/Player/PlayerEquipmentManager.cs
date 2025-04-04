using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class PlayerEquipmentManager : MonoBehaviour
{
    [Header("InventorySlot of Equipment")]
    public InventorySlot weaponSlot;
    public InventorySlot weaponSlotLeft;
    public InventorySlot helmetSlot;
    public InventorySlot chestSlot;
    public InventorySlot pantsSlot;
    public InventorySlot bootsSlot;

    public PlayerController playerController;
    public PlayerEquipment playerEquipment;

    [Header("Sockets for Equipment")] //Places equipment on the correct position
    public Transform weaponSocket;
    public Transform weaponSocketLeft;
    public Transform helmetSocket;
    public Transform chestSocket;
    public Transform pantsSocket;
    public Transform bootsSocket;


    public SkinnedMeshRenderer helmet;
    public SkinnedMeshRenderer chest;
    public SkinnedMeshRenderer pants;
    public SkinnedMeshRenderer boots;

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
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerEquipment = GameObject.Find("Player").GetComponent<PlayerEquipment>();
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

        foreach (Transform child in weaponSocketLeft)
        {
            Destroy(child.gameObject);

        }


        playerController.UnequipWeapon();
        Invoke("EquipWeapon", 1f);
     
    }

    void EquipWeapon()
    {
        WeaponItemData weaponItem = playerEquipment.GetWeaponItem() as WeaponItemData;
        if (weaponItem != null)
        {
            GameObject weaponObject = Instantiate(weaponItem.ItemPrefab, weaponSocket);
            
            if(weaponItem.WeaponType == WeaponItemData.Weapons.Daggers)
            {
                GameObject weaponObjectOffHand = Instantiate(weaponItem.ItemPrefab, weaponSocketLeft);
                ItemPickUp itemPickUpOffHand = weaponObjectOffHand.GetComponent<ItemPickUp>();
                itemPickUpOffHand.enabled = false;
                weaponObjectOffHand.layer = 6;
            }

          
            ItemPickUp itemPickUp = weaponObject.GetComponent<ItemPickUp>();
            itemPickUp.enabled = false;
            weaponObject.layer = 6;
            Debug.Log("layer changed");
            if (weaponObject.GetComponent<WeaponController>())
            {
                playerController.SetCurrentWeapon(weaponObject.GetComponent<WeaponController>());
            }
            else if (weaponObject.GetComponent<MagicWeaponController>())
            {
                playerController.SetCurrentMagicWeapon(weaponObject.GetComponent<MagicWeaponController>());
            }


        }
    }

    public void InitializeHelmet()
    {

        foreach (Transform child in helmetSocket)
        {
            Destroy(child.gameObject);
            helmet.enabled = false;

        }

        InventoryItemData helmetItem = playerEquipment.GetHelmetItem();
        if (helmetItem != null)
        {
            helmet.enabled = true;


            Material mat = helmetItem.ItemPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            if (mat != null) { helmet.material = mat; }
        
            
            


            GameObject helmetObject = Instantiate(helmetItem.ItemPrefab, helmetSocket);
            //Once instantiated equipment should only be cosmetic / change playerstats
            SetLayerRecursively(helmetObject.transform, 9);
            DisableColliders(helmetObject);
            DisableScripts(helmetObject);
           
        }
    }

    public void InitializeChest()
    {
        foreach (Transform child in chestSocket)
        {
            Destroy(child.gameObject);
            chest.enabled = false;
        }



        InventoryItemData chestItem = playerEquipment.GetChestItem();
        if (chestItem != null)
        {
            chest.enabled = true;
            Material mat = chestItem.ItemPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            if (mat != null) { chest.material = mat; }

            GameObject chestObject = Instantiate(chestItem.ItemPrefab, chestSocket);
            SetLayerRecursively(chestObject.transform, 9);
            DisableColliders(chestObject);
            DisableScripts(chestObject);
          
        }
    }
    public void InitializePants()
    {
        foreach (Transform child in pantsSocket)
        {
            Destroy(child.gameObject);
            pants.enabled = false;
        }


        InventoryItemData pantsItem = playerEquipment.GetPantsItem();
        if (pantsItem != null)
        {
            pants.enabled = true;
            Material mat = pantsItem.ItemPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            if (mat != null) { pants.material = mat; }

            GameObject pantsObject = Instantiate(pantsItem.ItemPrefab, pantsSocket);
            SetLayerRecursively(pantsObject.transform, 9);
            DisableColliders(pantsObject);
            DisableScripts (pantsObject);

        }
    }

    public void InitializeBoots()
    {
        foreach (Transform child in bootsSocket)
        {
            Destroy(child.gameObject);
            boots.enabled = false;

        }

        InventoryItemData bootsItem = playerEquipment.GetBootsItem();
        if (bootsItem != null)
        {
            boots.enabled = true;
            boots.sharedMesh = bootsItem.ItemPrefab.GetComponent<MeshFilter>().sharedMesh;
            Material mat = bootsItem.ItemPrefab.GetComponent<MeshRenderer>().sharedMaterial;
            if (mat != null) { boots.material = mat; }

            GameObject bootsObject = Instantiate(bootsItem.ItemPrefab, bootsSocket);
            SetLayerRecursively(bootsObject.transform, 9);
            DisableColliders(bootsObject);
            DisableScripts(bootsObject);

        }
    }




    private void DisableColliders(GameObject obj)
    {
        Collider[] colliders = obj.GetComponentsInChildren<Collider>(true);

        foreach (Collider collider in colliders)
        {
            collider.enabled = false;
        }
    }

    private void DisableScripts(GameObject obj)
    {
        Behaviour[] scripts = obj.GetComponentsInChildren<Behaviour>(true);

        foreach (Behaviour script in scripts)
        {
            if (script != this)
            {
                script.enabled = false;
            }
        }
    }



    private void SetLayerRecursively(Transform transform, int layer)
    {
        transform.gameObject.layer = layer;

        foreach (Transform child in transform)
        {
            SetLayerRecursively(child, layer);
        }
    }

}
