using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class MouseItemData : MonoBehaviour
{
    public Image ItemSprite;
    public TextMeshProUGUI ItemCount;
   


    private Transform _playerTransform;
    public float _dropOffset = 1f;

    private void Awake()
    {
        ItemSprite.color = Color.clear;
        ItemSprite.preserveAspect = true;   
        ItemCount.text = "";
        _playerTransform= GameObject.Find("Orientation").GetComponent<Transform>(); 
        if (_playerTransform == null) Debug.Log("Player not found!");
    }
   

    public void UpdateMouseSlot()
    {
      
       

    }

    private void Update()
    {
    }

    public void ClearSlot()
    {
        
      
        ItemCount.text = "";
        ItemSprite.color = Color.clear;
        ItemSprite.sprite = null;
        
       

    }
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = Mouse.current.position.ReadValue();
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
