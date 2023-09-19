using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField, Min(0.1f)] private float speed;
    [SerializeField, Required] private Transform groundDetection;
    [SerializeField, Min(0.1f)] private float groundDetectionDistance;
    [SerializeField, Tag] private string GroundTag;
    private bool movingRight = true;


    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(groundDetection.position, Vector2.down, groundDetectionDistance);
        if(groundInfo.collider == false)
        {
            Flip();
        }
        Debug.DrawRay(groundDetection.position, Vector2.down * groundDetectionDistance, Color.red, 0.25f);
    }
    private void Flip()
    {
        if (movingRight)
        {
            transform.eulerAngles = new Vector3(0, -180, 0);
        }
        else
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        movingRight = !movingRight;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(GroundTag))
        {
            Flip();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(this.gameObject.tag))
        {
            Flip();
        }
    }
}
