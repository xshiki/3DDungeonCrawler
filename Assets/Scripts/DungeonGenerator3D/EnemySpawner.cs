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
    int SpawnedEnemies = 0;

    public ObjectPool<GameObject> _enemyPool;
    public enum SpawnMethod {RoundRobin, Random};
    public SpawnMethod enemySpawnMethod = SpawnMethod.RoundRobin;

    public LayerMask layerMask = 11;
 

    private void Awake()
    {
        dungeonGenerator = GameObject.Find("DungeonGenerator").GetComponent<ProceduralDungeonGenerator>();
        _enemyPool = new ObjectPool<GameObject>(CreateEnemy, null, OnPutBackInPool, defaultCapacity: 50);


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
        SpawnedEnemies = 0;
        while(SpawnedEnemies < numberOfEnemiesToSpawn) 
        {
            SpawnEnemy();
           

            yield return wait;

        }
    }


    private void SpawnEnemy()
    {
      
            NavMeshTriangulation triangulation = NavMesh.CalculateTriangulation();
            int VertexIndex = UnityEngine.Random.Range(0, triangulation.vertices.Length);
            Vector3 offset = new Vector3(rndNumber(), 0f, rndNumber());
            NavMeshHit Hit;
          
            float yRotation = UnityEngine.Random.Range(0, 4) * 90f;
        if (NavMesh.SamplePosition(triangulation.vertices[VertexIndex] + offset, out Hit, 2f, 11))
            {


                GameObject enemy = _enemyPool.Get();
                if (enemy == null) { return; }
                enemy.transform.Rotate(0, yRotation, 0);
                NavMeshAgent enemyAgent = enemy.GetComponent<NavMeshAgent>();
                enemyAgent.Warp(Hit.position);
                enemy.GetComponent<EnemyAI>().SetSpawnPosition(Hit.position);
                enemyAgent.enabled = true;
                enemy.GetComponent<EnemyHealth>().SetPool(_enemyPool);
                SpawnedEnemies++;

        }
            else
            {
                //Debug.Log("Unable to place enemy on NavMesh");
            }
        
     
    }

    float rndNumber()
    {
        return UnityEngine.Random.Range(0f, 10f);
    }
    private void KillEnemy(GameObject enemy)
    {
        Destroy(enemy);
    }
   
}
