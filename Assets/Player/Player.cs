using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.Search;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int maxHealth = 200;
    public int health = 200;
    public HealthBar healthBar;
    public float moveSpeed;
    public Rigidbody2D rb;
    public Transform firePoint;
    public GameObject bulletPrefab;
    private Vector2 _moveDirection;
    private Vector3 _mousePos;
    [SerializeField] public FieldOfView FOV;
    public float FOV_angle;
    public float FOV_distance;
    public bool isDie=false;
    private Vector3 beginPosition;

    void Start()
    {
        beginPosition = transform.position;
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
        if (FOV != null)
        {
            FOV.SetAimDirection(this.transform.up);
            FOV.SetOrigin(transform.position);
            FOV.SetViewDistance(FOV_distance);
            FOV.SetFoV(FOV_angle);
        }
    }

    private void FixedUpdate()
    {
       //Move();
    }



    void ProcessInputs()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");
        _moveDirection = new Vector2(moveX, moveY).normalized;
        _mousePos = Input.mousePosition;
        _mousePos.z = 5.23f;
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Move()
    {
        rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        Vector3 objectPos = Camera.main.WorldToScreenPoint (transform.position);
        _mousePos.x -= objectPos.x;
        _mousePos.y -= objectPos.y;
 
        float angle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
    }

    void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        health = health <= 0 ? 0 : health;
        healthBar.SetHealth(health);

        if(health == 0)
            Die();
    }

    private void Die()
    {
        isDie = true;

        //Destroy(gameObject);
    }

    public void respawn()
    {
        Vector3 potentialPosition;
        isDie = false;
        health = 200;
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
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.GetComponent<Bullet>())
        {
            Color color = collision.gameObject.GetComponent<SpriteRenderer>().material.color;
            color = new Color(color.r, color.g, color.b, 0.5f);
            Component[] bushlist;
            bushlist = collision.transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer bush in bushlist)
            {
                bush.material.color = color;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Color color = collision.gameObject.GetComponent<SpriteRenderer>().material.color;
        color = new Color(color.r, color.g, color.b, 0.5f);
        Component[] bushlist;
        bushlist = collision.transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer bush in bushlist)
        {
            bush.material.color = color;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.GetComponent<Bullet>())
        {
            Color color = collision.gameObject.GetComponent<SpriteRenderer>().material.color;
            color = new Color(color.r, color.g, color.b, 1f);

            Component[] bushlist;
            bushlist = collision.transform.parent.gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer bush in bushlist)
            {
                bush.material.color = color;
            }
        }
    }
}
