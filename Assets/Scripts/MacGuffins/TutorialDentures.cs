using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDentures : MonoBehaviour
{
    public EatingBar eatingBar;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            eatingBar.changeTimeLeft(10f);
            Destroy(gameObject);
        }
    }
}
