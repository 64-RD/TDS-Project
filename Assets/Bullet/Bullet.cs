using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 25;
    public Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb.velocity = transform.up * speed;
        Destroy(gameObject, 2.0f);
    }

    private void OnTriggerEnter2D(Collider2D hit)
    {
        Enemy enemy = hit.GetComponent<Enemy>();
        GameObject enemyObject = GameObject.Find("Enemy");
        EnemyAI_0 tmp = enemyObject.GetComponent<EnemyAI_0>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
        Player player  = hit.GetComponent<Player>();
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
