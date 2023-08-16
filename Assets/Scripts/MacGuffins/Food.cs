using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    [SerializeField] SingleplayerText textBox;
    [SerializeField] EatingBar eatingBar;
    [SerializeField] SingleplayerGreenCrocodile croc;

    public HealthBar timebar;
    public float addTime;

    bool val;

    private void Awake()
    {
        //addTime = 2f;
    }

    private void Start()
    {
        textBox.GetComponent<SingleplayerText>();
        timebar.GetComponent<HealthBar>();
        eatingBar.GetComponent<EatingBar>();
        croc.GetComponent<SingleplayerGreenCrocodile>();
    }

    // Update is called once per frame
    void Update()
    {
        val = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (val) return;
        val = true;

        if (eatingBar.timeLeft > 0)
        {
            if (collision.tag == "Mask")
            {
                //textBox.maskScore += 1f;
                timebar.changeTimeLeft(addTime);
                croc.maxVelocity += .5f;
            }
        }
    }
}
