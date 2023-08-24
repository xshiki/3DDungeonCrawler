using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;



public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;
    public InventoryItemData item;
    public InventoryItemType myType;
    public int count = 1;
    public int maxStacks = 1;
    [HideInInspector] public Transform parentAfterDrag;
    public void InitialiseItem(InventoryItemData newItem)
    {   
        item = newItem;
        myType = newItem.ItemType;
        maxStacks = item.MaxStackSize;
        image.sprite = newItem.Icon;
        Refresh();
    }

    public void Refresh()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
    }

    public void OnDrag(PointerEventData eventData)
    {
       transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);
    }

  
}
