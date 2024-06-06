using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed;
    public bool decel;
    private float start_time;
    void Start()
    {
        start_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        if (decel)
        {
            speed *= Mathf.Pow(0.05f, Time.deltaTime);
        }
        if (Time.time - start_time > 5)
        {
            Destroy(gameObject);
        }
    }
}
