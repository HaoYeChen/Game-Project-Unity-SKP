using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //movement & jump
    private float movementInputDirection;
    public float movementSpeed = 5f;
    public float jumpForce = 15f;

    //bool
    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;



    // groundcheck
    public Transform groundCheck;
    public float groundCheckRadius = 0.5f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private Animator anim;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    //Runs once per frame
    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
    }

    //Can run once, zero or several times per frame
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }

    //Ground check
    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }

    private void CheckIfCanJump()
    {
        if (isGrounded && rb.velocity.y <= 0)
        {
            canJump = true;
        }
        else
        {
            canJump = false;
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

        if (rb.velocity.x != 0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking", isWalking);
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
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
