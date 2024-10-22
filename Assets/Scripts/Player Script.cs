using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    HelperScript helper;
    HealthManager manager;
    public GameObject enemy;

    public float PlayerMovementSpeed, PlayerJumpHeight, PlayerSlidSpeed, rayLength;
    public int MaxJumps;
    private int jumps = 1;
    private bool isGrounded, attack, start;
    float bounceTime;

    // Start is called before the first frame update
    void Start()
    {
        start = true;

        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        helper = gameObject.AddComponent<HelperScript>();
        manager = gameObject.AddComponent<HealthManager>();

        attack = false;

        bounceTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerStart();
        PlayerMove();
        PlayerJump();
        DoGroundCheck();
        Attack();
    }

    void PlayerStart()
    {
        if (Input.anyKey == true && start == true)
        {
            anim.SetBool("OnFloor", true);
            anim.SetBool("GetUp", true);
        }
    }

    
    public void StartEnd()
    {
        anim.SetBool("OnFloor", false);
        start = false;
    }

    void PlayerMove()
    {
        if (start == false)
        {
            if (bounceTime > 0)
            {
                bounceTime -= Time.deltaTime;
            }
            else if (attack == true)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            else
            {

                if (Input.GetKey(KeyCode.A) || (Input.GetKey(KeyCode.LeftArrow) == true))
                {
                    rb.velocity = new Vector2(-PlayerMovementSpeed, rb.velocity.y);
                    anim.SetBool("Run", true);
                    sr.flipX = false;
                }

                else if (Input.GetKey(KeyCode.D) || (Input.GetKey(KeyCode.RightArrow) == true))
                {
                    rb.velocity = new Vector2(PlayerMovementSpeed, rb.velocity.y);
                    anim.SetBool("Run", true);
                    sr.flipX = true;
                }

                else
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                    anim.SetBool("Run", false);
                }
            }
        }
    }

    void PlayerJump()
    {
        if (start == false && attack == false)
        {
            if (Input.GetKeyDown(KeyCode.Space) == true && isGrounded == true || (Input.GetKeyDown(KeyCode.Space) == true && jumps < MaxJumps))
            {
                rb.AddForce(new Vector2(rb.velocity.x, PlayerJumpHeight), ForceMode2D.Impulse);
                jumps++;
                anim.SetBool("Jump", true);
            }
        }
    }

    void DoGroundCheck()
    {
        if (helper.ExtendedRayCollisionCheck(0, 0, rayLength) == true)
        {
            isGrounded = true;
            jumps = 1;
        }
        else
        {
            isGrounded = false;
        }
    }

    void Attack()
    {
        if (Input.GetKey(KeyCode.S) == true)
        {
            attack = true; 
            anim.SetBool("Attack Idle", true);
        }
        if (Input.GetKeyUp(KeyCode.S) == true)
        {
            anim.SetBool("Attack Idle", false);
            anim.SetBool("Attack", true);
        }
    }

    public void AttackFinished()
    {
        print("attack");
        attack = false;
        anim.SetBool("Attack", false);
    }

    public void DoBounce()
    {
        float px = transform.position.x;
        float py = transform.position.y;
        float ex = enemy.transform.position.x;
        float ey = enemy.transform.position.y;

        anim.SetBool("Hurt", true);
        manager.TakeDamage(10);
        print("Damage Taken");

        if (py > ey + 0.8)
        {
            rb.AddForce(new Vector2(rb.velocity.x, 8), ForceMode2D.Impulse);
            bounceTime = 0.3f;
        }
        else
        {
            if (px < ex)
            {
                rb.AddForce(new Vector2(-8, rb.velocity.y), ForceMode2D.Impulse);
                bounceTime = 0.3f;
            }
            else if (px > ex)
            {
                rb.AddForce(new Vector2(8, rb.velocity.y), ForceMode2D.Impulse);
                bounceTime = 0.3f;
            }
        }
    }

    public void DamageEnd()
    {
        anim.SetBool("Hurt", false);
    }
}
