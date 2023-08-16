using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed;

    Vector2 direction;
    bool hit;
    float lifetime;
    Vector2 myPos;
    Vector2 targetPos;

    CircleCollider2D circleCollider;
    Rigidbody2D body;
    SingleplayerGreenCrocodile croc;

    private void Awake()
    {

        body = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        croc = FindObjectOfType<SingleplayerGreenCrocodile>();
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if (hit) return; //When hit is not true, shoot the projectile at a certain speed

        //Vector2 direction = -(myPos - targetPos); //get the direction to the target
        body.velocity = direction * speed; //shoot the bu

        /* float movementSpeed = speed * Time.deltaTime * direction;
         transform.Translate(movementSpeed, 0, 0);*/


        //  body.velocity = new Vector2(movementSpeed, 0f);

        lifetime += Time.deltaTime; //So that the projectile doesn't go on forever
        //print(lifetime);
        if (lifetime > 3f) gameObject.SetActive(false);
    }


    void OnTriggerEnter2D(Collider2D collision)//For hitting the enemy
    {


        if (collision.tag == "Player")
        {

            hit = true;

            float xForce = transform.position.x - collision.transform.position.x;
            croc.takeDamage(xForce);
            circleCollider.enabled = false;
            gameObject.SetActive(false);
        }

    }

    public void setDirection(Vector2 _direction) //Set the direction of
    {
        lifetime = 0;
        /* myPos = new Vector2(transform.position.x, transform.position.y);
         targetPos = target.position;*/
        hit = false;
        direction = _direction;
        gameObject.SetActive(true); //Make sure the fireball is active

        circleCollider.enabled = true;
        /*
                float localScaleX = transform.localScale.x; //Make sure that the fireball is facing the same direction it is movement
                if (Mathf.Sign(localScaleX) != _direction) //If it's not facing the right way, flip it
                    localScaleX = -localScaleX;*/

        /* transform.localScale = new Vector2(localScaleX, transform.localScale.y);*/
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
    }
}
