using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected float health = 100f;
    public Weapon weapon;
    public GameObject player; // gracz, z początku on jest celem
    protected float speed = 1.0f;
    protected float damageResist = 1.0f;

    public void Update()
    {
       /* Vector3 targetPos = player.transform.position;
        Vector3 thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        if (Input.GetKey(KeyCode.Space))
        {
            Shoot();
        }*/
    }

    public void Shoot()
    {
        weapon.TryShoot();
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
