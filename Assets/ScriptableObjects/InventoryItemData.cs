using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This is a scriptable object, what defines what an item is in our game
///  it could be inherited to have brachned ersions of items
/// </summary>

[CreateAssetMenu(menuName = "Inventory System/Inventory Item")]
public class InventoryItemData : ScriptableObject
{
    public Sprite Icon;
    public int MaxStackSize;
    public int ID = -1;
    public string DisplayName;
    [TextArea(4, 4)]
    public string Description;
    public string ItemType;
    public int GoldValue;
    public GameObject ItemPrefab;
    public bool Throwable;
    public bool Consumable;

    //Make new Script that inherites from inventory item data example weapons
    public virtual void UseItem()
    {
        Debug.Log($"Using {DisplayName}");

    }

}
