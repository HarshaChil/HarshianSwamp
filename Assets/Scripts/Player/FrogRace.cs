using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogRace : MonoBehaviour
{
    [SerializeField] float moveSpeed; //Default move speed
    [SerializeField] float jumpPower;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashLength;
    [SerializeField] LayerMask groundLayer; //Where ground is
    [SerializeField] LayerMask wallLayer; ////Layer for detecting when OnWall

    [SerializeField] SetA setA;
    [SerializeField] SetB setB;

    public CoopTimeBar timeBar;
    public CoopRacingDialogue textBox;
    public Token token;
    Animator anim;


    CapsuleCollider2D capsuleCollider; //Best collider
    Rigidbody2D body;

    bool canDoubleJump; //If player can double jump
    bool canFirstJump; //If player can single jump
    bool canDash; //If player can dash
    bool jumpDash; //If player can dash up or down
    bool timeLeft;

    float activeSpeed;
    float activeJumpSpeed;
    float dashCooldown;
    float numOfJumps;

    float cooldown;

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
        timeBar.GetComponent<CoopTimeBar>();
        textBox.GetComponent<CoopRacingDialogue>();
        token.GetComponent<Token>();
        setA.GetComponent<SetA>();
        setB.GetComponent<SetB>();
    }


    // Update is called once per frame
    void Update()
    {
        ; if (Input.GetKey(KeyCode.A))
        {
            body.velocity = new Vector2(-activeSpeed, body.velocity.y);
            transform.localScale = new Vector2(-4, 4);
        }


        if (Input.GetKey(KeyCode.D))
        {
            body.velocity = new Vector2(activeSpeed, body.velocity.y);
            transform.localScale = new Vector2(4, 4);
        }

        if (jumpDash) //Only use up and down movement when dashing
        {

            if (Input.GetKey(KeyCode.W))
            {
                body.velocity = new Vector2(body.velocity.x, activeJumpSpeed);
            }

            if (Input.GetKey(KeyCode.S))
            {
                body.velocity = new Vector2(body.velocity.x, -activeJumpSpeed);
            }
        }

        //Dash
        if (Input.GetKeyDown(KeyCode.V))
        {
            if (canDash)
            {
                //Set the length of the dash, disable the collider, increase move speed
                jumpDash = true;
                activeJumpSpeed = dashSpeed;
                activeSpeed = dashSpeed;
                dashCooldown = dashLength;
                capsuleCollider.enabled = false;

                //turn tiles on or off

                if (setA.isSetA)
                {
                    setA.isSetA = false;
                }
                else
                    setA.isSetA = true;

                if (setB.isSetB)
                {
                    setB.isSetB = false;
                }
                else
                    setB.isSetB = true;




            }
        }

        if (dashCooldown > 0) //If the cooldown is over
        {
            dashCooldown -= Time.deltaTime;
            if (dashCooldown <= 0)
            {
                //Stop the dash, set everything back to normal
                jumpDash = false;
                body.velocity = new Vector2(body.velocity.x, 0);
                activeSpeed = moveSpeed;
                dashCooldown = 0;
                canDash = false;
                capsuleCollider.enabled = true;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
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
            timeLeft = true;

            if (timeBar.color == Color.blue)
                anim.SetBool("BlueEating", true);
            else
            {
                anim.SetBool("BlueEating", false);
            }
        }
        else
        {
            timeLeft = false;
            anim.SetBool("BlueEating", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (timeLeft)
        {

            if (collision.tag == "Mask")
            {
                if (token.frog)
                {
                    textBox.frogScore += 1;
                    timeBar.timeLeft = 0;

                }
                else
                {
                    textBox.maskScore += 1;
                    timeBar.timeLeft = 0;

                }
            }
        }
    }

    void Jump() //Jumping code
    {
        if (numOfJumps > 0)
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
        }
    }


    bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, .1f, groundLayer);
        return raycastHit.collider != null;
    }
    bool OnWall()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, new Vector2(transform.localScale.x, 0), .1f, wallLayer);
        return raycastHit.collider != null;
    }
}
