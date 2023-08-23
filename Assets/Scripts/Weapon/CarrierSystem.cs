using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CarrierSystem : MonoBehaviour
{


    [SerializeField]
    private Transform RigSocket;

    [SerializeField]
    
    private Animator RigAnimator;
    private InventoryItemData currentItemData;
    private GameObject currentGameObject;
    private IHandHeldObject currentHandheldInterface;
    private void Awake()
    {
        
    }

    public void SwitchHandHeld(InventoryItemData inventoryItemData)
    {
        if(inventoryItemData == currentItemData) { return; }
        Destroy(currentGameObject);

        currentItemData = inventoryItemData;
        currentGameObject = Instantiate(inventoryItemData.ItemPrefab, RigSocket, true); //currently in world space
        currentGameObject.transform.localPosition = Vector3.zero;
        currentGameObject.transform.localRotation = Quaternion.identity;

        currentHandheldInterface = currentGameObject.GetComponentInChildren<IHandHeldObject>();
        if (currentHandheldInterface != null)
        {
            currentHandheldInterface.AnAttachedCarrier(this);
            currentHandheldInterface.OnEquip();
            RigAnimator.runtimeAnimatorController = inventoryItemData.RigAnimatorController;
        }
        else
        {
            DestroyImmediate(currentGameObject);
            currentItemData = null;
            currentHandheldInterface= null;
            currentGameObject= null;
        }
    }
  
}
