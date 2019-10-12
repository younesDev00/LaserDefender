using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //[Header("Player"]
    [SerializeField] float playerSpeed = 10f, rotateSpeed = 10f, padding = 0.6f, projectileSpeed = 10f,
    secondsOfFire = 0.1f, health = 1000f, destroyTime = 1.0f;
    [SerializeField] GameObject laserPrefab, explosion;
    [SerializeField] AudioClip deathSound, shootShound;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.75f, playerShootSound = 0.25f;
    SceneLoader scene;
    //Quaternion angle; // no longer needed

    private float XMin, XMax, YMin, YMax;
    private Coroutine firingCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        scene = FindObjectOfType<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        SetupMovement();
        Fire();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.GetComponent<DamageDealer>();
        //if (other.gameObject.tag == "EnemyLaser")
        if (!damageDealer) { return; }
            ProcessHit(damageDealer);
    }

    private void ProcessHit(DamageDealer damageDealer)
    {
        health -= damageDealer.getDamage();
        damageDealer.destroyProjectile();
        if (health <= 0  )
        {
            Die();
            AudioSource.PlayClipAtPoint(deathSound, Camera.main.transform.position, deathSoundVolume);
        }
    }

    private void Die()
    {
        GameObject explosionParent = Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(explosionParent, destroyTime);
        scene.PlayerDied();
        Destroy(gameObject);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        XMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x + padding;
        XMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        YMin = gameCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).y + padding;
        YMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;

    }

    private void SetupMovement()
    {
        //angle = transform.rotation;
        MovePlayer("Horizontal", "Vertical");//keyboard
        MovePlayer("Mouse X", "Mouse Y");//mouse
        RotatePlayer();//keyboard rotation
    }

    private void RotatePlayer()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
            //Quaternion angle = transform.rotation;// like transform.translate simple version of rotation
            // float z = angle.eulerAngles.z;
            // z += rotateSpeed * Time.deltaTime;
            // angle = Quaternion.Euler(0, 0, z);
            // transform.rotation = angle;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, 0, -rotateSpeed * Time.deltaTime);
            //Quaternion angle = transform.rotation;
            // float z = angle.eulerAngles.z;
            // z -=  rotateSpeed * Time.deltaTime;
            // angle = Quaternion.Euler(0, 0, z);
            // transform.rotation = angle;
        }
        // if(Input.GetKey(KeyCode.T)) //Testing purposes
        // {
            
        // }
    }

    private void MovePlayer(String XAis , String YAxis)
    {
        var deltaX = Input.GetAxis(XAis) * Time.deltaTime * playerSpeed; // Time.deltaTime makes it independent of time
        var deltaY = Input.GetAxis(YAxis) * Time.deltaTime * playerSpeed; // Time.deltaTime makes it independent of time
        transform.Translate(deltaX, deltaY, 0);
        //var newXPos = transform.position.x + deltaX;
        //var newYPos = transform.position.y + deltaY;
        //var newXPos = Mathf.Clamp(deltaX, XMin, XMax);
        //var newYPos = Mathf.Clamp(deltaY, YMin, YMax);

        Vector3 clampedPos = transform.position;
        clampedPos.y = Mathf.Clamp(clampedPos.y, YMin, YMax);
        clampedPos.x = Mathf.Clamp(clampedPos.x, XMin, XMax);
        transform.position = clampedPos;

        //transform.position = new Vector3(newXPos, newYPos, angle.z);
    }

    private void Fire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (firingCoroutine != null)
                StopCoroutine(firingCoroutine);
            firingCoroutine = StartCoroutine(FireContinuously());

        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(firingCoroutine);
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true) // needed to loop back the code,
        {
            GameObject laser = Instantiate(laserPrefab, transform.position , transform.rotation) as GameObject;
            //laser.GetComponent<Rigidbody2D>().velocity = new Vector3(-transform.rotation.z * Mathf.Rad2Deg, projectileSpeed, 0);
            AudioSource.PlayClipAtPoint(shootShound, Camera.main.transform.position, playerShootSound);
            yield return new WaitForSeconds(secondsOfFire);
        }
    }

    public float GetHealth()
    {
        return health;
    }
}
