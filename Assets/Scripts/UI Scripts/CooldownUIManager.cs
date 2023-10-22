using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUIManager : MonoBehaviour
{
    public static CooldownUIManager Instance
    {
        get
        {
            if (instance != null)
            {
                return instance;
            }
            instance = FindObjectOfType<CooldownUIManager>();
            if (instance != null) { return instance; }
            CreateNewInstance();
            return instance;
        }
    }
    public static CooldownUIManager CreateNewInstance()
    {
        CooldownUIManager cooldownUIManagerPrefab = Resources.Load<CooldownUIManager>("CooldownUIManager");
        instance = Instantiate(cooldownUIManagerPrefab);
        return instance;    
    }
    public static CooldownUIManager instance;

    private void Awake()
    {
        if (Instance != this) { Destroy(gameObject); }
    }

    [SerializeField] private Transform cdPanel;
    [SerializeField] protected GameObject cooldownPrefab;


    public void SetNewCoolDown(Sprite icon, float duration)
    {
        GameObject cdObject = Instantiate(cooldownPrefab, cdPanel);
        cdObject.GetComponent<Image>().sprite = icon;
        cdObject.GetComponentInChildren<TextMeshProUGUI>().text = duration.ToString();

        StartCoroutine(StartCooldownTimer(cdObject, duration));
    }


    private IEnumerator StartCooldownTimer(GameObject cooldownObject, float duration)
    {
        Image fillImage = cooldownObject.GetComponent<Image>();
        TextMeshProUGUI cooldown = cooldownObject.GetComponentInChildren<TextMeshProUGUI>();
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            fillImage.fillAmount = 1 - currentTime / duration;
            cooldown.text = Mathf.Ceil(duration - currentTime).ToString();
            yield return null;
        }

        Destroy(cooldownObject);
    }
}

