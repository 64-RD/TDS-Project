using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;
using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using Unity.Barracuda;
using TMPro;

/// <summary>
/// A Enemy Machine Learning Agent
/// </summary>
public class EnemyAI_1 : Agent
{
    [Tooltip("Moving speed")]
    public float moveSpeed;

    [Tooltip("Rotation speed")]
    public float rotationSpeed;



    [Tooltip("Whether this is training mode or gameplay mode")]
    public bool trainingMode;
    public bool record=false;
    public bool extModel = false;

    private float smoothRotation = 0f;

    // The rigidbody of the agent
    private Rigidbody2D rigidbody;

    public Player player;
    public PlayerBehaviour_1 playerBehaviour1;
    private Weapon weapon;
    private Enemy enemy;
    private bool frozen = false;
   
    private int lastTotalDamage;
    private int lastHealth;

    private Vector2 _moveDirection;
    private Vector3 _mousePos;

    public NNModel model;
    private Model runtimeModel;
    private IWorker worker;
    private string output1;
    private string output2;
    private string output3;
    private string output4;
    private string output5;
    IList<float[]> input2D = new List<float[]>();


    public override void Initialize()
    {
        float[] tmp = new float[737];
        for (int i = 0; i < 10; i++)
            input2D.Add(tmp);
            

        runtimeModel = ModelLoader.Load(model);
        worker = WorkerFactory.CreateWorker(WorkerFactory.Type.Auto, runtimeModel);
        output5 = runtimeModel.outputs[runtimeModel.outputs.Count - 1];
        output4 = runtimeModel.outputs[runtimeModel.outputs.Count - 2];
        output3 = runtimeModel.outputs[runtimeModel.outputs.Count - 3];
        output2 = runtimeModel.outputs[runtimeModel.outputs.Count - 4];
        output1 = runtimeModel.outputs[runtimeModel.outputs.Count - 5];

        rigidbody = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        weapon = enemy.weapon;
        lastTotalDamage = enemy.totalDamage;
        lastHealth = enemy.Health;
        // If not training mode, no max step, play forever
        if (!trainingMode) MaxStep = 0;

        //Prediction.loadModel();


    }

    public override void OnEpisodeBegin()
    {
        if (trainingMode)
        {
            
            playerBehaviour1.respawn();
            enemy.respawn();
            lastTotalDamage = enemy.totalDamage;
            lastHealth = enemy.Health;
        }
        //if (record) save_data();

    }

   

    public override void OnActionReceived(float[] vectorAction)
    {
        // Don't take actions if frozen
        if (frozen) return;

        if (record)
        {
            
        }


        // Calculate movement vector mapowanie z 0,1,2 na -1,0,1
        float moveX = 0f;
        float moveY = 0f;
        float rotation = 0f;

        switch (vectorAction[1])
        {
            case 0: moveX = -1f; break;
            case 1: moveX = 0f; break;
            case 2: moveX = 1f; break;

        }
        switch (vectorAction[0])
        {
            case 0: moveY = -1f; break;
            case 1: moveY = 0f; break;
            case 2: moveY = 1f; break;

        }
        switch (vectorAction[2])
        {
            case 0: rotation = -1f; break;
            case 1: rotation = 0f; break;
            case 2: rotation = 1f; break;

        }

        // Add force in the direction of the move vector
        Vector2 direction = new Vector2(moveX, moveY).normalized;
        rigidbody.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

        // Get the current rotation
        Vector3 rotationVector = transform.rotation.eulerAngles;


        // Calculate smooth rotation changes
        //smoothRotation = Mathf.MoveTowards(smoothRotation, vectorAction[2]-1, rotationSpeed * Time.fixedDeltaTime);
        smoothRotation = rotation * rotationSpeed * Time.fixedDeltaTime;
        transform.rotation = Quaternion.Euler(0, 0, rotationVector.z + smoothRotation);

        if (vectorAction[3] == 1)
            if (enemy.Shoot())
                AddReward(-0.01f);


        if (trainingMode)
            AddReward(-1f / MaxStep);

    }

    public override void CollectObservations(VectorSensor sensor)
    {

        //Total 17 observations
        // Observe the agent's local rotation (4 observations)
        sensor.AddObservation(transform.localRotation.normalized);

        //3 observations
        sensor.AddObservation(transform.position);
        sensor.AddObservation(player.Health);
        sensor.AddObservation(enemy.Health);
        sensor.AddObservation(enemy.weapon.bulletsLeft);
        /*sensor.AddObservation(Vector3.zero);
        sensor.AddObservation(Vector3.zero);
        sensor.AddObservation(0.0f);*/

       /* sensor.AddObservation(player.transform.position);
        sensor.AddObservation(player.GetComponent<Rigidbody2D>().velocity);
        Vector3 toPlayer = player.transform.position - transform.position;
        Vector2 toPlayer2D = new Vector2(toPlayer.x, toPlayer.y);
        sensor.AddObservation(Vector2.Dot(toPlayer2D.normalized, new Vector2(transform.right.x, transform.right.y).normalized));*/
        //Debug.Log(Vector2.Dot(toPlayer2D.normalized, new Vector2(transform.right.x, transform.right.y).normalized));
    }

    public override void Heuristic(float[] actionsOut)
    {
        // Create placeholders for all movement/turning
        if (!extModel)
        {
            float forward = 1f;
            float left = 1f;
            Vector3 up = Vector3.zero;
            float shoot = 0f;
            float yaw = 1f;

            // Convert keyboard inputs to movement and turning
            // All values should be between -1 and +1

            // Forward/backward
            if (Input.GetKey(KeyCode.W)) forward = 2f;
            else if (Input.GetKey(KeyCode.S)) forward = 0f;

            // Left/right
            if (Input.GetKey(KeyCode.A)) left = 0f;
            else if (Input.GetKey(KeyCode.D)) left = 2f;


            if (Input.GetKey(KeyCode.Space))
            {
                shoot = 1f;
            }

            if (Input.GetKey(KeyCode.LeftArrow)) yaw = 0f;
            else if (Input.GetKey(KeyCode.RightArrow)) yaw = 2f;

            /*_mousePos = Input.mousePosition;
            _mousePos.z = 5.23f;
            Vector3 objectPos = Camera.main.WorldToScreenPoint(this.transform.position);
            _mousePos.x -= objectPos.x;
            _mousePos.y -= objectPos.y;

            float angle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
            angle = (angle - 360);
           
            while (true)
                if (transform.rotation.eulerAngles.z - angle >= -180 && transform.rotation.eulerAngles.z - angle <= 180)
                    break;
                else
                    angle = (angle + 360);

            if (transform.rotation.eulerAngles.z -angle >1 )
            {
                yaw = 0f;
            }
            else if (transform.rotation.eulerAngles.z - angle <- 1)
                yaw = 2f;
            else
                yaw = 1f;*/



            actionsOut[0] = forward;
            actionsOut[1] = left;
            actionsOut[2] = yaw;
            actionsOut[3] = shoot;
        }
        if (extModel)
        {
            List<float> input = new List<float>();
            var x = enemy.transform.GetChild(2).GetComponent<RayPerceptionSensorComponent2D>().GetRayPerceptionInput();
            var y = RayPerceptionSensor.Perceive(x);
            var rays = y.RayOutputs;
            foreach (var ray in rays)
            {
                float[] vector = new float[10];
                ray.ToFloatArray(8, 0, vector);
                foreach (var o in vector)
                    input.Add(o);
            }

            x = enemy.transform.GetChild(3).GetComponent<RayPerceptionSensorComponent2D>().GetRayPerceptionInput();
            y = RayPerceptionSensor.Perceive(x);
            rays = y.RayOutputs;
            foreach (var ray in rays)
            {
                float[] vector = new float[10];
                ray.ToFloatArray(8, 0, vector);
                foreach (var o in vector)
                    input.Add(o);
            }

            var obs = GetObservations();
            foreach (float o in obs)
                input.Add(o);


            input2D.Add(input.ToArray());
            input2D.RemoveAt(0);

            var array = input2D.ToArray();
           // Debug.Log(input2D.Count());
            //using Tensor inputTensor = new Tensor(1 ,737, input.ToArray());
            using Tensor inputTensor = new Tensor(0,10,737, 1);
            for (int j = 0; j < 737; j++)
                for (int r = 0; r < 10; r++)

                    inputTensor[0,9-r, j,0] = array[r][j];
            //Debug.Log(inputTensor[0, 1, 728, 0]);
            worker.Execute(inputTensor);
            Tensor outputTensor = worker.PeekOutput(output1);
            actionsOut[0] = outputTensor.ArgMax()[0];
            Debug.Log(actionsOut[0]);
            outputTensor = worker.PeekOutput(output2);
            actionsOut[1] = outputTensor.ArgMax()[0];
            outputTensor = worker.PeekOutput(output3);
            actionsOut[2] = outputTensor.ArgMax()[0];
            outputTensor = worker.PeekOutput(output4);
            actionsOut[3] = outputTensor.ArgMax()[0];
            actionsOut[3] = 1f;
        }


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
        Debug.DrawLine(player.transform.position, transform.position, Color.green);
        Vector3 toPlayer = player.transform.position - transform.position;
        //Debug.Log(Vector3.Dot(firePoint.up.normalized, toPlayer.normalized));

        if (player.isDie)
            PlayerDie();
        if (enemy.isDie)
            EnemyDie();
        if (lastTotalDamage != enemy.totalDamage)
        {
            Positive();
            lastTotalDamage = enemy.totalDamage;
        }

        if (lastHealth != enemy.Health)
        {
            Negative();
            lastHealth = enemy.Health;
        }

    }
    #region rewards
    public void PlayerDie()
    {
        AddReward(1f);
        EndEpisode();
    }

    public void EnemyDie()
    {
        AddReward(-1f);
        EndEpisode();
    }
    public void Positive()
    {
        AddReward(.2f);
    }
    public void Negative()
    {
        AddReward(-.2f);
    }

    #endregion


    #region collisions
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (trainingMode)
            AddReward(-.1f);
    }
    /*private void OnCollisionStay2D(Collision2D collision)
    {
        if (trainingMode)
            AddReward(-.01f);
    }
    */
    #endregion


    
    
   
}
