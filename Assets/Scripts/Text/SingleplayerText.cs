using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleplayerText : MonoBehaviour
{
    TMPro.TextMeshProUGUI myText;

    public static int score;
    public static int counter;

    string maskText;

    //this is harsha
    void Awake()
    {
        myText = GetComponent<TMPro.TextMeshProUGUI>();
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("FirstLevel"))
        {
            score = 0;
        }
        counter = 0;
    }

    void Update()
    {
        maskText = "Score: " + score;
        myText.text = maskText;
    }
    /*if (frogScore >= 40 || maskScore >= 40)
    {

    }*/
    public static void addScore(int addedScore)
    {
        score = score + addedScore + counter * 50;
    }
}
