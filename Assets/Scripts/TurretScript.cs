using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretScript : MonoBehaviour
{
    public enum TurretType
    {
        shoot_guy,
        tack_shoot_guy,
        triple_shoot_guy,
        slow_field_guy
    }
    public GameObject bullet;
    public float shoot_dealy;
    public GameObject player;
    public TurretType turret_type;
    public bool checks_distance;
    public bool shoots;
    public float spread;
    public int bullet_speed;

    public Transform spawn_parent;
    
    private bool shooting = false;
    public int radius;

    void Start()
    {
        if (!checks_distance)
        {
            InvokeRepeating("Shoot", 0.5f, shoot_dealy);
        }
    }
    void Update()
    {
        if (checks_distance)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < radius)
            {
                if (shoots)
                {
                    transform.rotation = Quaternion.Euler(0, 0, Mathf.Rad2Deg * Mathf.Atan2(player.transform.position.y - transform.position.y, player.transform.position.x - transform.position.x));
                }
                
                if (!shooting)
                {
                    shooting = true;
                    if (shoots)
                    {
                        InvokeRepeating("Shoot", 0.5f, shoot_dealy);
                    }
                    else
                    {
                        if (turret_type == TurretType.slow_field_guy)
                        {
                            player.GetComponent<PlayerMovement>().speed = 3;
                        }
                    }
                }
            }
            else if (shooting)
            {
                shooting = false;
                if (shoots)
                {
                    CancelInvoke("Shoot");
                }
                else
                {
                    if (turret_type == TurretType.slow_field_guy)
                    {
                        player.GetComponent<PlayerMovement>().speed = 5;
                    }
                }
            }
        }
    }

    void Shoot()
    {
        switch (turret_type)
        {
            case TurretType.shoot_guy:
                SpawnBullet(Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, UnityEngine.Random.Range(-(spread / 2), spread / 2))));
                break;
            case TurretType.tack_shoot_guy:
                for (int i = 0; i < 6; i++)
                {
                    SpawnBullet(Quaternion.Euler(0, 0, i * 60));
                }
                break;
            case TurretType.triple_shoot_guy:
                SpawnBullet(transform.rotation);
                SpawnBullet(Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, 30)));
                SpawnBullet(Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, -30)));
                break;
        }
    }

    void SpawnBullet(Quaternion rotation)
    {
        GameObject shot_bullet = Instantiate(bullet, transform.position, rotation, spawn_parent);
        shot_bullet.GetComponent<BulletScript>().speed = bullet_speed;
    }

    private void OnDrawGizmos()
    {
        if (checks_distance)
        {
            Gizmos.DrawWireSphere(transform.position, radius);
        }
    }
}