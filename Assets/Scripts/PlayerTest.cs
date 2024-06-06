using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTest : MonoBehaviour
{
    public Camera cam;
    public int speed = 5;
    void Start()
    {
        transform.position = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 mouse_pos = cam.ScreenToWorldPoint(Input.mousePosition);
        mouse_pos.z = 0;
        transform.position += Vector3.ClampMagnitude(mouse_pos - transform.position, speed / 10f);
    }

    void OnCollisionEnter2D(){
        Debug.Log("bah");
    }
}
