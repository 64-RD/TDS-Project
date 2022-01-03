using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : Enemy
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        health = 300;
        speed = 0.5f;
        damageResist = 0.5f;
    }
}
