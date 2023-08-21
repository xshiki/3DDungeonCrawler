using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    [SerializeField ]private float PickUpRadius = 0.25f;
    [SerializeField] private float _rotationSpeed  = 10f;
    public InventoryItemData ItemData;

    private SphereCollider myCollider;
    [SerializeField] private ItemPickUpSaveData itemSaveData;
    private string id;

    private void Awake()
    {
       
        SaveLoad.OnLoadGame += LoadGame;
        itemSaveData = new ItemPickUpSaveData(ItemData, transform.position, transform.rotation);    
        myCollider = GetComponent<SphereCollider>();   
        myCollider.radius = PickUpRadius;   
        myCollider.isTrigger= true;
    
       
   }
    private void Start()
    {
        id = GetComponent<UniqueID>().ID;
        if (!SaveGameManager.data.activeItems.ContainsKey(id))
        {
            SaveGameManager.data.activeItems.Add(id, itemSaveData);
        }
       

    }
    private void Update()
    {
        //transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }
    private void LoadGame(SaveData data)
    {
        //if (data.collectedItems.Contains(id))
        //{
        //    Destroy(this.gameObject);
        //}

    }
    private void OnDestroy()
    {
        //if (SaveGameManager.data.activeItems.ContainsKey(id))
        //{
        //    SaveGameManager.data.activeItems.Remove(id);    
        //    SaveLoad.OnLoadGame -= LoadGame;    
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        var inventory= other.GetComponent<PlayerInventoryHolder>();
        if(!inventory) return;
        if(inventory.AddToInventory(ItemData, 1))
        {
            SaveGameManager.data.collectedItems.Add(id);
            Destroy(this.gameObject); //destroy item in overworld

        }
    }
}

[System.Serializable]
public struct ItemPickUpSaveData
{
    public InventoryItemData itemData;
    public Vector3 position;
    public Quaternion rotation;

    public ItemPickUpSaveData(InventoryItemData _data, Vector3 _position, Quaternion _rotation)
    {
        itemData = _data;
        position = _position;  
        rotation= _rotation;
    }

}
