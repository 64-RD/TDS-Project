using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.MLAgents;

public class PlayerBehaviour_1 : MonoBehaviour
{
    public GameObject enemy;
    private Vector2 beginPosition;
    private Vector3 targetPos; //pozycja celu
    private Vector3 thisPos;
    public Player player;
    private Vector2 targetPosition;
    private Vector2 potentialPosition;
    private float angle;
    NavMeshAgent agent;



    // Start is called before the first frame update
    void Start()
    {
        beginPosition = transform.position;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        GoToRandomPosition();
        ShootAtEnemy();   //Odkomentuj je�eli gracz ma strzela� w bota

    }

    public void GoToRandomPosition()
    {
        if (Vector2.Distance(transform.position, targetPosition) < 0.5f)
        {
          //  while (true)
            //{

                targetPosition = enemy.transform.position;
                //if (Vector2.Distance(transform.position, targetPosition) > 5) 
                //{

                    agent.SetDestination(targetPosition);
                 //   break;
                //}

            //}

        }
    }

    private void ShootAtEnemy()
    {
        if (enemy != null)
        {
           // player.Shoot();
        }
        targetPos = enemy.transform.position;
        thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
    public void respawn()
    {
       
        player.isDie = false;
        player.Health = player.maxHealth;
        player.healthBar.SetHealth(player.maxHealth);
        float distance = 5.0f;//Academy.Instance.EnvironmentParameters.GetWithDefault("distanceToAgent", 5f);
        while (true)
        {
            potentialPosition = beginPosition + new Vector2(UnityEngine.Random.Range(-distance, distance), UnityEngine.Random.Range(-distance, distance));
            targetPosition = enemy.transform.localPosition;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(potentialPosition.x,potentialPosition.y), 0.5f);
            //Collider2D[] colliders2 = Physics2D.OverlapCircleAll(new Vector2(targetPosition.x,targetPosition.y), 0.5f);

            // Safe position has been found if no colliders are overlapped*/
            if (Vector3.Distance(potentialPosition, targetPosition) > 5  && colliders.Length == 0)
            {
                Debug.LogWarning(targetPosition);
                agent.SetDestination(targetPosition); 
                break;
            }
                
        }

        transform.position = potentialPosition;
    }


}