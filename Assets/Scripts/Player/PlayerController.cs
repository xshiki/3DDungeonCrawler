using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerController : MonoBehaviour
{
    public GameObject playerInventory;
    public GameObject pauseScreen, gameOverScreen;
    public Transform playerOrientation;
    public Button exitInventoryButton;
    public FirstPersonController firstPersonController;
    public PlayerRessource playerRessource;

    public UnityAction OnInventoryClosed;
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
    public float punchDamage = 5f;
    public float punchRange = 1f;
    public float timeBetweenAttack = 2.5f;
    public AudioClip punchAudioClip, punchSwingAudioClip;
    public AudioSource audioSource;


    [Header("Map Camera")]
    public GameObject mapCamera;
    bool mapOpen = false;
    bool attacking = false;
    int attackCount = 0;
    public const string PUNCH1 = "Punch 1";
    public const string PUNCH2 = "Punch 2";
    public const string PUNCH3 = "Punch 3";

    private void Awake()
    {
        playerInput = new PlayerInput();
        audioSource = GetComponent<AudioSource>();

        playerRessource = GetComponent<PlayerRessource>();
        playerOrientation = GameObject.Find("Orientation").transform;
        playerInventory = GameObject.Find("PlayerInventory");
        pauseScreen = GameObject.Find("Pause Panel");
        pauseScreen.SetActive(false);
        playerInventory.SetActive(false);
        animator = GetComponentInChildren<Animator>();
        input = playerInput.Player;
        mapCamera = GameObject.Find("OverheadCamera");
        mapCamera.SetActive(false);


        AssignInputs();
    }


    private void Update()
    {
        SetAnimations();
    }

    void AssignInputs()
    {
        openInventory.action.performed += OpenInventory;
        //hotbarSelection.action.performed += UseItem;
        input.Attack.performed += OnAttackPerformed;
        input.OpenMap.performed += OnOpenMapPerformed;
    }

   

    private void OnEnable()
    {
        input.Enable();
        openInventory.action.performed += OpenInventory;
        //hotbarSelection.action.performed += UseItem;
        input.Attack.performed += OnAttackPerformed;
        input.OpenMap.performed += OnOpenMapPerformed;
    }


    private void OnDisable()
    {
        input.Disable();
        openInventory.action.performed -= OpenInventory;
        //hotbarSelection.action.performed -= UseItem;
        input.Attack.performed -= OnAttackPerformed;
        input.Pause.performed -= OpenInventory;
        input.OpenMap.performed -= OnOpenMapPerformed;
    }


    void SetAnimations()
    {

        if (currentWeapon != null)
        {
            if(currentWeapon.weaponData.WeaponType == WeaponItemData.Weapons.Sword)
            {
                animator.SetInteger("WeaponType", 1);
            }
            else if (currentWeapon.weaponData.WeaponType == WeaponItemData.Weapons.Sword)
            {
                animator.SetInteger("WeaponType", 3);
                Debug.Log("Dagger equipped");
            }
           
        }
        else if (currentMagicWeapon != null)
        {

            animator.SetInteger("WeaponType", 2);
        }
        else
        {
            animator.SetInteger("WeaponType", 0);

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











    public void SetCurrentWeapon(WeaponController equippedWeapon)
    {

        this.currentWeapon = equippedWeapon;
    }

    public void SetCurrentMagicWeapon(MagicWeaponController equippedWeapon)
    {

        this.currentMagicWeapon = equippedWeapon;
    }

    public void UnequipWeapon()
    {
        animator.SetTrigger("unequipWeapon");
    }

    public void PlayAnimation(string newState)
    { animator.Play(newState); }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {

        //dont perform attacks when inventory is open
        if (playerInventory.activeInHierarchy || pauseScreen.activeInHierarchy) { return; }


        if (currentWeapon != null)
        {  
            currentWeapon.swingWeapon();
           
        }
        else if (currentMagicWeapon != null)
        {
            currentMagicWeapon.CastSpell();

        }
        else
        {
            Punch();
        }
    }


    void Punch()
    {
        if (attacking) { return; }
        attacking = true;
        Invoke(nameof(ResetAttack), timeBetweenAttack);


        if (attackCount == 0)
        {
            PlayAnimation(PUNCH1);
            attackCount++;
        }
        else if(attackCount == 1) 
        {
            PlayAnimation(PUNCH2);
            attackCount++;
        }else{
                PlayAnimation(PUNCH3);
                attackCount = 0;
        }
        //audioSource.PlayOneShot(punchSwingAudioClip);
       
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
                HitTarget(hits[i]);
            }
        }
        


        
    }

    public void HitTarget(RaycastHit hit)
    {
        if (hit.collider.GetComponent<EnemyManager>())
        {

            float damage = 0;
            float damageModifier = (1f + ((float)playerRessource.strength.GetValue() / 100f));
            damage = punchDamage * damageModifier;
            hit.collider.GetComponent<EnemyManager>().TakeDamage((int)damage);
            audioSource.pitch = Random.Range(0.6f, 1.3f);
            audioSource.PlayOneShot(punchAudioClip);
        }
    }



    private void OnOpenMapPerformed(InputAction.CallbackContext context)
    {
        mapOpen = !mapOpen;
        mapCamera.SetActive(mapOpen);
        GameObject.Find("UI").GetComponent<CanvasGroup>().alpha = mapOpen ? 0 : 1;
    }


    private void OpenInventory(InputAction.CallbackContext context)
    {
        if (playerInventory.activeInHierarchy)
        {
            playerInventory.SetActive(false);
            OnInventoryClosed?.Invoke();
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
