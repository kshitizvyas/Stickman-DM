using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    private float HorizontalInputDirection;
    private float VerticalInputDirection;
    private float turnTimer;

    private int amountOfJumpsLeft;
    private int facingDirection = 1;

    private bool isFacingRight = true;
    private bool isLeft = false;
    private bool isWalking;
    private bool isGrounded;
    private bool canJump;
    private bool canMove;
    private bool canFlip;

    private Rigidbody2D rb;
    private Animator anim;
    public int Health = 5;
    public int amountOfJumps = 1;
    [SerializeField]
    private int GravityScale;

    public float movementSpeed;
    public float jumpForce;
    public float groundCheckRadius;
    public float airDragMultiplier = 0.95f;
    public float variableJumpHeightMultiplier = 0.5f;
    public float turnTimerSet = 0.1f;
    public GameObject[] Heart;


    public Transform groundCheck;

    public LayerMask whatIsGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        amountOfJumpsLeft = amountOfJumps;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
        if (Health <= 0)
        {
            //game end
            Destroy(gameObject);

        }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckSurroundings();
    }


    private void CheckSurroundings()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);



    }

    private void CheckIfCanJump()
    {
        if ((isGrounded && rb.velocity.y <= 0))
        {
            amountOfJumpsLeft = amountOfJumps;
        }

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
        if (isFacingRight && HorizontalInputDirection < 0)
        {
            Flip();
        }
        else if (!isFacingRight && HorizontalInputDirection > 0)
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
        //anim.SetBool("isWalking", isWalking);
        //anim.SetBool("isGrounded", isGrounded);
        //anim.SetFloat("yVelocity", rb.velocity.y);
        //anim.SetBool("isWallSliding", isWallSliding);
    }

    private void CheckInput()
    {
        HorizontalInputDirection = Input.GetAxisRaw("Horizontal");
        VerticalInputDirection = Input.GetAxisRaw("Vertical");


        if (Input.GetButtonDown("Horizontal"))
        {
            if (!isGrounded && HorizontalInputDirection != facingDirection)
            {
                canMove = false;
                canFlip = false;
            }
        }

        switch (Health)
        {
            case 0:
                Heart[0].SetActive(false);
                break;
            case 1:
                Heart[1].SetActive(false);
                break;
            case 2:
                Heart[2].SetActive(false);
                break;
            case 3:
                Heart[3].SetActive(false);
                break;
            case 4:
                Heart[4].SetActive(false);
                break;
            default:
                break;
        }

        if (!canMove)
        {
            turnTimer -= Time.deltaTime;

            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * variableJumpHeightMultiplier);
        }


    }

    private void Jump()
    {
        if (canJump )
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
        //else if (isWallSliding && HorizontalInputDirection == 0 && canJump) //Wall hop
        //{
        //    isWallSliding = false;
        //    amountOfJumpsLeft--;
        //    Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDirection.x * -facingDirection, wallHopForce * wallHopDirection.y);
        //    rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        //}
        //else if ((isWallSliding || isTouchingWall) && HorizontalInputDirection != 0 && canJump) // Wall Jump
        //{
        //    isWallSliding = false;
        //    amountOfJumpsLeft--;
        //    Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDirection.x * HorizontalInputDirection, wallJumpForce * wallJumpDirection.y);
        //    rb.AddForce(forceToAdd, ForceMode2D.Impulse);
        //}
    }

    private void ApplyMovement()
    {

        if (!isGrounded && HorizontalInputDirection == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x * airDragMultiplier, rb.velocity.y);
        }
        else if (canMove)
        {
            rb.velocity = new Vector2(movementSpeed * HorizontalInputDirection, rb.velocity.y);
        }

    }

    private void Flip()
    {
        if (canFlip)
        {
            facingDirection *= -1;
            isFacingRight = !isFacingRight;
            //    transform.Rotate(0.0f, 180.0f, 0.0f);
            if (!isLeft)
            {
                this.transform.GetComponent<SpriteRenderer>().flipX = true;
                isLeft = true;
            }
            else
            {
                this.transform.GetComponent<SpriteRenderer>().flipX = false;
                isLeft = false;
            }
        }
        else
        {

        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

    }
}
