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
    private bool isGrounded;

    
    
    // groundcheck
    public Transform groundCheck;
    public float checkRadius = 0.5f;
    public LayerMask whatIsGround;

    Rigidbody2D playerRigidbody;


    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();

    }


    private void Update()
    {
        CheckInput();
        CheckMovementDirection();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);
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
        playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
    }

    private void ApplyMovement() //Movement function
    {
        playerRigidbody.velocity = new Vector2(movementSpeed * movementInputDirection, playerRigidbody.velocity.y);
    }

    private void Flip() //Sprite turning left changes Y to 180°, turning right changes X back to 0°
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
