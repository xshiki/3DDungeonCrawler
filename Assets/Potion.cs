using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
   
    public InventoryItemData item;
    [SerializeField] private PlayerRessource playerRessource;
    public void UseItem()
    {
        playerRessource = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerRessource>();
        if(playerRessource != null ) { 
            //playerRessource.ReplenshHealthMana()
        }
    }
}
