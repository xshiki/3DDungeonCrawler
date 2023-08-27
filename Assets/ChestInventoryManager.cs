using System.Collections;
using System.Collections.Generic;
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
            PlayerInventory.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            firstPersonController.enabled = false;

        }
        else
        {
            dynamicInventoryUI.SetActive(false);
            PlayerInventory.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            firstPersonController.enabled = true;
        }
        interactSucessful = true;

    }


}
