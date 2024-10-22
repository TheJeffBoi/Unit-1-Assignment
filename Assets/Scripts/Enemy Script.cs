using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;
using System;

public class EnemyScript : MonoBehaviour
{
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sr;
    HelperScript helper;
    public float maxSpeed, rayLength;


    public GameObject player;
    PlayerScript ps;

    private float moveSpeed;
    private bool playerNear;
    int state;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        helper = gameObject.AddComponent<HelperScript>();
        sr = GetComponent<SpriteRenderer>();
        ps = player.gameObject.GetComponent<PlayerScript>();

        playerNear = false;
        moveSpeed = maxSpeed;

        state = 0; //0 = patrol, 1=follow, 2=attack
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        State();
    }

    void State()
    {
        if (state == 0)
        {
            Patrol(); //patrol code
        }

        if (state == 1)
        {
            FollowPlayer(); //follow code
        }   
    }

    void Attack()
    {
        System.Random randomDelay = new System.Random();
        int attackDelay = randomDelay.Next(3, 5);
        helper.Delay(attackDelay);
        //state = 1; // attack player then set state to 1 
    }

    void FollowPlayer()
    {
        anim.SetBool("Run", true);

        if (playerNear == false)
        {
            state = 0;
        }
        else
        {
            // if player touches enemy set state to 2 else set state to 0
            float px = player.transform.position.x;
            float ex = transform.position.x;

            if (px < ex)
            {
                moveSpeed = -maxSpeed;
                sr.flipX = true;
            }
            else if (px > ex)
            {
                moveSpeed = +maxSpeed;
                sr.flipX = false;
            }
            else if (px == ex)
            {
                sr.flipX = false;
            }
        }
    }

    void Patrol()
    {
        if (playerNear == true)
        {
            state = 1;
        }
        else
        {
            if (moveSpeed < 0)
            {
                if (helper.ExtendedRayEdgeCheck(-0.35f, 0, rayLength) == false)
                {
                    moveSpeed = maxSpeed;
                    sr.flipX = false;
                }
            }
            else
            {
                if (helper.ExtendedRayEdgeCheck(0.35f, 0, rayLength) == false)
                {
                    moveSpeed = -maxSpeed;
                    sr.flipX = true;
                }
            }
        // if player near set state to 1
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
            playerNear = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Player")
        {
           playerNear = false;
        }
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            ps.DoBounce();
        }
    }

}