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
    [SerializeField] private Image _healthMeter, _manaMeter;
    [SerializeField] private TextMeshProUGUI _healthMeterValue, _manaMeterValue;


    [Header("Player Stats UI")]
    [SerializeField] private TextMeshProUGUI _healthValue;
    [SerializeField] private TextMeshProUGUI _manaValue;
    [SerializeField] private TextMeshProUGUI _strValue;  
    [SerializeField] private TextMeshProUGUI _intValue;
    [SerializeField] private TextMeshProUGUI _spdValue;
    [SerializeField] private TextMeshProUGUI _armorValue;


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
        healthBarCoroutine = FadeOutBar(_healthMeter, _healthMeterValue);
        StartCoroutine(healthBarCoroutine);
    }

    private void Update()
    {


      
        lerpSpeed = 3f * Time.deltaTime;
        _healthMeter.fillAmount = Mathf.Lerp(_healthMeter.fillAmount, _playerRessource.currentHealth / _playerRessource.maxHealth, lerpSpeed);
        _manaMeter.fillAmount = Mathf.Lerp(_manaMeter.fillAmount, _playerRessource.currentMana/_playerRessource.maxMana, lerpSpeed);
        //_healthMeterValue.text = (Mathf.Round((_playerRessource.currentHealth/ _playerRessource.maxHealth) *100)).ToString() + " %";
        //_manaMeterValue.text = (Mathf.Round(_playerRessource.currentManaPercent * 100)).ToString() + " %";
        _healthMeterValue.text = Mathf.Round(_playerRessource.currentHealth).ToString() + "/" + Mathf.Round(_playerRessource.maxHealth).ToString();
        _manaMeterValue.text = Mathf.Round(_playerRessource.currentMana).ToString() + "/" + Mathf.Round(_playerRessource.maxMana).ToString();
        _healthValue.text = _playerRessource.maxHealth.ToString();
        _manaValue.text = _playerRessource.maxMana.ToString();
        _strValue.text = _playerRessource.strength.GetValue().ToString();
        _intValue.text = _playerRessource.intelligence.GetValue().ToString();
        _spdValue.text = _playerRessource.speed.GetValue().ToString();
        _armorValue.text = _playerRessource.armor.GetValue().ToString();

        _intValue.color = _playerRessource.intelligence.statModified ? Color.green : Color.white;
        _spdValue.color = _playerRessource.speed.statModified ? Color.green : Color.white;
        _armorValue.color = _playerRessource.armor.statModified ? Color.green : Color.white;
        _strValue.color = _playerRessource.strength.statModified ? Color.green : Color.white;
    }

    private void FixedUpdate()
    {

        if (healthFull && _playerRessource.healthPercent == 1)
        {

            OnHealthbarFull?.Invoke();
            healthFull = false;
        }
        else if(_playerRessource.healthPercent < 1) {
            
            _healthMeter.color = new Color(_healthMeter.color.r, _healthMeter.color.g, _healthMeter.color.b,255);
            _healthMeterValue.alpha = 255;
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
