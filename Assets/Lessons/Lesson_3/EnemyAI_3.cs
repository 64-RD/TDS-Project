using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;

/// <summary>
/// A Enemy Machine Learning Agent
/// </summary>
public class EnemyAI_3 : Agent
{
    [Tooltip("Moving speed")]
    public float moveSpeed;

    [Tooltip("Rotation speed")]
    public float rotationSpeed;



    [Tooltip("Whether this is training mode or gameplay mode")]
    public bool trainingMode;

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

    public override void Initialize()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
        weapon = enemy.weapon;
        lastTotalDamage = enemy.totalDamage;
        lastHealth = enemy.Health;
        // If not training mode, no max step, play forever
        if (!trainingMode) MaxStep = 0;

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

    }

    public override void OnActionReceived(float[] vectorAction)
    {
        // Don't take actions if frozen
        if (frozen) return;

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
        sensor.AddObservation(Vector3.zero);
        sensor.AddObservation(Vector3.zero);
        sensor.AddObservation(0.0f);

        // Get a vector from the beak tip to the nearest flower
        // Vector3 toPlayer = player.transform.position - transform.position;
        //toPlayer = Vector3.zero;


        //float dist = Vector3.Distance(player.transform.position, transform.position);

        // Observe a normalized vector pointing to the nearest flower (1 observations)
        //sensor.AddObservation(enemy.currentTime);
    }

    public override void Heuristic(float[] actionsOut)
    {
        // Create placeholders for all movement/turning
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

        {/*{Vector3 _mousePos;
        _mousePos = Input.mousePosition;
        _mousePos.z = 5.23f;
        Vector3 objectPos = Camera.main.WorldToScreenPoint(this.transform.position);
        _mousePos.x -= objectPos.x;
        _mousePos.y -= objectPos.y;

        float angle = Mathf.Atan2(_mousePos.y, _mousePos.x) * Mathf.Rad2Deg;
        //Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
        //angle = angle - 90;
        yaw = (angle / 180f) - 1f ;
        // Turn left/right
        if (Input.GetKey(KeyCode.LeftArrow)) yaw = 0f;
        else if (Input.GetKey(KeyCode.RightArrow)) yaw = 2f;
        else yaw = 1f;}*/
        }

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
    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (trainingMode)
            AddReward(-.1f);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (trainingMode)
            AddReward(-.01f);
    }
    */
    #endregion
}
