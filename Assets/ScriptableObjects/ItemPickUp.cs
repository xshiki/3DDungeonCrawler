using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UniqueID))]
public class ItemPickUp : MonoBehaviour
{
    //[SerializeField] private float PickUpRadius = 0.25f;
    [SerializeField] private float _rotationSpeed = 0f;
    public InventoryItemData ItemData;

    private BoxCollider myCollider;
    private BoxCollider myCollider2;
    [SerializeField] private ItemPickUpSaveData itemSaveData;
    private string id;

    private void Awake()
    {

        myCollider = gameObject.AddComponent<BoxCollider>();
        myCollider.isTrigger = true;

        myCollider2= gameObject.AddComponent<BoxCollider>();
    

    }
    private void Start()
    {
        id = GetComponent<UniqueID>().ID;
        /*
        if (!SaveGameManager.data.activeItems.ContainsKey(id))
        {
            SaveGameManager.data.activeItems.Add(id, itemSaveData);
        }
       */

    }
    private void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
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
        var inventory = other.GetComponentInChildren<InventoryManager>();
        if (!inventory) return;
        if (inventory.AddItem(ItemData))
        {
            //Debug.Log(ItemData.DisplayName +" added to inventory");
            NotificationManager.Instance.SetNewNotification("Picked up "+ItemData.DisplayName);
            Destroy(this.gameObject);
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
        rotation = _rotation;
    }

}
