using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisingPlatform : MonoBehaviour
{
    [SerializeField] float upRange;
    [SerializeField] float downrange;
    [SerializeField] float speed;

    float top;
    float bottom;
    bool movingDown;
    public float movement;

    void Awake()
    {
        bottom = transform.position.y - downrange;
        top = transform.position.y + upRange;
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
                movement = -speed;
            }
            else
                movingDown = false;
        }
        else
        {
            if (transform.position.y < top)
            {
                transform.position = new Vector2(transform.position.x, transform.position.y + speed * Time.deltaTime);
                movement = speed;
            }
            else
                movingDown = true;
        }

    }
}
