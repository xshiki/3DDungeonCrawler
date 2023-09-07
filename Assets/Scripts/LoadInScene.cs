using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadInScene : MonoBehaviour
{
    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
