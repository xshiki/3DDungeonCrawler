using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Create Items/Consumable Item")]
public class ConsumableItemData : InventoryItemData
{
    public float _healthToReplenish, _manaToReplenish;
    public AudioClip consumSoundClip;
    //[SerializeField] private Animator playerAnimator;
 
    public override void UseItem()
    {
        base.UseItem();
        //change that so not only the player can drink/eat 
        PlayerRessource playerRessource = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRessource>();
        //playerAnimator = GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();

        if (playerRessource is null) return;
        //playerAnimator.SetTrigger("Consume");
        playerRessource.ReplenshHealthMana(_healthToReplenish, _manaToReplenish);
        
        //playerAnimator.SetBool("isIdle", true);

    }


}
