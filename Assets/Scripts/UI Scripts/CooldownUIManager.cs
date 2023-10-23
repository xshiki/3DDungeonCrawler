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



    private Dictionary<string, Coroutine> activeCoroutines = new Dictionary<string, Coroutine>();
    private Dictionary<string, GameObject> cooldownObjects = new Dictionary<string, GameObject>();
    public void RemoveCooldown(string cooldownName)
    {
        if (activeCoroutines.ContainsKey(cooldownName))
        {
            // Stop the coroutine associated with this cooldown and remove it from the dictionary.
            StopCoroutine(activeCoroutines[cooldownName]);
            activeCoroutines.Remove(cooldownName);
        }

        if (cooldownObjects.ContainsKey(cooldownName))
        {
            // Destroy the GameObject associated with this cooldown and remove it from the dictionary.
            Destroy(cooldownObjects[cooldownName]);
            cooldownObjects.Remove(cooldownName);
        }
    }

    public void SetNewCoolDown(SupportSpell supportSpell, Sprite icon, float duration)
    {
        GameObject coolDownGO = Instantiate(cooldownPrefab, cdPanel);
        coolDownGO.name = supportSpell.supportType.ToString();
        coolDownGO.GetComponent<Image>().sprite = icon;
        coolDownGO.GetComponentInChildren<TextMeshProUGUI>().text = duration.ToString();
        Coroutine newCoroutine = StartCoroutine(StartCooldownTimer(coolDownGO, duration));
        activeCoroutines[coolDownGO.name] = newCoroutine;
        cooldownObjects[coolDownGO.name] = coolDownGO;
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

