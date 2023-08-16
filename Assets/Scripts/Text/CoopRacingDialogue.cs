using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoopRacingDialogue : MonoBehaviour
{
    TMPro.TextMeshProUGUI myText;

    public float frogScore { get; set; }
    public float maskScore { get; set; }

    string frogText;
    string maskText;

    //this is harsha
    void Awake()
    {
        myText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    void Update()
    {
        frogText = "Frog: " + frogScore.ToString("F0");
        maskText = "                             Mask: " + maskScore.ToString("F0");
        myText.text = frogText + maskText;
    }
    /*if (frogScore >= 40 || maskScore >= 40)
    {

    }*/

}

