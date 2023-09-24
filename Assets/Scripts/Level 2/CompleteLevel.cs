using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CompleteLevel : MonoBehaviour
{
    CameraFollow cameraFollow;

    SpriteRenderer ren;
    void Start()
    {
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        ren = GameObject.Find("LevelCompleteText").GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cameraFollow.enabled = false;
            Invoke("LevelComplete1", 1);
        }    
    }

    void LevelComplete1() 
    { 
        ren.enabled = true;
        Invoke("LevelComplete2", 3);
    }

    void LevelComplete2()
    {
        SceneManager.LoadScene(0);
    }
}
