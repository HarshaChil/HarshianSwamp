using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    //public Transform target;
    /*    public GameObject sre;
        EatingBar eatingBar;*/

    public float speed;
    public float nextWayPointDistance; //How close enemy gets to waypoint before moving to the next one

    public Transform enemyGraphics;
    public Transform popUp;

    Path path; //Current path enemy is following
    int currentWaypoint = 0; //stores current wavepoint enemy is targeting
    bool reachedEndOfPath = false; //Whether or not it has reached the path

    Seeker seeker;
    Rigidbody2D body;
    CapsuleCollider2D capsuleCollider;
    Animator anim;

    public AudioClip bite;

    /* [SerializeField] EatingBar eatingBar;
     [SerializeField] TimeBar timeBar;*/
    SingleplayerGreenCrocodile croc; //Get a variable for the croc

    public float knockBackMultiplier;
    bool dead;
    bool canBeHit;

    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        body = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        anim = GetComponent<Animator>();

        /*   GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
           target = searchResult.transform;*/

        croc = FindObjectOfType<SingleplayerGreenCrocodile>();

        /* sre = GameObject.FindGameObjectWithTag("EatingBar");
         eatingBar = sre.GetComponent<EatingBar>();*/
        /*  timeBar.GetComponent<TimeBar>();
          eatingBar.GetComponent<EatingBar>();*/



        InvokeRepeating("UpdatePath", 0f, .5f); //Repeat path every .5 seconds

    }

    void UpdatePath()
    {
        if (seeker.IsDone()) //Isn't currently calculating a path
        {
            seeker.StartPath(body.position, croc.transform.position, OnPathComplete); //Start point, end of path(target), function to call on completition
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count) //Reached end of path
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        //point arrow to where we want to be, arrow always has a lenth of one
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - body.position).normalized; //Points from position to the next waypoint
        Vector2 force = direction * speed * Time.deltaTime;

        body.AddForce(force);

        float distance = Vector2.Distance(body.position, path.vectorPath[currentWaypoint]); //Distance from current position to next waypoint

        if (distance < nextWayPointDistance)
        {
            currentWaypoint++; //Move on to the next waypoint
        }

        if (force.x >= 0.01f) //Enemy faces right direction
        {
            enemyGraphics.localScale = new Vector3(-3f, 3f, 1f);
        }
        else if (force.x <= -0.01f)
        {
            enemyGraphics.localScale = new Vector3(3f, 3f, 1f);
        }
        stopFall();
    }

    void OnPathComplete(Path p) //When the path is complete
    {
        if (!p.error)
        {
            path = p; //Set current path to the the path
            currentWaypoint = 0; //reset progress along path
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

    void takeDamage()
    {
        /*if (eatingBar.timeLeft > 0)
                       {*/
        HealthBar.timeLeft += 4f;
        SingleplayerText.counter++;
        PointPopUp.Create(popUp.position, 100 + 50 * SingleplayerText.counter);
        SingleplayerText.addScore(100);

        SoundManager.instance.PlaySound(bite);
        anim.SetTrigger("die");


    }

    private void Destroy()
    {
        Destroy(gameObject);
    }







}
/*public void takeDamage(float knockBack)
{
    print("hi");
    body.AddForce(new Vector2(knockBack * knockBackMultiplier, knockBackMultiplier * 20));
    capsuleCollider.enabled = false;
    body.freezeRotation = false;
    body.AddTorque(1000);
    dead = true;
    *//*if (eatingBar.timeLeft > 0)
    {
        timeBar.changeTimeLeft(4f);
        //croc.maxVelocity += .5f;
    }*//*
}*/


