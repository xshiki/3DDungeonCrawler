using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{

    [SerializeField] private PlayerRessource _playerRessource;
    [SerializeField] private Image _hungerMeter, _manaMeter;
    [SerializeField] private TextMeshProUGUI _hungerMeterValue, _manaMeterValue;


    public UnityAction OnHealthbarFull;
    public UnityAction OnManabarFull;


    [SerializeField] private float fadeTime = 5f;

    [Header("Animation")]
    public float lerpSpeed = 3f;
    private IEnumerator healthBarCoroutine;
    private IEnumerator manaBarCoroutine;
    private bool healthFull = true;
    private bool manaFull = true;

    private void Awake()
    {
        OnHealthbarFull += FadeAwayHealth;
        OnManabarFull += FadeAwayMana;

    }

    private void FadeAwayMana()
    {
        manaBarCoroutine = FadeOutBar(_manaMeter, _manaMeterValue);
        StartCoroutine(manaBarCoroutine);
       
    }

    private void FadeAwayHealth()
    {
        healthBarCoroutine = FadeOutBar(_hungerMeter, _hungerMeterValue);
        StartCoroutine(healthBarCoroutine);
    }

    private void Update()
    {
      
        lerpSpeed = 3f * Time.deltaTime;
        _hungerMeter.fillAmount = Mathf.Lerp(_hungerMeter.fillAmount, _playerRessource.currentHealth / _playerRessource.maxHealth, lerpSpeed);
        _manaMeter.fillAmount = Mathf.Lerp(_manaMeter.fillAmount, _playerRessource.currentMana/_playerRessource.maxMana, lerpSpeed);
        _hungerMeterValue.text = (Mathf.Round(_playerRessource.healthPercent*100)).ToString() + " %";
        _manaMeterValue.text = (Mathf.Round(_playerRessource.currentManaPercent * 100)).ToString() + " %";  
    }

    private void FixedUpdate()
    {

        if (healthFull && _playerRessource.healthPercent == 1)
        {

            OnHealthbarFull?.Invoke();
            healthFull = false;
        }
        else if(_playerRessource.healthPercent < 1) {
            
            _hungerMeter.color = new Color(_hungerMeter.color.r, _hungerMeter.color.g, _hungerMeter.color.b,255);
            _hungerMeterValue.alpha = 255;
            healthFull = true;
            StopCoroutine(healthBarCoroutine);
        }




       if (manaFull && _playerRessource.currentManaPercent == 1)
        {

            OnManabarFull?.Invoke();
            manaFull = false;

        }else if (_playerRessource.currentManaPercent < 1)
        {
            _manaMeter.color = new Color(_manaMeter.color.r, _manaMeter.color.g, _manaMeter.color.b, 255);
            _manaMeterValue.color = new Color(_manaMeterValue.color.r, _manaMeterValue.color.g, _manaMeterValue.color.b, 255);
            manaFull = true; 
            StopCoroutine(manaBarCoroutine);
        }
        
        
    }


    private IEnumerator FadeOutBar(Image fillbar, TextMeshProUGUI message)
    {
       
        float t = 0;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime; // 
            message.color = new Color(message.color.r,
                                                message.color.g,
                                                message.color.b,
                                                Mathf.Lerp(1f, 0f, t / fadeTime));
            fillbar.color = new Color(fillbar.color.r,
                                                fillbar.color.g,
                                                fillbar.color.b,
                                                Mathf.Lerp(1f, 0f, t / fadeTime));

            yield return null;
        }



    }
}
