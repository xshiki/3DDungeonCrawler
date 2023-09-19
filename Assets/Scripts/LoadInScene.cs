using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadInScene : MonoBehaviour
{
    public static LoadInScene instance = null;
    private void Awake()
    {   
       
       DontDestroyOnLoad(gameObject);

     
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnMainMenu;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnMainMenu;
    }

    void OnMainMenu(Scene scene, LoadSceneMode mode)
    {   

        if(scene.name == "MainMenu" || scene.name == "LoadingScene")
        {
            Destroy(gameObject);
        }
    }
}
