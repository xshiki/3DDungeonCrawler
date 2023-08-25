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
            DynamicInventoryDisplay.SetActive(true);
            OnDynamicDisplayRequested?.Invoke(this);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }
        else
        {
            Debug.Log("turn off");
            //copy changed values back to treasure
          
            inventorySlots = dynamicInventoryScript.inventorySlots;
            foreach (InventorySlot slot in inventorySlots)
            {
                Destroy(slot.transform.GetChild(0).gameObject);
            }
            for (int i = 0; i < inventorySlots.Length; i++)
            {
                Instantiate(inventorySlots[i], transform);
            }
            DynamicInventoryDisplay.SetActive(false);
            OnDynamicDisplayRequested?.Invoke(this);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        interactSucessful = true;

    }

    private void Update()
    {
   
       
    }


}
