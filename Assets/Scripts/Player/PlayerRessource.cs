using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Rendering.Universal;

public class PlayerRessource : MonoBehaviour
{

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthPercent => currentHealth / maxHealth;
    [SerializeField] private float healthRechargeRate = 1f;
    [SerializeField] private float healthRechargeDelay = 10f;
    private float currentHealthDelayCounter;




    [Header("Mana")]
    public float maxMana = 100; // Maximum ressource of the player
    public float currentMana; // Current ressource of the player
    public float currentManaPercent => currentMana / maxMana;
    [SerializeField] private float manaRechargeRate = 2f;
    [SerializeField] private float manaRechargeDelay = 2.5f;
    private float currentManaDelayCounter;


    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;
    [SerializeField] private float _staminaDepletionRate = 1f;
    [SerializeField] private float _staminaRechargeRate = 2f;
    [SerializeField] private float _staminaRechargeDelay = 1f;

    private float currentStamina;
    private float _currentStaminaDelayCounter;

    public bool hasStamina => currentStamina != 0;

    public float StaminaPercent => currentStamina / maxStamina;

    public GameObject GOScreen;

    public PlayerEquipment playerEquipment;
    [Header("Stats")]
    public Stat stamina;
    public Stat strength;
    public Stat intelligence;
    public Stat armor;
    public Stat speed;



    int currentSpeedBuff = 0;
    int currentArmorBuff = 0;
    int currentDamageBuff = 0;

    Coroutine speedCO, armorCO, damageCO;



    Volume volume;
    UnityEngine.Rendering.Universal.Vignette vignette;
    private void Awake()
    {
        volume = GameObject.Find("Global Volume").GetComponent<Volume>();

        currentHealth = maxHealth;
        currentMana = maxMana;
        currentStamina = maxStamina;
        GOScreen = GameObject.Find("Game Over Panel");
        GOScreen.SetActive(false);
        playerEquipment.OnEquipmentChanged += ChangeStats;
    }

    private void ChangeStats(object sender, PlayerEquipment.OnEquipChangedArgs e)
    {
      
        ArmorItemData newItem = e.newItem as ArmorItemData;
        ArmorItemData oldItem = e.oldItem as ArmorItemData;


        if (newItem != null)
        {
            stamina.AddModifier(newItem.stamina);
            strength.AddModifier(newItem.strength);
            armor.AddModifier(newItem.armor);
            speed.AddModifier(newItem.speed);
            intelligence.AddModifier(newItem.intelligence);

        }

        if (oldItem != null)
        {
  
            stamina.RemoveModifier(oldItem.stamina);
            strength.RemoveModifier(oldItem.strength);
            armor.RemoveModifier(oldItem.armor);
            speed.RemoveModifier(oldItem.speed);
            intelligence.RemoveModifier(oldItem.intelligence);

        }

      

    }



    // Function to reduce the player's ressource by a specified amount

    public void TakeDamage(int baseDamage)
    {
        int damage = 0;
        if (baseDamage >= armor.GetValue())
        {
            damage = baseDamage * 2 - armor.GetValue();
        }
        else
        {
            damage = baseDamage * baseDamage / armor.GetValue();
        }

        currentHealth -= damage;
        FindAnyObjectByType<AudioManager>().Play("Hit");
        if (currentHealth <= 0)
        {
            Die();
           
        }

    }

    public void TakeDamage(int baseDamage, int attackStat)
    {
        int damage = 0;
        if(baseDamage >= armor.GetValue())
        {
            damage = (attackStat/100) * baseDamage * 2 - armor.GetValue();
        }
        else
        {
            damage = (attackStat / 100) * baseDamage * baseDamage / armor.GetValue();
        }

        currentHealth -= damage;
        FindAnyObjectByType<AudioManager>().Play("Hit");
        if (currentHealth <= 0) {
            Die();
        }

    }

    void Die()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        var player = GameObject.Find("Player").GetComponent<FirstPersonController>().enabled = currentHealth <=0 ? false : true;
        FindAnyObjectByType<AudioManager>().Play("Death");
        GOScreen.SetActive(true);
    }
    public void UseMana(int manacost) {

        currentMana -= manacost;
    }

    private void Update()
    {
        if(currentMana < maxMana)
        {

            if (currentManaDelayCounter < manaRechargeDelay)
            {
                currentManaDelayCounter += Time.deltaTime;
            }

            if (currentManaDelayCounter >= manaRechargeDelay)
            {
                currentMana += manaRechargeRate * Time.deltaTime;
                if (currentMana > maxMana) {currentMana = maxMana; }
            }

        }


        if (currentHealth < maxHealth)
        {

            if (currentHealthDelayCounter < healthRechargeDelay)
            {
                currentHealthDelayCounter += Time.deltaTime;
            }

            if (currentHealthDelayCounter >= healthRechargeDelay)
            {
                currentHealth += healthRechargeRate * Time.deltaTime;
                if (currentHealth > maxHealth) { currentHealth = maxHealth; }
            }

        }
         if((currentHealth/maxHealth) * 100 <= 33) {

            if (volume.profile.TryGet(out vignette))
            {
                vignette.active = true;
            }

        }
        else
        {
            if (volume.profile.TryGet(out vignette))
            {
                vignette.active = false;
            }
        }

        if (currentHealth <= 0)
        {
            currentHealth = 0;

        }

        if (currentMana <= 0)
        {
            currentMana = 0;

        }
    }


    public void ReplenshHealthMana(float healthAmount, float manaAmount)
    {   

        
        if(currentHealth + healthAmount > maxHealth)
        {
            currentHealth = maxHealth;
            
        }
        else
        {
            currentHealth += healthAmount;
        }


        if (currentMana + manaAmount > maxMana)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += manaAmount;
        }


    }


    public void ApplySupport(SupportSpell supportSpell)
    {
        if (supportSpell.supportType == SupportSpell.SupportEffect.Heal)
        {
            UseMana((int)supportSpell.ManaCost);
            ReplenshHealthMana(supportSpell.amount, 0);
            Debug.Log("has healed");
            return;
        }




        SupportSpell buff = supportSpell;
        int modifier =(int) buff.amount;
        if (supportSpell.supportType == SupportSpell.SupportEffect.SpeedBuff)
        {
            if (currentSpeedBuff > 0)
            {
            
                RemoveSpeedBuff(currentSpeedBuff);
                CooldownUIManager.Instance.RemoveCooldown(supportSpell.supportType.ToString());
                StopCoroutine(speedCO);
            }
            speed.AddModifier(modifier);
            currentSpeedBuff = modifier;
            speedCO = StartCoroutine(RemoveBuffAfterDuration(supportSpell, supportSpell.duration, modifier));
            CooldownUIManager.Instance.SetNewCoolDown(supportSpell, supportSpell.icon, supportSpell.duration);

        }

        if (supportSpell.supportType == SupportSpell.SupportEffect.ArmorBuff)
        {
            if (currentArmorBuff > 0)
            {

                RemoveArmorBuff(currentArmorBuff);
                CooldownUIManager.Instance.RemoveCooldown(supportSpell.supportType.ToString());
                StopCoroutine(armorCO);
            }
            armor.AddModifier(modifier);
            currentArmorBuff = modifier;
            armorCO = StartCoroutine(RemoveBuffAfterDuration(supportSpell, supportSpell.duration, modifier));
            CooldownUIManager.Instance.SetNewCoolDown(supportSpell, supportSpell.icon, supportSpell.duration);

        }

        if (supportSpell.supportType == SupportSpell.SupportEffect.DamageBuff)
        {
            if (currentDamageBuff > 0)
            {

                RemoveDamageBuff(currentDamageBuff);
                CooldownUIManager.Instance.RemoveCooldown(supportSpell.supportType.ToString());
                StopCoroutine(damageCO);
            }
            strength.AddModifier(modifier);
            intelligence.AddModifier(modifier);
            currentDamageBuff = modifier;
            damageCO = StartCoroutine(RemoveBuffAfterDuration(supportSpell, supportSpell.duration, modifier));
            CooldownUIManager.Instance.SetNewCoolDown(supportSpell, supportSpell.icon, supportSpell.duration);

        }


    }
    void RemoveSpeedBuff(int speed)
    {
        this.speed.RemoveModifier(speed);
        currentSpeedBuff = 0;
    }

    void RemoveArmorBuff(int armor)
    {

        this.armor.RemoveModifier(armor);
        currentArmorBuff = 0;
    }

    void RemoveDamageBuff(int damage)
    {

        this.strength.RemoveModifier(damage);
        this.intelligence.RemoveModifier(damage);
        currentDamageBuff = 0;
    }

    private IEnumerator RemoveBuffAfterDuration(SupportSpell supportSpell, float duration, int modifier)
    {
        yield return new WaitForSeconds(duration);


        if (supportSpell.supportType == SupportSpell.SupportEffect.SpeedBuff)
        {
            RemoveSpeedBuff(modifier);
        }
        else if (supportSpell.supportType == SupportSpell.SupportEffect.ArmorBuff)
        {
            RemoveArmorBuff(modifier);
        }
        else if (supportSpell.supportType == SupportSpell.SupportEffect.DamageBuff)
        {
            RemoveDamageBuff(modifier);
        }


    }


}