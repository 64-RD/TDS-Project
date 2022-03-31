using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class Enemy : MonoBehaviour
{

    public int maxHealth = 100;
    protected int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    
    //public float waitTime;
    protected float damageResist = 1.0f;
    protected float speed = 1.0f;

    public int totalDamage=0;

    public Weapon weapon;
    //public GameObject player; // gracz , z pocz�tku on jest celem

    private float currentTime = 0;
    public bool isDie = false;
    //public Agent agent;

    private Vector3 targetPos; //pozycja celu
    private Vector3 thisPos;
    private float angle;

    private Vector3 beginPosition;
    private Quaternion beginRotation;

    void Awake()
    {
        //player = GameObject.FindWithTag("Player");
        beginPosition = transform.position;
        beginRotation = transform.rotation;
    }

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

    public bool Shoot()
    {
        return weapon.TryShoot();
    }

    public void respawn()
    {
        Vector3 potentialPosition;
        isDie = false;
        health = maxHealth;
        totalDamage = 0;


        while (true)
        {
            potentialPosition = beginPosition + new Vector3(UnityEngine.Random.Range(-3.0f, 3.0f), UnityEngine.Random.Range(-3.0f, 3.0f), 0.0f);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(potentialPosition.x,potentialPosition.y), 0.5f);

            // Safe position has been found if no colliders are overlapped
            if (colliders.Length == 0)
                break;
        }
        transform.rotation = beginRotation;
        transform.position = potentialPosition;


    }
    public void TakeDamage(int damage)
    {
        health -= (int)(damage * damageResist);
        if (health <= 0) Die();
    }

    private void Die()
    {
        isDie = true;
        //Destroy(gameObject);
    }
}
