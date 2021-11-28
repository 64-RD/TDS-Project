using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float fov = 90f;
    public float vievDistance = 5f;
    public float rayCount = 10;
    private float angle = 0f;
    private float angleIncrease;
    void Start()
    {
        angleIncrease = fov / rayCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
