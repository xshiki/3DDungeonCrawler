using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{

    public Camera mainCamera;
    public float DestroyTime = 1f;
    public Vector3 Offset = new Vector3(0, 0.3f, 0);
    public Vector3 RandomOffset = new Vector3(1f, 2f, 0);


    void Start()
    {
        mainCamera = Camera.main;
        transform.localPosition = Vector3.zero;
        transform.localPosition += new Vector3(Random.Range(-1f, 1f),0, 0);
        
        transform.localPosition += Offset;
    }

    private void LateUpdate()
    {
        if(mainCamera == null) {  return; } 
        transform.LookAt(mainCamera.transform);
        transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
    }

    public void DestroyFloatingText()
    {
        Destroy(gameObject);
    }
}
