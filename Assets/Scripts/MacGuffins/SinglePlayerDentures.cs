using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinglePlayerDentures : MonoBehaviour
{
    int rnd;

    public GameObject dentureGraphics;
    //int oldRnd;
    Vector3 coordinate;
    public Transform[] dentureSpawn;
    [SerializeField] SingleplayerText textBox;
    public EatingBar eatingBar;
    public float addTime;

    public AudioClip teeth;

    public float maxTime;
    float cooldown;

    bool val;
    bool hit;

    private void Awake()
    {
        randomPosition();
        //addTime = 2f;
    }

    private void Start()
    {
        /*  textBox.GetComponent<SingleplayerText>();
          timebar.GetComponent<EatingBar>();*/
    }

    // Update is called once per frame
    void Update()
    {

        val = false;
        if (transform.position.y < -5.11f) //If they fall through the ground, they lose the cherry
        {
            randomPosition();
        }

        if (hit)
        {
            cooldown += Time.deltaTime;
            if (cooldown > maxTime)
            {
                randomPosition();
                dentureGraphics.SetActive(true);
                hit = false;
                cooldown = 0;

            }
        }


        //positions[0] = new Vector3(platform1.position.x + 1f, platform1.position.y + 1f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            if (val) return;
            val = true;
            hit = true;
            SoundManager.instance.PlaySound(teeth);
            dentureGraphics.SetActive(false);
            //textBox.maskScore += 1f;
            eatingBar.changeTimeLeft(addTime);
        }
    }

    void randomPosition()
    {
        //transform.position = new Vector3()
        coordinate = dentureSpawn[Random.Range(0, dentureSpawn.Length)].position;

        while (coordinate == transform.position)
        {
            coordinate = dentureSpawn[Random.Range(0, dentureSpawn.Length)].position;
        }
        transform.position = coordinate;

    }
}
