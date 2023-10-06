using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Spell : MonoBehaviour
{
    public SpellScriptableObject SpellToCast;
    private SphereCollider myCollider;
    private Rigidbody myRigidbody;

    float damageModifier = 1f;

    private void Awake()
    {
        myCollider = GetComponent<SphereCollider>();
        myCollider.isTrigger = true;
        myCollider.radius = SpellToCast.SpellRadius;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        Destroy(this.gameObject, SpellToCast.LifeTime);
    }

    private void Update()
    {
        if (SpellToCast.Speed > 0)
        {
            transform.Translate(Vector3.forward * SpellToCast.Speed * Time.deltaTime);

        }
    }

    public void ModifySpellDamage(float damage)
    {
        damageModifier = damage;
    }

    private void OnTriggerEnter(Collider other)
    {

        //Apply sound sfx, particle effects etc
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyManager>().TakeDamage((int)(SpellToCast.DamageAmount * damageModifier));
            Destroy(this.gameObject);
        }

        if (other.gameObject.tag != "Player" || other.gameObject.tag != "Enemy")
        {
            Destroy(this.gameObject);
        }

    }
}
