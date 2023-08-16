using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathMenu : MonoBehaviour
{
    public static bool isdeadPlayer;

    public GameObject deathMenuUI;
    public GameObject leaderboard;
    public SceneFader fader;
    public Input playername;
    public Name_Input namer;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip click;
    string username;
    bool val;
    bool isWinningLevel;

    Scoreboard scoreboard;


    private void Awake()
    {
        Time.timeScale = 1;
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("WinningScreen"))
        {
            deathMenuUI.SetActive(true);
            isWinningLevel = true;
            SoundManager.instance.PlaySound(win);
        }
        else
        {
            deathMenuUI.SetActive(false);
        }


        leaderboard.SetActive(false);
        scoreboard = gameObject.GetComponentInParent<Scoreboard>();
        namer = GetComponent<Name_Input>();
    }

    private void Update()
    {
        if (isdeadPlayer || isWinningLevel)
        {

            //Time.timeScale = 0;
            deathMenuUI.SetActive(true);
            leaderboard.SetActive(true);

            if (isdeadPlayer) SoundManager.instance.PlaySound(lose);


            if (!val)
            {
                namer.show();
                username = namer.getName();
                scoreboard.AddEntry(new ScoreboardEntryData()
                {
                    
                    entryName = username,
                    entryScore = SingleplayerText.score
                });

                val = true;

                //scoreboard.UpdateUI();
            }


        }
    }

    public void Retry()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        SoundManager.instance.PlaySound(click);
        SceneManager.LoadScene("FirstLevel");

        deathMenuUI.SetActive(false);
        isdeadPlayer = false;
    }

    public void MoveToScene()
    {
        SoundManager.instance.PlaySound(click);
        SceneManager.LoadScene("FirstLevel");

        deathMenuUI.SetActive(false);
    }

    public void LoadMenu()
    {
        SoundManager.instance.PlaySound(click);
        //Time.timeScale = 1f;
        fader.FadeTo("MainMenu");
    }

}
