using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SidePlatform : MonoBehaviour
{
    [SerializeField] float leftRange;
    [SerializeField] float rightRange;
    [SerializeField] float speed;

    public float movement;

    float right;
    float left;
    bool movingLeft;

    void Awake()
    {
        left = transform.position.x - leftRange;
        right = transform.position.x + rightRange;
    }

    // Update is called once per frame
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
}
