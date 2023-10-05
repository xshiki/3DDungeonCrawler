using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField] public List<InventoryItemData> lootList = new List<InventoryItemData>();
    [SerializeField] public GameObject databaseGO;

    // Start is called before the first frame update

    private void Awake()
    {
        //If no Loot List is specified, generate a loot table based on the available items in the database
        if (lootList.Count == 0)
        {
            databaseGO = GameObject.Find("Database");
            List<InventoryItemData> allItemList = databaseGO.GetComponent<LoadDatabase>().database.GetInventoryItemData();
            int lootListSize = Random.Range(0, allItemList.Count);
            if (allItemList != null)
            {

                for (int i = 0; i < lootListSize; i++)
                {
                    int rndNumber = Random.Range(0, allItemList.Count);
                    lootList.Add(allItemList[rndNumber]);


                }




            }
        }

    }

    public InventoryItemData getDroppedItem()
    {
        int rndNumber = Random.Range(1, 101);
        List<InventoryItemData> possibleItems = new List<InventoryItemData>();  

        foreach(InventoryItemData item in lootList)
        {
                if(rndNumber <= item.dropChance)
            {
                possibleItems.Add(item);
            }
        }
        if(possibleItems.Count > 0)
        {
            InventoryItemData droppedItem = possibleItems[Random.Range(0, possibleItems.Count)]; //picks randomly an item from the loottable 
            return droppedItem;
        }
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {

        InventoryItemData droppedItem = getDroppedItem();
        if(droppedItem != null) {
            GameObject lootGameObject = Instantiate(droppedItem.ItemPrefab.gameObject, spawnPosition, Quaternion.identity);
        }

    }
}
