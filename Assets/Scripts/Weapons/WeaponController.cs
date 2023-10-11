using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{

    public const string SWING = "Swing";
    // Start is called before the first frame update
    AudioSource audioSource;
    public Animator weaponAnimator;
    public WeaponItemData weaponData;

    public PlayerController playerController;
    public PlayerEquipment playerEquipment;
    public PlayerRessource playerRessource;

    [SerializeField] private Transform playerOrientation;

    private bool attacking = false;
    public bool isAttacking => attacking;

    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";



    float timeBetweenSwing = 2.5f;
    int attackCount = 0;
    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
        weaponAnimator = GetComponent<Animator>();
        if(weaponData == null)
        {
            weaponData = GetComponent<ItemDataProvider>().Item as WeaponItemData;
            timeBetweenSwing = weaponData.timeBetweenSwing;
        }
        playerController = FindObjectOfType<PlayerController>();
        playerEquipment = FindObjectOfType<PlayerEquipment>();
        playerOrientation = GameObject.Find("Orientation").transform;
        playerRessource = FindAnyObjectByType<PlayerRessource>();
    }



    public void swingWeapon()
    {

        Debug.Log("swinging WEapon");
        if(attacking) { return; }
        attacking = true;
        Invoke(nameof(ResetAttack), timeBetweenSwing);
        AttackRaycast();
        audioSource.PlayOneShot(weaponData.weaponSwingSound);


        if (attackCount == 0)
        {
            playerController.PlayAnimation(ATTACK1);
            attackCount++;
        }
        else
        {
            playerController.PlayAnimation(ATTACK2);
            attackCount = 0;
        }
       
      
        //weaponAnimator.Play(ATTACK1);

    }

    void ResetAttack()
    {
        attacking = false;
     
    }
    public void AttackRaycast()
    {

       
        RaycastHit[] hits;

        hits = Physics.RaycastAll(playerOrientation.transform.position, playerOrientation.transform.forward,2);
        Debug.Log(hits);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log("target found");
                HitTarget(hits[i]);
            }
        }
       


        // Assuming you have a GameObject with BoxCollider components as the weapon's attack hitboxes
        // Adjust this part according to your setup

        /*
        BoxCollider[] hitboxes = this.GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider hitbox in hitboxes)
        {
            // Perform overlap check with the hitbox
            Collider[] colliders = Physics.OverlapBox(hitbox.bounds.center, hitbox.bounds.extents, Quaternion.identity);

            foreach (Collider collider in colliders)
            {
                if (collider.CompareTag("Enemy"))
                {
                    // Assuming you have a script on the enemy objects that handles damage
                    EnemyManager enemyHealth = collider.GetComponent<EnemyManager>();
                    enemyHealth.TakeDamage(weaponData.DamageAmount); // Call a method on the enemy to apply damage
                    
                }
            }
        }
        */
    }



    public void HitTarget(RaycastHit hit)
    {
        if (hit.collider.GetComponent<EnemyManager>())
        {
            int damage = 0;
            damage = weaponData.DamageAmount * (1 + playerRessource.strength.GetValue() / 100);
            hit.collider.GetComponent<EnemyManager>().TakeDamage(damage);
            audioSource.PlayOneShot(weaponData.weaponHitSound);
            if(weaponData.hitEffect != null)
            {
                GameObject GO = Instantiate(weaponData.hitEffect);
                Destroy(GO, 20);
                Debug.Log("hiteffect created");
            }
           
        }
    }
}
