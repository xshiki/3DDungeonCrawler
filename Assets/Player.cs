using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject playerInventory;
    public Button exitInventoryButton;
    public FirstPersonController firstPersonController;
    // Start is called before the first frame update
    [SerializeField]
    private InputActionReference openInventory;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    private void OnEnable()
    {
        openInventory.action.performed += OpenInventory;
        
    }

   

    private void OnDisable()
    {
        openInventory.action.performed -= OpenInventory;
    }

  
    private void OpenInventory(InputAction.CallbackContext context)
    {
        if (playerInventory.activeInHierarchy)
        {
            playerInventory.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            firstPersonController.enabled = true;
        }
        else if(!playerInventory.activeInHierarchy)
        {
            playerInventory.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            firstPersonController.enabled = false;
        }
    }
}
