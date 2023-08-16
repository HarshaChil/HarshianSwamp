using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public SceneFader fader;
    public void Select(string levelName)
    {
        fader.FadeTo(levelName);
    }

    public void Quit()
    {
        Application.Quit();
        print("Quit");
    }
}
