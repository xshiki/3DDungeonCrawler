using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField] public List<InventoryItemData> lootList = new List<InventoryItemData>();
    // Start is called before the first frame update
    void Start()
    {
        //lootList.add
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
        Debug.Log("no loot dropped");
        return null;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {

        InventoryItemData droppedItem = getDroppedItem();
        if(droppedItem != null) {
            GameObject lootGameObject = Instantiate(droppedItem.ItemPrefab.gameObject, spawnPosition, Quaternion.identity);
            //lootGameObject.GetComponent<
        }

    }
}
