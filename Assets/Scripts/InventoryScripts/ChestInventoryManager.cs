using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class ChestInventoryManager : InventoryManager, IInteractable
{
    [SerializeField] public int InventorySize;
    [SerializeField] private string _prompt;
    [SerializeField] public GameObject PlayerInventory;
    [SerializeField] public FirstPersonController firstPersonController;
    public string InterActionPrompt => _prompt;
    [SerializeField] public GameObject dynamicInventoryUI;
    [SerializeField] public List<InventoryItemData> lootList = new List<InventoryItemData>();
    [SerializeField] public LootTable lootTable;

 


    private void Awake()
    {
        PlayerInventory = GameObject.Find("PlayerUI");
        firstPersonController= GameObject.Find("Player").GetComponent<FirstPersonController>();
        lootTable = GetComponent<LootTable>();  
        lootList = lootTable.lootList;

        fillChest();
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
        if (!dynamicInventoryUI.activeInHierarchy)
        {
            dynamicInventoryUI.SetActive(true);
            Transform inventory = PlayerInventory.transform.Find("PlayerInventory");
            inventory.gameObject.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            firstPersonController.enabled = false;

        }
        else
        {
            dynamicInventoryUI.SetActive(false);
            Transform inventory = PlayerInventory.transform.Find("PlayerInventory");
            inventory.gameObject.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            firstPersonController.enabled = true;
        }
        interactSucessful = true;

    }


}
