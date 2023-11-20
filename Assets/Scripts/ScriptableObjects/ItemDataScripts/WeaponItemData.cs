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
    [Range(0f, 100f)]
    public float lifeStealChance = 0f;
    [Range(0f, 1f)]
    public float lifeStealPercentage = 0.05f;

    [Range(0f, 1f)]
    public float critRate = 0f;
    public float critDamageMultiplier = 1f;
    public enum Weapons { Sword, Axe, Polearm, Staff, Daggers };
   

    [Header("Audio")]
    public AudioClip weaponSwingSound;
    public AudioClip weaponHitSound;


    [Header("Effects")]
    public GameObject hitEffect;
    public override void UseItem()
    {
       
      

    }





}
