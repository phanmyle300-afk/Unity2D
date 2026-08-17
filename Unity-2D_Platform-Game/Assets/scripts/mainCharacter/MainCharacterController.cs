using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainCharacterController : MonoBehaviour
{
    [Header("Run Settings")]
    private float runInputX;
    public float runVectorMagnitude = 1;
    private Vector2 runVector2;

    [Header("Jump Settings")]
    public float jumpInputY;
    public float jumpVectorMagnitude;
    private Vector2 jumpVector2;
    public float jumpDuration = 0.2f;
    private float jumpTimer = 0;
    public float gravityScaleWhileJumping = 0.5f;
    public float gravityScaleWhileFalling = 5;

    [Header("Wall Cling Settings")]
    public float wallClingDuration = 1f;
    private float wallClingTimer = 0;

    [Header("Ladder Movement Settings")]
    private float ladderMoveInputY;
    public float ladderMoveVectorMagnitude = 1f;
    private Vector2 ladderMoveVector2;

    [Header("Dash Settings")]
    public float dashInputX = 1;
    public float dashVectorMagnitude = 1;
    private Vector2 dashVector2;
    public float dashDurantion = 0.1f;
    private float dashTimer = 0;
    public float dashCooldown = 1f; // Cooldown süresi
    private float dashCooldownTimer = 0; // Cooldown zamanlayıcısı

    private Rigidbody2D playerRigidBody2D;
    private SpriteRenderer playerSpriteRenderer;
    private GameObject StairsHelper;
    private Animator playerAnimator;
    private TrailRenderer trailRenderer; // Trail Renderer reference

    private bool isOnGround = false;
    private bool isJumping = false;
    private bool isOnWall = false;
    private bool isOnLadder = false;
    private bool isDashing = false;

    private void Start()
    {
        playerRigidBody2D = this.gameObject.GetComponent<Rigidbody2D>();
        playerSpriteRenderer = this.gameObject.GetComponent<SpriteRenderer>();
        StairsHelper = GameObject.FindWithTag("StairsHelper");
        playerAnimator = this.gameObject.GetComponent<Animator>();
        playerRigidBody2D.gravityScale = 20f;
        trailRenderer = this.gameObject.GetComponentInChildren<TrailRenderer>(); // Assuming the Trail Renderer is a child of the character
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false; // Ensure it's disabled initially
        }
    }

    private void Update()
    {
        HandleJumpTimer();
        Run();
        Jump();
        HandleWallClingTimer();
        LadderMovement();
        DashCooldownTimer(); // Cooldown zamanlayıcısını güncelle
        dashMovement();
        DashTimer();
    }

    private void Run()
    {
        if (!isOnWall && !isDashing)
        {
            runInputX = Input.GetAxis("Horizontal");
            playerAnimator.SetFloat("isRun", Mathf.Abs(runInputX));
            float x = runInputX * runVectorMagnitude;

            if (runInputX < 0)
            {
                this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
            }
            else if (runInputX > 0)
            {
                this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
            }

            runVector2 = new Vector2(x, playerRigidBody2D.velocity.y);
            playerRigidBody2D.velocity = runVector2;

            if (playerRigidBody2D.velocity.y < 0 && !isOnLadder)
            {
                playerRigidBody2D.gravityScale = gravityScaleWhileFalling;
            }
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (isOnGround || isOnWall))
        {
            playerAnimator.SetTrigger("isJumping");
            playerRigidBody2D.gravityScale = gravityScaleWhileJumping;

            float y = jumpInputY * jumpVectorMagnitude;
            jumpVector2 = new Vector2(playerRigidBody2D.velocity.x, y);
            playerRigidBody2D.AddForce(jumpVector2,ForceMode2D.Impulse);
            

            isJumping = true;
            isOnGround = false;
        }
    }

    private void LadderMovement()
    {
        if (isOnLadder)
        {
            ladderMoveInputY = Input.GetAxis("Vertical");
            float y = ladderMoveInputY * ladderMoveVectorMagnitude;
            ladderMoveVector2 = new Vector2(playerRigidBody2D.velocity.x, y);

            if (y != 0)
            {
                playerSpriteRenderer.sortingOrder = 2;
                playerAnimator.SetBool("isMoveingStair",true);
            } else
            {
                playerAnimator.SetBool("isMoveingStair", false);
            }

            playerRigidBody2D.velocity = ladderMoveVector2;
        }
    }

    private void dashMovement()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && dashCooldownTimer <= 0) // Cooldown kontrolü
        {
            isDashing = true;
            dashCooldownTimer = dashCooldown; // Cooldown zamanlayıcısını başlat

            if (trailRenderer != null)
            {
                trailRenderer.enabled = true; // Enable Trail Renderer when dashing starts
            }

            float x = dashInputX * dashVectorMagnitude;

            if (playerRigidBody2D.velocity.x < 0)
            {
                dashVector2 = new Vector2(-x, 0);
            }
            else if(playerRigidBody2D.velocity.x > 0)
            {
                dashVector2 = new Vector2(x, 0);
            }
            
            playerRigidBody2D.AddForce(dashVector2, ForceMode2D.Impulse);
        }
    }

    private void DashTimer()
    {
        if (isDashing)
        {
            dashTimer += Time.deltaTime;

            if (dashDurantion < dashTimer)
            {
                isDashing = false;
                dashTimer = 0;
                playerRigidBody2D.velocity = Vector2.zero;

                if (trailRenderer != null)
                {
                    trailRenderer.enabled = false; // Disable Trail Renderer when dashing ends
                }
            }
        }
    }

    private void DashCooldownTimer()
    {
        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime; // Cooldown zamanlayıcısını azalt
        }
    }

    private void HandleJumpTimer()
    {
        if (isJumping)
        {
            jumpTimer += Time.deltaTime;

            if (jumpDuration < jumpTimer || Input.GetKeyUp(KeyCode.Space))
            {
                playerRigidBody2D.gravityScale = gravityScaleWhileFalling;
                jumpTimer = 0;
                isJumping = false;
            }
        }
    }

    private void HandleWallClingTimer()
    {
        if (isOnWall)
        {   
            wallClingTimer += Time.deltaTime;

            if (wallClingDuration < wallClingTimer)
            {
                wallClingTimer = 0;
                playerRigidBody2D.gravityScale = gravityScaleWhileFalling;
                isOnWall = false;
                playerAnimator.SetBool("isHandlingWall", false);
            }
            else if (isJumping)
            {
                wallClingTimer = 0;
                isOnWall = false;
                playerAnimator.SetBool("isHandlingWall", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Ground"))
        {
            isOnGround = true;
        }

        if (collision.gameObject.tag.Equals("Wall"))
        {
            print(playerRigidBody2D.velocity.y);
            if (!isOnGround && (playerRigidBody2D.velocity.y != 0 || isJumping))
            {
                playerAnimator.SetBool("isHandlingWall", true);
                playerRigidBody2D.gravityScale = 0;
                playerRigidBody2D.velocity = Vector3.zero;
                jumpTimer = 0;
                isOnWall = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Stairs"))
        {
            playerAnimator.SetBool("isOnStair", true);
            StairsHelper.GetComponent<TilemapCollider2D>().isTrigger = true;
            isOnLadder = true;
            playerRigidBody2D.gravityScale = 0;
            playerRigidBody2D.velocity = new Vector2(playerRigidBody2D.velocity.x, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Stairs"))
        {
            playerAnimator.SetBool("isOnStair", false);
            StairsHelper.GetComponent<TilemapCollider2D>().isTrigger = false;
            isOnLadder = false;
            playerRigidBody2D.gravityScale = 1;
            playerSpriteRenderer.sortingOrder = -1;
            playerRigidBody2D.velocity = new Vector2(playerRigidBody2D.velocity.x, 0);
        }
    }
}
