using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class HudController : MonoBehaviour
{
    [SerializeField] GameObject startGameButton;
    [SerializeField] GameObject resumeGameButton;
    [SerializeField] GameObject quitGameButton;
    [SerializeField] GameObject gameStatsMenu;
    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text livesText;
    [SerializeField] TMP_Text killedEnemysText;
    [SerializeField] TMP_InputField nameInput;
    [SerializeField] GameObject submitButton;
    [SerializeField] GameObject highScoresButton;
    [SerializeField] GameObject closeHighScoresButton;
    [SerializeField] GameObject highscoreMenu;
    [SerializeField] List<TMP_Text> nameTextList;
    [SerializeField] string nullNameText;
    [SerializeField] string nullScoreText;
    [SerializeField] List<TMP_Text> scoreTextList;
    [SerializeField] GameObject howToPlayButton;
    [SerializeField] GameObject howToMenu;
    [SerializeField] GameObject howToCloseButton;
    [SerializeField] GameObject settingsButton;
    [SerializeField] GameObject settingsMenu;
    [SerializeField] GameObject settingsCloseButton;
    [SerializeField] Slider mouseSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider sfxSlider;

    bool highscoreOpen = false;

    public void Init()
    {
        OpenHighScores();
        mouseSlider.value = PlayerPrefs.GetFloat("mouseSlider");
        musicSlider.value = PlayerPrefs.GetFloat("musicSlider");
        sfxSlider.value = PlayerPrefs.GetFloat("sfxSlider");
        UpdatePlayerPrefs();
    }

    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
        howToMenu.SetActive(false);
    }

    public void CloseSettings()
    {
        settingsMenu.SetActive(false);
    }

    public void OpenHowTo()
    {
        howToMenu.SetActive(true);
        settingsMenu.SetActive(false);
        gameStatsMenu.SetActive(false);
        highscoreOpen = highscoreMenu.activeSelf;
        highscoreMenu.SetActive(false);

    }

    public void CloseHowTo()
    {
        howToMenu.SetActive(false);

        if (GameManager.instance.gamePaused)
            gameStatsMenu.SetActive(true);
        else if (highscoreOpen)
            highscoreMenu.SetActive(true);
    }

    public void OpenHighScores()
    {
        highscoreMenu.SetActive(true);
        highScoresButton.SetActive(false);
        gameStatsMenu.SetActive(false);
        closeHighScoresButton.SetActive(true);

        List<Score> scores = GameManager.instance.scores;
        for (int i = 0; i < nameTextList.Count; i++)
        {
            nameTextList[i].text = scores.Count > i ? scores[i].name : nullNameText;
            scoreTextList[i].text = scores.Count > i ? scores[i].score.ToString() : nullScoreText;
        }
    }

    public void CloseHighScores()
    {
        highscoreMenu.SetActive(false);
        closeHighScoresButton.SetActive(false);
        highScoresButton.SetActive(true);

        if (GameManager.instance.gamePaused)
            gameStatsMenu.SetActive(true);
    }

    public void SubmitScore()
    {
        GameManager.instance.SubmitScore(nameInput.text, GameManager.instance.player.score);
        GameManager.instance.LoadScene("Game");

    }
    public void StartGame()
    {
        if (GameManager.instance.gameOver)
        {
            GameManager.instance.LoadScene("Game");
            return;
        }

        if (GameManager.instance.gamePaused)
        {
            ClosePausedGame();
            return;
        }

        GameManager.instance.StartGame();

        howToMenu.SetActive(false);
        settingsMenu.SetActive(false);
        highscoreMenu.SetActive(false);
        gameStatsMenu.SetActive(false);
        startGameButton.SetActive(false);
        highScoresButton.SetActive(false);
        settingsButton.SetActive(false);
        howToPlayButton.SetActive(false);

    }

    public void OpenGameOver()
    {
        gameStatsMenu.SetActive(true);
        nameInput.interactable = true;
        scoreText.text = GameManager.instance.player.score.ToString();
        livesText.text = GameManager.instance.player.livesLeft.ToString();
        submitButton.SetActive(true);
        startGameButton.SetActive(true);
        highScoresButton.SetActive(true);
        closeHighScoresButton.SetActive(false);
        settingsButton.SetActive(true);
        howToPlayButton.SetActive(true);
        quitGameButton.SetActive(true);
    }

    public void QuitGame()
    {
        GameManager.instance.QuitGame();
    }

    public void OpenPauseGame()
    {
        highscoreMenu.SetActive(false);
        gameStatsMenu.SetActive(true);
        scoreText.text = GameManager.instance.player.score.ToString();
        livesText.text = GameManager.instance.player.livesLeft.ToString();
        resumeGameButton.SetActive(true);
        highScoresButton.SetActive(true);
        closeHighScoresButton.SetActive(false);
        settingsButton.SetActive(true);
        howToPlayButton.SetActive(true);
        quitGameButton.SetActive(true);
    }

    public void ClosePausedGame()
    {
        GameManager.instance.ResumeGame();
        gameStatsMenu.SetActive(false);
        resumeGameButton.SetActive(false);
        highScoresButton.SetActive(false);
        settingsButton.SetActive(false);
        howToPlayButton.SetActive(false);
    }

    public void UpdatePlayerPrefs()
    {
        Debug.Log("Update");
        PlayerPrefs.SetFloat("mouseSlider", mouseSlider.value);
        GameManager.instance.UpdateCameraSpeed(mouseSlider.value);
        PlayerPrefs.SetFloat("musicSlider", musicSlider.value);
        GameManager.instance.ChangeMasterVolumeMusic(musicSlider.value);
        PlayerPrefs.SetFloat("sfxSlider", sfxSlider.value);
        GameManager.instance.ChangeMasterVolumeSFX(sfxSlider.value);
    }





}

