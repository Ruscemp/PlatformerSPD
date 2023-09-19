using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    [SerializeField] private float KnockbackForce = 0f;
    [SerializeField] private float KnockbackUpward = 300f;
    [SerializeField] private float KnockbackTime = 0.1f;
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Player":
                triggerKnockBack(other.gameObject, KnockbackForce, KnockbackUpward, KnockbackTime);
                break;
        }
    }
    public void triggerKnockBack(GameObject player, float KnockbackForce, float KnockbackUpward, float KnockbackTime)
    {
        if (player.transform.position.x > transform.position.x)
        {
            player.GetComponent<Player_Movement>().TakeKnockback(KnockbackForce, KnockbackUpward, KnockbackTime);
        }
        else
        {
            player.GetComponent<Player_Movement>().TakeKnockback(-KnockbackForce, KnockbackUpward, KnockbackTime);
        }
    }
}
