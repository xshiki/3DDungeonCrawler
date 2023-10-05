using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class MagicWeaponController : MonoBehaviour
{
    [SerializeField] public Spell spell;
    [SerializeField] private float timeBetweenCast = 2.5f;

    MagicWeaponItemData weaponData;
    Animator weaponAnimator;
    AudioSource audioSource;
    PlayerController playerController;
    PlayerRessource playerRessource;
    Transform playerTransform;
    PlayerInput playerInput;
    Transform _castPoint;
    
    bool castingMagic = false;
    public bool isCasting => castingMagic;  
   



    void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        playerInput = new PlayerInput();
        playerRessource = FindObjectOfType<PlayerRessource>();
        audioSource = GetComponent<AudioSource>();

        ItemDataProvider dataProvider = GetComponent<ItemDataProvider>();


        if (weaponData == null)
        {
            weaponData = GetComponent<ItemDataProvider>().Item as MagicWeaponItemData;
        }
        weaponAnimator = GetComponent<Animator>();
      
        spell = weaponData.spellToCast;
        playerTransform= GameObject.Find("SpellCastPoint").GetComponent<Transform>();
        _castPoint = playerTransform;
       
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
       
        bool hasEnoughMana = playerRessource.currentMana - spell.SpellToCast.ManaCost > 0f;
        if (!castingMagic && hasEnoughMana)
        {
           
            castingMagic = true;
            playerController.PlayAnimation("Ranged");
            audioSource.PlayOneShot(weaponData.weaponSwingSound);
            playerRessource.currentMana -= spell.SpellToCast.ManaCost;
            InstantiateSpell();
            Debug.Log("casting spell");
        }
        else { NotificationManager.Instance.SetNewNotification("The ability isn't ready yet.", new Color(255,0,0));
            return;
        }
        Invoke(nameof(ResetAttack), timeBetweenCast);

    }

    void ResetAttack()
    {
        castingMagic = false;

    }
    void InstantiateSpell()
    {
        Instantiate(spell, _castPoint.position, _castPoint.rotation);
    }

}
