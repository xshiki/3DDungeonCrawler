using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Magic Weapon", menuName = "Create Items/Magic Weapons")]
public class MagicWeaponItemData : WeaponItemData
{
    [Header("Magic")]
    public Spell spellToCast;
    public SpellScriptableObject spellSO;


}
