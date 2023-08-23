using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "Spells")]
public class SpellScriptableObject : InventoryItemData
{
    // Start is called before the first frame update
    public float ManaCost = 5f;
    public float LifeTime = 2f;
    public float Speed = 15f;
    public float DamageAmount = 10f;
    public float timeBetweenCast = 1f;
    public float SpellRadius = 0.5f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
