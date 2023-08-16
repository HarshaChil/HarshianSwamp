using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PointPopUp : MonoBehaviour
{

    private const float DISAPPEAR_TIMER_MAX = 1f;
    private TextMeshPro textMesh;
    float disappearTimer;
    Color textColor;
    Vector3 moveVector;

    public static PointPopUp Create(Vector3 position, int pointAmount)
    {
        Transform pointPopUpTransform = Instantiate(GameAssets.i.pointPopUp, position, Quaternion.identity);

        PointPopUp pointPopUp = pointPopUpTransform.GetComponent<PointPopUp>();
        pointPopUp.Setup(pointAmount);

        return pointPopUp;
    }

    public static PointPopUp CreateText(Vector3 position, string text)
    {
        Transform pointPopUpTransform = Instantiate(GameAssets.i.pointPopUp, new Vector3(position.x, position.y + 5f, position.y), Quaternion.identity);

        PointPopUp pointPopUp = pointPopUpTransform.GetComponent<PointPopUp>();
        pointPopUp.SetupString(text);

        return pointPopUp;
    }

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
    }
    public void Setup(int points)
    {
        textMesh.SetText(points.ToString());

        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        moveVector = new Vector3(1, 1) * 2f;
    }

    public void SetupString(string text)
    {
        textMesh.SetText(text);

        textColor = textMesh.color;
        disappearTimer = DISAPPEAR_TIMER_MAX;

        moveVector = new Vector3(1, 1) * 2f;
    }
    // Update is called once per frame
    void Update()
    {


        transform.position += moveVector * Time.deltaTime;
        transform.position -= moveVector * 2f * Time.deltaTime;


        /*if (disappearTimer > DISAPPEAR_TIMER_MAX * .5f)
        {
            //first half
            float increaseScaleAmount = 1f;
            transform.localScale += Vector3.one * increaseScaleAmount * Time.deltaTime;
        }
        else
        {
            //second half
            float decreaseScaleAmount = 1f;
            transform.localScale -= Vector3.one * decreaseScaleAmount * Time.deltaTime;
        }*/

        disappearTimer -= Time.deltaTime;
        if (disappearTimer < 0)
        {
            float disapearSpeed = 3f;
            textColor.a -= disapearSpeed * Time.deltaTime;
            textMesh.color = textColor;

            if (textColor.a < 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
