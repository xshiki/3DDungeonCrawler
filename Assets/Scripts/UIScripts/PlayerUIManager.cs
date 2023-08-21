using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{

    [SerializeField] private PlayerRessource _playerRessource;
    [SerializeField] private Image _hungerMeter, _manaMeter;
    // Start is called before the first frame update

    private void FixedUpdate()
    {
        _hungerMeter.fillAmount = _playerRessource.healthPercent;
        _manaMeter.fillAmount = _playerRessource.currentManaPercent;
    }
}
