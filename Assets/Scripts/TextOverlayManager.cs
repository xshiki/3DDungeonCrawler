using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextOverlayManager : MonoBehaviour
{


    //Singleton
    public static TextOverlayManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }
            instance = FindObjectOfType<TextOverlayManager>();
            if(instance != null ) { return instance; }
            CreateNewInstance();
            return instance;
        }
    }

    public static TextOverlayManager CreateNewInstance()
    {
        TextOverlayManager textOverlayManagerPrefab = Resources.Load<TextOverlayManager>("TextOverLayManager");
        instance = Instantiate(textOverlayManagerPrefab);
        return instance;

    }

    private static TextOverlayManager instance;


    private void Awake()
    {
        if (Instance != this) { Destroy(gameObject); }
    }

    [SerializeField] private TextMeshProUGUI textOverlayText;
    [SerializeField] private Canvas canvas;
    [SerializeField] private float fadeTime;

    private IEnumerator textOverlayCoroutine;
    public void SetNewTextOverlay(string message)
    {
        if(textOverlayCoroutine != null) 
        { 
           StopAllCoroutines();
        }

        textOverlayCoroutine = FadeOutText(message);
        StartCoroutine(textOverlayCoroutine);
    }


    public void SetNewTextOverlay(string message, Color color)
    {
        if (textOverlayCoroutine != null)
        {
            StopAllCoroutines();
        }

        textOverlayCoroutine = FadeOutText(message, color);
        StartCoroutine(textOverlayCoroutine);
    }



    private IEnumerator FadeOutText(string message, Color color)
    {

        textOverlayText.text = message;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime; //
            textOverlayText.color = new Color(color.r,
                                                color.g,
                                               color.b,
                                                Mathf.Lerp(1f, 0f, t / fadeTime));
            yield return null; //do it over frames
        }





    }


    private IEnumerator FadeOutText(string message)
    {

        textOverlayText.text = message;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime; //
            textOverlayText.color = new Color(textOverlayText.color.r,
                                                textOverlayText.color.g,
                                               textOverlayText.color.b,
                                                Mathf.Lerp(1f, 0f, t / fadeTime));
            yield return null; //do it over frames
        }





    }



}
