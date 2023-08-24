using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Inventory System/Armor Item")]
public class ArmorItemData : InventoryItemData
{
    public int armourAmount;
    public int healthUp;
    public int manaUp;
    public float speedUp;
}
