using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]   
public class ChestInventory : InventoryHolder, IInteractable
{

    [SerializeField] private string _prompt;
    public UnityAction<IInteractable> OnInteractionComplete { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        //SaveLoad.OnLoadGame += LoadInventory; //maybe useless?
    }
    private void Start()
    {
        var chestSaveData = new InventorySaveData(primaryInventorySystem, transform.position, transform.rotation);
       // SaveGameManager.data.chestDictionary.Add(GetComponent<UniqueID>().ID, chestSaveData);
    }
    protected override void LoadInventory(SaveData data)
    {







        //TODO Rewrite this code so it contains random items from a given rule


        Debug.Log("attempt to load inventory chest");
        //check the save for specific chests inventory, if it exists, load it in.
        if (data.chestDictionary.TryGetValue(GetComponent<UniqueID>().ID, out InventorySaveData chestData)){

            this.primaryInventorySystem = chestData.InvSystem;
            this.transform.position = chestData.Position;
            this.transform.rotation = chestData.Rotation;   
        }
    }

    public string InterActionPrompt => _prompt;

    UnityAction<IInteractable> IInteractable.OnInteractionComplete { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    public void EndInteraction()
    {
        Debug.Log("closing chest!");
    }

    public void Interact(InteractScript interactor, out bool interactSucessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(primaryInventorySystem, 0);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        interactSucessful = true;
    }

    public bool Interact(InteractScript interactor)
    {
        throw new System.NotImplementedException();
    }
}

