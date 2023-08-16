using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EatingBar : MonoBehaviour
{
    Image timerBar;
    public float maxTime;
    public float timeLeft;

    public static bool isTimeLeft;

    private void Awake()
    {
        timeLeft = 0;
        timerBar = GetComponent<Image>();
    }

    private void Start()
    {

    }

    void Update()
    {
        if (timeLeft > 0)
        {
            isTimeLeft = true;
        }
        else
        {
            isTimeLeft = false;
            SingleplayerText.counter = -1;
        }

        if (isTimeLeft)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
        else
        {
            timerBar.fillAmount = 0;
        }
    }

    public void changeTimeLeft(float addedTime)
    {
        timeLeft = Mathf.Clamp(timeLeft + addedTime, -1, maxTime);
    }
}
