using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Movement2D : MonoBehaviour
{
    public GUIStyle mainHeaderStyle;
    public GUIStyle subHeaderStyle;


    [SerializeField] Movement2D.MovingType movingType;
    [SerializeField] Transform spriteTransform;
    [SerializeField] Rigidbody2D rb2;
    [SerializeField] CapsuleCollider2D capsuleCollider;
    Vector2 input;
    [HideInInspector] public float currentHorizontalSpeed;
    [HideInInspector] public float currentVerticalSpeed;
    [HideInInspector] public PlayerStates currentState;

    [Space]
    //[Header("SPEED VALUES")]
    [Range(1, 10)]
    [SerializeField] float movementSpeed = 4f;
    [Range(0.02f, 1)]
    [SerializeField] float speedUpAccelaration = 2f;
    [Range(0.02f, 1)]
    [SerializeField] float speedDownAccelaration = 3f;
    [Range(0.02f, 1)]
    [SerializeField] float stopAccelaration = 3f;



    [Space]
    //[Header("DASH")]
    [SerializeField] bool Dash;
    [SerializeField] KeyCode dashButton = KeyCode.LeftShift;
    [SerializeField] bool cancelDashOnWallHit;
    //[Header("-Dash Values")]
    [Range(1f, 5f)]
    [SerializeField] float dashDistance = 3f;
    [Range(0.1f, 10)]
    [SerializeField] float dashDuration = 0.3f;
    [Range(0f, 1)]
    [SerializeField] float dashStopEffect = 0.5f;

    //[Header("-Dash Settings")]
    [SerializeField] bool resetDashOnGround;
    [SerializeField] bool resetDashOnWall;
    [SerializeField] bool airDash;
    [SerializeField] bool dashCancelsGravity;
    [SerializeField] bool verticalDash;
    [SerializeField] bool horizontalDash;
    [SerializeField] Vector2 dashColliderScale;
    [SerializeField] Vector2 dashColliderOffset;
    [SerializeField] GameObject dashParticle;
    [Space]
    [SerializeField] float dashShakeDuration;
    [SerializeField] float dashShakeMagni;
    [SerializeField] float dashShakeFreq;

    bool canDash;
    Vector2 defaultColliderOffset;
    Vector2 defaulColliderSize;


    //DASH INFO
    [HideInInspector] public bool isDashing;
    float dashSpeed;
    float dashTimer;

    [Space]
    //[Header("WALL JUMP")]
    public bool WallJump;
    [SerializeField] Vector2 wallJumpVelocity;
    [Range(0.01f, 10f)]
    [SerializeField] float wallSlideSpeed = 0.5f;
    [SerializeField] bool isSlidingOnWall;


    [Space]
    //[Header("----Jumping Values----")]
    [Range(0.5f, 10f)]
    [SerializeField] float jumpHight = 1f;
    [Range(1f, 20f)]
    [SerializeField] float jumpVelocity = 10f;
    [Range(0.5f, 50f)]
    [SerializeField] float jumpUpAcceleration = 2.5f;
    [Range(0.5f, 50f)]
    [SerializeField] float jumpDownAcceleration = 4f;
    [Range(0.1f, 50f)]
    [SerializeField] float fallSpeedClamp = 50;
    [Range(1f, 20f)]
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float fallClamp;

    [Space]
    //[Header("----Jump Adjustments----")]
    [Range(0f, 1f)]
    [SerializeField] float coyoteTime = 0.15f;
    [Range(0f, 1f)]
    [SerializeField] float jumpBuffer = 0.1f;
    [Range(0f, 1f)]
    [SerializeField] float onAirControl = 1f;

    [Range(0f, 0.3f)]
    [SerializeField] float variableJumpHeightDuration = 0f;
    [Range(0f, 1f)]
    [SerializeField] float jumpReleaseEffect = 0f;
    [SerializeField] KeyCode jumpButton = KeyCode.Space;
    bool isHoldingJumpButton;
    float jumpHoldTimer;
    bool isForcingJump;

    //[Header("Ledge Climb")]
    [SerializeField] bool LedgeGrab;
    [SerializeField] bool autoClimbLedge;
    [SerializeField] float ledgeCheckOffset = 1f;
    [SerializeField] float ledgeCheckDistance = 1f;
    [SerializeField] LayerMask ledgeCheckLayer;
    public bool isLedge;
    Vector2 ledgePosition;
    [SerializeField] Vector2[] ledgeClimbPosList;
    [SerializeField] int ledgeClimpPosIndex;
    public bool isClimbingLedge;


    [Space]
    //[Header("----GroundCheck----")]
    [SerializeField] float groundCheckRayDistance = 1f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundCheckCircleRadius = 0.5f;

    [Space]
    //[Header("----CeilCheck----")]
    [SerializeField] float ceilCheckRayDistance = 0.1f;
    [SerializeField] LayerMask ceilLayer;
    [SerializeField] float ceilCheckCircleRadius = 0.5f;

    [Space]
    //[Header("----WallCheck----")]
    [SerializeField] float wallCheckRayDistance;
    [SerializeField] LayerMask wallCheckLayer;

    [Space]
    //[Header("JUPM DEBUG")]
    [SerializeField] float jumpToleranceTimer;
    [SerializeField] float fallToleranceTimer;
    public bool isGrounded;
    [SerializeField] bool onCeil;
    public bool canJump;

    [Space]

    //[Header("*****DEBUG*****")]
    public bool leftWallHit;
    public bool rightWallHit;
    public bool hitWall;
    [SerializeField] float dist;
    public bool isJumped;//to check if the player is on air because of jumping or falling
    public bool isPressedJumpButton;
    [Range(-1f, 1f)]
    [SerializeField] float verticalSpeedDebugger;
    [Range(-1f, 1f)]
    [SerializeField] float horizontalSpeedDebugger;

    float onAirControlMultiplier;

    

    private void Reset()
    {
        rb2 = GetComponent<Rigidbody2D>();
        rb2.freezeRotation = true;
        rb2.gravityScale = 0;
        rb2.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb2.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Start()
    {
        fallClamp = fallSpeedClamp;
    }

    public enum MovingType
    {
        TopDown,
        Platformer
    }

    public enum PlayerStates
    {
        None,
        Grounded,
        Jumping,
        Falling
    }

    void SwitchState()
    {
        PlayerStates newState;
        if (isGrounded)
        {
            newState = PlayerStates.Grounded;
        }
        else
        {

            if (currentVerticalSpeed >= 0)
            {
                newState = PlayerStates.Jumping;
            }
            else
            {
                newState = PlayerStates.Falling;
            }
        }
        if (newState != currentState)
        {
            currentState = newState;
        }
    }

    private void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        defaulColliderSize = capsuleCollider.size;
        defaultColliderOffset = capsuleCollider.offset;

    }
    private void Update()
    {
        if (movingType == MovingType.TopDown)
        {
            HandleTopDownMovement();
        }
        else if (movingType == MovingType.Platformer)
        {
            HandlePlatformerMovement();
        }

    }

    private void FixedUpdate()
    {
        if (movingType == MovingType.TopDown)
        {
            UpdateTopDownSpeed();
        }
        else if (movingType == MovingType.Platformer)
        {
            
            UpdatePlatformerSpeed();
        }
    }

    void HandlePlatformerMovement()
    {
        GetPlatformerInput();
        CheckSideWall();
        DoDash();
        CheckCeil();
        CheckGround();
        MoveTopDownPlayer();
        FlipThePlayer();
        CountDownJumpTolerance();
        SwitchState();
        CheckLedge();
    }
    void DoDash()
    {
        if (isDashing)
        {
            dashTimer -= Time.deltaTime;
            if (dashTimer <= 0 && !onCeil)
            {
                CancelDash();

            }
        }
    }

    void ClimbLedge()
    {
        isClimbingLedge = true;

    }

    public void UpdateLedgeClimbPosition()
    {

        //Vector2 _posOffset = new Vector2(spriteTransform.right.x * ledgeClimbPosList[ledgeClimpPosIndex].x,
        //    spriteTransform.up.y * ledgeClimbPosList[ledgeClimpPosIndex].y);

        //rb2.MovePosition((Vector2)transform.position + _posOffset);

        //ledgeClimpPosIndex++;
        //if (ledgeClimpPosIndex >= ledgeClimbPosList.Length)
        //{
        //    isClimbingLedge = false;
        //    capsuleCollider.isTrigger = false;
        //}
        transform.position = spriteTransform.position;
        Vector2 _posOffset = new Vector2(spriteTransform.right.x * ledgeClimbPosList[0].x,
            spriteTransform.up.y * ledgeClimbPosList[0].y);
        transform.position = (Vector2)transform.position + _posOffset;

        isClimbingLedge = false;

    }

    void CheckLedge()
    {
        if (LedgeGrab) 
        { 
            //CHECH THE LEDGE
            Vector2 _ledgeCheckOrigin = new Vector2(transform.position.x,transform.position.y + ledgeCheckOffset);
            RaycastHit2D _HitLedge = Physics2D.Raycast(_ledgeCheckOrigin, spriteTransform.right, ledgeCheckDistance, ledgeCheckLayer);
            bool _canGrabLedge = !_HitLedge && hitWall;


            if ( _canGrabLedge)
            {
                //GET LEDGE POSITION
                Vector2 _ledgeGroundCheckOrigin = _ledgeCheckOrigin + (Vector2)(spriteTransform.right * ledgeCheckDistance);
                RaycastHit2D _hit = Physics2D.Raycast(_ledgeGroundCheckOrigin, Vector2.down, ledgeCheckDistance, ledgeCheckLayer);

                if (_hit)
                {
                
                    if (!isLedge)
                    {
                        ledgePosition = (Vector2)transform.position - new Vector2(0f, _hit.distance - 0.1f);
                        isLedge = true; 
                        currentVerticalSpeed = 0f;
                        fallClamp = 0f;
                        rb2.MovePosition(ledgePosition);
                        if (autoClimbLedge)
                        {
                            ClimbLedge();

                        }
                    }
                    if (Input.GetKeyDown(KeyCode.W) && !autoClimbLedge)
                    {
                        ClimbLedge();
                    }

                }
            
            }
        
            else if (isLedge && !_canGrabLedge)
            {
                isLedge = false;
                isClimbingLedge = false;
                fallClamp = fallSpeedClamp;
            }

        }



    }

    void CancelDash()
    {
        if (isDashing)
        {
            isDashing = false;

            dashParticle.transform.SetParent(null);
            currentHorizontalSpeed *= dashStopEffect;
            currentVerticalSpeed *= dashStopEffect;

            capsuleCollider.offset = defaultColliderOffset;
            capsuleCollider.size = defaulColliderSize;
        }
    }

    void DashPressed()
    {
        if ( canDash && !isDashing && Dash && ( (horizontalDash && input.x != 0) || (verticalDash && input.y != 0) ))
        {

            if (!isGrounded) 
            {
                canDash = false;

            }
            CameraShake.instance.ShakeCamera(dashShakeDuration,dashShakeMagni,dashShakeFreq);
            isDashing = true;
            dashSpeed = (dashDistance) / dashDuration;
            dashParticle.SetActive(false);
            dashParticle.transform.SetParent(spriteTransform);
            dashParticle.transform.localPosition = Vector3.zero;
            dashParticle.transform.localRotation = Quaternion.identity;
            dashParticle.SetActive(true);

            dashTimer = dashDuration;

            Vector2 _dir = Vector2.zero;

            if (horizontalDash && !verticalDash)
            {
                _dir.x = input.x;
                currentHorizontalSpeed = dashSpeed * _dir.x;

                if (dashCancelsGravity)
                {
                    currentVerticalSpeed = dashSpeed * _dir.y;
                }
                //if (!dashCancelsGravity)
                //{

                //}
            }
            if (verticalDash && !horizontalDash)
            {
                _dir.y = input.y;

                currentHorizontalSpeed = dashSpeed * _dir.x;
                currentVerticalSpeed = dashSpeed * _dir.y;
            }
            if (horizontalDash && verticalDash) 
            {
                _dir = input.normalized;

                currentHorizontalSpeed = dashSpeed * _dir.x;
                currentVerticalSpeed = dashSpeed * _dir.y;
            }

            


            capsuleCollider.offset = dashColliderOffset;
            capsuleCollider.size = dashColliderScale;
            
        }
    }

    void HandleTopDownMovement()
    {
        GetTopDownInput();
        UpdateTopDownSpeed();
        CheckSideWall();
        MoveTopDownPlayer();
        FlipThePlayer();
    }

    void GetPlatformerInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        
        if (Input.GetButtonDown("Jump"))
        {
            PressJumpButton();

        }
        if (Input.GetButtonUp("Jump"))
        {
            isHoldingJumpButton = false;
        }
        if (Input.GetButton("Fire3"))
        {
            DashPressed();
            
        }
        Jump();
    }


    private void OnDrawGizmos()
    {
        if (isGrounded)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(transform.position - transform.up * groundCheckRayDistance, groundCheckCircleRadius);

        if (onCeil)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(transform.position + transform.up * ceilCheckRayDistance, ceilCheckCircleRadius);


        if (rightWallHit)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawRay(transform.position,transform.right * wallCheckRayDistance);

        if (leftWallHit)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawRay(transform.position, -transform.right * wallCheckRayDistance);
        Vector2 _ledgePos;
        if (isLedge)
        {
            Gizmos.color = Color.green;
            _ledgePos = new Vector2(transform.position.x, transform.position.y + ledgeCheckOffset);
            Vector2 _ledgeGroundCheckCenter = _ledgePos + (Vector2)(spriteTransform.right * ledgeCheckDistance);

            Gizmos.DrawRay(_ledgeGroundCheckCenter, Vector2.down * ledgeCheckDistance);
        }
        else
        {
            Gizmos.color = Color.red;
        }

        _ledgePos = new Vector2(transform.position.x, transform.position.y + ledgeCheckOffset);
        Gizmos.DrawRay(_ledgePos, spriteTransform.right * ledgeCheckDistance);


    }

    void CheckCeil()
    {
        RaycastHit2D hit2D = Physics2D.CircleCast(transform.position,ceilCheckCircleRadius,transform.up,ceilCheckRayDistance,ceilLayer);

        if (hit2D )
        {
            if (!onCeil)
            {
                onCeil = true;
                currentVerticalSpeed = 0;
            }
        }
        else
        {
            onCeil = false;
        }
    }

    void CheckSideWall()
    {
        rightWallHit = Physics2D.Raycast(transform.position,transform.right,wallCheckRayDistance,wallCheckLayer);
        leftWallHit = Physics2D.Raycast(transform.position, -transform.right, wallCheckRayDistance, wallCheckLayer);

        if (rightWallHit || leftWallHit)
        {
            if (!hitWall)
            {
                if (resetDashOnWall)
                {
                    canDash = true;
                }
                hitWall = true;
                currentHorizontalSpeed = 0;
                
            }

            if (WallJump && !isGrounded)
            {
                SlideOnWall(true);
            }

            if (cancelDashOnWallHit)
            {
                CancelDash();
            }
        }
        else
        {
            if (WallJump)
            {
                SlideOnWall(false);
                
                //fallSpeedClamp /= wallSlideSpeedEffect;
            }
            hitWall = false;
        }

    }

    void SlideOnWall(bool _sliding)
    {
        if (!isSlidingOnWall && _sliding)
        {
            isSlidingOnWall = true;
            fallClamp = wallSlideSpeed;
        }
        else if (isSlidingOnWall && !_sliding)
        {
            isSlidingOnWall = false;
            fallClamp = fallSpeedClamp;
        }
    }


    void CheckGround()
    {
        RaycastHit2D hit2D = Physics2D.CircleCast(transform.position, groundCheckCircleRadius, -transform.up, groundCheckRayDistance, groundLayer);
        if (hit2D)
        {
            if (!isGrounded)
            {
                isGrounded = true;
                fallClamp = fallSpeedClamp;
                canJump = true;
                canDash = resetDashOnGround;
                onAirControlMultiplier = 1;
                SlideOnWall(false);
            }

            //if (resetDashOnGround)
            //{
            //    canDash = true;
            //}
            if (currentVerticalSpeed <= 0)//to check if the player if grounded while falling
            {
                isJumped = false;
            }
        }
        else
        {
            if (isGrounded)
            {
                onAirControlMultiplier = onAirControl;
                isGrounded = false;
                

                if (!isJumped)
                {
                    fallToleranceTimer = coyoteTime;
                    if (!dashCancelsGravity)
                    {
                        CancelDash();
                    }
                }
            }
            

        }

        if (!isGrounded)
        {

            fallToleranceTimer -= Time.deltaTime;
            if (fallToleranceTimer <= 0)
            {
                if (!airDash)
                {
                    canDash = false;
                }
                canJump = false;
            }
        }
    }

    void PressJumpButton()
    {
        if (!isDashing)
        {
            isHoldingJumpButton = true;
            isPressedJumpButton = true;
            isForcingJump = true;
            jumpHoldTimer = variableJumpHeightDuration;
            jumpToleranceTimer = jumpBuffer;
        }
    }
    void Jump()
    {
        if (isForcingJump)
        {
            jumpHoldTimer -= Time.deltaTime;
            if (jumpHoldTimer <= 0)
            {
                isForcingJump = false;
            }
            if (!isHoldingJumpButton && isForcingJump)
            {
                currentVerticalSpeed *= jumpReleaseEffect;
                isForcingJump = false;
            }

        }

        if (canJump && isPressedJumpButton)
        {
            jumpVelocity = Mathf.Sqrt(2 * jumpUpAcceleration * jumpHight * gravity);

            isJumped = true;
            canJump = false;
            isPressedJumpButton = false;
            currentVerticalSpeed = jumpVelocity;
        }
        if (isPressedJumpButton && isSlidingOnWall && !isClimbingLedge)
        {
            jumpVelocity = Mathf.Sqrt(2 * jumpUpAcceleration * jumpHight * gravity);

            isJumped = true;
            canJump = false;
            isPressedJumpButton = false;
            currentVerticalSpeed = jumpVelocity * wallJumpVelocity.y;
            if (leftWallHit)
            {
                currentHorizontalSpeed = jumpVelocity * wallJumpVelocity.x;
            }
            else if (rightWallHit)
            {
                currentHorizontalSpeed = -jumpVelocity * wallJumpVelocity.x;
            }
        }
    }

    void CountDownJumpTolerance()
    {
        if (isPressedJumpButton)
        {
            jumpToleranceTimer -= Time.deltaTime;
            if (jumpToleranceTimer <= 0)
            {
                isPressedJumpButton = false;
            }
        }
    }

    void UpdatePlatformerSpeed()
    {
        if (!isDashing) 
        {
            if (input.x != 0)
            {
                if (input.x * currentHorizontalSpeed >= 0)
                {
                    if ((input.x > 0 && !rightWallHit) || (input.x < 0 && !leftWallHit))
                    {
                        float xDist = Mathf.Abs(input.x * movementSpeed - currentHorizontalSpeed);
                        currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, input.x * movementSpeed, (Time.deltaTime / speedUpAccelaration) * onAirControlMultiplier * movementSpeed);// /(xDist)
                        dist = xDist;
                    }


                }
                else
                {
                    float xDist = Mathf.Abs(0 - currentHorizontalSpeed);
                    currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, (Time.deltaTime / speedDownAccelaration) * onAirControlMultiplier * movementSpeed);
                    dist = xDist;
                }
            }
            else
            {
                float xDist = Mathf.Abs(0 - currentHorizontalSpeed);
                dist = xDist;
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, (Time.deltaTime / stopAccelaration) * onAirControlMultiplier * movementSpeed);
            }
            horizontalSpeedDebugger = currentHorizontalSpeed / movementSpeed;
        }
        
        //if (!isGrounded)
        //{
        //    if (currentVerticalSpeed >= 0)
        //    {
        //        currentVerticalSpeed -= jumpUpAcceleration * Time.deltaTime * jumpVelocity;
        //    }
        //    else if(currentVerticalSpeed < 0)
        //    {
        //        currentVerticalSpeed -= jumpDownAcceleration * Time.deltaTime * jumpVelocity;
        //    }
        //}

        if (!isGrounded && (!isDashing || (!dashCancelsGravity && isDashing)))
        {
            if (currentVerticalSpeed >= 0)
            {
                currentVerticalSpeed -= jumpUpAcceleration * Time.fixedDeltaTime * gravity;
            }
            else if (currentVerticalSpeed < 0)
            {
                currentVerticalSpeed -= jumpDownAcceleration * Time.fixedDeltaTime * gravity;
            }
            currentVerticalSpeed = Mathf.Clamp(currentVerticalSpeed,-fallClamp, 100f);
        }
        else if(isGrounded)
        {
            if (currentVerticalSpeed < 0)
            {
                currentVerticalSpeed = Mathf.MoveTowards(currentVerticalSpeed,0,jumpDownAcceleration * jumpVelocity * 3 * Time.fixedDeltaTime);
            }
        }
        verticalSpeedDebugger = currentVerticalSpeed / jumpUpAcceleration;
    }

//TopDown
    void FlipThePlayer()
    {
        if (currentHorizontalSpeed > 0 || (WallJump && isSlidingOnWall && rightWallHit))
        {
            spriteTransform.eulerAngles = new Vector3(0f,0f,0f);
        }
        else if (currentHorizontalSpeed < 0 || (WallJump && isSlidingOnWall && leftWallHit))
        {
            spriteTransform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }

    void GetTopDownInput()
    {
        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");
        input = input.normalized;
    }

    void UpdateTopDownSpeed()
    {
        if (input.x != 0)
        {
            
            if (input.x * currentHorizontalSpeed >= 0)
            {
                float xDist = Mathf.Abs(input.x * movementSpeed - currentHorizontalSpeed);
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, input.x * movementSpeed, speedUpAccelaration );// /(xDist)
                dist = xDist;

            }
            else
            {
                float xDist = Mathf.Abs(0 - currentHorizontalSpeed);
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, 0, speedDownAccelaration);
                dist = xDist;   
            }
        }
        else
        {
            float xDist = Mathf.Abs(0 - currentHorizontalSpeed);
            dist = xDist;
            currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, 0, stopAccelaration );
        }

        if (input.y != 0)
        {
            
            if (input.y * currentVerticalSpeed >= 0)
            {
                float yDist = Mathf.Abs(input.y * movementSpeed - currentVerticalSpeed);
                currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, input.y * movementSpeed, speedUpAccelaration );// /(yDist)
            }
            else
            {
                float yDist = Mathf.Abs(0 - currentVerticalSpeed);
                currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, 0, speedDownAccelaration );
            }
        }
        else
        {
            float yDist = Mathf.Abs(0 - currentVerticalSpeed);
            currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, 0, stopAccelaration );
        }
    }
    void MoveTopDownPlayer()
    {
        rb2.velocity = new Vector2(currentHorizontalSpeed,currentVerticalSpeed); 
    }
}
