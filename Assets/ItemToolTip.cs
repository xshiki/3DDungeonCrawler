using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public GameObject itemToolTipPanel;
    public Vector3 toolTipOffset;
    public RectTransform popUpObject;


    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryItem hoveredItem = eventData.pointerEnter.GetComponent<InventoryItem>();  
      
            if (hoveredItem != null)
            {
                itemToolTipPanel.SetActive(false);
                itemToolTipPanel.transform.position = hoveredItem.transform.position + toolTipOffset;
                var tooltip = itemToolTipPanel.GetComponent<ItemToolTipDescription>();
                tooltip.SetItemName(hoveredItem.item.DisplayName);
                tooltip.SetItemDescription(hoveredItem.item.Description);
                itemToolTipPanel.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(popUpObject);
                

            
        }



    }

    public void OnPointerExit(PointerEventData eventData)
    {

        itemToolTipPanel.SetActive(false);
    }
}
