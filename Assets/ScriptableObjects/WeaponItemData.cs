using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Inventory System/Weapon Item")]
public class WeaponItemData : InventoryItemData
{


    public enum Weapons { Sword, Axe, Polearm, Zweihander, Staff };
    public Weapons WeaponType;
    public int WeaponDamage;
    public UnityAction OnWeaponUsed;
    
    public override void UseItem()
    {
       
        //base.UseItem();
        // Get the player's Transform component
        //Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        //playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        //Debug.Log(playerAnimator);
        //Debug.Log("Attacking with");
        //playerAnimator.SetBool("isAttacking", true);
        //playerAnimator.SetBool("isIdle", true);

        //playerAnimator.SetBool("isAttacking", false);
        OnWeaponUsed?.Invoke();

    }





}
