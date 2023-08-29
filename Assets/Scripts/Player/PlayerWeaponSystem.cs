using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWeaponSystem : MonoBehaviour
{

    [SerializeField] private Weapon weaponToUse;
    // Start is called before the first frame update
    [SerializeField] private Transform _castPoint;
    [SerializeField] private float timeBetweenSwing = 0.25f;
    private float currentSwingTimer;

    private bool swingingWeapon = false;
    private PlayerInput playerInput;
    
    void Awake()
    {
        playerInput = new PlayerInput();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
    
    // Update is called once per frame
    void Update()
    {
       
       /*
            if (!swingingWeapon) {
            swingingWeapon = true;
            currentSwingTimer = 0;
            swingWeapon();
            Debug.Log("swinging weapon");
        }


        if (swingingWeapon)
        {
            currentSwingTimer += Time.deltaTime;
            if (currentSwingTimer > timeBetweenSwing)
            {
                swingingWeapon = false;
            }
        }
       */
    }


    void swingWeapon()
    {
      

    }
}
