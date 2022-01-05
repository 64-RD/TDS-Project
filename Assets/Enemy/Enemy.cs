using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    protected float health = 100;
    public float waitTime;
    protected float damageResist = 1.0f;
    protected float speed = 1.0f;
    protected Weapon weapon;
    public GameObject bulletPrefab;
    public GameObject player; // gracz , z pocz�tku on jest celem
    public Transform firePoint;

  
    private Vector3 targetPos; //pozycja celu
    private Vector3 thisPos;
    private float angle;
    public float currentTime=0;
    public bool isDie = false;

    private Vector3 beginPosition;
    private Quaternion beginRotation;

    public void Start()
    {
        //player = GameObject.FindWithTag("Player");
        beginPosition = transform.position;
        beginRotation = transform.rotation;
    }
    protected float health = 100f;
    public Weapon weapon;
    public GameObject player; // gracz, z początku on jest celem
    protected float speed = 1.0f;
    protected float damageResist = 1.0f;

    public void Update()
    {
       /* 
        Vector3 targetPos = player.transform.position;
        Vector3 thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }
        */
    }

    public void Shoot()
    {
        weapon.TryShoot();
    }

    public void respawn()
    {
        Vector3 potentialPosition;
        isDie = false;
        health = 100;
        while (true)
        {
            potentialPosition = beginPosition + new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), UnityEngine.Random.Range(-10.0f, 10.0f), 0.0f);
            Collider[] colliders = Physics.OverlapSphere(potentialPosition, 0.9f);

            // Safe position has been found if no colliders are overlapped
            if (colliders.Length == 0)
                break;
        }

        transform.position = potentialPosition;


    }
    public void TakeDamage(int damage)
    {
        health -= (damage * damageResist);
        if (health <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
