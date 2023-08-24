using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChestInventoryManager : InventoryManager, IInteractable
{

    [SerializeField] public int InventorySize;
    [SerializeField] private string _prompt;

    public void Awake()
    {
        for (int i = 0; i < InventorySize; i++)
        {
            var uiSlot = Instantiate(inventoryItemPrefab);
          

        }
    }
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
        Debug.Log("Opening chest!");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        interactSucessful = true;
    }

  
}
