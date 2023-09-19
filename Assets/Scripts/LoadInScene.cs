using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadInScene : MonoBehaviour
{
    public static LoadInScene instance;
    private void Awake()
    {
        if (instance == null)
        {
            //if it doesnt, make it exist
            instance = this;
        }
        else if (instance != this)
        {
            //destroy duplicate instances
            Destroy(gameObject);
        }
        //set this instance as protected
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
