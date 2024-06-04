using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackScript : MonoBehaviour
{
    // Start is called before the first frame update
    private float speed = 5;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * speed;
        speed *= Mathf.Pow(0.5f, Time.deltaTime);
        if (Time.realtimeSinceStartup > 2)
        {
            Destroy(gameObject);
        }
    }
}
