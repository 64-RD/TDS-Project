using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR;
using Random = UnityEngine.Random;

public class State_machine : MonoBehaviour
{
    private float rotationSpeed = 90f;
    private int rotationDirection;
    private float searchAfterEnemyTime = 5f;
    private float goToPositiomTime = 10f;
    private float rotationTime = 5f;
    private float currentTime = 0f;
    private Enemy enemy;
    private Agent mlAgent;
    private NavMeshAgent navMeshAgent;
    private Vector3 startPosition;


    private enum State
    {
        Start,
        GoToPosition,
        Rotate,
        BackToStart,
        EnemyInView
    }
    private State currentState;

    void Start()
    {
        startPosition = enemy.transform.position;
        currentState = State.Start;
        ChangeFlags(false,false);
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updateRotation = true;
        navMeshAgent.updateUpAxis = false;

        SetDestination();
    }
    /// <summary>
    /// obs³uga rotacji bota //DO POPRAWY
    /// </summary>
    private void Rotate()
    {
        Vector3 currentRotationVector = enemy.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, 0, currentRotationVector.z + rotationSpeed* rotationDirection);
 

    }
    /// <summary>
    /// Wykrycie dotarcia do celu i ustawienie nowego
    /// </summary>
    private void Move()
    {
        if(Vector3.Distance(navMeshAgent.destination,enemy.transform.position)<3)
        {
           SetDestination();
        }
    }

    private void SetDestination()
    {
        while (true)
        {
            var potentialPosition = enemy.transform.parent.transform.position +
                                    new Vector3(UnityEngine.Random.Range(-24, 24),
                                        UnityEngine.Random.Range(-24, 24));
            var currentPosition = enemy.transform.localPosition;
            Collider2D[] colliders =
                Physics2D.OverlapCircleAll(new Vector2(potentialPosition.x, potentialPosition.y), 0.5f);

            // Safe position has been found if no colliders are overlapped*/
            if (Vector3.Distance(potentialPosition, currentPosition) > 5 && colliders.Length == 0)
            {
                navMeshAgent.SetDestination(potentialPosition);
                break;
            }

        }
    }

    private void BackToStart()
    {
        navMeshAgent.SetDestination(startPosition);
    }

    private bool IsPlayerInView()
    {
        List<float> input = new List<float>();
        var x = enemy.transform.GetChild(2).GetComponent<RayPerceptionSensorComponent2D>().GetRayPerceptionInput();
        var y = RayPerceptionSensor.Perceive(x);
        var rays = y.RayOutputs;
        foreach (var ray in rays)
        {
            if (ray.HitTagIndex == 1)
                return true;
        }

        return false;
    }

    private void TryToChangeState(State state, State newState, float timerLimiter)
    {
        if (currentState == state)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timerLimiter)
            {
                currentState = newState;
                currentTime = 0f;
            }
        }
    }
    /// <summary>
    /// Obs³uga zmiany stanów
    /// </summary>
    private void ChangeState()
    {
        if (IsPlayerInView())
        {
            currentState = State.EnemyInView;
            currentTime = 0;
        }
        else
        {
            TryToChangeState(State.Start,State.GoToPosition,0);
            TryToChangeState(State.EnemyInView,State.GoToPosition, searchAfterEnemyTime);
            TryToChangeState(State.GoToPosition, State.Rotate, goToPositiomTime);
            TryToChangeState(State.Rotate, State.GoToPosition, rotationTime);
            TryToChangeState(State.BackToStart, State.GoToPosition,0);
        }
    }
    /// <summary>
    /// Sprawdzamy Czy nast¹pi³a jakaœ zmiana flagi, je¿eli tak to zamieniamy, je¿eli nie to nic nie robimyy
    /// </summary>
    /// <param name="mlAgentEnabled">Flaga od w³¹czonego modelu</param>
    /// <param name="navMeshAgentIsStopped">Flaga od algorytmu znajduywania œcie¿ki</param>
    private void ChangeFlags(bool mlAgentEnabled, bool navMeshAgentIsStopped)
    {
        if (mlAgentEnabled != mlAgent.enabled)
            mlAgent.enabled = mlAgentEnabled;
        if (navMeshAgentIsStopped != navMeshAgent.isStopped)
            navMeshAgent.isStopped = navMeshAgentIsStopped;
    }
    void Update()
    {
        
        ChangeState();
        switch (currentState)
        {
            case State.Start:
                ChangeFlags(false, false);
                Debug.Log("Niby coœ siê dzieje, ale nawet nie mo¿na tu wejœæ");
                break;
            // Idz do losowego miejsca
            case State.GoToPosition:
                ChangeFlags(false,false);
                rotationDirection = Random.Range(-1, 1);
                Move();
                break;
            //Ustawiamy pocz¹tkowe destination
            case State.BackToStart:
                ChangeFlags(false,false);
                BackToStart();
                break;
            //Obracamy bota
            case State.Rotate:
                ChangeFlags(false,true);
                Rotate();
                break;
            //Po prostu odblokowujemy agenta a blokujemy pathfinding
            case State.EnemyInView:
                ChangeFlags(true,true);
                break;
        }

    }

}

