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
    public enum Rarity {Common, Uncommon, Rare, Epic, Legendary };
    [Header("Set UI Attributes")]
    public Sprite Icon;
    public int MaxStackSize;
    public int ID = -1;
    public string DisplayName;
    [TextArea(4, 4)]
    public string Description;

    public Rarity rarity = Rarity.Common;
    [Header("Set Item Attributes")]
    public GameObject ItemPrefab;
    public InventoryItemType ItemType;

    [Range(0,100)]
    public int dropChance;
    public bool consumable = false;
    public bool isAbility =false;


    //Make new Script that inherites from inventory item data example weapons
    public virtual void UseItem()
    {
        Debug.Log($"Using {DisplayName}");

    }

}
