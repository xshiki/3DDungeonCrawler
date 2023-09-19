using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ASyncLoader : MonoBehaviour
{

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Image loadingFill;

    public void LoadLevel(string levelToLoad)
    {
        Time.timeScale = 1;
        loadingScreen.SetActive(true);
        StartCoroutine(LoadLevelASync(levelToLoad));
    }

    IEnumerator LoadLevelASync(string levelToLoad)
    {
       AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad);
       while(!loadOperation.isDone)
        {

            float progresValue = Mathf.Clamp01(loadOperation.progress /0.9f);
            loadingFill.fillAmount = progresValue;
            yield return null;
        }

    }
   
}
