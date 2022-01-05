using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public int health = 100;
    public GameObject bulletPrefab;
    public GameObject player; // gracz , z pocz�tku on jest celem
    public Transform firePoint;
    public float waitTime;

  
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

    public void Update()
    {
        
       /* targetPos = player.transform.position;
        thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle -90));*/

        /*if(currentTime>=waitTime)
        {
            //Shoot();
            currentTime = 0;
        }
        else
        {
            currentTime += 1 * Time.deltaTime;
        }*/

        currentTime += 1 * Time.deltaTime;
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
        health -= damage;
        if (health <= 0) Die();
    }

    private void Die()
    {
         isDie = true;
    //Destroy(gameObject);
}

    public void Shoot()
    {
        if (currentTime >= waitTime)
        {
            currentTime = 0;
            //firePoint.transform.Rotate(0,0, Random.Range(-10.0f, 10.0f));
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        }
    } 
}
