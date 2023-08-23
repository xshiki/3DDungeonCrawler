using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Inventory System/Consumable Item")]
public class ConsumableItemData : InventoryItemData
{
    [SerializeField] private float _healthToReplenish, _manaToReplenish;
    //[SerializeField] private Animator playerAnimator;
 


    public override void UseItem()
    {
        //change that so not only the player can drink/eat 
        PlayerRessource playerRessource = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRessource>();
        Debug.Log(playerRessource);
        //playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        if (playerRessource is null) return;
        //playerAnimator.SetTrigger("Consume");
  
            playerRessource.ReplenshHealthMana(_healthToReplenish, _manaToReplenish);
            Debug.Log("used consum");
        
        //playerAnimator.SetBool("isIdle", true);

    }


}
