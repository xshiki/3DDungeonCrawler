using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;



[RequireComponent(typeof(LootTable))]
public class ChestInventoryManager : InventoryManager, IInteractable
{
    //[SerializeField] public int InventorySize;
    [SerializeField] private string _prompt;
    [SerializeField] public GameObject PlayerInventory;
    [SerializeField] public GameObject DynamicInventory;
    [HideInInspector]
    [SerializeField] public FirstPersonController firstPersonController;
    PlayerController playerController;
    public string InterActionPrompt => _prompt;
    [SerializeField] public List<InventoryItemData> lootList = new List<InventoryItemData>();

    [SerializeField] public LootTable lootTable;


    bool isOpen = false;
 


    private void Awake()
    {
        PlayerInventory = GameObject.Find("PlayerUI");
        DynamicInventory = GameObject.Find("DynamicInventory");

        firstPersonController= GameObject.Find("Player").GetComponent<FirstPersonController>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        playerController.OnInventoryClosed += Close;
        lootTable = GetComponent<LootTable>();  
        lootList = lootTable.lootList;

        fillChest();
    }

    private void OnEnable()
    {
        playerController.OnInventoryClosed += Close;
    }

    private void OnDisable()
    {
        playerController.OnInventoryClosed -= Close;
    }
    void fillChest()
    {
      
        foreach(InventorySlot slot in inventorySlots)
        {
            InventoryItemData randomItem = lootTable.getDroppedItem();

            if(randomItem != null)
            {
                fillInventorySlot(randomItem, slot);
             
            }
           

        }
    }
    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(InteractScript interactor)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(InteractScript interactor, out bool interactSucessful)
    {
        if (!isOpen)
        {
        
            Transform inventory = PlayerInventory.transform.Find("PlayerInventory");
            inventory.gameObject.SetActive(true);
            DisplayDynamicInventory();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            firstPersonController.enabled = false;
            isOpen = true;

        }
        else
        {
          Close();
        }
        interactSucessful = true;

    }

    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Close();
          
        }
    }
    void DisplayDynamicInventory()
    {
        foreach (InventorySlot slot in inventorySlots)
        {

            slot.transform.SetParent(DynamicInventory.transform, false);
            slot.transform.rotation = Quaternion.identity;
        }
    }
    void ClearDynamicInventory()
    {
        if(inventorySlots.Length <= 0) { return; }
        foreach (InventorySlot slot in inventorySlots)
        {
            slot.transform.SetParent(this.transform, false);
        }

    }


    private void OnDestroy()
    {
        playerController.OnInventoryClosed -= Close;
    }

    void Close()
    {
        isOpen = false;
        ClearDynamicInventory();
        Transform inventory = PlayerInventory.transform.Find("PlayerInventory");
        inventory.gameObject.SetActive(false);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        firstPersonController.enabled = true;
        isOpen = false;
    }
}
