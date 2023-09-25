using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEngine;

public class PlayerRessource : MonoBehaviour
{

    [Header("Health")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float healthPercent => currentHealth / maxHealth;




    [Header("Mana")]
    public float maxMana = 100; // Maximum ressource of the player
    public float currentMana; // Current ressource of the player
    public float currentManaPercent => currentMana / maxMana;
    [SerializeField] private float manaRechargeRate = 2f;
    [SerializeField] private float manaRechargeDelay = 1f;
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


    private void Awake()
    {
     
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
        if(currentHealth <= 0) {
            Die();
        }

    }

    void Die()
    {
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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


        if(currentHealth <= 0)
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






}