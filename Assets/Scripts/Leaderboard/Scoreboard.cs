using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;



public class Scoreboard : MonoBehaviour
{
    [SerializeField] private int maxScoreboardEntries = 5; //How many entries on the scoreboard
    [SerializeField] private Transform highscoresHolderTransform = null;
    [SerializeField] private GameObject scoreboardEntryObject = null;

    [Header("Test")] //test entry data
    [SerializeField] private string testEntryName = "New Name";
    [SerializeField] private int testEntryScore = 0;

    private string SavePath => $"{Application.persistentDataPath}/highscores.json";

    private void Start()
    {
        ScoreboardSaveData savedScores = GetSavedScores(); //get the scores

        UpdateUI(savedScores);

        SaveScores(savedScores);
    }

    private void Update()
    {
        /*if (Input.GetMouseButton(0))
        {
            AddTestEntry();
        }*/
    }

    [ContextMenu("Add Test Entry")]
    public void AddTestEntry() //Test
    {
        AddEntry(new ScoreboardEntryData()
        {
            entryName = testEntryName,
            entryScore = testEntryScore
        });
    }

    public void AddEntry(ScoreboardEntryData scoreboardEntryData) //Adding entry
    {
        ScoreboardSaveData savedScores = GetSavedScores();

        bool scoreAdded = false;

        //Check if the score is high enough to be added.
        for (int i = 0; i < savedScores.highscores.Count; i++) //Order the scores
        {
            if (scoreboardEntryData.entryScore > savedScores.highscores[i].entryScore)
            {
                savedScores.highscores.Insert(i, scoreboardEntryData);
                scoreAdded = true; //Only add once
                break;
            }
        }

        //Check if the score can be added to the end of the list.
        if (!scoreAdded && savedScores.highscores.Count < maxScoreboardEntries) //Add the score if it hasn't already been added
        {
            savedScores.highscores.Add(scoreboardEntryData);
        }

        //Remove any scores past the limit.
        if (savedScores.highscores.Count > maxScoreboardEntries) //Remmove entry
        {
            savedScores.highscores.RemoveRange(maxScoreboardEntries, savedScores.highscores.Count - maxScoreboardEntries);
        }

        UpdateUI(savedScores); //Update then save the scores

        SaveScores(savedScores);
    }

    public void UpdateUI(ScoreboardSaveData savedScores) //Display the scores
    {
        foreach (Transform child in highscoresHolderTransform) //Destroy then replace entries
        {
            Destroy(child.gameObject);
        }

        foreach (ScoreboardEntryData highscore in savedScores.highscores)
        {
            Instantiate(scoreboardEntryObject, highscoresHolderTransform).GetComponent<ScoreboardEntryUI>().Initialise(highscore);
        }
    }

    private ScoreboardSaveData GetSavedScores()
    {
        if (!File.Exists(SavePath)) //if the file doesn't exist, delete it
        {
            File.Create(SavePath).Dispose();
            return new ScoreboardSaveData();
        }

        using (StreamReader stream = new StreamReader(SavePath)) //prevents leakage
        {
            string json = stream.ReadToEnd(); //Get the string

            return JsonUtility.FromJson<ScoreboardSaveData>(json);
        }
    }

    private void SaveScores(ScoreboardSaveData scoreboardSaveData) //Save the scores
    {
        using (StreamWriter stream = new StreamWriter(SavePath))
        {
            string json = JsonUtility.ToJson(scoreboardSaveData, true);
            stream.Write(json); //write into save file
        }
    }
}

