using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskRace : MonoBehaviour
{
    // Using "[SerializeField]" here is unnecessary, please use "public" instead.
    [SerializeField] float moveSpeed; //Default move speed
    [SerializeField] float jumpPower;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashLength;
    [SerializeField] LayerMask groundLayer; //Where ground is
    [SerializeField] LayerMask wallLayer; ////Layer for detecting when OnWall

    [SerializeField] CoopTimeBar timeBar;

    public float drift = .25f; // How far a player moves after letting go of left or right. Higher = further.
    public float wallSlidingSpeed;
    // bool isTouchingFront;
    public Transform frontCheck;
    bool wallSliding;
    public float wallCastCheckLength;
    public int wallJumpKickBack;
    public int wallJumpBoost;
    public float maxVelocity;
    // public int checkRadius;
    CapsuleCollider2D capsuleCollider; //Best collider
    Rigidbody2D body;
    Animator anim;

    bool canDoubleJump; //If player can double jump
    bool canFirstJump; //If player can single jump
    bool canDash; //If player can dash
    bool jumpDash; //If player can dash up or down

    float activeSpeed;
    float activeJumpSpeed;
    float dashCooldown;
    float numOfJumps;

    float cooldown;
    float iFrameTimer;
    public float iFrameLength = 2;

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeSpeed = moveSpeed; //Set activeSpeed to the default speed
        numOfJumps = 1f;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        timeBar.GetComponent<HealthBar>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.AddForce(new Vector2(-activeSpeed, 0));
            transform.localScale = new Vector2(-4, 4);
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.AddForce(new Vector2(activeSpeed, 0));
            transform.localScale = new Vector2(4, 4);
        }

        // Slow player down when you let go of left or right. Gives more prescision.
        if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
        {
            body.velocity = new Vector2(body.velocity.x * drift, body.velocity.y);
        }

        if (jumpDash) //Only use up and down movement when dashing
        {

            if (Input.GetKey(KeyCode.UpArrow))
            {
                body.velocity = new Vector2(body.velocity.x, activeJumpSpeed);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                body.velocity = new Vector2(body.velocity.x, -activeJumpSpeed);
            }
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.Period))
        {
            if (canDash)
            {
                //Set the length of the dash, disable the collider, increase move speed
                jumpDash = true;
                activeJumpSpeed = dashSpeed;
                activeSpeed = dashSpeed;
                dashCooldown = dashLength;
                body.AddForce(new Vector2(Input.GetAxisRaw("Horizontal") * dashSpeed * moveSpeed * 10, 0));
            }
        }

        if (dashCooldown > 0) //If the cooldown is over
        {
            dashCooldown -= Time.deltaTime;
            if (dashCooldown <= 0)
            {
                //Stop the dash, set everything back to normal
                jumpDash = false;
                body.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * activeSpeed, 0);
                activeSpeed = moveSpeed;
                dashCooldown = 0;
                canDash = false;

            }
        }

        if (Input.GetKeyDown(KeyCode.Slash))
        {

            Jump();
            numOfJumps--;
        }

        if (IsGrounded()) //If you touch the ground, you can jump or dash
        {
            numOfJumps = 1f;
            canDash = true;
        }

        if (OnWall())
        {
            numOfJumps = 2f;
            canDash = true;
        }

        if (transform.position.y < -5.11f) //If they fall through the ground, they lose the cherry
        {
            transform.position = new Vector2(.15f, 6.42f);
        }

        if (timeBar.timeLeft > 0)
        {
            if (timeBar.color == Color.green)
                anim.SetBool("greenEating", true);
        }
        else
            anim.SetBool("greenEating", false);

        if (OnWall() && IsGrounded() == false && Input.GetAxisRaw("Horizontal") != 0)
        {
            wallSliding = true;
        }
        else
        {
            wallSliding = false;
        }

        if (wallSliding)
        {
            body.velocity = new Vector2(body.velocity.x, Mathf.Clamp(body.velocity.y, -wallSlidingSpeed, float.MaxValue));
            numOfJumps = 2f;
            canDash = true;
        }
        // Keep AddForce from making the player too fast
        if (activeSpeed != dashSpeed)
            body.velocity = new Vector2(Mathf.Clamp(body.velocity.x, -maxVelocity, maxVelocity), body.velocity.y);

        if (iFrameTimer > 0) // Reduce iframe if active
        {
            iFrameTimer -= Time.deltaTime;
        }
    }

    void Jump() //Jumping code
    {
        if (numOfJumps > 0)
        {
            if (wallSliding)
            {
                body.AddForce(new Vector2(Mathf.Sign(body.velocity.x) * -activeSpeed * wallJumpKickBack, jumpPower * wallJumpBoost));
            }
            else
            {
                body.velocity = new Vector2(body.velocity.x, jumpPower);
            }
        }
    }


    bool IsGrounded()
    {
        // Make collider size two times as small so it doesn't falsely detect something above the player
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size / 2, 0, Vector2.down, .1f, groundLayer);
        return raycastHit.collider != null;
    }
    bool OnWall()
    {
        return Physics2D.OverlapCircle(frontCheck.position, .5f, wallLayer) != null;
        // Couldn't figure this out so I used my own method, feel free to fix the commented out old part
        // RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, transform.forward, wallCastCheckLength, wallLayer);
        // return raycastHit.collider != null;
    }

    public void takeDamage(float knockBack)
    {
        if (iFrameTimer <= 0)
        {
            if (timeBar.timeLeft <= 0) // Check if croc has dentures
            {
                body.AddForce(new Vector2(knockBack * -activeSpeed * wallJumpKickBack / 2, jumpPower * wallJumpBoost));
                iFrameTimer = iFrameLength;
            }
        }
    }

    /* void OnCollisionEnter2D(Collision2D collision)
     {
         if (collision.gameObject.tag == "Enemy" && timeBar.timeLeft > 0)
         {
             float xForce = transform.position.x - collision.transform.position.x;
             collision.gameObject.GetComponent<BasicEnemy>().takeDamage(xForce);
         }
     }*/
}
