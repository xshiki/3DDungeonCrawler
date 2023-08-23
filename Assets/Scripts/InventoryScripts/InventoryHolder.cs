using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public abstract class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] private int equipmentSize;
    [SerializeField] protected InventorySystem primaryInventorySystem;
    [SerializeField] protected InventorySystem equipmentInventorySystem;
    [SerializeField] protected int offset = 10;
    public int Offset => offset;
    public static UnityAction<InventorySystem, int> OnDynamicInventoryDisplayRequested; //Inv System to Display, amount to offset display by
    public InventorySystem PrimaryInventorySystem => primaryInventorySystem;
    public InventorySystem EquipmentInventorySystem => equipmentInventorySystem;

    protected virtual void Awake()
    {
        SaveLoad.OnLoadGame += LoadInventory;
        primaryInventorySystem = new InventorySystem(inventorySize);   
        equipmentInventorySystem = new InventorySystem(equipmentSize);
    }


    protected abstract void LoadInventory(SaveData data);

}






[System.Serializable]
public struct InventorySaveData
{
    public InventorySystem InvSystem;
    public Vector3 Position;
    public Quaternion Rotation;
    public InventorySaveData(InventorySystem _invSystem, Vector3 _position, Quaternion _rotation)
    {
        InvSystem = _invSystem;
        Position = _position;
        Rotation = _rotation;
    }
    public InventorySaveData(InventorySystem _invSystem)
    {
        InvSystem = _invSystem;
        Position = Vector3.zero;
        Rotation = Quaternion.identity;
    }
}