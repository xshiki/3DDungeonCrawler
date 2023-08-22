using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class PlayerInventoryHolder : InventoryHolder
{
    // Start is called before the first frame update
    //[SerializeField] protected int secondaryInventorySize;
    //[SerializeField] protected InventorySystem secondaryInventorySystem;

    //public InventorySystem SecondaryInventorySystem => secondaryInventorySystem;

    //public static UnityAction<InventorySystem> OnPlayerBackpackDisplayRequested;
    private void Start()
    {
        //SaveGameManager.data.playerInventory = new InventorySaveData(primaryInventorySystem);
    }

    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySystem, int> OnPlayerInventoryDisplayRequested; 
    //Inv System to Display, amount to offset display by
                                                                                        
    //protected override void Awake()
                                                                                        
    //{
                                                                                        
    // base.Awake();
                                                                                        
    //secondaryInventorySystem = new InventorySystem(secondaryInventorySize);
                                                                                        
    // }
    protected override void LoadInventory(SaveData data)
    {
        Debug.Log("attempt to load inventory");
        //check the save for specific chests inventory, if it exists, load it in.
        if (data.playerInventory.InvSystem != null)
        {
            Debug.Log(data.playerInventory.InvSystem);
            this.primaryInventorySystem = data.playerInventory.InvSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(Keyboard.current.bKey.wasPressedThisFrame) { OnPlayerInventoryDisplayRequested?.Invoke(primaryInventorySystem, offset);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if(primaryInventorySystem.AddToInventory(data, amount)) //Add item to hotbar, else add to secondary(bag)
        {
            return true;
        }//else if(secondaryInventorySystem.AddToInventory(data, amount))
        //{
           // return true;
        //}
        return false;   

    }
}
