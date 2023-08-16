using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoopTimeBar : MonoBehaviour
{
    Image timerBar;
    public float maxTime { get; set; }
    public float timeLeft { get; set; }

    [SerializeField] Token token;

    public Color color { get; set; } = Color.blue;

    private void Awake()
    {
        timerBar = GetComponent<Image>();
        maxTime = 2.5f;

    }

    private void Start()
    {
        token.GetComponent<Token>();
    }

    void Update()
    {
        if (token.frog)
        {
            color = Color.blue;
            timerBar.color = color;
        }
        else
        {
            color = Color.green;
            timerBar.color = color;
        }


        if (timeLeft > 0)
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
    // Update is called once per frame
}
