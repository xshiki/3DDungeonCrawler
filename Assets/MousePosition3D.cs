using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePosition3D : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        if(Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity,layerMask))
        {
            print(hitInfo.collider.gameObject.layer);
            if (layerMask != 6)
            {
               
                transform.position = hitInfo.point;
            }
        
        }
    }
}
