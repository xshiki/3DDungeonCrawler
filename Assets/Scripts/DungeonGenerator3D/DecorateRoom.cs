using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class DecorateRoom : MonoBehaviour
{


    [SerializeField] public GameObject[] chestPrefabs;
    [SerializeField] public GameObject[] roomLayoutPrefabs;
    [SerializeField] public int numberOfDecorations = 5;
    [Range(1f, 101f)]
    public float chestSpawnChance = 15f;
    ProceduralDungeonGenerator dungeonGenerator;
    public float chestToWallDistance = 2.0f;
    public bool isBossRoom = false;
    private bool isCompleted;
 
    private GridSystem grid;

    private void Awake()
    {
        dungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<ProceduralDungeonGenerator>();

        dungeonGenerator.OnFinishBuilding += SpawnRandomObjects;
        dungeonGenerator.OnFinishBuilding += SpawnRandomDecoration;
        if (isBossRoom)
        {
            dungeonGenerator.OnFinishBuilding -= SpawnRandomObjects;
            dungeonGenerator.OnFinishBuilding -= SpawnRandomDecoration;
        }
    }

    private void OnDestroy()
    {
        dungeonGenerator.OnFinishBuilding -= SpawnRandomObjects;
        dungeonGenerator.OnFinishBuilding -= SpawnRandomDecoration;
    }


    void SpawnRandomObjects()
    {
        if (chestPrefabs == null) { return; }
        if (chestPrefabs.Length <= 0) { return; }
        int spawnedDecorations = 0;
        if (chestSpawnChance > 0)
        {
            while (spawnedDecorations < numberOfDecorations)
            {
                int roll = Random.Range(1, 101);

                if (roll <= chestSpawnChance)
                {

                    // Instantiate the chest at the random point
                    int chestIndex = Random.Range(0, chestPrefabs.Length);
                    GameObject door = Instantiate(chestPrefabs[chestIndex], transform.position, transform.rotation, transform);
                    door.name = chestPrefabs[chestIndex].name;
                    spawnedDecorations++;

                }


            }


        }



    








    /*

    grid = GetComponent<GridSystem>();
    var renderer = GetComponent<Renderer>();
    int spawnedDecorations = 0;

    if (renderer != null )
    {
        var bounds = renderer.bounds;


        while (spawnedDecorations < numberOfDecorations)
        {
            Vector3 startPosition = transform.position;
            Vector3 possiblePosition = new Vector3(
                Random.Range(startPosition.x - renderer.bounds.extents.x, startPosition.x + renderer.bounds.extents.x), 
                transform.localPosition.y, 
                Random.Range(startPosition.z - renderer.bounds.extents.z, startPosition.z + renderer.bounds.extents.z));

            Vector3 possibleGridCellPos = grid.GetNearestPointOnGrid(possiblePosition);
            GameObject go = Instantiate(decorationPrefabs[0], possibleGridCellPos, Quaternion.identity);
            go.transform.SetParent(transform);
            Vector3 origin = go.transform.position;
            Vector3 direction = go.transform.TransformDirection(Vector3.down);

            if (!Physics.Raycast(origin, direction, out RaycastHit hit, 1f,11))
            {

                go.transform.position = possibleGridCellPos + new Vector3(0, 0.4f, 0);
                float yRotation = Random.Range(0, 4) * 90f;
                go.transform.Rotate(0, yRotation, 0);

                spawnedDecorations++;


            }
            else
            {
                Destroy(go);
            }



        }


    }

    */


}


    void SpawnRandomDecoration()
    {
        if (roomLayoutPrefabs == null) { return; }
        if (roomLayoutPrefabs.Length <= 0) { return; }

        int layoutIndex = Random.Range(0, roomLayoutPrefabs.Length);

        GameObject goLayout = Instantiate(roomLayoutPrefabs[layoutIndex],transform.position, transform.rotation, transform) as GameObject;
        goLayout.name = roomLayoutPrefabs[layoutIndex].name;
    }
}
