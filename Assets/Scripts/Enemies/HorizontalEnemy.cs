using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalEnemy : MonoBehaviour
{
    [SerializeField] float leftRange;
    [SerializeField] float rightRange;
    [SerializeField] float speed;

    public Transform popUp;

    float right;
    float left;
    bool movingLeft;
    bool canBeHit;

    SingleplayerGreenCrocodile croc;
    CircleCollider2D circleCollider;
    Animator anim;

    public AudioClip bite;

    void Awake()
    {
        left = transform.position.x - leftRange;
        right = transform.position.x + rightRange;
        circleCollider = GetComponent<CircleCollider2D>();
        croc = FindObjectOfType<SingleplayerGreenCrocodile>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        Moving();
    }

    void Moving()
    {
        if (movingLeft)
        {
            if (transform.position.x > left)
            {
                transform.position = new Vector2(transform.position.x - speed * Time.deltaTime, transform.position.y);
            }
            else
                movingLeft = false;
        }
        else
        {
            if (transform.position.x < right)
            {
                transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);
            }
            else
                movingLeft = true;
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
