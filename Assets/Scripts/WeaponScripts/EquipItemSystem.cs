using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR.Haptics;

public class EquipItemSystem : MonoBehaviour
{
    // Start is called before the first frame update
    
    [SerializeField] GameObject weaponHolder;

    public bool ItemInHand => currentItemInHand != null;
    public UnityAction equippedItem;


    public GameObject currentItemInHand;
    public WeaponItemData currentItemInHandSO;
  
    public void EquipItem()
    {
      
          
        
    }

    public void UnEquipItem()
    {
        // Check if currentItemInHand is not null
        if (currentItemInHand != null)
        {
            
         
            
           
        }
       
       
    }

}

 

 
