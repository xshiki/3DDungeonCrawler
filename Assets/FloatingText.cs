using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public Camera mainCamera;
    public float DestroyTime = 1f;
    public Vector3 Offset = new Vector3(0, 2, 0);
    public Vector3 RandomOffset = new Vector3(0.5f, 1, 0);


    void Start()
    {
        mainCamera = Camera.main;
        
        transform.localPosition += new Vector3(Random.Range(-RandomOffset.x, RandomOffset.x),
        0,
        0);
        
        transform.position += Offset;
    }

    private void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
    }

    public void DestroyFloatingText()
    {
        Destroy(gameObject);
    }
}
