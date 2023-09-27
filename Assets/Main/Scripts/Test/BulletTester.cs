using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletTester : MonoBehaviour
{
    public Bullet bullet;
    IObjectPool<Bullet> bulletPool;
    // Start is called before the first frame update

    float timer = 0;
    void Start()
    {
        bulletPool = new LinkedPool<Bullet>(() => Instantiate(bullet), OnBulletGet);
    }

    private void OnBulletGet(Bullet bullet)
    {
        bullet.Initialize(20, 0, -1, bulletPool, null, transform.forward, 5);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1)
        {
            bulletPool.Get();
            timer = 0;
        }
    }
}
