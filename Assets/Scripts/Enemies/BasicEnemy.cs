using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    //PointPopUp FloatingText;
    Rigidbody2D body;
    CapsuleCollider2D capsuleCollider;
    public LayerMask groundLayer;
    public LayerMask playerLayer;
    public float speed = 20f;
    public Transform frontCheck;
    public Transform popUp;
    Collider2D player;
    public float knockBackMultiplier;
    bool dead;
    bool canBeHit;

    public AudioClip bite;

    [SerializeField] private float idleDuration;
    private float idleTimer;

    SingleplayerGreenCrocodile croc;
    Animator anim;

    public float leftRange;
    public float rightRange;

    float right;
    float left;
    bool movingLeft;

    Vector3 initScale;

    /*  [SerializeField] EatingBar eatingBar;
      [SerializeField] SingleplayerGreenCrocodile croc;
      [SerializeField] TimeBar timebar;*/

    void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        dead = false;
        left = transform.position.x - leftRange;
        right = transform.position.x + rightRange;
        initScale = transform.localScale;
        anim = GetComponent<Animator>();
    }

    /* void ShowFloatingText()
     {
         Instantiate(FloatingText, transform.position, Quaternion.identity, transform);
     }*/

    // Start is called before the first frame update
    void Start()
    {
        croc = GameObject.FindObjectOfType<SingleplayerGreenCrocodile>(); //Get croc
        /*body.freezeRotation = true;
        eatingBar.GetComponent<EatingBar>();
        croc.GetComponent<SingleplayerGreenCrocodile>();
        timebar.GetComponent<TimeBar>();

        GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
        croc = searchResult.transform;*/
    }

    // Update is called once per frame

    void Update()
    {
        if (OnWall() && !dead)
            body.AddForce(Vector2.up * 5f);

        //transform.localScale = new Vector2(-transform.localScale.x, 1);

        /*if (Mathf.Abs(body.velocity.x) < speed)
            body.velocity += new Vector2(transform.localScale.x * speed, 0);

        if (Mathf.Abs(transform.position.x) > 9f)
        {
            transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
            body.velocity = new Vector2(-body.velocity.x, body.velocity.y);
        }
    }
*/

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

        stopFall();
    }
    /*  private void OnTriggerEnter2D(Collider2D collision)
      {
          if (collision.tag == "Ground")
          {
              body.AddForce(Vector2.up * 10f);
          }
      }*/

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
            if (canBeHit) return;
            if (EatingBar.isTimeLeft)
            {
                takeDamage();
                canBeHit = true;
            }

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


    bool OnWall()
    {
        return Physics2D.OverlapCircle(frontCheck.position, .3f, groundLayer) != null;
    }


    void takeDamage()
    {
        /*if (eatingBar.timeLeft > 0)
                       {*/
        // FloatingText = GameObject.FindObjectOfType<PointPopUp>();
        //Vector3 childTransform = transform.GetChild(1).gameObject.transform.localScale;
        //Vector3 childTransform = croc.transform.position;

        HealthBar.timeLeft += 4f;
        //Instantiate(FloatingText, transform.position, Quaternion.identity);
        SingleplayerText.counter++;
        PointPopUp.Create(popUp.position, 100 + 50 * SingleplayerText.counter);

        SingleplayerText.addScore(100);
        //  timeBar.changeTimeLeft(4f);
        SoundManager.instance.PlaySound(bite);



        anim.SetTrigger("die");
    }

    private void Destroy()
    {
        Destroy(gameObject);
    }









}

/* public void takeDamage(float knockBack)
 {
     body.AddForce(new Vector2(knockBack * knockBackMultiplier, knockBackMultiplier * 20));
     capsuleCollider.enabled = false;
     body.freezeRotation = false;
     body.AddTorque(1000);
     dead = true;
     *//* if (eatingBar.timeLeft > 0)
      {
          timebar.changeTimeLeft(4f);
          //croc.maxVelocity += .5f;
      }*//*
 }*/

