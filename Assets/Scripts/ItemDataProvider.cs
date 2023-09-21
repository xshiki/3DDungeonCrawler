using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(ItemPickUp))]
[RequireComponent(typeof(ItemPickUpInteractable))]
public class ItemDataProvider : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public InventoryItemData item;
    public InventoryItemData Item => item;
}
