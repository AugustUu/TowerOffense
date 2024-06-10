using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public String next_scene;
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        SceneManager.LoadScene(next_scene);
    }
}
