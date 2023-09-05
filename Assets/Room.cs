using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(BoxCollider))]
public class Room : MonoBehaviour
{
    // Start is called before the first frame update
    private List<Room> connectedRooms = new List<Room>();
    private bool isPlayerInside = false;
    public Vector3Int roomPosition;
    private RectInt bounds;


   

    private void Awake()
    {
      
    }

    public void SetRoomPosition(Vector3Int position)
    {
        roomPosition = position;
    }

    public Vector3Int GetRoomPosition() { return roomPosition; }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
