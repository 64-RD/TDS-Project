using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : Weapon
{
    void Awake()
    {
        rateOfFire = 2.0f;
        magazineSize = 7;
        reloadTime = 3.0f;
        bullet.initBullet( 3.0f, 15);
    }
}