using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
        
    }


    public void LoadGame()
    {
        SceneManager.LoadScene("MainGame");
    }
 

    public void QuitGame()
    {
        Application.Quit(); 
    }
}
