using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
//using Unity.MLAgents.Actuators;

using UnityEngine;

/// <summary>
/// A Enemy Machine Learning Agent
/// </summary>
public class EnemyAI_0 :Agent
{
    [Tooltip("Moving speed")]
    public float moveSpeed = 5;

    [Tooltip("Rotation speed")]
    public float rotationSpeed = 2f;


    [Tooltip("The agent's camera")]
    public Camera agentCamera;

    [Tooltip("Whether this is training mode or gameplay mode")]
    public bool trainingMode;

    private float smoothRotation = 0f;

    // The rigidbody of the agent
    private Rigidbody2D rigidbody;

    public Player player;
    public GameObject playerObject;
    public Transform firePoint;
    public GameObject bulletPrefab;
    private Enemy enemy;
    private Vector3  beginPosition;
    private Quaternion beginRotation;
    private bool frozen = false;

    public override void Initialize()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
  
        // If not training mode, no max step, play forever
        if (!trainingMode) MaxStep = 0;
        beginPosition = transform.position;
        beginRotation = transform.rotation;
    }

    public override void OnEpisodeBegin()
    {
        if (trainingMode)
        {
            player.respawn();
            transform.rotation = beginRotation;
            transform.position = beginPosition + new Vector3(UnityEngine.Random.Range(-5.0f,5.0f), UnityEngine.Random.Range(-5.0f, 5.0f), 0.0f);
        }

        
       

       
    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Don't take actions if frozen
        if (frozen) return;

        // Calculate movement vector
        Vector2 move = new Vector2(vectorAction[0], vectorAction[1]);

        // Add force in the direction of the move vector
        rigidbody.velocity = new Vector2(vectorAction[1] * moveSpeed, vectorAction[0] * moveSpeed);

        // Get the current rotation
        Vector3 rotationVector = transform.rotation.eulerAngles;


        // Calculate smooth rotation changes
        smoothRotation = Mathf.MoveTowards(smoothRotation, vectorAction[2], rotationSpeed * Time.fixedDeltaTime);

        transform.rotation = Quaternion.Euler(0, 0, rotationVector.z + smoothRotation);

        if(vectorAction[3]>0)
         enemy.Shoot();
        
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        

        // Observe the agent's local rotation (4 observations)
        sensor.AddObservation(transform.localRotation.normalized);

        // Get a vector from the beak tip to the nearest flower
        Vector3 toPlayer = player.transform.position - transform.position;
        sensor.AddObservation(toPlayer.normalized);

        float dist = Vector3.Distance(player.transform.position, transform.position);
        sensor.AddObservation(dist);

        // Observe a normalized vector pointing to the nearest flower (1 observations)
        //sensor.AddObservation(enemy.currentTime);

        sensor.AddObservation(Vector3.Dot(firePoint.up.normalized, toPlayer.normalized));



        // 9 total observations
    }

    public override void Heuristic(float[] actionsOut)
    {
        // Create placeholders for all movement/turning
        float forward=0;
        float left = 0;
        Vector3 up = Vector3.zero;
        float shoot = 0f;
        float yaw = 0f;

        // Convert keyboard inputs to movement and turning
        // All values should be between -1 and +1

        // Forward/backward
        if (Input.GetKey(KeyCode.W)) forward = 1f;
        else if (Input.GetKey(KeyCode.S)) forward = -1f;

        // Left/right
        if (Input.GetKey(KeyCode.A)) left = -1f;
        else if (Input.GetKey(KeyCode.D)) left = 1f;


        if (Input.GetKey(KeyCode.Q))
            shoot = 1.0f;
        else
            shoot = -1.0f;


        // Turn left/right
        if (Input.GetKey(KeyCode.LeftArrow)) yaw = -1f;
        else if (Input.GetKey(KeyCode.RightArrow)) yaw = 1f;

        // Combine the movement vectors and normalize
        //Vector2 combined = (forward + left).normalized;

        // Add the 3 movement values, pitch, and yaw to the actionsOut array
        actionsOut[0] = forward;
        actionsOut[1] = left;
        actionsOut[2] = yaw;
        actionsOut[3] = shoot;



    }
    public void FreezeAgent()
    {
        Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
        frozen = true;
        rigidbody.Sleep();
    }
    public void UnfreezeAgent()
    {
        Debug.Assert(trainingMode == false, "Freeze/Unfreeze not supported in training");
        frozen = false;
        rigidbody.WakeUp();
    }

    private void Update()
    {
        Debug.DrawLine(player.transform.position , transform.position, Color.green);
    }

    public void Positive()
    {
        AddReward(.1f );
    }
    public void Negative()
    {
        AddReward(-.01f);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(trainingMode)
            AddReward(-.1f);
    }
}