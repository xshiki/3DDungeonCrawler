using System;
using System.Collections.Generic;
using UnityEngine;
using Graphs;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;
using UnityEditor;
using Random = UnityEngine.Random;
using Graphs;
using Unity.VisualScripting;

public class TinyGen: MonoBehaviour
{



    [SerializeField]
    public int borderSize = 20;
    public int min_roomSize = 2;
    public int max_roomSize = 4;
    public int roomNumber = 4;
    public int roomMargin = 1;
    public int roomRecursion = 50;
    public List<Vector3Int> roomPositions;
    public List<Room> rooms;
    public float hallwayChance = 0.25f; //Storing a probability of creating a connecting hallway

    public Grid grid;

    [Header("Tilemap")]
    public Tilemap tilemap;
    public TileBase tileBase;
    public TileBase[] placeableObjects;


    Delaunay2D delaunay;


    private void Awake()
    {
        tilemap.ClearAllEditorPreviewTiles();
        tilemap.ClearAllTiles();
        VisualizeBorder();
        GenerateDungeon();

    }


    void VisualizeBorder()
    {
      
        for(int i=-1; i<=borderSize+1; i++) {
            tilemap.SetTile(new Vector3Int(i, 1, 0), tileBase);
            tilemap.SetTile(new Vector3Int(i, borderSize, 0), tileBase);
            tilemap.SetTile(new Vector3Int(borderSize, i, 0), tileBase);
            tilemap.SetTile(new Vector3Int(-1, i, 0), tileBase);
        }

    }

    void CreateRoom(int roomRecursion)
    {
        if(!(roomRecursion > 0))
        {
            return;
        }

        /*
        int x = (int)(Random.Range(0, borderSize));
        int z = (int)(Random.Range(0, borderSize));
        Vector3Int startPos = new Vector3Int(x,z,0);
        if(tilemap.GetTile(startPos) == placeableObjects[0]) { CreateRoom(roomRecursion-1); return; } //if cell is occupied, create new room
        tilemap.SetTile(startPos, placeableObjects[0]);
        var room = tilemap.GetInstantiatedObject(startPos).GetComponent<Room>();
        room.SetRoomPosition(startPos);
        rooms.Add(room);
        roomPositions.Add(startPos);
        */


        /*
        int width = (int) (Random.Range(0,10) % (max_roomSize - min_roomSize)) + min_roomSize;
        int height = (int) (Random.Range(0,10) % (max_roomSize - min_roomSize)) + min_roomSize;

        int x = Random.Range(0, 10) % (borderSize - width + 1);
        int z = Random.Range(0, 10) % (borderSize - height + 1);

        Vector3Int startPos = new Vector3Int(x,z);
        for(int i=0; i<width; i++)
        {
            for(int j=0; j < height; j++)
            {
                Vector3Int position = startPos + new Vector3Int(j,0,i);
                tilemap.SetTile(position, placeableObjects[1]);
            } 
        }
        */
    }

    void Triangulate()
    {
        List<Graphs.Vertex> vertices = new List<Graphs.Vertex>();


        foreach (var room in rooms)
        {

            Vector2 roomsPos = new Vector2(room.GetRoomPosition().x, room.GetRoomPosition().z);
            vertices.Add(new Vertex<Room>(roomsPos + (roomsPos) / 2, room));

        }
        delaunay = Delaunay2D.Triangulate(vertices);
    }

    void GenerateDungeon()
    {
        for(int i = 0; i < roomNumber; i++) {  CreateRoom(roomRecursion); }
        //foreach(Vector3Int pos in roomPositions) { Debug.Log(pos); }
        //Triangulate();
    
    }
}



public class DungeonGeneratorEditor: Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}