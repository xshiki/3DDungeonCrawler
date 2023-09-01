using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Weapon", menuName = "MagicWeapons")]
public class MagicWeaponItemData : WeaponItemData
{
    [Header("Magic")]
    public Spell spellToCast;
}
