using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Create Items/Armor Item")]
public class ArmorItemData : InventoryItemData
{
    public int stamina;
    public int strength;
    public int intelligence;
    public int armor;
    public int speed;

    public Material armorColor;

   
}
