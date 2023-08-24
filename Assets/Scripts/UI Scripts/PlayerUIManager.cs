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
    // Start is called before the first frame update

    private void FixedUpdate()
    {
        _hungerMeter.fillAmount = _playerRessource.healthPercent;
        _manaMeter.fillAmount = _playerRessource.currentManaPercent;
        _hungerMeterValue.text = (Mathf.Round(_playerRessource.healthPercent*100)).ToString() + " %";
        _manaMeterValue.text = (Mathf.Round(_playerRessource.currentManaPercent * 100)).ToString() + " %";  
    }
}
