using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField]
    private float size = 1f;
    
    private float width;
    private float height;
    public float Size { get { return size; } }
    private void Start()
    {
        var renderer = GetComponent<Renderer>();
        width = renderer.bounds.size.x;
        height = renderer.bounds.size.z;
    }
    public Vector3 GetNearestPointOnGrid(Vector3 position)
    {

        position -= transform.position;

        int xCount = Mathf.RoundToInt(position.x/size);
        int yCount = Mathf.RoundToInt(position.y/size);
        int zCount = Mathf.RoundToInt(position.z/size);

        Vector3 result = new Vector3((float) xCount * size, (float) yCount * size, (float) zCount * size);

        result += transform.position;

        return result;
    }


    private void OnDrawGizmos()
    {
        Vector3 startPosition = transform.position;
       
        var renderer = GetComponent<Renderer>();

        /*
         *  Debug.Log(startPosition +"startposition");
        Debug.Log(renderer.bounds.size + "s ");
        Debug.Log(renderer.bounds.min + "min ");
        Debug.Log(renderer.bounds.max + "max ");
        Debug.Log(renderer.bounds.extents + " extens");
        */
        for (float x= 0; x < width+2; x += size)
        {
            for (float z = 0; z < height+2; z += size)
            {

                var point = GetNearestPointOnGrid(new Vector3(startPosition.x - renderer.bounds.extents.x + x, startPosition.y, startPosition.z - renderer.bounds.extents.z + z));
                Gizmos.DrawSphere(point, 0.1f);
               
            }
        }

        
    }

}
