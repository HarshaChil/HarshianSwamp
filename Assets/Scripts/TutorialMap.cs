using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialMap : MonoBehaviour
{
    public SingleplayerGreenCrocodile croc;
    void Update()
    {
        if (croc.transform.position.x > 18f)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
