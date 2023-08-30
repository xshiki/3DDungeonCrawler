using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{

    [SerializeField] private PlayerRessource _playerRessource;
    [SerializeField] private Image _hungerMeter, _manaMeter;
    [SerializeField] private TextMeshProUGUI _hungerMeterValue, _manaMeterValue;


    [Header("Animation")]
    public float lerpSpeed = 3f;
 
    private void Update()
    {

        lerpSpeed = 3f * Time.deltaTime;
        _hungerMeter.fillAmount = Mathf.Lerp(_hungerMeter.fillAmount, _playerRessource.currentHealth / _playerRessource.maxHealth, lerpSpeed);
        _manaMeter.fillAmount = Mathf.Lerp(_manaMeter.fillAmount, _playerRessource.currentMana/_playerRessource.maxMana, lerpSpeed);
        _hungerMeterValue.text = (Mathf.Round(_playerRessource.healthPercent*100)).ToString() + " %";
        _manaMeterValue.text = (Mathf.Round(_playerRessource.currentManaPercent * 100)).ToString() + " %";  
    }
}
