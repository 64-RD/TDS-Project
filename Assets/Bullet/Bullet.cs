using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 20f;
    private int damage = 25;
    public Rigidbody2D rb;

    public void initBullet(float speed, int damage)
    {
        this.speed = speed;
        this.damage = damage;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * speed;
        Destroy(gameObject, 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        Enemy enemy = hit.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            return;
        }

        Player player = hit.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
            return;
        }

        Bullet bullet = hit.GetComponent<Bullet>();
        if (bullet == null)
        {
            Destroy(gameObject);
        }
    }
}
