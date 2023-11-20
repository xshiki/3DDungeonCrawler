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
    private Camera mainCamera;
    public float dropOffset = 3f;

    public bool isSplit = false;
    InventoryItem newItemData;

    private void Awake()
    {
        playerTransform = GameObject.Find("Orientation").GetComponent<Transform>();
        mainCamera = Camera.main;
    }

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
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if(count > 1)
            {
               
                // Calculate the split amount (half of the current count)
                int splitAmount = Mathf.CeilToInt(count / 2f);

                if (splitAmount >= 1)
                {
                    // Create a new item with the split amount
                    newItemData = Instantiate(this, transform.parent);
                    newItemData.count = splitAmount;
                    newItemData.Refresh();

                    // Reduce the count of the original item
                    count -= splitAmount;
                    Refresh();
                    isSplit = true;

                }
            }
         
        }
     

        
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        Transform playerUI = GameObject.Find("PlayerUI").transform;
        Transform throwAway = GameObject.Find("ThrowAway").transform;
        throwAway.GetChild(0).gameObject.SetActive(true);
        transform.SetParent(playerUI);
      
      
    }

    public void OnDrag(PointerEventData eventData)
    {
       transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Drop item out of inventory
        GameObject dropTarget = eventData.pointerEnter;
      
        if (dropTarget != null && dropTarget.CompareTag("OutSideInventory"))
        {
            Debug.Log("destroying dragged item");
            for(int i = 0; i < count; i++)
            {
                Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hitInfo, 5f))
                {
     
                    GameObject prefabInstance = Instantiate(item.ItemPrefab, hitInfo.point, Quaternion.identity);
                    prefabInstance.layer = 7;
                    prefabInstance.AddComponent<Rigidbody>();
                    Debug.Log("dropped with raycast");
                }
                else
                {
                    GameObject prefabInstance = Instantiate(item.ItemPrefab, playerTransform.position + playerTransform.forward * dropOffset, Quaternion.identity);
                    prefabInstance.layer = 7;
                    prefabInstance.AddComponent<Rigidbody>();
                }
              
            }
            
            Destroy(gameObject); // Destroy the dragged item if dropped outside
        }
        else
        {
            if (isSplit)
            {
                // The split stack wasn't dropped in a different slot, so combine it with the original stack.
                if (parentAfterDrag != null)
                {
                    InventoryItem originalItem = parentAfterDrag.GetComponentInChildren<InventoryItem>();
                    if (originalItem != null && originalItem.item == item)
                    {
                       
                        originalItem.count += count;
                        originalItem.Refresh();
                        isSplit = false;
                        Destroy(gameObject); // Destroy the dragged split stack
                    }
                }
            }
            image.raycastTarget = true;
            transform.SetParent(parentAfterDrag);
            isSplit = false;
        }

        Transform throwAway = GameObject.Find("ThrowAway").transform;
        throwAway.GetChild(0).gameObject.SetActive(false);
    }


   
}
