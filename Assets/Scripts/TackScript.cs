using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 5;
    private float start_time;
    void Start()
    {
        start_time = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        speed *= Mathf.Pow(0.05f, Time.deltaTime);
        if (Time.time - start_time > 2)
        {
            Destroy(gameObject);
        }
    }
}
