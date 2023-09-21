using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FloorTextOverlay : MonoBehaviour
{
    public int floorCounter = 1;
    public TextMeshProUGUI gameOverScore;
    public void SetFloorText()
    {
        Debug.Log("set floor test triggered");
        TextOverlayManager.Instance.SetNewTextOverlay("Floor "+ floorCounter.ToString());
        gameOverScore.text = "Reached Floor: "+ floorCounter.ToString();
        floorCounter++;
    }
}
