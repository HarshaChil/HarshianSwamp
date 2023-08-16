using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    Image timerBar;
    public float maxTime;
    public static float timeLeft;

    public DeathMenu death;

    private void Awake()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maxTime;
    }

    private void Start()
    {
        death.GetComponent<DeathMenu>();
    }

    void Update()
    {

        timeLeft = Mathf.Clamp(timeLeft, -1, maxTime);
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maxTime;
        }
    }

    public void changeTimeLeft(float addedTime)
    {
        timeLeft = Mathf.Clamp(timeLeft + addedTime, -1, maxTime);
    }
    // Update is called once per frame
    void AnimateBar()
    {

    }
}
