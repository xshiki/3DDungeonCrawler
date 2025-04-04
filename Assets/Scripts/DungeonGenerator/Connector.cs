using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Connector : MonoBehaviour
{
    public Vector2 size = Vector2.one * 4f;
    public bool isConnected = false;

    bool isPlaying = false;


    void Start()
    {
        isPlaying = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = isConnected ? Color.red : Color.green;
        if(!isPlaying ) { Gizmos.color = Color.cyan; }
        Vector2 halfSize = size * 0.5f;
        Vector3 offset = transform.position + transform.up * halfSize.y;
        Gizmos.DrawLine(offset, offset + transform.forward);
        //define top and side vectors
        Vector3 top = transform.up * size.y;
        Vector3 side = transform.right * halfSize.y;
        Vector3 topRight = transform.position + top + side;
        Vector3 topLeft = transform.position + top - side;
        Vector3 botRight = transform.position + side;
        Vector3 botLeft = transform.position - side;

        //define corner vectors
        Gizmos.DrawLine(topRight, topLeft);
        Gizmos.DrawLine(topLeft, botLeft);
        Gizmos.DrawLine(botLeft, botRight);
        Gizmos.DrawLine(botRight, topRight);


        //draw diagonal lines
        Gizmos.color *= 0.7f;

        Gizmos.DrawLine(topRight, offset);
        Gizmos.DrawLine(topLeft, offset);
        Gizmos.DrawLine(botRight, offset);
        Gizmos.DrawLine(botLeft, offset);


    }
}
