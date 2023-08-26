using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    [SerializeField] public InventoryItemType itemType = InventoryItemType.None;

    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;
    public class OnItemDroppedEventArgs : EventArgs
    {
        public InventoryItem item;
    }
    public void OnDrop(PointerEventData eventData)
    {

        InventoryItem draggedItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        InventoryItemType tag = draggedItem.myType;
        if (transform.childCount == 0)
        {
            
            if (tag == this.itemType || this.itemType == InventoryItemType.None)
            {
                draggedItem.parentAfterDrag = transform;
                OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = draggedItem });
            }


        }
        else
        {
            Debug.Log("Swap items)");

            GameObject dropped = eventData.pointerDrag;
         
            if (tag == this.itemType || this.itemType == InventoryItemType.None)
            {
                Transform originalParent = draggedItem.parentAfterDrag;

                // Swap
                Transform itemInSlot = transform.GetChild(0);
                draggedItem.parentAfterDrag = transform;
                itemInSlot.SetParent(originalParent);
                dropped.transform.SetParent(transform);
                itemInSlot.SetAsLastSibling();

            }



        }

    }
}
