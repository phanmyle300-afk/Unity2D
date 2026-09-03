using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MainCharacterController : MonoBehaviour
{
    [Header("Run Settings")]
    public float runVectorMagnitude = 5f;

    [Header("Jump Settings")]
    public float jumpInputY = 1f;
    public float jumpVectorMagnitude = 10f;
    public float jumpDuration = 0.2f;
    private float jumpTimer = 0f;
    public float gravityScaleWhileJumping = 0.5f;
    public float gravityScaleWhileFalling = 5f;

    [Header("Wall Cling Settings")]
    public float wallClingDuration = 1f;
    private float wallClingTimer = 0f;

    [Header("Ladder Movement Settings")]
    private float ladderMoveInputY;
    public float ladderMoveVectorMagnitude = 1f;

    [Header("Dash Settings")]
    public float dashInputX = 1f;
    public float dashVectorMagnitude = 1f;
    public float dashDurantion = 0.1f;
    private float dashTimer = 0f;
    public float dashCooldown = 1f;
    private float dashCooldownTimer = 0f;

    private Rigidbody2D playerRigidBody2D;
    private SpriteRenderer playerSpriteRenderer;
    private GameObject StairsHelper;
    private Animator playerAnimator;
    private TrailRenderer trailRenderer;

    private bool isOnGround = false;
    private bool isJumping = false;
    private bool isOnWall = false;
    private bool isOnLadder = false;
    private bool isDashing = false;

    // Biến di chuyển nút cảm ứng Mobile UI
    private float mobileRunInputX = 0f;

    private void Start()
    {
        playerRigidBody2D = GetComponent<Rigidbody2D>();
        playerSpriteRenderer = GetComponent<SpriteRenderer>();
        StairsHelper = GameObject.FindWithTag("StairsHelper");
        playerAnimator = GetComponent<Animator>();
        
        if (playerRigidBody2D != null)
        {
            playerRigidBody2D.gravityScale = 20f;
            playerRigidBody2D.constraints = RigidbodyConstraints2D.FreezeRotation;
        }

        trailRenderer = GetComponentInChildren<TrailRenderer>();
        if (trailRenderer != null)
        {
            trailRenderer.enabled = false;
        }
    }

    private void Update()
    {
        HandleJumpTimer();
        Run();
        Jump();
        HandleWallClingTimer();
        LadderMovement();
        DashCooldownTimer();
        dashMovement();
        DashTimer();
    }

    private void Run()
    {
        if (!isOnWall && !isDashing)
        {
            float keyboardInput = Input.GetAxisRaw("Horizontal");
            float currentInput = (keyboardInput != 0) ? keyboardInput : mobileRunInputX;

            if (playerAnimator != null)
            {
                playerAnimator.SetFloat("isRun", Mathf.Abs(currentInput));
            }

            if (currentInput == 0)
            {
                playerRigidBody2D.velocity = new Vector2(0f, playerRigidBody2D.velocity.y);
                return;
            }

            if (currentInput < 0) transform.rotation = Quaternion.Euler(0, 180, 0);
            else if (currentInput > 0) transform.rotation = Quaternion.Euler(0, 0, 0);

            playerRigidBody2D.velocity = new Vector2(currentInput * runVectorMagnitude, playerRigidBody2D.velocity.y);

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
            ExecuteJump();
        }
    }

    private void ExecuteJump()
    {
        if (isOnGround || isOnWall)
        {
            if (playerAnimator != null) playerAnimator.SetTrigger("isJumping");
            playerRigidBody2D.gravityScale = gravityScaleWhileJumping;

            float y = jumpInputY * jumpVectorMagnitude;
            playerRigidBody2D.AddForce(new Vector2(playerRigidBody2D.velocity.x, y), ForceMode2D.Impulse);

            isJumping = true;
            isOnGround = false;
        }
    }

    // ==========================================
    // CÁC HÀM CẢM ỨNG MOBILE (EVENT TRIGGER)
    // ==========================================

    public void PointerDownLeft()
    {
        mobileRunInputX = -1f;
    }

    public void PointerDownRight()
    {
        mobileRunInputX = 1f;
    }

    public void PointerUpMove()
    {
        mobileRunInputX = 0f;
        if (playerRigidBody2D != null)
        {
            playerRigidBody2D.velocity = new Vector2(0f, playerRigidBody2D.velocity.y);
        }
    }

    public void PointerDownJump()
    {
        ExecuteJump();
    }

    // SỬA TẠI ĐÂY: KÍCH HOẠT ANIMATOR TRỰC TIẾP ĐỂ TRÁNH LỖI CS1061
    public void PointerDownPunch()
    {
        if (playerAnimator != null)
        {
            playerAnimator.ResetTrigger("isAttacking");
            playerAnimator.SetTrigger("isAttacking");
        }
    }

    // ==========================================

    private void LadderMovement()
    {
        if (isOnLadder)
        {
            ladderMoveInputY = Input.GetAxis("Vertical");
            float y = ladderMoveInputY * ladderMoveVectorMagnitude;

            if (playerAnimator != null)
            {
                playerAnimator.SetBool("isMoveingStair", y != 0);
            }

            if (y != 0 && playerSpriteRenderer != null)
            {
                playerSpriteRenderer.sortingOrder = 2;
            }

            playerRigidBody2D.velocity = new Vector2(playerRigidBody2D.velocity.x, y);
        }
    }

    private void dashMovement()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && dashCooldownTimer <= 0)
        {
            isDashing = true;
            dashCooldownTimer = dashCooldown;

            if (trailRenderer != null) trailRenderer.enabled = true;

            float x = dashInputX * dashVectorMagnitude;
            Vector2 dashVector2 = Vector2.zero;

            if (playerRigidBody2D.velocity.x < 0) dashVector2 = new Vector2(-x, 0);
            else if (playerRigidBody2D.velocity.x > 0) dashVector2 = new Vector2(x, 0);

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
                if (trailRenderer != null) trailRenderer.enabled = false;
            }
        }
    }

    private void DashCooldownTimer()
    {
        if (dashCooldownTimer > 0) dashCooldownTimer -= Time.deltaTime;
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
            if (wallClingDuration < wallClingTimer || isJumping)
            {
                wallClingTimer = 0;
                playerRigidBody2D.gravityScale = gravityScaleWhileFalling;
                isOnWall = false;
                if (playerAnimator != null) playerAnimator.SetBool("isHandlingWall", false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isOnGround = true;
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            if (!isOnGround && (playerRigidBody2D.velocity.y != 0 || isJumping))
            {
                if (playerAnimator != null) playerAnimator.SetBool("isHandlingWall", true);
                playerRigidBody2D.gravityScale = 0;
                playerRigidBody2D.velocity = Vector3.zero;
                jumpTimer = 0;
                isOnWall = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            if (playerAnimator != null) playerAnimator.SetBool("isOnStair", true);
            if (StairsHelper != null) StairsHelper.GetComponent<TilemapCollider2D>().isTrigger = true;
            isOnLadder = true;
            playerRigidBody2D.gravityScale = 0;
            playerRigidBody2D.velocity = new Vector2(playerRigidBody2D.velocity.x, 0);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Stairs"))
        {
            if (playerAnimator != null) playerAnimator.SetBool("isOnStair", false);
            if (StairsHelper != null) StairsHelper.GetComponent<TilemapCollider2D>().isTrigger = false;
            isOnLadder = false;
            playerRigidBody2D.gravityScale = 1;
            if (playerSpriteRenderer != null) playerSpriteRenderer.sortingOrder = -1;
            playerRigidBody2D.velocity = new Vector2(playerRigidBody2D.velocity.x, 0);
        }
    }
}