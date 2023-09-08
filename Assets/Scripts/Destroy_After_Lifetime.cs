using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroy_After_Lifetime : MonoBehaviour
{
    [SerializeField, MinValue(0.01f)] private float Lifetime = 1f;

    void Start()
    {
        Destroy(gameObject, Lifetime);
    }
}
