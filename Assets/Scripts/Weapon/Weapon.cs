using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{   

    public WeaponItemData weaponItemData;
    private BoxCollider myCollider;
    private Rigidbody myRigidbody;

    // Start is called before the first frame update
    private void Awake()
    {
        myCollider = GetComponent<BoxCollider>();
        myCollider.isTrigger = true;

        myRigidbody = GetComponent<Rigidbody>();
        myRigidbody.isKinematic = true;

        //Destroy(this.gameObject, WeaponItemData.LifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    private void OnTriggerEnter(Collider other)
    {

        //Apply sound sfx, particle effects etc
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage((int)weaponItemData.DamageAmount);
            Destroy(this.gameObject);
        }

    }
}
