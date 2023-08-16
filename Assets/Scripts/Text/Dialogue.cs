using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Dialogue : MonoBehaviour
{
    TMPro.TextMeshProUGUI myText;
    public bool frog { get; set; }
    public bool startGame { set; get; }

    float frogScore;
    float maskScore;

    string frogText;
    string maskText;

    //this is harsha
    void Awake()
    {
        myText = GetComponent<TMPro.TextMeshProUGUI>();
    }

    public void changeFrogScore(float points)
    {
        frogScore = Mathf.Clamp(frogScore + points, 0, 30);
    }

    public void changeMaskScore(float points)
    {
        maskScore = Mathf.Clamp(maskScore + points, 0, 30);
    }

    void Update()
    {
        if (startGame)
        {
            if (frog)
            {
                maskScore += Time.deltaTime;
                myText.text = frogText + "<b>" + maskText + "</b>";
            }
            else
            {
                frogScore += Time.deltaTime;
                myText.text = "<b>" + frogText + "</b>" + maskText;
            }
            //myText.text = timeValue.ToString("F2") + " seconds to catch " + player;

            frogText = "Frog: " + frogScore.ToString("F0");
            maskText = "                             Mask: " + maskScore.ToString("F0");
        }
        /*if (frogScore >= 40 || maskScore >= 40)
        {
            
        }*/

    }
}
