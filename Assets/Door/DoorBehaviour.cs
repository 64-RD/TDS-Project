using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    public GameObject hinge;
    private bool isOpen;
    
    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            toggleDoor();
        }
    }

    void toggleDoor()
    {
        if (isOpen)
        {
            transform.RotateAround(hinge.transform.position, Vector3.forward, 90);
            this.isOpen = false;
            return;
        }
        transform.RotateAround(hinge.transform.position, Vector3.forward, -90);
        isOpen = true;
    }
}
