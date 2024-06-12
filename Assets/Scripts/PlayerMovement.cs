using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class PlayerMovement : MonoBehaviour
{

    public SplineContainer track;

    public float speed;
    
    public float position = 0;

    public int current_spline = 0;
    
    private List<float> lengths = new();

    private Dictionary<int,List<(Vector3,int)>> splits = new();

    void Start(){
        foreach(Spline spline in track.Splines){
            lengths.Add(spline.GetLength());
            //Debug.Log(spline.GetLength());
        }
        Debug.Log(track.Splines.Count);
        if (track.Splines.Count > 1){
            for (int i = 0; i < track.Splines.Count; i++){
                foreach (BezierKnot knot in track.Splines[i].Knots){
                    for (int v = 0; v < track.Splines.Count; v++){
                        if (v != i){
                            foreach (BezierKnot knot2 in track.Splines[v].Knots){
                                Vector3 pos1 = track.transform.TransformPoint(knot.Position);
                                Vector3 pos2 = track.transform.TransformPoint(knot2.Position);

                                if (Vector3.Distance(pos1, pos2) < 0.2f){
                                    Debug.Log(i + " " + pos1 + " " + v);
                                    if (!splits.ContainsKey(i))
                                    {
                                        splits.Add(i, new List<(Vector3, int)>());
                                    }

                                    splits[i].Add((pos1, v));

                                    Debug.DrawLine(pos1, pos2 + Vector3.down, Color.red, 10f);
                                }
                            }
                        }
                    }
                }
            }
        }
        
        Debug.Log(splits.Count);


    }

/*
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
                            //Debug.Log(Vector3.Distance(this.transform.position,track.transform.TransformPoint(knot.Position)));
                        }
                    }
                }
            }
    }*/


    void Update()
    {


        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 movement = new Vector3(horizontal, vertical, 0f);

        float3 stupid_forward = SplineUtility.EvaluateTangent(track[current_spline], position);
        Vector3 forward = new Vector3(stupid_forward.x,stupid_forward.y,stupid_forward.z);

        float dotProduct = Vector3.Dot(movement.normalized, forward.normalized);


        if (track.Splines.Count > 1)
        {

            foreach (var test in splits[current_spline])
            {
                if (Vector3.Distance(this.transform.position, test.Item1) < 0.1f)
                {

                    SplineUtility.GetNearestPoint(track.Splines[test.Item2],
                        track.transform.InverseTransformPoint(this.transform.position), out float3 nearest, out float t,
                        16, 16);
                    float3 stupid_forward2 = SplineUtility.EvaluateTangent(track[test.Item2], t);
                    Vector3 forward2 = new Vector3(stupid_forward2.x, stupid_forward2.y, stupid_forward2.z);

                    float dotProduct2 = Vector3.Dot(movement.normalized, forward2.normalized);

                    //float new_pos =  Math.Clamp(position + (speed * dotProduct * 0.1f / lengths[current_spline]),0,1);
                    //float new_pos2 = Math.Clamp(t + (speed * dotProduct2 * 0.1f / lengths[test.Item2]),0,1);



                    //if(Math.Abs(dotProduct) < Math.Abs(dotProduct2) && ((new_pos2*1.1 < 1 && new_pos2 > t) || (new_pos2*1.1 > 0  && new_pos2 < t)) ){
                    if (movement != Vector3.zero)
                    {
                        Debug.DrawLine(this.transform.position, this.transform.position + movement, Color.blue);
                        Debug.DrawLine(this.transform.position, this.transform.position + forward.normalized,
                            Color.green);
                        Debug.DrawLine(this.transform.position, this.transform.position + forward2.normalized,
                            Color.red);

/*
                    float player_angle = Vector3.Angle(Vector3.zero - movement,Vector3.right);
                    float player_angle_test = (float)Math.Atan2(movement.y,movement.x);

                    Debug.Log(movement + " " + player_angle + " " + player_angle_test);
                    float track_angle = Vector3.Angle(Vector3.zero - forward.normalized,Vector3.right);
                    float track2_angle = Vector3.Angle(Vector3.zero - forward2.normalized,Vector3.right);

                    float angleDiffWithTrack = Mathf.Abs(Mathf.DeltaAngle(player_angle, track_angle));
                    float angleDiffWithTrack2 = Mathf.Abs(Mathf.DeltaAngle(player_angle, track2_angle));
*/

                        //Debug.Log(player_angle + " " + angleDiffWithTrack + " " + angleDiffWithTrack2);

                        Vector3 adjusted_forward = forward;
                        Vector3 adjusted_forward2 = forward2;

                        if (position > 0.9)
                        {
                            adjusted_forward *= -1;
                        }
                        else if (position > 0.1)
                        {
                            if (Vector3.Angle(movement, adjusted_forward) > Vector3.Angle(movement, -adjusted_forward))
                            {
                                adjusted_forward *= -1;
                            }
                        }

                        if (t > 0.9)
                        {
                            adjusted_forward2 *= -1;
                        }
                        else if (t > 0.1)
                        {
                            if (Vector3.Angle(movement, adjusted_forward2) >
                                Vector3.Angle(movement, -adjusted_forward2))
                            {
                                adjusted_forward2 *= -1;
                            }
                        }








                        if (Vector3.Angle(movement, adjusted_forward2) < Vector3.Angle(movement, adjusted_forward))
                        {
                            Debug.Log(Vector3.Angle(Vector3.zero, forward.normalized) + " " +
                                      Vector3.Angle(Vector3.zero, forward2.normalized));
                            current_spline = test.Item2;
                            position = t;
                        }
                        else
                        {
                            //Debug.Log("B" + dotProduct + " " + dotProduct2);
                        }
                    }
                }
            }
        }

        position += speed * dotProduct * Time.deltaTime / lengths[current_spline];

        position = Math.Clamp(position,0.0001f,0.999f);

        this.transform.position = track.EvaluatePosition(current_spline,position);

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (!other.gameObject.CompareTag("Finish"))
        {
            //current_spline = 0;
            //position = 0;
        }
    }
}
