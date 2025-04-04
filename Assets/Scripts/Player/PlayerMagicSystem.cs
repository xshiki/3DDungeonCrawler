using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMagicSystem : MonoBehaviour
{


    [SerializeField] private Spell spellToCast;
    // Start is called before the first frame update
    [SerializeField] private PlayerRessource _playerRessource;
    [SerializeField] private Transform _castPoint;
    [SerializeField] private float timeBetweenCast = 0.25f;
    private float currentCastTimer;

    private bool castingMagic = false;
    private PlayerInput playerInput;
    bool hasSpeedBuff = false;
    bool hasArmorBuff = false;
    bool hasDamageBuff = false;
    void Awake()
    {
        playerInput = new PlayerInput();
        _playerRessource = GetComponent<PlayerRessource>();
        _castPoint = GameObject.Find("SpellCastPoint").transform;
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
        bool isSpellCastHeldDown = playerInput.Player.SpellCast.ReadValue<float>() > 0.1;
        bool hasEnoughMana = _playerRessource.currentMana - spellToCast.SpellToCast.ManaCost > 0f;

        
        if (!castingMagic && isSpellCastHeldDown && hasEnoughMana)
        {
            castingMagic = true;
            _playerRessource.currentMana -= spellToCast.SpellToCast.ManaCost;   
            currentCastTimer = 0;
            CastSpell();
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

    void CastSpell()
    {

      

    }

    void RemoveSpeedBuff(int speed)
    {
        _playerRessource.speed.RemoveModifier(speed);
        hasSpeedBuff = false;
    }

    void RemoveArmorBuff(int armor)
    {

    }

    void RemoveDamageBuff(int damage)
    {

    }


    private IEnumerator RemoveBuffAfterDuration(SupportSpell supportSpell, float duration)
    {
        yield return new WaitForSeconds(duration);
      
    }
}

