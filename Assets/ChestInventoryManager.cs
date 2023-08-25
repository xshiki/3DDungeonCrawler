using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestInventoryManager : InventoryManager, IInteractable
{
    [SerializeField] public int InventorySize;
    [SerializeField] private string _prompt;
    [SerializeField] public GameObject DynamicInventoryDisplay;
    [SerializeField] public DynamicInventoryScript dynamicInventoryScript;
    [SerializeField] public GameObject PlayerInventory;
    public string InterActionPrompt => _prompt;
    [SerializeField] public GameObject dynamicInventoryUI;

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
        if (!DynamicInventoryDisplay.activeInHierarchy)
        {
            dynamicInventoryUI.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            Debug.Log("turn off");
            //copy changed values back to treasuref
            for(int i = 0;  i< inventorySlots.Length; i++)
            {
                InventoryItem itemInSlot = dynamicInventoryScript.inventorySlots[i].GetComponentInChildren<InventoryItem>();
                InventoryItem oldItemInSlot = inventorySlots[i].transform.GetComponent<InventoryItem>();
             
                if(itemInSlot != null)
                {
                    Destroy(oldItemInSlot);
                   
                }
              
            }
 
            dynamicInventoryUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        interactSucessful = true;

    }

    private void Update()
    {
   
       
    }


}
