using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{

    public const string SWING = "Swing";
    // Start is called before the first frame update
    AudioSource audioSource;
    Animator weaponAnimator;
    public WeaponItemData weaponData;
    public PlayerController playerController;
    public PlayerEquipment playerEquipment;

    [SerializeField] private Transform playerOrientation;
    [SerializeField] private float timeBetweenSwing = 0.5f;
    private bool attacking = false;
    public bool isAttacking => attacking;

    public const string ATTACK1 = "Attack 1";
    public const string ATTACK2 = "Attack 2";

    private void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
        weaponAnimator = GetComponent<Animator>();
        playerController = FindObjectOfType<PlayerController>();
        playerEquipment = FindObjectOfType<PlayerEquipment>();
        playerOrientation = GameObject.Find("Orientation").transform;
    }



    public void swingWeapon()
    {

        Debug.Log("swinging WEapon");
        if(attacking) { return; }
        attacking = true;
        playerOrientation = GameObject.Find("Orientation").transform;
        Invoke(nameof(ResetAttack), timeBetweenSwing);
        AttackRaycast();
        audioSource.PlayOneShot(weaponData.weaponSwingSound);
        playerController.PlayAnimation(ATTACK1);
        playerController.PlayAnimation(ATTACK2);

    }

    void ResetAttack()
    {
        attacking = false;
     
    }
    public void AttackRaycast()
    {
        RaycastHit[] hits;

        hits = Physics.RaycastAll(playerOrientation.transform.position, playerOrientation.transform.forward,weaponData.WeaponRange);
        Debug.Log(hits);
        if (hits.Length > 0)
        {
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log("target found");
                HitTarget(hits[i]);
            }
        }

    }



    public void HitTarget(RaycastHit hit)
    {
        if (hit.collider.GetComponent<EnemyHealth>())
        {
            hit.collider.GetComponent<EnemyHealth>().TakeDamage(weaponData.DamageAmount);
            audioSource.PlayOneShot(weaponData.weaponHitSound);
        }
    }
}
