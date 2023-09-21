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
    public Transform playerOrientation;
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

    [Header("Punch")]
    public int punchDamage = 2;
    public float punchRange = 1f;
    public float timeBetweenAttack = 2.5f;
    public AudioClip punchAudioClip;
    public AudioSource audioSource;


    bool attacking = false;

    public const string PUNCH1 = "Punch 1";
    public const string PUNCH2 = "Punch 2";

    private void Awake()
    {
      playerInput = new PlayerInput();
      audioSource = GetComponent<AudioSource>();
     
      playerOrientation = GameObject.Find("Orientation").transform;
      playerInventory = GameObject.Find("PlayerInventory");
       playerInventory.SetActive(false);
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
        input.Pause.performed -= OpenInventory;
    }


    void SetAnimations()
    { 

        
        Rigidbody playerRb = GetComponent<Rigidbody>();
        Vector3 velocity = playerRb.velocity;
        
       



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
        }else
        {
            Punch();
            Debug.Log("No weapon equipped!");
        }
    }


    void Punch()
    {
        if (attacking) { return; }
        attacking = true;
        Invoke(nameof(ResetAttack), timeBetweenAttack);
        PlayAnimation(PUNCH1);
        PlayAnimation(PUNCH2);
        AttackRaycast();
    }

    void ResetAttack()
    {
        attacking = false;

    }
    public void AttackRaycast()
    {

        RaycastHit[] hits;

        hits = Physics.RaycastAll(playerOrientation.transform.position, playerOrientation.transform.forward,punchRange);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log("target found");
                HitTarget(hits[i]);
            }
        }
        


        
    }

    public void HitTarget(RaycastHit hit)
    {
        if (hit.collider.GetComponent<EnemyManager>())
        {
            hit.collider.GetComponent<EnemyManager>().TakeDamage(punchDamage);
            audioSource.PlayOneShot(punchAudioClip);
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
