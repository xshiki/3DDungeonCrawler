using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;



/// <summary>
///  Shows a pop up notification text for a fixed amount of time
///  Singleton
/// </summary>
public class NotificationManager : MonoBehaviour
{

    //Singleton
    public static NotificationManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            instance = FindObjectOfType<NotificationManager>();
            if (instance != null) { return instance; }
            CreateNewInstance();
            return instance;
        }
    }

    public static NotificationManager CreateNewInstance()
    {
        NotificationManager notificationManagerPrefab = Resources.Load<NotificationManager>("NotificationManager");
        instance = Instantiate(notificationManagerPrefab);
        return instance;
    }
    private static NotificationManager instance;


    private void Awake()
    {
        if (Instance != this) { Destroy(gameObject); }
    }


    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private float fadeTime;


    private IEnumerator notificationCoroutine;
    public void SetNewNotification(string message)
    {
        if (notificationCoroutine != null)
        {

            StopCoroutine(notificationCoroutine);
        }

        notificationCoroutine = FadeOutNotification(message);
        StartCoroutine(notificationCoroutine);
    }


    public void SetNewNotification(string message, Color color)
    {
        if (notificationCoroutine != null)
        {

            StopCoroutine(notificationCoroutine);
        }

        notificationCoroutine = FadeOutNotification(message, color);
        StartCoroutine(notificationCoroutine);
    }

    private IEnumerator FadeOutNotification(string message, Color color)
    {

        notificationText.text = message;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime; //
            notificationText.color = new Color(color.r,
                                                color.g,
                                               color.b,
                                                Mathf.Lerp(1f, 0f, t / fadeTime));
            yield return null; //do it over frames
        }





    }
    private IEnumerator FadeOutNotification(string message)
    {
        notificationText.color = Color.white;
        notificationText.text = message;
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime; //
            notificationText.color = new Color(notificationText.color.r,
                                                notificationText.color.g,
                                                notificationText.color.b,
                                                Mathf.Lerp(1f, 0f, t / fadeTime));
            yield return null; //do it over frames
        }



    }
}
