using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject playerInventory;
    public Button exitInventoryButton;
    public FirstPersonController firstPersonController;

    // Start is called before the first frame update
    [SerializeField]
    private InputActionReference openInventory, hotbarSelection;
    [SerializeField]
    private PlayerInput playerInput;
    PlayerInput.PlayerActions input;

    [Header("Equipment")]
    public PlayerEquipment playerEquipment;

    [Header("Weapon")]
    public Transform weaponHolderPosition;
    public WeaponController currentWeapon;
    private void Awake()
    {
      playerInput = new PlayerInput();
      input = playerInput.Player;
      AssignInputs();
    }

    private void OnEnable()
    {   
        input.Enable();
 
    }


    private void OnDisable()
    {
        input.Disable();
        openInventory.action.performed -= OpenInventory;
        hotbarSelection.action.performed -= UseItem;
        input.Attack.performed -= OnAttackPerformed;
    }
    void AssignInputs()
    {
        openInventory.action.performed += OpenInventory;
        hotbarSelection.action.performed += UseItem;
        input.Attack.performed += OnAttackPerformed;
    }


    public void SetCurrentWeapon(WeaponController equippedWeapon)
    {

        this.currentWeapon = equippedWeapon;
    }
  
    private void UseItem(InputAction.CallbackContext context)
    {
        /*
        
        InventoryItemData selectedItem = InventoryManager.Instance.GetSelectedItem(true);
        if(selectedItem != null)
        {
            if (selectedItem.consumable)
            {
                selectedItem.UseItem();
            }
        }
        */

    }


    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (currentWeapon != null)
        {   if(currentWeapon is WeaponController)
            {
                currentWeapon.swingWeapon();
            }
            if (currentWeapon is MagicWeaponController)
            {
                MagicWeaponController magicWeapon = currentWeapon as MagicWeaponController;
                magicWeapon.CastSpell();
            }
        }
        else
        {
            Debug.Log("No weapon equipped!");
        }
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
