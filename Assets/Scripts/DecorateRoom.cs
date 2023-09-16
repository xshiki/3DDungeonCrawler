using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DecorateRoom : MonoBehaviour
{


    [SerializeField] public GameObject[] decorationPrefabs;
    [SerializeField] public int numberOfDecorations = 5;
    ProceduralDungeonGenerator dungeonGenerator;

    private bool isCompleted;
 
    private GridSystem grid;


    void Start()
    {
        dungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<ProceduralDungeonGenerator>();
        dungeonGenerator.OnFinishBuilding += SpawnRandomObjects;
    }

   

    void SpawnRandomObjects()
    {

        if (decorationPrefabs.Length <= 0) { return; }


        grid = GetComponent<GridSystem>();
        var renderer = GetComponent<Renderer>();
        int spawnedDecorations = 0;
        if (renderer != null )
        {
            var bounds = renderer.bounds;
            

            while (spawnedDecorations <= numberOfDecorations)
            {
                Vector3 startPosition = transform.position;
                Vector3 possiblePosition = new Vector3(
                    Random.Range(startPosition.x - renderer.bounds.extents.x, startPosition.x + renderer.bounds.extents.x), 
                    startPosition.y+2, 
                    Random.Range(startPosition.z - renderer.bounds.extents.z, startPosition.x + renderer.bounds.extents.z));
               
                Vector3 possibleGridCellPos = grid.GetNearestPointOnGrid(possiblePosition);
                GameObject go = Instantiate(decorationPrefabs[0], possibleGridCellPos, Quaternion.identity);
                go.transform.SetParent(transform);
                Vector3 origin = transform.position;
                Vector3 direction = transform.TransformDirection(Vector3.down);
           
                if (!Physics.Raycast(origin, direction, out RaycastHit hit, 1f))
                {

                    go.transform.position = possibleGridCellPos;
                    go.transform.rotation = transform.rotation;
                    spawnedDecorations++;


                }


               
            }
        }
        
      
    }
}
