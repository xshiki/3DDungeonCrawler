using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DecorateRoom : MonoBehaviour
{


    [SerializeField] public GameObject[] decorationPrefabs;
    [SerializeField] public int numberOfDecorations = 5;
    ProceduralDungeonGenerator dungeonGenerator;

    public bool isBossRoom = false;
    private bool isCompleted;
 
    private GridSystem grid;

    private void Awake()
    {
        dungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<ProceduralDungeonGenerator>();

        dungeonGenerator.OnFinishBuilding += SpawnRandomObjects;
        if(isBossRoom)
        {
            dungeonGenerator.OnFinishBuilding -= SpawnRandomObjects;
        }
    }

    private void OnDestroy()
    {
        dungeonGenerator.OnFinishBuilding -= SpawnRandomObjects;
    }


    void SpawnRandomObjects()
    {
        if(decorationPrefabs == null) { return; }
        if (decorationPrefabs.Length <= 0) { return; }


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
        
      
    }
}
