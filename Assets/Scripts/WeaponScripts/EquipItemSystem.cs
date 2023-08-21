using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.XR.Haptics;

public class EquipItemSystem : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private HotbarDisplay currentItem;
    [SerializeField] GameObject weaponHolder;

    public bool ItemInHand => currentItemInHand != null;
    public UnityAction equippedItem;


    public GameObject currentItemInHand;
    public WeaponItemData currentItemInHandSO;
  
    public void EquipItem()
    {
      
            // Check if currentItem.HighlightedItemData.ItemPrefab is not null
            if (currentItem.HighlightedItemData.ItemPrefab != null)
            {
                // Instantiate the prefab as a child of weaponHolder 
                
                currentItemInHand = Instantiate(currentItem.HighlightedItemData.ItemPrefab, weaponHolder.transform);
               
                if (currentItem.HighlightedItemData is WeaponItemData)
                {   
                    currentItemInHandSO =  currentItem.HighlightedItemData as WeaponItemData;
                   
                    equippedItem?.Invoke();
                }
                            
            }
        
    }

    public void UnEquipItem()
    {
        // Check if currentItemInHand is not null
        if (currentItemInHand != null)
        {
            
            Destroy(currentItemInHand);
            if (currentItem.HighlightedItemData is WeaponItemData)
            {
              
                equippedItem?.Invoke();
             
            }
            
           
        }
       
       
    }

}

 

 
