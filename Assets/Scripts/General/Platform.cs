using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private PlatformEffector2D effector;
    private float waitTime;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var vert = Input.GetAxis("Vertical");
        if (vert > -1)
        {
            waitTime = 0.2f;
            effector.rotationalOffset = 0f;
        }

        if(vert == -1)
        {
            if (waitTime <= 0)
            {
                effector.rotationalOffset = 180f;
                waitTime = 0;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
