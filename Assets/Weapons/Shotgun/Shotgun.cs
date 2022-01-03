using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    void Awake()
    {
        rateOfFire = 1.0f;
        magazineSize = 3;
        reloadTime = 5.0f;
        bullet.initBullet(1.5f, 20);
    }
    protected override IEnumerator Shoot()
    {
        allowFire = false;
        Vector3 rt = gameObject.transform.eulerAngles;
        for (int x = 0; x < 7; x++)
        {
            float randAngle = (float) (new System.Random().NextDouble() * 20) - 10; // [0; 1] => [-10; 10]
            var target = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler(rt.x, rt.y, rt.z + randAngle), 1);
            Instantiate(bullet, gameObject.transform.position, target);  
        }
        Debug.Log(allowFire);
        yield return new WaitForSeconds(1 / rateOfFire);
        allowFire = true;
    }
}