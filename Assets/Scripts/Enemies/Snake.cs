using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    // [SerializeField] private int damage;

    public AudioClip fireballSound;
    public AudioClip bite;

    [Header("Ranged Attack")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject[] fireballs;
    public Transform popUp;

    bool canBeHit;

    //public Transform croc;


    Rigidbody2D body;
    BoxCollider2D boxCollider;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask playerLayer;

    SingleplayerGreenCrocodile croc;
    Animator anim;

    bool playerInRange;
    bool isMoving;

    [SerializeField] private float idleDuration;
    private float idleTimer;

    Vector2 myPos;
    Vector2 crocPos;

    public float leftRange;
    public float rightRange;
    public float shootingRange;

    float right;
    float left;
    bool movingLeft;

    Vector3 initScale;

    public float speed;
    public Transform frontCheck;
    public float knockBackMultiplier;
    bool dead;

    private void Awake()
    {

        body = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        dead = false;
        left = transform.position.x - leftRange;
        right = transform.position.x + rightRange;
        initScale = transform.localScale;
        anim = GetComponent<Animator>();
    }

    private void Start()
    {
        croc = FindObjectOfType<SingleplayerGreenCrocodile>();
    }



    float cooldownTimer = 1000f;

    private void Update()
    {
        //print(playerInRange);
        //playerInRange = false;
        if (OnWall() && !dead)
            body.AddForce(Vector2.up * 5f);

        stopFall();

        //transform.localScale = new Vector2(-transform.localScale.x, 1);

        // body.velocity = new Vector2(-transform.localScale.x * speed * Time.deltaTime, 0);

        if (Mathf.Abs(croc.transform.position.x - transform.position.x) < shootingRange) //If within the shooting range
        {
            /*if (cooldownTimer >= attackCooldown)
            {
                Attack();
            }*/
            if (transform.position.x > croc.transform.position.x)
            {
                transform.localScale = new Vector2(transform.localScale.x, transform.localScale.y);
            }
            else
            {
                transform.localScale = new Vector2(-3f, transform.localScale.y);
            }

            anim.SetBool("move", false);
            anim.SetBool("shoot", true);
        }
        else
        {
            anim.SetBool("move", true);
            anim.SetBool("shoot", false);
            if (movingLeft)
            {
                if (transform.position.x >= left) //If the player is to the left of the left edge, move left, otherwise change direction
                    MoveInDirection(-1);
                else
                    DirectionChange();
            }
            else
            {
                if (transform.position.x <= right)
                    MoveInDirection(1);
                else
                    DirectionChange();
            }
        }

    }
    void Attack()
    {
        cooldownTimer = 0;

        myPos = new Vector2(transform.position.x, transform.position.y);
        crocPos = croc.transform.position;

        fireballs[FindFireball()].transform.position = firePoint.position; //Put fireball on the firepoint object
        fireballs[FindFireball()].GetComponent<Projectile>().setDirection(-(myPos - crocPos)); //Fire in direction of player

        SoundManager.instance.PlaySound(fireballSound);


    }
    int FindFireball()
    {
        for (int i = 0; i < fireballs.Length; i++)
        {
            if (!fireballs[i].activeInHierarchy) //If the fireball is not active you can use it
                return i;
        }
        return 0;
    }

    private void DirectionChange() //Turn the enemy around
    {
        idleTimer += Time.deltaTime;

        if (idleTimer > idleDuration) //How long they stay on the edge
            movingLeft = !movingLeft;
    }

    private void MoveInDirection(int _direction)
    {
        idleTimer = 0;

        //Make enemy face direction
        transform.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);

        //Move in that direction
        transform.position = new Vector3(transform.position.x + Time.deltaTime * _direction * speed,
            transform.position.y, transform.position.z);
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


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (!EatingBar.isTimeLeft)
            {
                float xForce = transform.position.x - collision.transform.position.x;
                croc.takeDamage(xForce);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hurtbox")
        {

            if (EatingBar.isTimeLeft)
            {
                if (canBeHit) return;
                takeDamage();
                canBeHit = true;
            }

        }
    }

    void takeDamage()
    {
        /*if (eatingBar.timeLeft > 0)
                       {*/

        HealthBar.timeLeft += 4f;
        //  timeBar.changeTimeLeft(4f);
        SingleplayerText.counter++;
        PointPopUp.Create(popUp.position, 100 + 50 * SingleplayerText.counter);
        SingleplayerText.addScore(100);

        SoundManager.instance.PlaySound(bite);
        anim.SetTrigger("die");

    }

    bool OnWall()
    {
        return Physics2D.OverlapCircle(frontCheck.position, .1f, groundLayer) != null;
    }

    /* private void OnTriggerEnter2D(Collider2D collision)
     {
         playerInRange = false;
         if (collision.tag == "Player")
         {
             playerInRange = true;
         }
     }*/

    void Destroy()
    {
        Destroy(gameObject);
    }



}
