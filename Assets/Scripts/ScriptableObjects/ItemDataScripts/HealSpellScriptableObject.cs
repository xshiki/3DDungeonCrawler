using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Heal", menuName = "Spells/Support/Heal")]
public class HealSpellScriptableObject : SupportSpellSO
{
    public float healAmount;
    public float healOverTime;
}
