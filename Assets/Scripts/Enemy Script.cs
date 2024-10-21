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
    public float maxSpeed;


    public GameObject player;

    private float moveSpeed;
    private bool playerNear, playerHit;

    int state;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        helper = gameObject.AddComponent<HelperScript>();
        sr = GetComponent<SpriteRenderer>();

        playerNear = false;
        playerHit = false;
        moveSpeed = maxSpeed;

        state = 0; //0 = patrol, 1=follow, 2=attack
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(moveSpeed, rb.velocity.y);

        Patrol();
    }

    /*void State()
    {
        if (state == 0)
        {
            Patrol(); //patrol code
        }

        if (state == 1)
        {
            FollowPlayer(); //follow code
        }

        if (state == 2)
        {
            Attack(); //attack code
        }
    }*/

    void Attack()
    {
        System.Random randomDelay = new System.Random();
        int attackDelay = randomDelay.Next(1, 3);
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
                print("moving left");
                moveSpeed = -maxSpeed;
            }
            else
            {
                print("moving right");
                moveSpeed = +maxSpeed;

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
                if (helper.ExtendedRayEdgeCheck(-0.35f, 0, 2f) == false)
                {
                    moveSpeed = maxSpeed;
                    sr.flipX = false;
                }
            }
            else
            {
                if (helper.ExtendedRayEdgeCheck(0.35f, 0, 2f) == false)
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
            playerHit = true;
        }
    }

}