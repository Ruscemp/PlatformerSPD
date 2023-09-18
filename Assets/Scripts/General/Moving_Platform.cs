using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moving_Platform : MonoBehaviour
{
    [SerializeField] private List<Transform> Targets;
    [SerializeField] private float Move_Speed = 2f;

    private Transform Current_Target;
    void Start()
    {
        Current_Target = Targets[0];
    }

    void FixedUpdate()
    {
        if(transform.position == Current_Target.position)
        {
            int newIndex = Targets.IndexOf(Current_Target) + 1;
            if (newIndex >= Targets.Count) newIndex = 0;
            Current_Target = Targets[newIndex];
        }

        transform.position = Vector2.MoveTowards(transform.position, Current_Target.position, Move_Speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && other.transform.position.y > transform.position.y)
        {
            other.transform.SetParent(transform);
        }
    }
    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.SetParent(null);
        }
    }
}
