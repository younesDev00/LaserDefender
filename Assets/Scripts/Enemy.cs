using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 100, minTimeBetweenShots = 0.3f, maxTimeBetweenShots = 3f,
    shotCounter, projectileSpeed = 2.0f, destroyTime = 1.0f;
    [SerializeField] GameObject explosion, enemyLaser;
    [SerializeField] AudioClip deathSound, shootShound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f, enemyShootSound = 0.25f;
    [SerializeField] private int score = 100;




    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
    }

    // Update is called once per frame
    void Update()
    {
        CountDownAndShoot();
    }

    private void CountDownAndShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0.0f)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        GameObject laser = Instantiate(enemyLaser, transform.position, Quaternion.identity) as GameObject;
        laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(shootShound, Camera.main.transform.position, enemyShootSound);

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();

        //if(other.gameObject.tag == "Player") Layers used instead
        if (!damageDealer) { return; }
            ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.getDamage();
        damageDealer.destroyProjectile();
        if (health <= 0)
        {
            Die();
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        }
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(score);
        GameObject explosionParent = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosionParent, destroyTime);
        Destroy(gameObject);
    }
}
