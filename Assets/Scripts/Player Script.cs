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
    public GameObject enemy;

    public float PlayerMovementSpeed, PlayerJumpHeight, PlayerSlidSpeed, rayLength;
    public int MaxJumps;
    private int jumps = 1;
    private bool isGrounded, attack;
    float bounceTime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        helper = gameObject.AddComponent<HelperScript>();

        attack = false;

        bounceTime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMove();
        PlayerJump();
        DoGroundCheck();
        Attack();
    }

    void PlayerMove()
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

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) == true && isGrounded == true || (Input.GetKeyDown(KeyCode.Space) == true && jumps < MaxJumps))
        {
            rb.AddForce(new Vector2(rb.velocity.x, PlayerJumpHeight), ForceMode2D.Impulse);
            jumps++;
            anim.SetBool("Jump", true);
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
            print("Attack Idle");
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
        attack = false;
        anim.SetBool("Attack", false);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        float px = transform.position.x;
        float py = transform.position.y;
        float ex = enemy.transform.position.x;
        float ey = enemy.transform.position.y;

        if (other.gameObject.tag == "Enemy")
        {
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

        if (other.gameObject.tag == "Ground")
        {
            anim.SetBool("Jump", false);
        }
    }
}
