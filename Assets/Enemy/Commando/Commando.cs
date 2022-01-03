using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Commando : Enemy
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        health = 150;
        speed = 1.0f;
        damageResist = 1.0f;
    }
}
