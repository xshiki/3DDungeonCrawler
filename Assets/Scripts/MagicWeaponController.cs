using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicWeaponController : MonoBehaviour
{

    public MagicWeaponItemData weaponData;
    public PlayerController playerController;
    public PlayerEquipment playerEquipment;
    public PlayerRessource playerRessource;
    public Transform playerTransform;
    public Spell spellToCast;
    [SerializeField] public Transform _castPoint;
    [SerializeField] private float timeBetweenCast = 2.5f;
    private bool castingMagic = false;
    public bool isCasting => castingMagic;
    private PlayerInput playerInput;



    void Awake()
    {
     
        playerController = FindObjectOfType<PlayerController>();
        playerEquipment = FindObjectOfType<PlayerEquipment>();
        playerInput = new PlayerInput();
        playerRessource = FindObjectOfType<PlayerRessource>();
        spellToCast = weaponData.spellToCast;
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
       
        bool hasEnoughMana = playerRessource.currentMana - spellToCast.SpellToCast.ManaCost > 0f;
        if (!castingMagic && hasEnoughMana)
        {
           
            castingMagic = true;
            playerRessource.currentMana -= spellToCast.SpellToCast.ManaCost;
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
        Instantiate(spellToCast, _castPoint.position, _castPoint.rotation);
    }

}
