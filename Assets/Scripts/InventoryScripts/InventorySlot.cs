using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public InventoryItemType itemType = InventoryItemType.None;
    public PlayerEquipment playerEquipment;
    public event EventHandler<OnItemDroppedEventArgs> OnItemDropped;


    public Image image;
    public Color selectedHighlightColor, notSelectedColor;



    private void Awake()
    {
       
    }



    public class OnItemDroppedEventArgs : EventArgs
    {
        public InventoryItem item;
    }




    public void Select() 
    {
        image.color = selectedHighlightColor;
    
    }
    public void Deselect()
    { 
        image.color = notSelectedColor;
    }


    public void SlotChanged(InventoryItem item) {

        OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = item });
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
            InventoryItem itemInSlotData = transform.GetChild(0).GetComponent<InventoryItem>();
            Debug.Log(itemInSlotData.item.DisplayName);

            

            if (tag == this.itemType || this.itemType == InventoryItemType.None)
            {

               
                if (draggedItem.item == itemInSlotData.item)
                {
                    int totalItemCount = itemInSlotData.count + draggedItem.count;

                    if(totalItemCount <= itemInSlotData.maxStacks)
                    {
                        itemInSlotData.count = totalItemCount;
                        itemInSlotData.Refresh();
                        Destroy(eventData.pointerDrag);
                        Transform throwAway = GameObject.Find("ThrowAway").transform;
                        throwAway.GetChild(0).gameObject.SetActive(false);
                        OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = draggedItem });
                    }
                }
                else
                {
                    Transform originalParent = draggedItem.parentAfterDrag;
                    // Swap  
                    Transform itemInSlot = transform.GetChild(0);
                    draggedItem.parentAfterDrag = transform;
                    itemInSlot.SetParent(originalParent);
                    dropped.transform.SetParent(transform);
                    itemInSlot.SetAsLastSibling();
                    OnItemDropped?.Invoke(this, new OnItemDroppedEventArgs { item = draggedItem });
                }

          
            }



        }


        }
    }
