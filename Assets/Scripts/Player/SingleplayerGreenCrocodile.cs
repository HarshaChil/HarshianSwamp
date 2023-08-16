using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleplayerGreenCrocodile : MonoBehaviour
{
    // Using "[SerializeField]" here is unnecessary, please use "public" instead.
    public float moveSpeed; //Default move speed
    public float jumpPower;
    public float dashSpeed;
    public float dashLength;
    [SerializeField] LayerMask groundLayer; //Where ground is
    [SerializeField] LayerMask wallLayer; //Layer for detecting when OnWall
    public PowerUpController powerUpController;
    public Transform popUp;
    public AudioClip jump;
    public AudioClip death;
    public AudioClip hurt;

    /* [SerializeField] SetA setA;
     [SerializeField] SetB setB;*/

    // bool isTouchingFront;
    [SerializeField] EatingBar timeBar;

    public float drift = .25f;



    // How far a player moves after letting go of left or right. Higher = further.
    [Header("Wall Stats")]
    public float wallSlidingSpeed;
    bool wallSliding;
    public float wallCastCheckLength;
    public int wallJumpKickBack;
    public int wallJumpBoost;



    public Transform frontCheck;
    // public int checkRadius;
    CapsuleCollider2D capsuleCollider; //Best collider
    Rigidbody2D body;
    Animator anim;
    public float movingPlatformMultiplier;

    bool canDoubleJump; //If player can double jump
    bool canFirstJump; //If player can single jump
    bool canDash; //If player can dash
    public bool dashThroughWalls;

    // bool jumpDash; //If player can dash up or down
    public float jumpDashMultiplier;

    float activeSpeed;
    float activeJumpSpeed;
    float dashCooldown;
    float numOfJumps;

    [Header("Speed Stats")]
    public float maxVelocity;
    float moveInput;
    float verticalMovement;
    public float acceleration;
    public float decceleration;
    public float velPower;
    float lastGroundedTime;
    public float frictionAmount;
    float gravityScale;
    public float lastJumpTime;
    bool isJumping;
    public float jumpCutMultiplier;
    public float fallGravityMultiplier;

    public float iFrameTimer;
    public float iFrameLength = 2;

    public float initScale;
    public float scale;

    Vector3 initChildScale;

    public bool smallerCroc;
    bool isDead;


    float cooldown;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeSpeed = moveSpeed; //Set activeSpeed to the default speed
        numOfJumps = 1f;
        anim = GetComponent<Animator>();
        gravityScale = body.gravityScale;
        initScale = transform.localScale.x;
        scale = initScale;
        initChildScale = transform.GetChild(2).gameObject.transform.localScale;
    }

    private void Start()
    {
        timeBar.GetComponent<EatingBar>();
        /*      setA.GetComponent<SetA>();
              setB.GetComponent<SetB>();*/
    }

    void FixedUpdate()
    {
        float targetSpeed = moveInput * activeSpeed;
        float speedDif = targetSpeed - body.velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > .01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        if (dashCooldown > 0)
        {
            float targetYSpeed = verticalMovement * jumpDashMultiplier;
            float speedDifY = targetYSpeed - body.velocity.y;
            float accelRateY = (Mathf.Abs(targetYSpeed) > .01f) ? acceleration : decceleration;
            float movementY = Mathf.Pow(Mathf.Abs(speedDifY) * accelRateY, velPower) * Mathf.Sign(speedDifY);
            body.AddForce(movementY * Vector2.up);
        }
        body.AddForce(movement * Vector2.right);

        if (lastGroundedTime > 0 && Mathf.Abs(moveInput) < 0.01f)
        {
            float amount = Mathf.Min(Mathf.Abs(body.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(body.velocity.x);
            body.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }

        if (dashCooldown <= 0)
        {
            if (body.velocity.y < 0)
            {
                body.gravityScale = gravityScale * fallGravityMultiplier;
            }
            else
            {
                body.gravityScale = gravityScale;
            }
        }
    }

    void Update()
    {

        if (HealthBar.timeLeft < 0)
        {
            if (isDead) return;
            SoundManager.instance.PlaySound(death);
            anim.SetTrigger("die");
            isDead = true;
        }

        //SmallerCroc();
        iFrameTimer -= Time.deltaTime;
        if (dashCooldown <= 0)
        {
            moveInput = -(Input.GetKey(KeyCode.LeftArrow) ? 1 : 0) + (Input.GetKey(KeyCode.RightArrow) ? 1 : 0);
            verticalMovement = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetBool("move", true);

            // body.AddForce(new Vector2(-activeSpeed, 0));
            if (transform.parent == null)
                transform.localScale = new Vector2(moveInput * scale, scale);
            else
                transform.localScale = new Vector2(moveInput * scale, scale) / transform.parent.localScale;
        }
        else
        {
            anim.SetBool("move", false);
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            // body.AddForce(new Vector2(activeSpeed, 0));
            if (transform.parent == null)
                transform.localScale = new Vector2(scale, scale);
            else
                transform.localScale = new Vector2(scale, scale) / transform.parent.localScale;
        }

        /* if (smallerCroc)
         {
             SmallerCroc();
         }*/


        // Slow player down when you let go of left or right. Gives more prescision.
        // if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        // {
        //     body.velocity = new Vector2(body.velocity.x * drift, body.velocity.y);
        // }

        // if (jumpDash) //Only use up and down movement when dashing
        // {

        //     if (Input.GetKey(KeyCode.UpArrow))
        //     {
        //         body.velocity = new Vector2(body.velocity.x, activeJumpSpeed * jumpDashMultiplier);
        //     }

        //     if (Input.GetKey(KeyCode.DownArrow))
        //     {
        //         body.velocity = new Vector2(body.velocity.x, -activeJumpSpeed * jumpDashMultiplier);
        //     }
        // }

        //Dash
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (canDash)
            {
                //Set the length of the dash, disable the collider, increase move speed
                if (moveInput == 0 && verticalMovement == 0)
                    moveInput = Mathf.Sign(transform.localScale.x);
                // jumpDash = true;
                activeJumpSpeed = dashSpeed;
                body.gravityScale = 0;
                activeSpeed = dashSpeed;
                dashCooldown = dashLength;
                canDash = false;
                body.velocity = Vector2.zero;

                //disable collider if he has the power up
                //print(dashThroughWalls);
                // if (dashThroughWalls) capsuleCollider.enabled = false;

                /*             setA.isSetA = (setA.isSetA) ? false : true;
                             setB.isSetB = (setB.isSetB) ? false : true;*/
            }
        }
        if (dashCooldown > 0)
        {
            dashCooldown -= Time.deltaTime;
            // float verticalMovement = (Input.GetKey(KeyCode.UpArrow) ? 1 : 0) - (Input.GetKey(KeyCode.DownArrow) ? 1 : 0);
            // body.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * dashSpeed, verticalMovement * jumpDashMultiplier), ForceMode2D.Impulse);
        }

        if (dashCooldown <= 0) //If the cooldown is over
        {
            //Stop the dash, set everything back to normal
            // jumpDash = false;
            activeSpeed = moveSpeed;
            dashCooldown = 0;
            body.gravityScale = gravityScale;
            capsuleCollider.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
            numOfJumps--;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            OnJumpUp();
        }

        stopFall(); //Prevent from falling off map

        if (IsGrounded() != null) //If you touch the ground, you can jump or dash
        {
            // Collider2D ground = IsGrounded();
            // if (ground.name.Contains("Platform"))
            // {
            //     transform.parent = ground.transform;
            // }
            numOfJumps = 1f;
            canDash = true;
        }

        // if (!IsGrounded() && !(lastGroundedTime > 0 && lastJumpTime > 0 && !isJumping) && numOfJumps > 1)
        // {
        //     numOfJumps = 1f;
        // }


        if (OnWall() != null)
        {
            // Collider2D wall = OnWall();
            // SidePlatform wallScript = wall.GetComponent<SidePlatform>();
            // body.AddForce(new Vector2(wallScript.movement, 0));
            numOfJumps = 2f;
        }

        if (EatingBar.isTimeLeft)
        {
            anim.SetBool("greenEating", true);
            anim.SetBool("move", false);
        }
        else
        {
            anim.SetBool("greenEating", false);
        }




        //anim.SetBool("greenEating", (EatingBar.isTimeLeft) ? true : false);



        wallSliding = (OnWall() != null && IsGrounded() == null && Input.GetAxisRaw("Horizontal") != 0) ? true : false;

        if (wallSliding)
        {
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlidingSpeed, float.MaxValue));
            numOfJumps = 2f;
        }
        // Keep AddForce from making the player too fast
        // if (activeSpeed != dashSpeed)
        //     body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxVelocity, maxVelocity), body.velocity.y);
        lastGroundedTime -= Time.deltaTime;
        lastJumpTime -= Time.deltaTime;
    }

    void Jump() //Jumping code
    {
        if (numOfJumps > 0)
        {
            SoundManager.instance.PlaySound(jump);
            Collider2D wall = OnWall();
            if (wall != null)
            {

                body.AddForce(new Vector2(Mathf.Sign(body.velocity.x) * -activeSpeed * wallJumpKickBack, jumpPower * wallJumpBoost));
            }
            else
            {
                body.gravityScale = gravityScale;
                body.velocity = new Vector2(body.velocity.x, 0);

                body.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
                lastGroundedTime = 0;
                isJumping = true;
            }
        }
    }


    Collider2D IsGrounded()
    {
        // Make collider size two times as small so it doesn't falsely detect something above the player
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size / 2, 0, Vector2.down, .1f, groundLayer);
        return raycastHit.collider;
    }
    Collider2D OnWall()
    {
        return Physics2D.OverlapCircle(frontCheck.position, .05f, wallLayer);
        // Couldn't figure this out so I used my own method, feel free to fix the commented out old part
        // RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, transform.forward, wallCastCheckLength, wallLayer);
        // return raycastHit.collider != null;
    }

    public void takeDamage(float knockBack)
    {
        if (iFrameTimer <= 0)
        {
            //print("hit");
            // if (timeBar.timeLeft <= 0) // Check if croc has dentures
            //{
            body.AddForce(new Vector2(knockBack * -activeSpeed * wallJumpKickBack / 2, jumpPower * wallJumpBoost));
            HealthBar.timeLeft -= 4f;
            //print("added");
            iFrameTimer = iFrameLength;
            PointPopUp.Create(popUp.position, -20);
            SingleplayerText.score -= 20;
        }
        //}
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (timeBar.timeLeft <= 0)
            {
                powerUpController.powerUps.Clear();
                SoundManager.instance.PlaySound(hurt);
            }
        }
    }



    public void OnJumpUp()
    {
        if (body.velocity.y > 0 && isJumping)
        {
            body.AddForce(Vector2.down * body.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
        lastJumpTime = 0;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        // Gizmos.DrawRay(transform.position, transform.forward * m_Hit.distance);
        Gizmos.DrawWireCube(transform.position + -transform.up * .1f, transform.localScale / 6);
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            if (timeBar.timeLeft <= 0)
                powerUpController.powerUps.Clear();
        }
    }*/
    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains("Platform"))
        {
            transform.parent = null;
            RisingPlatform groundScript = collision.gameObject.GetComponent<RisingPlatform>();
            body.AddForce(new Vector2(0, groundScript.movement * movingPlatformMultiplier));
        }
    }

    void stopFall()
    {
        if (transform.position.x < -18.5f)
        {
            transform.position = new Vector2(-transform.position.x - 1, transform.position.y);
        }

        if (transform.position.x > 18.5f)
        {
            transform.position = new Vector2(-transform.position.x + 1, transform.position.y);
        }

        if (transform.position.y < -10.5f)
        {
            transform.position = new Vector2(transform.position.x, -transform.position.y - .5f);
        }

    }

    public bool bigHitbox;
    public void ActivateLargerHitbox(bool hitBox)
    {
        if (hitBox)
        {
            // Deactivate small hitbox
            transform.GetChild(2).gameObject.SetActive(false);
            // Activate large hitbox
            transform.GetChild(3).gameObject.SetActive(true);
            // TODO: Make larger mouthed sprite, and change to it.
            bigHitbox = true;
        }
        else
        {
            transform.GetChild(2).gameObject.SetActive(true);
            // Activate large hitbox
            transform.GetChild(3).gameObject.SetActive(false);
            bigHitbox = false;
        }
    }


    public bool isSmall;
    public void SmallerCroc(bool small)
    {
        // Change and apply localScale to be smaller
        if (small)
        {
            scale *= .75f;
            transform.localScale = new Vector2(Mathf.Sign(transform.localScale.x) * scale, scale);

            // Undo making the hurtboxes localscale smaller, which will be invisible
            transform.GetChild(2).gameObject.transform.localScale = transform.GetChild(2).gameObject.transform.localScale / .75f;


            isSmall = true;
        }
        else
        {
            if (!isCursedSmall)
            {
                isSmall = false;
                scale = initScale;
                transform.GetChild(2).gameObject.transform.localScale = initChildScale;
            }
        }

        //transform.GetChild(3).gameObject.transform.localScale = tSransform.GetChild(3).gameObject.transform.localScale / .75f;
    }

    public bool isCursedSmall;
    public void CursedSmallerCroc(bool cursedSmall)
    {
        if (cursedSmall)
        {
            // Change and apply localScale to be smaller
            scale *= .75f;
            transform.localScale = new Vector2(Mathf.Sign(transform.localScale.x) * scale, scale);

            isCursedSmall = true;
        }
        else
        {
            if (!isSmall)
            {
                isCursedSmall = false;
                scale = initScale;
            }
        }
    }


    private void Destroy()
    {

    }

    void endGame()
    {

        DeathMenu.isdeadPlayer = true;
        Destroy(gameObject);
    }

    /*void Shoot()
    {
        Instantiate(wideBullet, bulletSpawnLocation.transform.position, Quaternion.Euler(0, 0, Mathf.Sign(transform.localScale.x) * -90 + 90));
    }

    void EnableWideShoot()
    {
        wideShootEnabled = true;
    }*/

}



