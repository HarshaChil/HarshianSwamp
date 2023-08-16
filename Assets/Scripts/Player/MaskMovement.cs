using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed; //Default move speed
    [SerializeField] float jumpPower;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashLength;
    [SerializeField] LayerMask groundLayer; //Where ground is
    [SerializeField] LayerMask wallLayer; ////Layer for detecting when OnWall
    [SerializeField] Dialogue textBox;
    [SerializeField] Cherry token;

    CapsuleCollider2D capsuleCollider; //Best collider
    Rigidbody2D body;

    bool canDoubleJump; //If player can double jump
    bool canFirstJump; //If player can single jump
    bool canDash; //If player can dash
    bool jumpDash; //If player can dash up or down

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
    }

    void Start()
    {
        textBox.GetComponent<Dialogue>();
        token.GetComponent<Cherry>();
    }

    // Update is called once per frame
    void Update()
    {
        ; if (Input.GetKey(KeyCode.LeftArrow))
        {
            body.velocity = new Vector2(-activeSpeed, body.velocity.y);
            transform.localScale = new Vector2(-2, 2);
        }


        if (Input.GetKey(KeyCode.RightArrow))
        {
            body.velocity = new Vector2(activeSpeed, body.velocity.y);
            transform.localScale = new Vector2(2, 2);
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
                capsuleCollider.enabled = false;
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
            transform.position = new Vector2(body.velocity.x, 6.42f);
            token.shouldHide = false;
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