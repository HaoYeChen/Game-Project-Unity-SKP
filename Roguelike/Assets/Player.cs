using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float movementSpeed = 5f;

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

    }

    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

        moveInput = Input.GetAxis("Horizontal");
        playerRigidbody.velocity = new Vector2(moveInput * movementSpeed, playerRigidbody.velocity.y);
    }
}
