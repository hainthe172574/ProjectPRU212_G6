using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;


    [Header("Double Jump")]
    [SerializeField] private float doubleJumpForce;

    private bool canDoubleJump;

    [Header("Collision Info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    private bool isAirborne;
    private float xInput;

    private bool facingRinght = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    private void Update()
    {
        UpdateAirbornStatus();
        HandleCollision();
        HandleInput();
        HandleMovement();
        HandleFlip();
        HandleAnimations();
    }

    private void UpdateAirbornStatus()
    {
        if (isGrounded && isAirborne)
        {
            HandleLanding();
        }
        if (!isGrounded && !isAirborne)
        {
            BecomeAirborne();
        }
    }

    private void BecomeAirborne()
    {
        isAirborne = true;
    }

    private void HandleLanding()
    {
        isAirborne = false;
        canDoubleJump = true;
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");


        if (Input.GetKeyDown(KeyCode.Space) )
        {
            JumpButton();
        }
    }
    private void JumpButton()
    {
        if (isGrounded) 
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }
    private void DoubleJump()
    {
        canDoubleJump=false;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x,doubleJumpForce);
    }

    private void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);

    }

    private void HandleAnimations()
    {
        anim.SetFloat("xVelocity", rb.linearVelocityX);
        anim.SetFloat("yVelocity", rb.linearVelocityY);
        anim.SetBool("isGrounded", isGrounded);
    }

    private void HandleMovement()
    {
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocityY);
    }
    private void HandleFlip()
    {
        if(rb.linearVelocityX<0 && facingRinght|| rb.linearVelocityX > 0 && !facingRinght)
        {
            Flip();
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x,transform.position.y -groundCheckDistance));
    }
    public void Flip()
    {
        rb.transform.Rotate(0, 180, 0);
        facingRinght = !facingRinght;
    }
}
