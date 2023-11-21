using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemToolTip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler

{
    public GameObject itemToolTipPanel;
    public Vector3 toolTipOffset;
    public RectTransform popUpObject;
    
    enum StatType { armor,stamina, strength, intelligence, speed};

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryItem hoveredItem = eventData.pointerEnter.GetComponent<InventoryItem>();
        if (hoveredItem != null)
            {
                itemToolTipPanel.SetActive(true);
                itemToolTipPanel.transform.position = hoveredItem.transform.position + toolTipOffset;
                var tooltip = itemToolTipPanel.GetComponent<ItemToolTipDescription>();
                tooltip.SetItemName(hoveredItem.item.DisplayName);
                string description = hoveredItem.item.Description + "\n";
                if (hoveredItem.item is ArmorItemData)
                {
                 description += SetDescriptionArmor(hoveredItem.item as ArmorItemData);

                }else if(hoveredItem.item is WeaponItemData)
                {
                description += SetDescriptionWeapon(hoveredItem.item as WeaponItemData);
                }else if(hoveredItem.item is ConsumableItemData)
                { 
                    var consum = hoveredItem.item as ConsumableItemData;
                    if(consum.supportEffect != null)
                {
                    description += SetDescriptionEffect(consum.supportEffect);
                }
                }

                if(hoveredItem.maxStacks > 1)
                {
                    description += "Stack Size: "+ hoveredItem.maxStacks.ToString() + "\n";
                }

                
                tooltip.SetItemDescription(description);
                itemToolTipPanel.SetActive(true);
                LayoutRebuilder.ForceRebuildLayoutImmediate(popUpObject);
                

            
        }



    }

    string SetDescriptionWeapon(WeaponItemData weaponItem)
    {

        string description = "";
        description += weaponItem.WeaponType.ToString() + "\n";

        if(weaponItem is not MagicWeaponItemData)
        {
            description += weaponItem.DamageAmount.ToString() + " Base Damage\n";



        }
        else
        {
            MagicWeaponItemData magicItem = weaponItem as MagicWeaponItemData;
            description += "Element: " + magicItem.spellSO.element.ToString() + "\n";
            description += magicItem.spellSO.DamageAmount.ToString() + " Damage\n";
            description += "Mana Cost: " + magicItem.spellSO.ManaCost.ToString() + "\n";


        }
        if(weaponItem.lifeStealChance > 0f)
        {
            //description += (Mathf.RoundToInt(weaponItem.lifeStealChance * 100)).ToString()+"% chance to steal life from enemy and give "+ (Mathf.RoundToInt(weaponItem.lifeStealPercentage * 100)).ToString() + "% of the damage dealt as HP to the wielder"+ "\n";
            description += "Lifesteal: Give "+ (Mathf.RoundToInt(weaponItem.lifeStealPercentage * 100)).ToString() + "% of the damage dealt as healthpoints to the wielder."+ "\n";
        }

        description += "\n";
        if(weaponItem.critRate > 0f && weaponItem.critDamageMultiplier > 1f)
        {
            description += "Critical strike: "+ Mathf.RoundToInt(weaponItem.critRate * 100) + "% chance to multiply damage by " + weaponItem.critDamageMultiplier + " times. " + "\n";
        }

        return description;
    }
    string SetDescriptionArmor(ArmorItemData armorItem)
    {

        string description = "";
      

        Dictionary<string, int> stats = new Dictionary<string, int>
                {
                    { "Armor", armorItem.armor },
                    { "Stamina", armorItem.stamina },
                    { "Strength", armorItem.strength },
                    { "Intelligence", armorItem.intelligence },
                    { "Speed", armorItem.speed }
                };

        foreach (var stat in stats)
        {
            if (stat.Value > 0)
            {
                description += $"+ {stat.Value} {stat.Key}\n";
            }
        }


        return description;
    }

    string SetDescriptionEffect(SupportSpell effect)
    {
        string description = "";

        description += "Use: Increases " + effect.supportType.ToString().Replace("Buff","") + " by " + effect.amount + " for " + effect.duration + " seconds. \n";


        return description;
    }
    public void OnPointerExit(PointerEventData eventData)
    {

        itemToolTipPanel.SetActive(false);
    }
}
