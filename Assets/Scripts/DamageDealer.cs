using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{

    [SerializeField] private int damage = 10;

    public int getDamage()
    {
        return damage;
    }

    public void destroyProjectile()
    {
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Laser")
            Destroy(other.gameObject);
    }
}
