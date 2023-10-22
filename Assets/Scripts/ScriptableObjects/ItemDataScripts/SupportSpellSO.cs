using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Support Spell", menuName = "Spells/Support")]
public class SupportSpellSO : SpellScriptableObject
{
   public enum SupportType {Heal, SpeedBuff, DamageBuff,ArmorBuff, Custom};
   public SupportType supportType = SupportType.Custom;
   public Sprite icon;
   //duration in minutes
   public float duration = 600f;
}

