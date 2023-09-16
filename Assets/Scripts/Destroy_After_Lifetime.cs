using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_After_Lifetime : MonoBehaviour
{
    [SerializeField] private float Lifetime = 1f;

    void Start()
    {
        Destroy(gameObject, Lifetime);
    }
}
