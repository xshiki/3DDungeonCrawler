using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemPickUpInteractable : MonoBehaviour, IInteractable
{


    [SerializeField] private string _prompt;
    public string InterActionPrompt => _prompt;

    [SerializeField]
    private InventoryItemData _item;

    public UnityAction<IInteractable> OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void EndInteraction()
    {
        throw new System.NotImplementedException();
    }

    public bool Interact(InteractScript interactor)
    {
        throw new System.NotImplementedException();
    }

    public void Interact(InteractScript interactor, out bool interactSucessfull)
    {

        var inventory = interactor.GetComponentInChildren<InventoryManager>();

        if (interactor.CompareTag("Player"))
        {
           
            Debug.Log("is player");
            if (inventory.AddItem(_item))
            {
                NotificationManager.Instance.SetNewNotification("Picked up " + _item.DisplayName);
                Destroy(this.gameObject);
            }
        }

        interactSucessfull = true;
   
    }


    private void Awake()
    {
        ItemPickUp _itemPickUp = GetComponent<ItemPickUp>();
        _item = _itemPickUp.ItemData;
        _prompt = _item.DisplayName;
        this.gameObject.layer = 7;
    }
}
