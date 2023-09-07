using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecorateRoom : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject[] prefabs;
    ProceduralDungeonGenerator dungeonGenerator;
    bool isCompleted;
    void Start()
    {
        dungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<ProceduralDungeonGenerator>();
        SpawnRandomObjects();
    }

    // Update is called once per frame
    void Update()
    {
       
    }



    void SpawnRandomObjects()
    {
        if(!isCompleted && dungeonGenerator.dungeonState == DungeonState.completed)
            isCompleted = true;
        {
            int index = Random.Range(0, prefabs.Length);
            GameObject decor = Instantiate(prefabs[index], transform.position, transform.rotation, transform) as GameObject;
            decor.name = "Decor";
           
        }
    }
}
