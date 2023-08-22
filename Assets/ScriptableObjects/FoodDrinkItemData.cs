using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Inventory System/FoodDrink Item")]
public class FoodDrinkItemData : InventoryItemData
{
    [SerializeField] private float _hungerToReplenish, _thirstToReplenish;
    [SerializeField] private PlayerRessource playerRessource;
    [SerializeField] private Animator playerAnimator;

    public override void UseItem()
    {
        //change that so not only the player can drink/eat 
        playerRessource = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRessource>();
        playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        
        if (playerRessource is null) return;
        playerAnimator.SetTrigger("Consume");
        // playerRessource.ReplenshingHungerThirst(_hungerToReplenish, _thirstToReplenish);
        playerAnimator.SetBool("isIdle", true);

    }


}
