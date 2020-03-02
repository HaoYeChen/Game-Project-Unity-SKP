﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //movement
    private float movementInputDirection;
    public float movementSpeed = 5f;

    //jump
    public float jumpForce = 15f;
    public int amountOfJumps = 1;
    private int amountOfJumpsLeft;

    //bool
    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;
    private bool isTouchingWall;
    private bool isWallSliding;



    //groundcheck
    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;
    public LayerMask whatIsGround;

    //wallcheck
    public Transform wallCheck;
    public float wallCheckDistance;

    //compponent
    private Rigidbody2D rb;
    private Animator anim;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    //Runs once per frame
    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        CheckIfWallSliding();
    }

    //Can run once, zero or several times per frame
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    //Check if wall sliding
    private void CheckIfWallSliding()
    {
        if (isTouchingWall && !isGrounded && rb.velocity.y <0)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }
    }


    //Ground check
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallCheck.position, transform.right, wallCheckDistance, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        //isGround & rb velocity y less than 0
        if (isGrounded && rb.velocity.y <= 0)
        {
            amountOfJumpsLeft = amountOfJumps;
        }

        //Amount of jumps less than 0 no jump
        if (amountOfJumpsLeft <= 0)
        {
            canJump = false;
        }
        else
        {
            canJump = true;
        }
    }
    private void CheckMovementDirection()
    {
        //Lesser than 0
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        //Greaster than 0
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }

        //rb velocity x is changing and not = 0
        if (rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    //Animations
    private void UpdateAnimations()
    {
        //Walking animation
        anim.SetBool("isWalking", isWalking);
        //Grounded
        anim.SetBool("isGround", isGrounded);
        //y velocity 0.0
        //Checks jump animation jump1=0
        anim.SetFloat("yVelocity", rb.velocity.y);
    }


    private void CheckInput() //All Inputs from the player
    {
        //move input
        movementInputDirection = Input.GetAxisRaw("Horizontal");

        //jump input
        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Jump() //Jump function
    {
        if (canJump)
        {
            // x and jumpforce
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            
            //everytime jumps -1
            amountOfJumpsLeft--; 
        }
        
    }

    private void ApplyMovement() //Movement function
    {
        rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
    }

    private void Flip() //Sprite turning left changes Y to 180°, turning right changes X back to 0°
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
