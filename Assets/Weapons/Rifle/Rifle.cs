using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle : Weapon
{
    void Awake()
    {
        rateOfFire = 5.0f;
        magazineSize = 30;
        reloadTime = 2.0f;
        bullet.initBullet( 1.0f, 25);
    }
}