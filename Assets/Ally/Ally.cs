using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;

public class Ally : MonoBehaviour
{
    public int maxHealth = 100;
    protected int health;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    
    public float waitTime;
    protected float damageResist = 1.0f;
    protected float speed = 1.0f;
    private bool canHeal = true;

    public Vector3 posA;
    public Vector3 posB;
    private Vector3 targetPos;

    public Weapon weapon;
    public Player player;

    private Vector3 beginPosition;
    private Quaternion beginRotation;

    private void healPlayer()
    {
        if (player != null)
            player.TakeDamage(-50);
            StartCoroutine(HealingCooldown());
    }

    private IEnumerator HealingCooldown()
    {
        canHeal = false;
        yield return new WaitForSeconds(10);
        canHeal = true;
    }

    public void Start()
    {
        beginPosition = transform.position;
        beginRotation = transform.rotation;
        targetPos = posA;
    }

    public void Update()
    {
       // tutaj jakąś mechanikę chodzenia trzeba zrobić ;-;

       if (player != null && player.Health < 40 && canHeal) {
           healPlayer();
       }
    }

    public bool Shoot()
    {
        return weapon.TryShoot();
    }

    public void TakeDamage(int damage)
    {
        health -= (int)(damage * damageResist);
        if (health <= 0) Die();
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
