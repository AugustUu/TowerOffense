using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletShoot : MonoBehaviour
{
    public enum TurretType
    {
        shoot_guy,
        tack_shoot_guy,
        triple_shoot_guy,
        fast_shoot_guy
    }
    public GameObject bullet;
    public int shoot_dealy;
    public GameObject player;
    public TurretType turret_type;
    public bool checks_distance;
    private bool shooting = false;
    public int radius;

    void Start()
    {
        if (!checks_distance)
        {
            InvokeRepeating("Shoot", shoot_dealy, shoot_dealy);
        }
    }
    void Update()
    {
        if (checks_distance)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < radius)
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x));
                if (!shooting)
                {
                    shooting = true;
                    InvokeRepeating("Shoot", shoot_dealy, shoot_dealy);
                }
            }
            else if (shooting)
            {
                shooting = false;
                CancelInvoke("Shoot");
            }
        }
    }

    void Shoot()
    {
        switch (turret_type)
        {
            case TurretType.shoot_guy:
                Instantiate(bullet, transform.position, transform.rotation);
                break;
            case TurretType.tack_shoot_guy:
                for (int i = 0; i < 6; i++)
                {
                    Instantiate(bullet, transform.position, Quaternion.Euler(0, 0, i * 60));
                    Debug.Log("aa");
                }
                break;
        }
        
    }

    private void OnDrawGizmos()
    {
        if (checks_distance)
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}