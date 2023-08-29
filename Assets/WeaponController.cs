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
    [SerializeField] private float timeBetweenSwing = 0.25f;
    private bool swingingWeapon = false;

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
        playerOrientation = GameObject.Find("Orientation").transform;
        AttackRaycast();

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
        }
    }
}
