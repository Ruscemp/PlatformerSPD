using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossManager : MonoBehaviour
{
    Scene currentScene;

    Rigidbody2D rigid;
    GameObject player;

    CameraFollow cameraFollow;

    float speed = 5;
    float breakForceMulti = 1.5f;
    float breakRotMulti = 1.5f;
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        rigid = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player_BossVer");
        cameraFollow = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
    }

    void Update()
    {
        rigid.velocity = Vector2.right * speed;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            cameraFollow.enabled = false;
            player.SetActive(false);
            Invoke("ReloadScene", 1);
        }
        else if(collision.CompareTag("PartOfPlatform"))
        {
            collision.GetComponent<BoxCollider2D>().enabled = false; ;
            Rigidbody2D platformRigid = collision.GetComponent<Rigidbody2D>();
            platformRigid.bodyType = RigidbodyType2D.Dynamic;
            float rightForce = Random.Range(-1.5f, 4.0f);
            float upForce = Random.Range(1.0f, 3.0f);
            Vector2 baseBreakForce = (Vector2.right * rightForce) + (Vector2.up * upForce);
            float baseBreakRotation = Random.Range(-2.0f, 2.0f);
            platformRigid.AddForce(baseBreakForce * breakForceMulti, ForceMode2D.Impulse);
            platformRigid.AddTorque(baseBreakRotation * breakRotMulti, ForceMode2D.Impulse);
        }
        else if(collision.CompareTag("BossTrigger"))
        {
            speed = collision.GetComponent<BossInstructions>().GetSpeed();
            Destroy(collision.gameObject);
        }
    }

    void ReloadScene() 
    {
        SceneManager.LoadScene(currentScene.name);
    }
}
