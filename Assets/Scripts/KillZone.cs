using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private Transform Spawn_Player;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {

            other.gameObject.GetComponent<Player_Movement>().Respawn();
            other.transform.position = Spawn_Player.position;
            other.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
