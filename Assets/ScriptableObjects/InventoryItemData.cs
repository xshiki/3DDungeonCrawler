using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  This is a scriptable object, what defines what an item is in our game
///  it could be inherited to have brachned ersions of items
/// </summary>

[CreateAssetMenu(menuName = "Create Items/Item")]
public class InventoryItemData : ScriptableObject
{

    [Header("Set UI Attributes")]
    public Sprite Icon;
    public int MaxStackSize;
    public int ID = -1;
    public string DisplayName;
    [TextArea(4, 4)]
    public string Description;


    [Header("Set Item Attributes")]
    public GameObject ItemPrefab;
    public InventoryItemType ItemType;
    public int dropChance;
    public bool consumable;


    //Make new Script that inherites from inventory item data example weapons
    public virtual void UseItem()
    {
        Debug.Log($"Using {DisplayName}");

    }

}
