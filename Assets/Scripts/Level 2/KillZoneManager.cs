using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KillZoneManager : MonoBehaviour
{
    Scene currentScene;

    CameraFollow cameraFollow;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerController>().PlayDeathSounds();
            cameraFollow.enabled = false;
            Invoke("ReloadScene", 1.7f);
        }
    }

    void ReloadScene() 
    {
        SceneManager.LoadScene(currentScene.name);
    }
}
