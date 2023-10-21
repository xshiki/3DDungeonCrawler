using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Create Items/Weapons")]
public class WeaponItemData : InventoryItemData
{



    [Header("Weapon Attributes (DAMAGE IS MELEE ONLY)")]
    public Weapons WeaponType;
    public int DamageAmount;
    public float WeaponRange = 1f;
    public float timeBetweenSwing = 2.5f;
    public enum Weapons { Sword, Axe, Polearm, Staff };
   

    [Header("Audio")]
    public AudioClip weaponSwingSound;
    public AudioClip weaponHitSound;


    [Header("Effects")]
    public GameObject hitEffect;
    public override void UseItem()
    {
       
      

    }





}
