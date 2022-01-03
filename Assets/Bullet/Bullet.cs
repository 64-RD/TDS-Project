using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    private float speed = 20f;
    private int damage = 25;
    public int constDamage = 25;
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
        int damage = constDamage + Random.Range(-10, 10);
        Enemy enemy = hit.GetComponent<Enemy>();
        GameObject enemyObject = GameObject.Find("Enemy");
        EnemyAI_0 tmp = enemyObject.GetComponent<EnemyAI_0>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            return;
        }

        Player player = hit.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
            
            tmp.Positive();
        }
        tmp.Negative();

        if(!hit.isTrigger)
            Destroy(gameObject);
            
    }
}
