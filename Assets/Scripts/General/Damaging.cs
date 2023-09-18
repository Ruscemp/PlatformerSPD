using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damaging : MonoBehaviour
{
    [SerializeField] private float DamageGiven = 1f;
    [SerializeField] private float KnockbackUpward = 300f;
    [SerializeField] private float KnockbackTime = 0.1f;
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                other.gameObject.GetComponent<Player_Movement>().TakeDamage(DamageGiven);
                other.gameObject.GetComponent<Player_Movement>().TakeKnockback(0, KnockbackUpward, KnockbackTime);
                break;
        }
    }
}
