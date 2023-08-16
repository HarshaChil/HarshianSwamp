using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    [SerializeField] float upRange;
    [SerializeField] float downrange;
    [SerializeField] float speed;

    public Transform popUp;

    float top;
    float bottom;
    bool movingDown;

    SingleplayerGreenCrocodile croc;
    Animator anim;

    public AudioClip bite;


    CircleCollider2D circleCollider;
    bool canBeHit;


    void Awake()
    {
        bottom = transform.position.y - downrange;
        top = transform.position.y + upRange;

        circleCollider = GetComponent<CircleCollider2D>();
        croc = FindObjectOfType<SingleplayerGreenCrocodile>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (movingDown)
        {
            if (transform.position.y > bottom)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y - speed * Time.deltaTime);
            }
            else
                movingDown = false;
        }
        else
        {
            if (transform.position.y < top)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
            }
            else
                movingDown = true;
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

    private void Destroy()
    {
        Destroy(gameObject);
    }


}
