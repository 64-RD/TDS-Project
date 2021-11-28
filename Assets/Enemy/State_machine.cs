using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_machine : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 startingPosition;
    public Vector3 goToPosition;
    public Vector3 target;
    private Handle_Movement movement;

    private void Awake()
    {
        movement = GetComponent<Handle_Movement>();
    }
    void Start()
    {  
        startingPosition = transform.position;
        target = startingPosition;
        
    }

    // Update is called once per frame
    void Update()
    {

        if( Vector3.Distance(this.transform.position, target) < 0.1f)
        {
            if (Vector3.Distance(this.transform.position, goToPosition) < 0.5f)
                target = startingPosition;
            
            if (Vector3.Distance(this.transform.position, startingPosition) < 0.5f)
                target = goToPosition;

            movement.MoveTo(target);
        }

    }


    public Vector3 GetNewPosition()
    {
        return startingPosition + GetRandomDirection() * Random.Range(5f, 15f);
    }

    public Vector3 GetRandomDirection()
    {
        return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
    }

}

