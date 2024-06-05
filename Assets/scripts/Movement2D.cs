using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
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
    [SerializeField] float movementSpeed = 5f;
    [Range(0.02f, 1)]
    [SerializeField] float speedUpDuration = 0.1f;
    [Range(0.02f, 1)]
    [SerializeField] float speedDownDuration = 0.06f;
    [Range(0.02f, 1)]
    [SerializeField] float stopDuration = 0.15f;

    //[Header("DASH")]
    [SerializeField] bool Dash;
    [SerializeField] KeyCode dashButton = KeyCode.LeftShift;
    [SerializeField] bool cancelDashOnWallHit;
    //[Header("-Dash Values")]
    [Range(1f, 10f)]
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
    [Space]
    [SerializeField] float dashCooldown = 0.5f;
    [SerializeField] float dashCoolTimer;

    bool canDash;
    Vector2 defaultColliderOffset;
    Vector2 defaulColliderSize;


    //DASH INFO
    [HideInInspector] public bool isDashing;
    float dashSpeed;
    float dashingTimer;

    [Space]
    //[Header("WALL JUMP")]
    public bool WallJump;
    [SerializeField] Vector2 wallJumpVelocity;
    [Range(0.02f, 1f)]
    [SerializeField] float wallJumpDecelerationFactor = 0.3f;
    [Range(0.01f, 10f)]
    [SerializeField] float wallSlideSpeed = 0.5f;
    [SerializeField] bool isSlidingOnWall;
    [SerializeField] bool variableJumpHeightOnWallJump;

    [Space]
    //[Header("----Jumping Values----")]
    [Range(0.5f, 10f)]
    [SerializeField] float jumpHight = 1.5f;
    [Range(0.5f, 50f)]
    [SerializeField] float jumpUpAcceleration = 2.5f;
    [Range(0.5f, 50f)]
    [SerializeField] float jumpDownAcceleration = 4f;
    [Range(0.1f, 50f)]
    [SerializeField] float fallSpeedClamp = 50;
    [Range(1f, 20f)]
    [SerializeField] float gravity = 9.8f;
    [SerializeField] float jumpVelocity;
    [SerializeField] float fallClamp;
    [SerializeField] float jumpUpDuration;
    [SerializeField] bool isNormalJumped;
    [SerializeField] bool isWallJumped;

    [Space]
    //[Header("----Jump Adjustments----")]
    [Range(0f, 1f)]
    [SerializeField] float coyoteTime = 0.15f;
    [Range(0f, 1f)]
    [SerializeField] float jumpBuffer = 0.1f;
    [Range(0f, 1f)]
    [SerializeField] float onAirControl = 1f;

    [Range(0f, 1f)]
    [SerializeField] float variableJumpHeightDuration = 0.75f;
    [Range(0f, 1f)]
    [SerializeField] float jumpReleaseEffect = 0.5f;
    [SerializeField] KeyCode jumpButton = KeyCode.Space;
    bool isHoldingJumpButton;
    float jumpHoldTimer;
    bool isForcingJump;

    //[Header("Ledge Climb")]
    [SerializeField] bool LedgeGrab;
    [SerializeField] bool autoClimbLedge;
    [SerializeField] KeyCode climbButton = KeyCode.W;
    [SerializeField] bool canWallJumpWhileClimbing;
    [SerializeField] float ledgeCheckOffset = 1f;
    [SerializeField] float ledgeCheckDistance = 1f;
    [SerializeField] LayerMask ledgeCheckLayer;
    public bool isLedge;
    Vector2 ledgePosition;
    [SerializeField] Vector2 ledgeClimbPosOffset;
    public bool isClimbingLedge;
    [SerializeField] float ledgeClimbDuration;
    [SerializeField] float ledgeClimbTimer;

    //SLOPE_CHECk
    [SerializeField] bool isSlopeActive;
    [SerializeField] int maxSlope = 45;
    [SerializeField] int currentSlope;
    [SerializeField] bool rotatePlayerOnSlope;
    [SerializeField] float slopeRotateSpeed;

    [SerializeField] float slopeCheckRayDistance = 1f;
    [SerializeField] float slopeRayOffset = 0.5f;
    [SerializeField] LayerMask slopeLayer;
    [SerializeField] bool isOverSlopeLimit;

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
    public bool isJumped;//to check if the player is on air because of jumping or falling
    public bool isPressedJumpButton;

    float onAirControlMultiplier;



    private void Reset()
    {
        rb2 = GetComponent<Rigidbody2D>();
        rb2.freezeRotation = true;
        rb2.gravityScale = 0;
        rb2.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb2.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        capsuleCollider = GetComponent<CapsuleCollider2D>();
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
            GetSlopeAngle();
            MovePlayer();
            LedgeClimbCountdown();
        }
    }

    void HandlePlatformerMovement()
    {
        GetPlatformerInput();
        CheckSideWall();
        DoDash();
        CheckCeil();
        CheckGround();
        FlipThePlayer();
        CountDownJumpTolerance();
        SwitchState();
        CheckLedge();
        DashCooldownCounter();
        
    }
    void DoDash()
    {
        if (isDashing)
        {
            dashingTimer -= Time.deltaTime;
            if (dashingTimer <= 0 && ( (isGrounded && !onCeil) || !isGrounded) )
            {
                CancelDash();

            }
        }
    }
    void DashCooldownCounter()
    {
        if (dashCoolTimer > 0)
        {
            dashCoolTimer -= Time.fixedDeltaTime;
        }
    }
    void ClimbLedge()
    {
        //transform.GetChild(0).GetComponent<Animator>().Play("ProtoLedgeClimb");
        
        isClimbingLedge = true;
        ledgeClimbTimer = ledgeClimbDuration;

    }
    void LedgeClimbCountdown()
    {
        if (isClimbingLedge)
        {
            ledgeClimbTimer -= Time.deltaTime;
            if (ledgeClimbTimer <= 0f)
            {
                UpdateLedgeClimbPosition();
            }
        }
    }
    public void UpdateLedgeClimbPosition()
    {
        isClimbingLedge = false;
        //transform.GetChild(0).GetComponent<Animator>().Play("ProtoIdle");
        transform.position = spriteTransform.position;
        Vector2 _posOffset = new (spriteTransform.right.x * ledgeClimbPosOffset.x,
            spriteTransform.up.y * ledgeClimbPosOffset.y);
        transform.position = (Vector2)transform.position + _posOffset;


    }

    void CheckLedge()
    {
        if (LedgeGrab) 
        { 
            //CHECH THE LEDGE
            Vector2 _ledgeCheckOrigin = new(transform.position.x,transform.position.y + ledgeCheckOffset);
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
                    if (Input.GetKeyDown(climbButton) && !autoClimbLedge)
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
            dashCoolTimer = dashCooldown;
            currentHorizontalSpeed *= dashStopEffect;
            currentVerticalSpeed *= dashStopEffect;

            capsuleCollider.offset = defaultColliderOffset;
            capsuleCollider.size = defaulColliderSize;
        }
    }

    void DashPressed()
    {
        if ( canDash && !isDashing && Dash && ( (horizontalDash && input.x != 0) || (verticalDash && input.y != 0) ) && dashCoolTimer <= 0f)
        {

            if (!isGrounded) 
            {
                canDash = false;

            }
            isDashing = true;
            dashSpeed = (dashDistance) / dashDuration;
            dashingTimer = dashDuration;

            Vector2 _dir = Vector2.zero;

            if (horizontalDash && !verticalDash)
            {
                _dir.x = input.x;
                currentHorizontalSpeed = dashSpeed * _dir.x;
                currentVerticalSpeed = 0f;
                

            }
            else if (verticalDash && !horizontalDash)
            {
                _dir.y = input.y;

                currentHorizontalSpeed = dashSpeed * _dir.x;
                currentVerticalSpeed = dashSpeed * _dir.y;
            }
            else if (horizontalDash && verticalDash) 
            {
                _dir = input.normalized;

                currentHorizontalSpeed = dashSpeed * _dir.x;
                currentVerticalSpeed = dashSpeed * _dir.y;
            }


            if (isGrounded)
            {
                capsuleCollider.offset = dashColliderOffset;
                capsuleCollider.size = dashColliderScale;

            }

            
        }
    }

    void HandleTopDownMovement()
    {
        GetTopDownInput();
        UpdateTopDownSpeed();
        CheckSideWall();
        MovePlayer();
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
        if (Input.GetButtonDown("Fire3"))
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

        Gizmos.color = Color.blue;

        Gizmos.DrawRay(transform.position - transform.up * slopeRayOffset,Vector2.down * slopeCheckRayDistance);


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

    public void GetAutoValueForCeilCheck()
    {
        if (capsuleCollider == null)
        {
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        }
        ceilCheckCircleRadius = capsuleCollider.size.x / 2f;
        ceilCheckRayDistance = (capsuleCollider.size.y / 2f) + capsuleCollider.offset.y - (ceilCheckCircleRadius) + 0.02f;
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
            isNormalJumped = false;
            isWallJumped = false;
            fallClamp = wallSlideSpeed;
        }
        else if (isSlidingOnWall && !_sliding)
        {
            isSlidingOnWall = false;
            fallClamp = fallSpeedClamp;
        }
    }

    void GetSlopeAngle()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(transform.position - transform.up * slopeRayOffset, Vector2.down, slopeCheckRayDistance, slopeLayer);

        Debug.DrawRay(hit2D.point, hit2D.normal, Color.yellow);

        currentSlope = Mathf.RoundToInt(Vector2.Angle(Vector2.up, hit2D.normal));
        isOverSlopeLimit = currentSlope > maxSlope && hit2D;
        Vector2 move_dir = input;
        move_dir.y = 0;

        bool isMovingUpSlope = Vector2.Dot(move_dir, hit2D.normal) < 0;

        if (hit2D && !isOverSlopeLimit)
        {
            if (rotatePlayerOnSlope)
            {
                transform.up = Vector3.MoveTowards(transform.up, hit2D.normal, slopeRotateSpeed * Time.fixedDeltaTime);
            }

            if ( !isJumped && isSlopeActive && currentSlope > 0)
            {
            
                if (isMovingUpSlope)
                {
                    currentVerticalSpeed = Mathf.Abs(currentHorizontalSpeed) * Mathf.Sin(currentSlope * Mathf.Deg2Rad);
                }
                else
                {
                    currentVerticalSpeed = -Mathf.Abs(currentHorizontalSpeed) * Mathf.Sin(currentSlope * Mathf.Deg2Rad) * 1.5f;
                }
            }   
        }
        

    }

    void CheckGround()
    {
        RaycastHit2D hit2D = Physics2D.CircleCast(transform.position, groundCheckCircleRadius, -transform.up, groundCheckRayDistance, groundLayer);
        if (hit2D)
        {
            
            if (!isGrounded && !isOverSlopeLimit)
            {
                
                isGrounded = true;
                isWallJumped = false;
                isNormalJumped = false;
                fallClamp = fallSpeedClamp;
                canJump = true;
                canDash = resetDashOnGround;
                onAirControlMultiplier = 1;
                SlideOnWall(false);

                if (currentVerticalSpeed <= 0)//to check if the player if grounded while falling
                {
                    currentVerticalSpeed = 0f;
                    isJumped = false;
                }
            }
            else if (isOverSlopeLimit)
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
            if (resetDashOnGround)
            {
                canDash = true;
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

    public void GetAutoValueForGroundCheck()
    {
        if (capsuleCollider == null)
        {
            capsuleCollider = GetComponent<CapsuleCollider2D>();
        }
        groundCheckCircleRadius = capsuleCollider.size.x / 2f;
        groundCheckRayDistance= (capsuleCollider.size.y / 2f) - capsuleCollider.offset.y - (ceilCheckCircleRadius) + 0.02f;
    }

    void PressJumpButton()
    {
        if (!isDashing)
        {
            isHoldingJumpButton = true;
            isPressedJumpButton = true;
            isForcingJump = true;
            jumpHoldTimer = variableJumpHeightDuration * jumpUpDuration;
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
            if (!isHoldingJumpButton && isForcingJump && ((variableJumpHeightOnWallJump && isWallJumped) || isNormalJumped))
            {
                currentVerticalSpeed *= jumpReleaseEffect;
                isForcingJump = false;
            }

        }

        if (canJump && isPressedJumpButton)
        {
            jumpVelocity = Mathf.Sqrt(2 * jumpUpAcceleration * jumpHight * gravity);
            jumpUpDuration = jumpVelocity / (jumpUpAcceleration * gravity);

            isJumped = true;
            canJump = false;
            isPressedJumpButton = false;
            currentVerticalSpeed = jumpVelocity;
            isNormalJumped = true;
        }
        if (isPressedJumpButton && isSlidingOnWall && ((!canWallJumpWhileClimbing && !isClimbingLedge)|| canWallJumpWhileClimbing ))
        {
            jumpVelocity = Mathf.Sqrt(2 * jumpUpAcceleration * jumpHight * gravity);
            isJumped = true;
            canJump = false;
            isWallJumped = true;
            isPressedJumpButton = false;
            onAirControlMultiplier = wallJumpDecelerationFactor;
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
                        currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, input.x * movementSpeed, (Time.deltaTime / speedUpDuration) * onAirControlMultiplier * movementSpeed);// /(xDist)
                    }
                }
                else
                {
                    currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, (Time.deltaTime / speedDownDuration) * onAirControlMultiplier * movementSpeed);
                }
            }
            else
            {
                currentHorizontalSpeed = Mathf.MoveTowards(currentHorizontalSpeed, 0, (Time.deltaTime / stopDuration) * onAirControlMultiplier * movementSpeed);
            }
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
        //else if(isGrounded)
        //{
        //    if (currentVerticalSpeed < 0)
        //    {
        //        currentVerticalSpeed = Mathf.MoveTowards(currentVerticalSpeed,0,jumpDownAcceleration * jumpVelocity * 3 * Time.fixedDeltaTime);
        //    }
        //}
    }

//TopDown
    void FlipThePlayer()
    {
        Vector3 _playerRot = spriteTransform.localEulerAngles;
        
        if (currentHorizontalSpeed > 0 || (WallJump && isSlidingOnWall && rightWallHit))
        {
            _playerRot.y = 0f;
            spriteTransform.localEulerAngles = _playerRot;
        }
        else if (currentHorizontalSpeed < 0 || (WallJump && isSlidingOnWall && leftWallHit))
        {
            _playerRot.y = 180f;
            spriteTransform.localEulerAngles = _playerRot;
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
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, input.x * movementSpeed, speedUpDuration );// /(xDist)
            }
            else
            {
                currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, 0, speedDownDuration);
            }
        }
        else
        {
            currentHorizontalSpeed = Mathf.Lerp(currentHorizontalSpeed, 0, stopDuration );
        }

        if (input.y != 0)
        {
            
            if (input.y * currentVerticalSpeed >= 0)
            {
                currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, input.y * movementSpeed, speedUpDuration );// /(yDist)
            }
            else
            {
                currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, 0, speedDownDuration );
            }
        }
        else
        {
            currentVerticalSpeed = Mathf.Lerp(currentVerticalSpeed, 0, stopDuration );
        }
    }

    //Vector2 CheckPosition(Vector2 _pos)
    //{
    //    Vector2 _dir = _pos - rb2.position; // Calculate the direction of movement
    //    Vector2 _center = rb2.position; // Center position of the capsule

    //    // Distance to check, including the size of the capsule
    //    float distance = _dir.magnitude;

    //    RaycastHit2D hit = Physics2D.CapsuleCast(
    //        _center,
    //        capsuleCollider.size,
    //        capsuleCollider.direction,
    //        0f,
    //        _dir,
    //        distance
    //    );

    //    if (hit.collider != null)
    //    {
    //        // Calculate the position to move to based on the hit distance, subtracting the capsule size to prevent penetration
    //        Vector2 newPosition = _center + (_dir.normalized * (hit.distance - 0.1f));
    //        print(rb2.position - newPosition);
    //        return newPosition;
    //    }

    //    // No collision, return the original position
    //    return _pos;
    //}

    void MovePlayer()
    {
        //KINEMATIC
        //Vector2 currentPosition = rb2.position;
        //Vector2 newPosition = currentPosition + new Vector2(currentHorizontalSpeed, currentVerticalSpeed) * Time.fixedDeltaTime;
        //rb2.MovePosition(CheckPosition(newPosition));


        rb2.velocity = new Vector2(currentHorizontalSpeed,currentVerticalSpeed); 
    }
}
