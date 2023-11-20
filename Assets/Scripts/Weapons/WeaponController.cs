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

    public const string DAGGER1 = "Dagger 1";
    public const string DAGGER2 = "Dagger 2";

    public const string POLEARM1 = "PoleArm 1";
    public const string POLEARM2 = "PoleArm 2";



    float timeBetweenSwing = 2.5f;
    int attackCount = 0;
    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
        weaponAnimator = GetComponent<Animator>();

        if (weaponData == null)
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

          
            if(weaponData.WeaponType == WeaponItemData.Weapons.Daggers)
            {
                playerController.PlayAnimation(DAGGER1);
            }else if(weaponData.WeaponType == WeaponItemData.Weapons.Polearm)
            {
                playerController.PlayAnimation(POLEARM1);
            }
            else
            {
                playerController.PlayAnimation(ATTACK1);
            }
            attackCount++;
        }
        else
        {

            if (weaponData.WeaponType == WeaponItemData.Weapons.Daggers)
            {
                playerController.PlayAnimation(DAGGER2);
            }
            else if (weaponData.WeaponType == WeaponItemData.Weapons.Polearm)
            {
                playerController.PlayAnimation(POLEARM2);
            }
            else
            {
                playerController.PlayAnimation(ATTACK2);
            }
            attackCount = 0;
        }
       
    }

    void ResetAttack()
    {
        attacking = false;
     
    }
    public void AttackRaycast()
    {

       
        RaycastHit[] hits;

        hits = Physics.RaycastAll(playerOrientation.transform.position, playerOrientation.transform.forward, weaponData.WeaponRange);
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
            float damage = 0;
            float damageModifier = (1f + ((float)playerRessource.strength.GetValue() / 100f));
            damage = (float)weaponData.DamageAmount * damageModifier;
            if (IsCriticalHit())
            {
                damage *= weaponData.critDamageMultiplier;
                Debug.Log("crit");
            }

            if (weaponData.lifeStealChance > 0f)
            {
                float lifestealAmount = CalculateLifesteal(damage);
                playerRessource.ReplenshHealthMana(lifestealAmount, 0);
                hit.collider.GetComponent<EnemyManager>().TakeDamage(Mathf.RoundToInt(damage + lifestealAmount));
            }
            else
            {
                hit.collider.GetComponent<EnemyManager>().TakeDamage((int)damage);
            }
       
            audioSource.PlayOneShot(weaponData.weaponHitSound);
            if(weaponData.hitEffect != null)
            {
                GameObject GO = Instantiate(weaponData.hitEffect);
                Destroy(GO, 20);
                Debug.Log("hiteffect created");
            }
           
        }
    }

    float CalculateLifesteal(float damage)
    {
        return damage * weaponData.lifeStealPercentage;
    }

    bool IsLifeStealHit()
    {
        return Random.value < weaponData.lifeStealChance;
    }
    bool IsCriticalHit()
    {
        return Random.value < weaponData.critRate;
    }
}
