using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEnemySpawner : MonoBehaviour
{

    ProceduralDungeonGenerator dungeonGenerator;
    public List<GameObject> enemyPrefabs = new();

    public LoadDatabase database;
    public ScalingScriptableObject scaler;
    public int floorCounter;
    public GameObject bossRoom;

    public bool isSpawned = false;
    // Start is called before the first frame update
    private void Awake()
    {
        dungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<ProceduralDungeonGenerator>();
      
        database = GameObject.Find("Database").GetComponent<LoadDatabase>();
        scaler = database.scaler;
        floorCounter = GameObject.Find("Floor Counter").GetComponent<FloorTextOverlay>().floorCounter;
        isSpawned = false;
        dungeonGenerator.OnFinishBuilding += SpawnBoss;

    }


    //spawn boss every 5th floor
    void SpawnBoss()
    {   
        if(floorCounter % 5 != 0) { return; }
        bossRoom = GameObject.Find("Boss Room");
        int spawnIndex = UnityEngine.Random.Range(0, enemyPrefabs.Count);
        GameObject boss = Instantiate(enemyPrefabs[spawnIndex], bossRoom.transform);
        isSpawned = true;
        boss.GetComponent<EnemyAI>().SetBossSpawnPoint(bossRoom.transform.position);
        boss.GetComponent<EnemyManager>().EnemyScaler(scaler, floorCounter*floorCounter + 3);
        Vector3 scaleChange = new Vector3(2, 2, 2);
        boss.transform.localScale = Vector3.Scale(boss.transform.localScale, scaleChange);
        boss.transform.rotation = bossRoom.transform.rotation;

    }
}
