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

    [Header("Animation")]
    public Animator animator;
    public const string IDLE = "Idle";
    public const string WALK = "Walk";

    string currentAnimationState;


    [Header("Equipment")]
    public PlayerEquipment playerEquipment;

    [Header("Weapon")]
    public Transform weaponHolderPosition;
    public WeaponController currentWeapon;
    public MagicWeaponController currentMagicWeapon;
    private void Awake()
    {
      playerInput = new PlayerInput();
      animator = GetComponentInChildren<Animator>();
      input = playerInput.Player;
      AssignInputs();
    }


    private void Update()
    {
        SetAnimations();
    }





    private void OnEnable()
    {   
        input.Enable();
 
    }


    private void OnDisable()
    {
        input.Disable();
        openInventory.action.performed -= OpenInventory;
        //hotbarSelection.action.performed -= UseItem;
        input.Attack.performed -= OnAttackPerformed;
    }


    void SetAnimations()
    { 

        
        Rigidbody playerRb = GetComponent<Rigidbody>();
        Vector3 velocity = playerRb.velocity;
        if (currentWeapon == null)
        {
             if (velocity.magnitude == 0)
            {
                ChangeAnimationState(IDLE);
            }
            else
            {
                ChangeAnimationState(WALK);
            }
        }else if(!currentWeapon.isAttacking)
        {
            if (velocity.magnitude == 0)
            {
                ChangeAnimationState(IDLE);
            }
            else
            {
                ChangeAnimationState(WALK);
            }
        }
       



    }

    public void ChangeAnimationState(string newState)
    {
        // STOP THE SAME ANIMATION FROM INTERRUPTING WITH ITSELF //
        if (currentAnimationState == newState) return;

        // PLAY THE ANIMATION //
        currentAnimationState = newState;
        animator.CrossFadeInFixedTime(currentAnimationState, 0.2f);
    }









    void AssignInputs()
    {
        openInventory.action.performed += OpenInventory;
        //hotbarSelection.action.performed += UseItem;
        input.Attack.performed += OnAttackPerformed;
    }


    public void SetCurrentWeapon(WeaponController equippedWeapon)
    {

        this.currentWeapon = equippedWeapon;
    }

    public void SetCurrentMagicWeapon(MagicWeaponController equippedWeapon)
    {

        this.currentMagicWeapon = equippedWeapon;
    }

    public void PlayAnimation(string newState)
    { animator.Play(newState); }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        if (currentWeapon != null)
        {  
            currentWeapon.swingWeapon();        
        }
        else if (currentMagicWeapon != null)
        {
            currentMagicWeapon.CastSpell();
        }
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
