using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerBullet : MonoBehaviour
{
    public float bulletSpeed;
    public float stageBoundsX;
    public float stageBoundsY;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * Time.deltaTime * bulletSpeed;
        if (transform.position.x > stageBoundsX || transform.position.x < -stageBoundsX ||
            transform.position.y > stageBoundsY || transform.position.y < -stageBoundsY)
            Destroy(this.gameObject);
    }
}
