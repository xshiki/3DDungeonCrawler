using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells/Damage")]


public class SpellScriptableObject : InventoryItemData
{
    public enum Element {Fire, Ice, Earth, Wind, Normal};
    // Start is called before the first frame update

    public Element element = Element.Fire;
    public float ManaCost = 5f;
    public float LifeTime = 2f;
    public float Speed = 15f;
    public float DamageAmount = 10f;
    public float timeBetweenCast = 1f;
    public float SpellRadius = 0.5f;
    
}
