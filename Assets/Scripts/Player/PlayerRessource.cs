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
        playerEquipment.OnEquipmentChanged += ChangeStats;
    }

    private void ChangeStats(object sender, PlayerEquipment.OnEquipChangedArgs e)
    {
        Debug.Log("change stats");

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
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

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
        if(currentHealth + healthAmount >= 100)
        {
            currentHealth = maxHealth;
        }
        else
        {
            currentHealth += healthAmount;
        }


        if (currentMana + manaAmount >= 100)
        {
            currentMana = maxMana;
        }
        else
        {
            currentMana += manaAmount;
        }

    }


}