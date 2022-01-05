using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject enemy;
    private Vector3 beginPosition;
    private Vector3 targetPos; //pozycja celu
    private Vector3 thisPos;
    public Player player;
    
    private float angle;
    //NavMeshAgent agent;
    // Start is called before the first frame update
    void Start()
    {
        beginPosition = transform.position;
        //agent = GetComponent<NavMeshAgent>();
        //agent.updateRotation = false;
        //agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void AI()
    {
        /*if (Vector3.Distance(transform.position, enemy.transform.position)>2f)
            agent.SetDestination(enemy.transform.position);
        else
            agent.SetDestination(transform.position);*/
        targetPos = enemy.transform.position;
        thisPos = transform.position;
        targetPos.x = targetPos.x - thisPos.x;
        targetPos.y = targetPos.y - thisPos.y;
        angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        

    }
    public void respawn()
    {
        Vector3 potentialPosition;
        player.isDie = false;
        player.Health = player.maxHealth;
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


}
