using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private LayerMask passthroughColliderMask;
    private LayerMask originalColliderMask;
    private PlatformEffector2D effector;
    private float waitTime;

    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        originalColliderMask = effector.colliderMask;
    }

    // Update is called once per frame
    void Update()
    {
        var vert = Input.GetAxis("Vertical");
        if (vert > -1)
        {
            waitTime = 0.2f;
            effector.colliderMask = originalColliderMask;
        }

        if(vert == -1)
        {
            if (waitTime <= 0)
            {;
                effector.colliderMask = passthroughColliderMask;
                waitTime = 0;
            }
            else
            {
                waitTime -= Time.deltaTime;
            }
        }
    }
}
