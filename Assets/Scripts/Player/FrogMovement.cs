using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FrogMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed; //Default move speed
    [SerializeField] float jumpPower;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashLength;
    [SerializeField] Dialogue textBox;
    [SerializeField] LayerMask groundLayer; //Find where ground is
    [SerializeField] Cherry cherry;

    CapsuleCollider2D capsuleCollider; //Capsule is the best collider
    Rigidbody2D body;

    bool canDoubleJump; //If the player can double jump
    bool canFirstJump; //If the player can single jump
    bool canDash; //If the player can dash
    bool jumpDash;

    float activeSpeed; //New speed when dashing
    float dashCooldown; //How long player dashes
    float activeJumpSpeed;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        activeSpeed = moveSpeed; //Set speed to default
    }

    void Start()
    {
        textBox.GetComponent<Dialogue>();
        cherry.GetComponent<Cherry>();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            body.velocity = new Vector2(-activeSpeed, body.velocity.y); //Directional controls
            transform.localScale = new Vector2(-2, 2);
        }


        if (Input.GetKey(KeyCode.D))
        {
            body.velocity = new Vector2(activeSpeed, body.velocity.y);
            transform.localScale = new Vector2(2, 2);
        }

        //Dash
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

        if (IsGrounded())
        {
            canFirstJump = true;
            canDash = true;
        }


        if (Input.GetKeyDown(KeyCode.C))
        {
            Jump();
        }

        if (transform.position.y < -5.11f) //If they fall through the ground, they lose the cherry
        {
            transform.position = new Vector2(body.velocity.x, 6.42f);
            cherry.shouldHide = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Mask")
        {
            if (textBox.frog)
            {
                textBox.frog = false;
            }
            else
            {
                textBox.frog = true;
            }
        }
    }



    void Jump()
    {
        if (canFirstJump) //If the player is grounded, then he can jump
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            canFirstJump = false;
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            body.velocity = new Vector2(body.velocity.x, jumpPower);
            canDoubleJump = false;
        }
    }




    bool IsGrounded()
    {
        RaycastHit2D raycastHit = Physics2D.BoxCast(capsuleCollider.bounds.center, capsuleCollider.bounds.size, 0, Vector2.down, .1f, groundLayer);
        return raycastHit.collider != null;
    }
}
