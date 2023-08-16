using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedSmallCroc : MonoBehaviour
{
    bool val;
    float lifetime = 12f;
    PowerUpController powerUpController;
    public AudioClip power;
    // Start is called before the first frame update

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

            PointPopUp.CreateText(transform.position, "CursedSmall");
            SoundManager.instance.PlaySound(power);
            powerUpController.powerUps.Add("CursedSmall");

            Destroy(gameObject);
        }
    }
}
