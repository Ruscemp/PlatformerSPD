using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float Jump_Force = 200f;
    [SerializeField] private AudioClip[] Jump_Sounds;
    private AudioSource Audio_Source;

    private void Start()
    {
        Audio_Source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D PlayerRigidbody = other.GetComponent<Rigidbody2D>();
            PlayerRigidbody.velocity = new Vector2(PlayerRigidbody.velocity.x, 0);
            PlayerRigidbody.AddForce(new Vector2(0, Jump_Force));
            GetComponent<Animator>().SetTrigger("Jump");
            PlayJumpSound();
        }
    }
    private void PlayJumpSound()
    {
        int RandomIndex = Random.Range(0, Jump_Sounds.Length);

        Audio_Source.pitch = Random.Range(0.75f, 1.25f);
        Audio_Source.PlayOneShot(Jump_Sounds[RandomIndex], 0.5f);
    }
}
