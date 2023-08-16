using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{
    //Get black canvas
    public Image img;
    public AnimationCurve curve;
    // Start is called before the first frame update

    private void Start()
    {
        StartCoroutine(FadeIn()); //Fade in to this scene
    }

    public void FadeTo(string scene)
    {
        StartCoroutine(FadeOut(scene)); //Fade out to new scene
    }




    IEnumerator FadeIn()
    {
        float t = 1f;

        while (t > 0f)
        {
            t -= Time.deltaTime * 1f; //How fast
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a); //Animate from 0 to 1 in 1 second
            yield return 0; //Skip to next frame
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;

        while (t > 1f)
        {
            t += Time.deltaTime * 1f; //How fast
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a); //Animate from 0 to 1 in 1 second
            yield return 0; //Skip to next frame
        }

        SceneManager.LoadScene(scene); //Load the new scene

    }







}
