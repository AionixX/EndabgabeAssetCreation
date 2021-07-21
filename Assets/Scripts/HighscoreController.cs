using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleLibrary;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
struct Score
{
    public string name;
    public int score;
}

public class HighscoreController : MonoBehaviour
{

    [SerializeField] string highScorePath = "highscoreList";
    [SerializeField] string nullNameText = "No highscore yet";
    [SerializeField] string nullScoreText = "-";
    [SerializeField] bool loadOnStart = true;
    [SerializeField] bool deleteOnStart = false;
    [SerializeField] int maxScores = 5;
    [SerializeField] TMP_InputField inputField;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] List<TMP_Text> nameList;
    [SerializeField] List<TMP_Text> scoreList;
    List<Score> highScoreList = new List<Score>();

    void Start()
    {
        if (deleteOnStart) SimpleDataLoader.DeleteData(highScorePath);
        if (!loadOnStart) return;
        LoadList();
    }

    void LoadList()
    {
        highScoreList = SimpleDataLoader.LoadData<List<Score>>(highScorePath);

        if (highScoreList == null)
        {
            highScoreList = new List<Score>();
            SaveList();
        }

        UpdateUI();
    }

    void SaveList()
    {
        SimpleDataLoader.SaveData(highScoreList, highScorePath);
        LoadList();
    }

    void UpdateUI()
    {
        for (int i = 0; i < scoreList.Count; i++)
        {
            nameList[i].text = highScoreList.Count > i ? highScoreList[i].name : nullNameText;
            scoreList[i].text = highScoreList.Count > i ? highScoreList[i].score.ToString() : nullScoreText;
        }
    }

    public void Submit()
    {
        if (inputField.text == "") return;
        // Debug.Log(inputField.text);

        Score newScore = new Score();
        newScore.name = inputField.text;
        newScore.score = GameManager.instance.player.score;

        // Debug.Log(newScore.name + " " + newScore.score);

        // foreach (Score score in highScoreList)
        // {
        //     Debug.Log(score.name + ":" + score.score);
        // }

        highScoreList.Add(newScore);
        // highScoreList.RemoveAll(x => x.score == 0);



        highScoreList.Sort((x, y) => y.score.CompareTo(x.score));
        if (highScoreList.Count > maxScores)
            highScoreList.RemoveRange(maxScores, highScoreList.Count - maxScores);

        SaveList();
    }

}
