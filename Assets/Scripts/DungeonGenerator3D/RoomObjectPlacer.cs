using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RoomObjectPlacer : MonoBehaviour
{
    public List<GameObject> prefabsToPlace;
    public int maxPrefabs = 10;

    private void Start()
    {
        PlaceRandomPrefabs();
    }

    private void PlaceRandomPrefabs()
    {
        if (prefabsToPlace.Count == 0)
        {
            Debug.LogWarning("No prefabs to place.");
            return;
        }

        Transform container = transform; // Change this if the container is not the script's parent

        for (int i = 0; i < maxPrefabs; i++)
        {
            GameObject randomPrefab = prefabsToPlace[Random.Range(0, prefabsToPlace.Count)];
            Vector3 randomPosition = new Vector3(
                Random.Range(container.position.x - container.localScale.x / 2, container.position.x + container.localScale.x / 2),
                Random.Range(container.position.y - container.localScale.y / 2, container.position.y + container.localScale.y / 2),
                Random.Range(container.position.z - container.localScale.z / 2, container.position.z + container.localScale.z / 2)
            );

            Instantiate(randomPrefab, randomPosition, Quaternion.identity, container);
        }
    }
}