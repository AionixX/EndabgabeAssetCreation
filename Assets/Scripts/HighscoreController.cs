using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleLibrary;
using UnityEngine.UI;
using TMPro;
using System;

[Serializable]
public struct Score
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
    // [SerializeField] TMP_InputField inputField;
    // [SerializeField] TMP_Text scoreText;
    // [SerializeField] List<TMP_Text> nameList;
    // [SerializeField] List<TMP_Text> scoreList;
    public List<Score> highScoreList = new List<Score>();

    void Start()
    {
        if (deleteOnStart) SimpleDataLoader.DeleteData(highScorePath);
        if (!loadOnStart) return;
        LoadList();
    }

    public List<Score> LoadList()
    {
        highScoreList = SimpleDataLoader.LoadData<List<Score>>(highScorePath);

        if (highScoreList == null)
        {
            highScoreList = new List<Score>();
            SaveList(highScoreList);
        }

        return highScoreList;

        // UpdateUI();
    }

    public void SaveList(List<Score> _scores)
    {
        SimpleDataLoader.SaveData(_scores, highScorePath);
        LoadList();
    }

    public void Submit(string _name = "", int _score = 0)
    {
        if (_name == "") return;

        Score newScore = new Score();
        newScore.name = _name;
        newScore.score = _score;

        highScoreList.Add(newScore);
        highScoreList.RemoveAll(x => x.score == 0);


        highScoreList.Sort((x, y) => y.score.CompareTo(x.score));
        if (highScoreList.Count > maxScores)
            highScoreList.RemoveRange(maxScores, highScoreList.Count - maxScores);

        SaveList(highScoreList);
    }

}
