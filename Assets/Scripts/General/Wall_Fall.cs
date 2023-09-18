using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall_Fall : MonoBehaviour
{
    [SerializeField] private GameObject Button;

    [SerializeField, Range(0f, 1f)] private float Sound_Volume = 1f;
    [SerializeField] private AudioClip[] Activation_Sounds;

    private Animator anim;
    private AudioSource Audio_Source;
    private bool hasPlayedAnimation = false;
    private void Start()
    {
        anim = GetComponent<Animator>();
        Audio_Source = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") && !hasPlayedAnimation)
        {
            if(Button != null) Button.SetActive(false);
            hasPlayedAnimation = true;
            PlaySound(Activation_Sounds);
            anim.SetTrigger("Move");
        }
    }
    private void PlaySound(AudioClip[] sounds)
    {
        Audio_Source.pitch = Random.Range(0.75f, 1.25f);
        int RandomIndex = Random.Range(0, sounds.Length);
        Audio_Source.PlayOneShot(sounds[RandomIndex], Sound_Volume);
    }
}

