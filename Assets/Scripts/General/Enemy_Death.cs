using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Death : MonoBehaviour
{
    [SerializeField] private AudioClip[] Death_Sounds;
    [SerializeField] private float KnockbackUpward = 300f;
    [SerializeField] private float KnockbackTime = 0.1f;

    private GameObject parent;

    private void Start()
    {
        parent = transform.parent.gameObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.transform.position.y > transform.position.y)
        { 
            parent.GetComponent<KnockBack>().triggerKnockBack(other.gameObject, other.GetComponent<Rigidbody2D>().velocity.x, KnockbackUpward, KnockbackTime);
            PlayDeathSound(other.GetComponent<AudioSource>());
            Destroy(parent);
        }
    }
    private void PlayDeathSound(AudioSource Audio_Source)
    {
        int RandomIndex = Random.Range(0, Death_Sounds.Length);

        Audio_Source.pitch = Random.Range(0.75f, 1.25f);
        Audio_Source.PlayOneShot(Death_Sounds[RandomIndex], 0.5f);
    }

}
