using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    int rnd;
    //int oldRnd;
    Vector3 coordinate;
    Vector3[] positions = new Vector3[3];
    [SerializeField] CoopRacingDialogue textBox;

    public CoopTimeBar timebar;

    bool val;
    public bool frog;

    private void Awake()
    {
        positions[0] = new Vector3(-5.82f, -1.64f, 0f);
        positions[1] = new Vector3(8.07f, -2.07f, 0f);
        positions[2] = new Vector3(6.08f, 2.35f, 0f);
    }

    private void Start()
    {
        textBox.GetComponent<CoopRacingDialogue>();
        timebar.GetComponent<CoopTimeBar>();
    }

    // Update is called once per frame
    void Update()
    {
        val = false;
        if (transform.position.y < -5.11f) //If they fall through the ground, they lose the cherry
        {
            randomPosition();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (val) return;
        val = true;
        if (collision.tag == "Mask")
        {
            randomPosition();
            timebar.changeTimeLeft(1.5f);
            frog = false;
            //textBox.maskScore += 1f;
        }
        if (collision.tag == "Frog")
        {
            randomPosition();
            timebar.changeTimeLeft(1.5f);
            frog = true;
            //eventually change timebar color
            //textBox.frogScore += 1f;
        }
    }

    void randomPosition()
    {
        coordinate = positions[Random.Range(0, 3)];

        while (coordinate == transform.position)
        {
            coordinate = positions[Random.Range(0, 3)];
        }
        transform.position = coordinate;

    }
}
