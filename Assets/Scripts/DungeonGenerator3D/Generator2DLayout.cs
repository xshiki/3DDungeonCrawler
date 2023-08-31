using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

public class Generator2DLayout : MonoBehaviour
{

    public Grid grid;
   


    [Header("Dungeon Generation Parameters")]
    [SerializeField] Vector2Int size; //Size of the dungeon
    [SerializeField] int roomCount; //Number of rooms
    [SerializeField] public List<RoomBehaviour> rooms;
    [SerializeField] public GameObject room;

    public enum CellType
    {
        None,
        Room,
        Hallway
    }
    // Start is called before the first frame update
    void Start()
    {
        Generate();
    }

   

    void Generate()
    {
        Vector2Int location = new Vector2Int(
               UnityEngine.Random.Range(0, 100),
               UnityEngine.Random.Range(0, 100)
           );
        PlaceRooms(location);
    }


    void PlaceRooms(Vector2Int location)
    {

       
        var newRoom = Instantiate(room, new Vector3(location.x, 0, location.y), Quaternion.identity, transform);
    }
}
