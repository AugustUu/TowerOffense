using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    public GameObject bullet;
    void Start()
    {
        InvokeRepeating("Shoot", 0, 5);
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Shoot()
    {
        Instantiate(bullet, transform.position, Quaternion.Euler(Math.Atan2(transform.position - Input.mousePosition)));
    }
}