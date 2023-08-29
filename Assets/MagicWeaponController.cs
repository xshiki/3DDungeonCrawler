using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWeaponController : WeaponController
{

    [SerializeField] private Spell spellToCast;
    // Start is called before the first frame update
    public PlayerRessource playerRessource;
    [SerializeField] private Transform _castPoint;
    [SerializeField] private float timeBetweenCast = 0.25f;
    private float currentCastTimer;

    private bool castingMagic = false;
    private PlayerInput playerInput;



    void Awake()
    {
     
        playerController = FindObjectOfType<PlayerController>();
        playerEquipment = FindObjectOfType<PlayerEquipment>();
        playerInput = new PlayerInput();
        playerRessource = FindObjectOfType<PlayerRessource>();
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
    }
  

    public void CastSpell()
    {
       
        bool hasEnoughMana = playerRessource.currentMana - spellToCast.SpellToCast.ManaCost > 0f;
        if (!castingMagic && hasEnoughMana)
        {
            castingMagic = true;
            playerRessource.currentMana -= spellToCast.SpellToCast.ManaCost;
            currentCastTimer = 0;
            InstantiateSpell();
            Debug.Log("casting spell");
        }
        if (castingMagic)
        {
            currentCastTimer += Time.deltaTime;
            if (currentCastTimer > timeBetweenCast)
            {
                castingMagic = false;
            }
        }

    }


    void InstantiateSpell()
    {
        Instantiate(spellToCast, _castPoint.position, _castPoint.rotation);
    }

}
