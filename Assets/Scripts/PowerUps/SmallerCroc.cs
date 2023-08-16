using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallerCroc : MonoBehaviour
{
    bool val;
    float lifetime = 12f;
    PowerUpController powerUpController;
    // Start is called before the first frame update
    public AudioClip power;

    private void Awake()
    {
        powerUpController = FindObjectOfType<PowerUpController>();
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;

        if (lifetime <= 0)
            Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            if (val) return;
            val = true;

            PointPopUp.CreateText(transform.position, "Small");
            SoundManager.instance.PlaySound(power);
            powerUpController.powerUps.Add("Small");

            Destroy(gameObject);
        }
    }
}
