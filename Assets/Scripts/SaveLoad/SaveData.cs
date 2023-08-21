using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SaveData
{
    public SerializableDictionary<string, InventorySaveData> chestDictionary;
    public SerializableDictionary<string, InventorySaveData> enemyDictionary;
    
    public SerializableDictionary<string, ItemPickUpSaveData> activeItems;
    public List<string> collectedItems;
    public InventorySaveData playerInventory;
    
    public SaveData() { 
        chestDictionary = new SerializableDictionary<string, InventorySaveData>(); 
        enemyDictionary = new SerializableDictionary<string, InventorySaveData>(); 
        activeItems = new SerializableDictionary<string, ItemPickUpSaveData>(); 
        playerInventory= new InventorySaveData();   

        collectedItems = new List<string>();    
    }   
}

