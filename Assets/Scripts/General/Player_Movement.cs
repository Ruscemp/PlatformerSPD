using NaughtyAttributes;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Movement : MonoBehaviour
{
    #region Ground Detection
    [SerializeField, Foldout("Ground Detection"), Required] private Transform L_Foot;
    [SerializeField, Foldout("Ground Detection"), Required] private Transform R_Foot;
    [SerializeField, Foldout("Ground Detection")] private float rayDistance = 0.25f;
    [SerializeField, Foldout("Ground Detection"), Required] private LayerMask whatIsGround;
    #endregion
    #region Sound
    [SerializeField, Range(0f, 1f), Foldout("Sound")]
    private float Jump_Volume;
    [SerializeField, Foldout("Sound"), Required] private AudioClip[] Jump_Sounds;
    [SerializeField, Range(0f, 1f), Foldout("Sound")]
    private float Pickup_Volume;
    [SerializeField, Range(0f, 1f), Foldout("Sound")]
    private float Quest_Pickup_Volume;
    [SerializeField, Foldout("Sound"), Required] private AudioClip[] Pickup_Sounds;
    [SerializeField, Range(0f, 1f), Foldout("Sound")]
    private float Hurt_Volume;
    [SerializeField, Foldout("Sound"), Required] private AudioClip[] Hurt_Sounds;
    [SerializeField, Range(0f, 1f), Foldout("Sound")]
    private float Death_Volume;
    [SerializeField, Foldout("Sound"), Required] private AudioClip[] Death_Sounds;
    #endregion
    #region Health
    [SerializeField, Foldout("Health")] private float HP_Start = 10f;
    [SerializeField, Foldout("Health")] private float HP_Current;
    [SerializeField, Foldout("Health")] private float HP_Max = 10f;
    #endregion
    #region Move Stats
    [SerializeField, Foldout("Move Stats")] private float Move_Speed = 150f;
    [SerializeField, Foldout("Move Stats")] private float jumpForce = 230f;
    [SerializeField, Foldout("Move Stats")] private float DashingPower = 25f;
    [SerializeField, Foldout("Move Stats")] private float DashingTime = 0.2f;
    [SerializeField, Foldout("Move Stats")] private float DashingCooldown = 1f;
    #endregion
    #region Transform Points
    [SerializeField, Foldout("Transform Points"), Required] private Transform Spawn_Point;
    #endregion
    #region UI Objects
    [SerializeField, Foldout("UI Objects"), Required] private Slider Health_Slider;
    [SerializeField, Foldout("UI Objects"), Required] private Image Health_Slider_Fill;
    [SerializeField, Foldout("UI Objects"), Required] private TMP_Text Health_Counter, Quest_Counter;
    [SerializeField, Foldout("UI Objects"), Required] private Color Health_Color_Min, Health_Color_Middle, Health_Color_Max;
    #endregion
    #region Particles
    [SerializeField, Foldout("Particles"), Required] private GameObject Apple_Particles;
    [SerializeField, Foldout("Particles"), Required] private GameObject Dust_Particles;
    #endregion
    #region Questing
    [Foldout("Questing")] public int QuestItems_Collected = 0;
    [Foldout("Questing")] public int QuestItems_Target_Count = 0;
    [Foldout("Questing")] public string QuestItems_Target_Tag;
    #endregion
    #region Private Variables
    #region Normal
    private float horizontalValue;
    private bool isFacingRight = true;
    private bool isGrounded;
    private bool canMove = true;
    private bool canDash = true;
    private bool isDashing;
    #endregion
    #region Unity
    private Rigidbody2D Rigidbody;
    private SpriteRenderer Sprite_Renderer;
    private Animator Animator;
    private AudioSource Audio_Source;
    private TrailRenderer Dash_Trail;
    #endregion
    #endregion

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        Sprite_Renderer = GetComponent<SpriteRenderer>();
        Animator = GetComponent<Animator>();
        Audio_Source = GetComponent<AudioSource>();
        Dash_Trail = GetComponent<TrailRenderer>();
        Spawn();
    }
    void Update()
    {
        if (isDashing) return;

        isGrounded = CheckIfGrounded();
        horizontalValue = Input.GetAxis("Horizontal");

        FlippingSprite();
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }
        if (Input.GetAxis("Fire3")==1 && canDash)
        {
            StartCoroutine(Dash());
        }
        Animator.SetFloat("Move_Speed", Mathf.Abs(Rigidbody.velocity.x));
        Animator.SetFloat("Vertical_Speed", Rigidbody.velocity.y);
        Animator.SetBool("isGrounded", isGrounded);
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = Rigidbody.gravityScale;
        Rigidbody.gravityScale = 0f;
        Rigidbody.velocity = new Vector2(transform.localScale.x * DashingPower, 0f);
        Dash_Trail.emitting = true;
        yield return new WaitForSeconds(DashingTime);
        Dash_Trail.emitting = false;
        Rigidbody.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(DashingCooldown);
        canDash = true;
    }
    private void FixedUpdate()
    {
        if (isDashing) return;

        if (!canMove)
        {
            return;
        }

        Rigidbody.velocity = new Vector2(horizontalValue * Move_Speed * Time.deltaTime, Rigidbody.velocity.y);
    }
    public void TakeKnockback(float Knockback_Force, float Knockback_Upward, float Knockback_Time)
    {
        canMove = false;

        Rigidbody.velocity = Vector2.zero;
        Rigidbody.AddForce(new Vector2(Knockback_Force, Knockback_Upward));

        Invoke(nameof(CanMoveAgain), Knockback_Time);
    }
    private void CanMoveAgain()
    {
        canMove = true;
    }
    private void FlippingSprite()
    {
        if (isFacingRight && horizontalValue < 0f || !isFacingRight && horizontalValue > 0f)
        {
            Vector3 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void Jump()
    {
        Rigidbody.AddForce(new Vector2(0, jumpForce));

        PlaySound(Jump_Sounds, Jump_Volume);
        Instantiate(Dust_Particles, transform.position, Dust_Particles.transform.localRotation);
    }
    public void TakeDamage(float damage)
    {
        HP_Current -= damage;
        PlaySound(Hurt_Sounds, Hurt_Volume);
        UpdateHealthBar();

        if (HP_Current <= 0)
        {
            Respawn();
        }
    }
    private void UpdateHealthBar()
    {
        Health_Slider.value = HP_Current;
        Health_Counter.text = $"{HP_Current}/{HP_Max}";

        if (HP_Current >= HP_Max * 0.75)
        {
            Health_Slider_Fill.color = Health_Color_Max;
        }
        else if (HP_Current >= HP_Max * 0.25)
        {
            Health_Slider_Fill.color = Health_Color_Middle;
        }
        else
        {
            Health_Slider_Fill.color = Health_Color_Min;
        }
    }
    public void Respawn()
    {
        canMove = false;
        PlaySound(Death_Sounds, Death_Volume);
        Spawn();
    }
    private void Spawn()
    {
        transform.position = Spawn_Point.position;
        Rigidbody.velocity = Vector2.zero;
        HP_Current = HP_Start;
        canMove = true;
        UpdateHealthBar();
    }
    private bool CheckIfGrounded()
    {
        RaycastHit2D L_Hit = Physics2D.Raycast(L_Foot.position, Vector2.down, rayDistance, whatIsGround);
        RaycastHit2D R_Hit = Physics2D.Raycast(R_Foot.position, Vector2.down, rayDistance, whatIsGround);

        Debug.DrawRay(L_Foot.position, Vector2.down * rayDistance, Color.blue, 0.25f);
        Debug.DrawRay(R_Foot.position, Vector2.down * rayDistance, Color.blue, 0.25f);

        return L_Hit.collider != null && L_Hit.collider.CompareTag("Ground") ||
                R_Hit.collider != null && R_Hit.collider.CompareTag("Ground");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.tag;

        if (!string.IsNullOrEmpty(QuestItems_Target_Tag) && tag.Equals(QuestItems_Target_Tag) && QuestItems_Collected < QuestItems_Target_Count)
        {
            UpdateQuest(other.gameObject);
        }

        switch (tag)
        {
            case "HP_Plus":
                RestoreHealth(other.gameObject);
                break;
            case "Apple":
                Instantiate(Apple_Particles, other.transform.position, Quaternion.identity);
                Destroy(other.gameObject);
                break;
        }

    }
    public void UpdateQuest(GameObject QuestItem)
    {
        if (QuestItem != null)
        {
            QuestItems_Collected++;

            PlaySound(Pickup_Sounds, Quest_Pickup_Volume);
        }
        Quest_Counter.text = $"{QuestItems_Collected}/{QuestItems_Target_Count}";
    }
    private void PlaySound(AudioClip[] sounds, float volume)
    {
        Audio_Source.pitch = Random.Range(0.75f, 1.25f);
        int RandomIndex = Random.Range(0, sounds.Length);
        Audio_Source.PlayOneShot(sounds[RandomIndex], volume);
    }
    private void RestoreHealth(GameObject restorePickup)
    {
        if (HP_Current < HP_Max)
        {
            HP_Current += restorePickup.GetComponent<HP_Pickup>().HP_Amount;
            if (HP_Current > HP_Max) HP_Current = HP_Max;

            UpdateHealthBar();

            Destroy(restorePickup);
            PlaySound(Pickup_Sounds, Pickup_Volume);
        }
        else
        {
            return;
        }
    }
}
