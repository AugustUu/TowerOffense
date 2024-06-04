using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerMovement : MonoBehaviour
{

    public SplineContainer track;

    public float speed;
    public float max_speed;
    public float velocity;
    
    public float position = 0;

    public int current_spline = 0;
    
    private List<float> lengths = new();

    void Start(){
        foreach(Spline spline in track.Splines){
            lengths.Add(spline.GetLength());
            Debug.Log(spline.GetLength());
        }
        Debug.Log(track.CalculateLength(0));
        Debug.Log(track.CalculateLength(1));
    }

    void SwapTracks(){
         for(int i=0; i < track.Splines.Count; i++){
                if(i != current_spline){
                    foreach(BezierKnot knot in track.Splines[i].Knots){

                        if(Vector3.Distance(this.transform.position,track.transform.TransformPoint(knot.Position)) < 0.2f){
                            current_spline = i;
                            SplineUtility.GetNearestPoint(track.Splines[i],track.transform.InverseTransformPoint(this.transform.position), out float3 nearest, out float t,16,16);

                            Debug.DrawLine(transform.position, track.transform.TransformPoint(nearest),Color.red,1f);

                            position = t;
                            return;
                        }else{
                            Debug.DrawLine(transform.position, track.transform.TransformPoint(knot.Position),Color.red,1f);
                            Debug.Log(Vector3.Distance(this.transform.position,track.transform.TransformPoint(knot.Position)));
                        }
                    }
                }
            }
    }

    
    void Update()
    {

        //Debug.Log(speed/lengths[0] + " " + speed/lengths[1]);

        /*if(Input.GetKey(KeyCode.UpArrow)){
            position += speed * Time.deltaTime / lengths[current_spline];
        }else if(Input.GetKey(KeyCode.DownArrow)){
            position -= speed * Time.deltaTime / lengths[current_spline];
        }*/
        
        if(Input.GetKey(KeyCode.UpArrow)){
            velocity += speed * Time.deltaTime;
        }else if(Input.GetKey(KeyCode.DownArrow)){
            velocity -= speed * Time.deltaTime;
        }
        else{
            velocity *= Mathf.Pow(0.0001f, Time.deltaTime);
        }

        velocity = Mathf.Clamp(velocity, -max_speed, max_speed);
        velocity = Mathf.Floor(velocity * 1000) / 1000;
        Debug.Log(velocity);

        position += velocity * Time.deltaTime / lengths[current_spline];

        if(Input.GetKeyDown(KeyCode.F)){
            SwapTracks();
        }

        position = Math.Clamp(position,0,1);
        this.transform.position = track.EvaluatePosition(current_spline,position);

    }
}
