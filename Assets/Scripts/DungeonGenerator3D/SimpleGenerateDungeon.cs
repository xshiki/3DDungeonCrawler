using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SimpleDungeonGenerate : MonoBehaviour
{
    public int dungeonWidth = 10; // Number of rooms in a row
    public int dungeonHeight = 10; // Number of rooms in a column
    public GameObject roomPrefab;
    public GameObject hallwayPrefab;

    private List<Vector2Int> generatedRooms = new List<Vector2Int>();

    private void Start()
    {
        GenerateDungeon();
    }

    private void GenerateDungeon()
    {
        for (int x = 0; x < dungeonWidth; x++)
        {
            for (int y = 0; y < dungeonHeight; y++)
            {
                Vector2Int roomPosition = new Vector2Int(x, y);
                GenerateRoom(roomPosition);

                if (x > 0)
                {
                    Vector2Int leftNeighbor = new Vector2Int(x - 1, y);
                    GenerateHallway(roomPosition, leftNeighbor);
                }

                if (y > 0)
                {
                    Vector2Int topNeighbor = new Vector2Int(x, y - 1);
                    GenerateHallway(roomPosition, topNeighbor);
                }
            }
        }
    }

    private void GenerateRoom(Vector2Int position)
    {
        Vector3 spawnPosition = new Vector3(position.x * 10, 0, position.y * 10); // Adjust the position multiplier as needed
        Instantiate(roomPrefab, spawnPosition, Quaternion.identity);
        generatedRooms.Add(position);
    }

    private void GenerateHallway(Vector2Int start, Vector2Int end)
    {
        if (!generatedRooms.Contains(start) || !generatedRooms.Contains(end))
            return;

        Vector3 startPosition = new Vector3(start.x * 10, 0, start.y * 10); // Adjust the position multiplier as needed
        Vector3 endPosition = new Vector3(end.x * 10, 0, end.y * 10); // Adjust the position multiplier as needed
        Vector3 midpoint = (startPosition + endPosition) * 0.5f;

        GameObject hallway = Instantiate(hallwayPrefab, midpoint, Quaternion.identity);
        hallway.transform.LookAt(startPosition);
        hallway.transform.localScale = new Vector3(Vector3.Distance(startPosition, endPosition), 1, 1); // Adjust the scale as needed
    }
}