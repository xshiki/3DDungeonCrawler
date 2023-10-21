using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;


/// <summary>
/// Creates a nav mesh of the dungeon, after generation is finished
/// </summary>


public class BakeNavMesh : MonoBehaviour
{
    private NavMeshSurface surface;

    private void Start()
    {
        surface = GetComponent<NavMeshSurface>();
    }
    public void BuildNavMesh()
    {
        surface.BuildNavMesh();
    }
   
}
