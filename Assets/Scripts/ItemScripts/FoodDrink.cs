using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class FoodDrink : MonoBehaviour
{
    [SerializeField] private float _hungerToReplenish, _thirstToReplenish;
    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger= true; 

    }
    private void Update()
    {
        transform.Rotate(transform.up  *20f  * Time.deltaTime); 
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        var survivalManager = other.gameObject.GetComponent<PlayerRessource>();
        if(survivalManager is null) return;

        //survivalManager.ReplenshingHungerThirst(_hungerToReplenish, _thirstToReplenish);
        Destroy(gameObject);
    }
}
