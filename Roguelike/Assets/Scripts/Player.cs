using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //movement
    private float movementInputDirection;
    public float movementSpeed = 5f;
    public float movementForceInAir;
    public float airDragMultiplier = 0.95f;

    //jump
    public float jumpForce = 15f;
    public int amountOfJumps = 1;
    private int amountOfJumpsLeft;
    public float variableJumpHeightMultiplier = 0.5f;

    //bool
    private bool isFacingRight = true;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;
    private bool isTouchingWall;
    private bool isWallSliding;



    //groundcheck
    public Transform groundCheck;
    public float groundCheckRadius = 0.1f;
    public LayerMask whatIsGround;

    //wallcheck
    public Transform wallCheck;
    public float wallCheckDistance;
    public float wallSlideSpeed;

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
        //If touching wall and not touching ground and rb velocity y is less than 0, wall sliding become true, else false.
        if (isTouchingWall && !isGrounded && rb.velocity.y < 0) 
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

        //Wall Sliding
        anim.SetBool("isWallSliding", isWallSliding);
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

        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
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

    private void ApplyMovement() 
    {
        if (isGrounded)
        {
            //Movement function
            rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
        }
        //Not grounded & not wall slidning & movement input direction is not equel to 0
        else if (!isGrounded && !isWallSliding && movementInputDirection != 0)
        {
            // vector2 x direction = movementForceInAir * movementInputDirection, 0 y direction cuz already moving in x direction
            Vector2 forceToAdd = new Vector2(movementForceInAir * movementInputDirection, 0);
            rb.AddForce(forceToAdd);

            if (Mathf.Abs(rb.velocity.x) > movementSpeed)
            {
                rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
            }
        }
        //not grounded & not wall sliding & movement input direcion is equal to 0
        else if (!isGrounded && !isWallSliding && movementInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        

        if (isWallSliding) 
        {
            //rb velocity y is less than -wall slide speed
            if (rb.velocity.y < -wallSlideSpeed)
            {
                rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
            }
        }
    }

    private void Flip() 
    {
        //If wall sliding cant flip, If not wall sliding sprite turning left changes Y to 180°, turning right changes X back to 0°
        if (!isWallSliding) 
        {
            isFacingRight = !isFacingRight;
            transform.Rotate(0.0f, 180.0f, 0.0f);
        }
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance, wallCheck.position.y, wallCheck.position.z));
        
    }
}
