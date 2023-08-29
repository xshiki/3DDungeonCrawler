using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Weapons")]
public class WeaponItemData : InventoryItemData
{


    public enum Weapons { Sword, Axe, Polearm, Zweihander, Staff };
    public Weapons WeaponType;
    public int DamageAmount;
    public float WeaponRange = 1f;

    public override void UseItem()
    {
       
      

    }





}
