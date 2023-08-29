using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

using UnityEngine.UI;



public class InventoryItem : MonoBehaviour,IBeginDragHandler , IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public TextMeshProUGUI countText;
    public InventoryItemData item;
    public InventoryItemType myType;
    public int count = 1;
    public int maxStacks = 1;
    [HideInInspector] public Transform parentAfterDrag;
    private Transform playerTransform;
    private Transform playerSocket;
    public float dropOffset = 2f;
  
    public void InitialiseItem(InventoryItemData newItem)
    {   
        item = newItem;
        myType = newItem.ItemType;
        maxStacks = item.MaxStackSize;
        image.sprite = newItem.Icon;
        playerTransform = GameObject.Find("Orientation").GetComponent<Transform>();
 
        if (playerTransform == null) Debug.Log("Player not found!");
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
        //Drop item out of inventory
        GameObject dropTarget = eventData.pointerEnter;
        if (dropTarget.CompareTag("OutSideInventory"))
        {
            Debug.Log("destroying dragged item");
            GameObject prefabInstance = Instantiate(this.item.ItemPrefab, playerTransform.position + playerTransform.forward * dropOffset, Quaternion.identity);
            prefabInstance.AddComponent<Rigidbody>();
            Destroy(gameObject); // Destroy the dragged item if dropped outside
        }
        else
        {
            // Item was dropped inside the inventory, reset position
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
        }
    }


   
}
