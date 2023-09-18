using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Movement : MonoBehaviour
{
    [SerializeField] private float Move_Speed = 2.0f;
    [SerializeField] private float bounciness = 100.0f;
    [SerializeField] private float Damage_Given = 1f;
    [SerializeField] private float Knockback_Force = 400f;
    [SerializeField] private float Knockback_Upward = 300f;
    [SerializeField] private float Knockback_Time = 0.5f;
    [SerializeField] private AudioClip[] Death_Sounds;
    private SpriteRenderer rend;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        transform.Translate(new Vector2(Move_Speed, 0)*Time.deltaTime);

        if(Move_Speed > 0)
        {
            rend.flipX = true;
        }else if(Move_Speed < 0)
        {
            rend.flipX = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (other.gameObject.tag)
        {
            case "Enemy_Block": case "Enemy":
                Move_Speed = -Move_Speed;
                break;
            case "Player":
                other.gameObject.GetComponent<Player_Movement>().TakeDamage(Damage_Given);
                if(other.transform.position.x > transform.position.x)
                {
                    other.gameObject.GetComponent<Player_Movement>().TakeKnockback(Knockback_Force, Knockback_Upward, Knockback_Time);
                }
                else
                {
                    other.gameObject.GetComponent<Player_Movement>().TakeKnockback(-Knockback_Force, Knockback_Upward, Knockback_Time);
                }
                break;
        }
    }

    private void PlayDeathSound(AudioSource Audio_Source)
    {
        int RandomIndex = Random.Range(0, Death_Sounds.Length);

        Audio_Source.pitch = Random.Range(0.75f, 1.25f);
        Audio_Source.PlayOneShot(Death_Sounds[RandomIndex], 0.5f);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<Rigidbody2D>().velocity = new Vector2(other.GetComponent<Rigidbody2D>().velocity.x, 0);
            other.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, bounciness));
            PlayDeathSound(other.GetComponent<AudioSource>());
            Destroy(gameObject);
        }
    }
}
