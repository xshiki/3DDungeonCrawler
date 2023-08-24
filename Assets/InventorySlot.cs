using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public InventoryItemType itemType = InventoryItemType.None;

  
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount == 0)
        {
            InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
            InventoryItemType tag = inventoryItem.myType;
            if (tag == this.itemType || this.itemType == InventoryItemType.None)
            {
                inventoryItem.parentAfterDrag = transform; 
            }
            


         
        }
    }


}
