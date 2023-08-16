using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSpeedBoost : MonoBehaviour
{
    bool val;
    float lifetime = 12f;
    SingleplayerGreenCrocodile croc;
    // Start is called before the first frame update
    public AudioClip power;

    private void Awake()
    {
        croc = FindObjectOfType<SingleplayerGreenCrocodile>();
    }

    private void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            if (val) return;
            val = true;

            SoundManager.instance.PlaySound(power);
            croc.moveSpeed = 25;

            Destroy(gameObject);
        }
    }
}
