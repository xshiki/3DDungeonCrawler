using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelNotification : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        NotificationManager.Instance.SetNewNotification(" Level 0");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
