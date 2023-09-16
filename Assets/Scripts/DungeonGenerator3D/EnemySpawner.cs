using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;



public class EnemySpawner : MonoBehaviour
{

    public int numberOfEnemiesToSpawn = 10;
    public float spawnDelay = 1f;
    ProceduralDungeonGenerator dungeonGenerator;
    public List<GameObject> enemyPrefabs = new();


    public ObjectPool<GameObject> _enemyPool;
    public enum SpawnMethod {RoundRobin, Random};
    public SpawnMethod enemySpawnMethod = SpawnMethod.RoundRobin;

    public LayerMask layerMask = 11;
 

    private void Awake()
    {
        dungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<ProceduralDungeonGenerator>();
        _enemyPool = new ObjectPool<GameObject>(CreateEnemy, null, OnPutBackInPool, defaultCapacity: 100);


        dungeonGenerator.OnFinishBuilding += StartCoroutine;




    }

    private void OnPutBackInPool(GameObject obj)
    {
        obj.gameObject.SetActive(false);
    }

    private GameObject CreateEnemy()
    {
        int spawnIndex = 0;

        if(enemySpawnMethod == SpawnMethod.RoundRobin)
        {
            
        }
        else if(enemySpawnMethod == SpawnMethod.Random)
        {
            spawnIndex = UnityEngine.Random.Range(0, enemyPrefabs.Count);
        }
        var enemy = Instantiate(enemyPrefabs[spawnIndex],transform);
        return enemy;
    }

    private void StartCoroutine()
    {
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {

        WaitForSeconds wait = new WaitForSeconds(spawnDelay);
        int SpawnedEnemies = 0;
        while(SpawnedEnemies < numberOfEnemiesToSpawn) 
        {
            DoSpawnEnemy();
            SpawnedEnemies++;

            yield return wait;

        }
    }


    private void DoSpawnEnemy()
    {
      
            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            int VertexIndex = UnityEngine.Random.Range(0, triangulation.vertices.Length);

            NavMeshHit Hit;
            if (NavMesh.SamplePosition(triangulation.vertices[VertexIndex], out Hit, 1f, 11))
            {
 
                GameObject enemy = _enemyPool.Get();
                NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();
                enemyAgent.Warp(Hit.position);
                enemyAgent.enabled = true;
            enemy.GetComponent<EnemyHealth>().SetPool(_enemyPool);
            
        }
            else
            {
                Debug.Log("Unable to place enemy on NavMesh");
            }
        
     
    }


    private void KillEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
   
}
