using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health = 100;
    public GameObject bulletPrefab;
    public GameObject player; // gracz , z pocz¹tku on jest celem
    public Transform firePoint;
    public float waitTime;

  
    private Vector3 targetPos; //pozycja celu
    private Vector3 thisPos;
    private float angle;
    private float currentTime=0;

    public void Start()
    {
        //player = GameObject.FindWithTag("Player");
    }

    public void Update()
    {
        
        targetPos = player.transform.position;
        thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle -90));

        if(currentTime>=waitTime)
        {
            Shoot();
            currentTime = 0;
        }
        else
        {
            currentTime += 1 * Time.deltaTime;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void Shoot()
    {
        
        firePoint.transform.Rotate(0,0, Random.Range(-10.0f, 10.0f));
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation );
    } 
}
