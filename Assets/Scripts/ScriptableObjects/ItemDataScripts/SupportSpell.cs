using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Support Spell", menuName = "Spells/Support")]
public class SupportSpell : SpellScriptableObject
{
   public enum SupportEffect {Heal, SpeedBuff, DamageBuff,ArmorBuff, Custom};
   public SupportEffect supportType = SupportEffect.Custom;
   public Sprite icon;
   //duration in minutes
   public float duration = 600f;

   public float amount;
   public float healOverTime;
}

