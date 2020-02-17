using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //movement & jump
    private float movementInputDirection;
    public float movementSpeed = 5f;
    private bool isFacingRight = true;
    public float jumpForce = 30f;

    // groundcheck
    private float moveInput;
    private bool isGrounded;
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

    private void CheckMovementDirection()
    {
        if (isFacingRight && movementInputDirection < 0)
        {
            Flip();
        }
        else if(!isFacingRight && movementInputDirection > 0)
        {
            Flip();
        }
    }

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
    }

    private void ApplyMovement()
    {
        playerRigidbody.velocity = new Vector2(movementSpeed * movementInputDirection, playerRigidbody.velocity.y);
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
