using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemToolTipDescription : MonoBehaviour
{
    public TextMeshProUGUI ItemName;
    public TextMeshProUGUI ItemDescription;


    public void SetItemName(string itemName)
    {
        this.ItemName.text = itemName;
    }


    public void SetItemDescription(string itemDescription)
    {
        this.ItemDescription.text = itemDescription;
    }
}
