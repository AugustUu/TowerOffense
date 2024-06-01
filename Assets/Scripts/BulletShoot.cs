using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    public GameObject bullet;
    public GameObject player;
    void Start()
    {
        InvokeRepeating("Shoot", 0, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x));
    }

    void Shoot()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }
}