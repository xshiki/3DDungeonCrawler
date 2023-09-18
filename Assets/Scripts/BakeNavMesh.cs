using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

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
