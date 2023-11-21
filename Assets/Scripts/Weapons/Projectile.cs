using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Projectile: MonoBehaviour
{
    public SpellScriptableObject SpellToCast;
    private SphereCollider myCollider;
    private Rigidbody myRigidbody;
    public Transform enemyTransform;
    public int damage;
private void Awake()
{
    myCollider = GetComponent<SphereCollider>();
    myCollider.isTrigger = true;
    myCollider.radius = SpellToCast.SpellRadius;

    myRigidbody = GetComponent<Rigidbody>();
    myRigidbody.isKinematic = true;
        damage = (int)SpellToCast.DamageAmount;
    Destroy(this.gameObject, SpellToCast.LifeTime);
}

private void Update()
{
    if (SpellToCast.Speed > 0)
    {
        transform.Translate(Vector3.forward * SpellToCast.Speed * Time.deltaTime);

    }
}

   public void SetDamage(int damage)
    {
        this.damage = damage;
    }

private void OnTriggerEnter(Collider other)
{

    //Apply sound sfx, particle effects etc
    if (other.gameObject.CompareTag("Player"))
    {
        other.GetComponent<PlayerRessource>().TakeDamage(damage);
        Destroy(this.gameObject);
    }
        if (other.gameObject.layer == 11 || other.gameObject.layer == 12 || other.gameObject.layer == 13)
        {
            Debug.Log("dadas");
            Destroy(this.gameObject);
        }


}
}
