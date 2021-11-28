using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Handle_Movement : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed;
    private List<Vector3> path;
    private int index;
    private Vector3 targertPosition = new Vector3(7.5f, -2.5f, 0);
    public Rigidbody2D rb;
    Vector2 _moveDirection = new Vector2(0,0);
    Vector2 LastMoveDirection;

    void Start()
    {
        //MoveTo(targertPosition);
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
       /* if (Input.GetMouseButtonDown(0))
        {
            MoveTo(UtilsClass.GetMouseWorldPosition());
        }
        if (path != null)
            Debug.Log(path[path.Count - index - 1].ToString());*/
    }

    private void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
         rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        
    }

    public void MoveTo(Vector3 targetPosition)
    {
        //GameObject go = GameObject.Find("Grid");
        //test other = (test)go.GetComponent(typeof(test));
        path = GameObject.Find("Grid").GetComponent<test>().getPath(transform.position, targetPosition);
        index = 0;
    }

    private void Movement()
    {
        if(path!=null)
        {
            Vector3 target = path[path.Count-index-1];
            if (Vector3.Distance(transform.position, target) > 0.1)
            {
                _moveDirection = (target - transform.position).normalized;
                LastMoveDirection = _moveDirection;

            }
            else
            {
                index++;
                if(index>=path.Count)
                {
                    StopMoving();
                    _moveDirection = new Vector2(0, 0);
                }
            }
        }
    }


    private void StopMoving()
    {
        path = null;
    }

}
