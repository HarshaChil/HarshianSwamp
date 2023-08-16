using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public SingleplayerGreenCrocodile croc;

    public ArrayList powerUps = new ArrayList();


    public float powerupMaxCountdown;
    public float countdown;
    public Transform[] spawnPoints;
    public Transform[] powerUp; //Position of power up


    private void Awake()
    {
        countdown = powerupMaxCountdown;
    }

    // Update is called once per frame
    void Update()
    {

        countdown -= Time.deltaTime;

        if (countdown <= 0)
        {

            SpawnPowerUp();
            countdown = powerupMaxCountdown;
        }


        if (powerUps.Contains("Speed"))
        {
            croc.moveSpeed = 25;
        }
        else
        {
            croc.moveSpeed = 15;
        }

        if (powerUps.Contains("Jump"))
        {
            croc.jumpPower = 25;
        }
        else
        {
            croc.jumpPower = 15;
        }

        if (powerUps.Contains("Dash"))
        {
            //croc.dashThroughWalls = true;
            croc.fallGravityMultiplier = .5f;
        }
        else
        {
            croc.fallGravityMultiplier = 2f;
        }

        if (powerUps.Contains("Small"))
        {
            if (!croc.isSmall)
            {
                croc.SmallerCroc(true);
            }

        }
        else
        {
            croc.SmallerCroc(false);
        }

        if (powerUps.Contains("CursedSmall"))
        {
            if (!croc.isCursedSmall)

                croc.CursedSmallerCroc(true);
        }
        else
        {
            croc.CursedSmallerCroc(false);
        }

        if (powerUps.Contains("BigHitbox"))
        {
            croc.ActivateLargerHitbox(true);
        }
        else
        {
            croc.ActivateLargerHitbox(false);
        }


    }

    void SpawnPowerUp() //Spawn an enemy
    {
        Transform pw = powerUp[Random.Range(0, powerUp.Length)];
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(pw, new Vector3(_sp.position.x, _sp.position.y, 0), Quaternion.identity);
    }

}

