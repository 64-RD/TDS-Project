using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    protected float rateOfFire;
    protected int magazineSize;
    protected float reloadTime;
    public Bullet bullet;
    public int bulletsLeft;
    protected bool allowFire;
    private bool isInit = false;
    protected GameObject owner;

    public void Init()
    {
        this.owner = this.transform.parent.gameObject;
        bulletsLeft = magazineSize;
        allowFire = true;
        isInit = true;
    }

    public bool TryShoot()
    {       
        if (!isInit) Init();
        if (allowFire) {
            if (bulletsLeft > 0) {
                bulletsLeft -= 1;
                StartCoroutine(Shoot());
                return true;
            }
            else StartCoroutine(Reload());
            return false;
        }
        return false;
    }

    private IEnumerator Reload()
    {
        allowFire = false;
        yield return new WaitForSeconds(reloadTime);
        bulletsLeft = magazineSize;
        allowFire = true;
    }

    protected virtual IEnumerator Shoot()
    {
        allowFire = false;
        Vector3 rt = gameObject.transform.eulerAngles;
        float randAngle = 0;// (float) (new System.Random().NextDouble() * 10) - 5; // [0; 1] => [-5; 5]
        var target = Quaternion.Slerp(gameObject.transform.rotation, Quaternion.Euler(rt.x, rt.y, rt.z + randAngle), 1);
        Bullet newBullet=Instantiate(bullet, gameObject.transform.position, target);
        newBullet.owner = this.owner;
        yield return new WaitForSeconds(1 / rateOfFire);
        allowFire = true;
    }
}