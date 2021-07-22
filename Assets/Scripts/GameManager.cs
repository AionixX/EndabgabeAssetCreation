using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Cinemachine;
using AudioManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] public PlayerController player = null;
    [SerializeField] public CinemachineFreeLook playerCam = null;
    [SerializeField] HighscoreController highscoreController = null;
    [SerializeField] public List<Score> scores = null;
    [SerializeField] public HudController hud = null;
    [SerializeField] GameObject menuCanvas = null;
    [SerializeField] GameObject gameCanvas = null;
    [SerializeField] GameObject menuButtons = null;
    [SerializeField] GameObject pauseButtons = null;
    // [SerializeField] GameObject pauseCanvas = null;
    [SerializeField] GameObject gameOverCanvas = null;
    [SerializeField] Slider volumeSliderMusic = null;
    [SerializeField] Slider volumeSliderSFX = null;
    [SerializeField] Slider mouseSlider = null;
    [SerializeField] float openGameOverTime = 2f;
    [SerializeField] TMP_Text gameOverScoreText = null;
    [SerializeField] List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] List<EnemyController> enemyPrefabs = new List<EnemyController>();
    [SerializeField] int startEnemys = 5;
    [SerializeField] float instantiateStartTime = 3f;
    [SerializeField] float instantiateTimeLoss = 0.05f;
    [SerializeField] float instantiateTimeMin = 0.2f;

    public int numEnemys = 0;
    public int activeEnemys = 0;
    public float totalEnemys = 0;

    public bool gamePaused = false;
    public bool gameStarted = false;
    public bool gameOver = false;
    private List<GameObject> enemys = new List<GameObject>();
    float instantiateTime = 2f;
    float actualInstantiateTimer = 0f;
    float baseCameraSpeedX = 0f;
    float baseCameraSpeedY = 0f;


    void Start()
    {
        
        if (GameManager.instance)
        {
            Debug.LogError("Already found an instance of GameManager!");
            return;
        }
        GameManager.instance = this;

        baseCameraSpeedX = playerCam.m_XAxis.m_MaxSpeed;
        baseCameraSpeedY = playerCam.m_YAxis.m_MaxSpeed;

        ChangeMasterVolumeMusic();
        ChangeMasterVolumeSFX();
        UpdateCameraSpeed();

        scores = highscoreController.LoadList();
        hud.Init();

        gamePaused = false;
        // StartGame();
    }

    void Update()
    {
        if (!gameStarted) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();

        actualInstantiateTimer -= Time.deltaTime;
        if (actualInstantiateTimer <= 0f)
            SpawnEnemy();
    }

    public void SubmitScore(string _name, int _score) {
        highscoreController.Submit(_name, _score);
    }
    public void UpdateCameraSpeed(float _value = 1f) {
        playerCam.m_XAxis.m_MaxSpeed = _value * baseCameraSpeedX;
        playerCam.m_YAxis.m_MaxSpeed = _value * baseCameraSpeedY;
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void UnlockCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlaySound(string _soundName) {
        AudioManager.instance.Play(_soundName);
    }

    public void StartGame()
    {
        AudioManager.instance.Play("Game");
        AudioManager.instance.Pause("Menu");

        LockCursor();
        baseCameraSpeedX = playerCam.m_XAxis.m_MaxSpeed;
        baseCameraSpeedY = playerCam.m_YAxis.m_MaxSpeed;

        gameOverCanvas.SetActive(false);
        menuButtons.SetActive(false);
        pauseButtons.SetActive(false);
        // pauseCanvas.SetActive(false);
        menuCanvas.SetActive(false);

        gameCanvas.SetActive(true);

        gamePaused = false;
        gameStarted = true;
        Time.timeScale = 1f;
        instantiateTime = instantiateStartTime;
        player.StartGame();

        for (int i = 0; i < startEnemys; i++)
        {
            SpawnEnemy();
        }
    }

    public void PauseGame()
    {
        // pauseCanvas.SetActive(true);
        menuCanvas.SetActive(true);
        // pauseButtons.SetActive(true);
        gameCanvas.SetActive(false);
        gamePaused = true;
        hud.OpenPauseGame();
        // Time.timeScale = 0f;
        UnlockCursor();
    }

    public void ResumeGame()
    {
        // pauseCanvas.SetActive(false);
        menuCanvas.SetActive(false);
        // pauseButtons.SetActive(false);
        gameCanvas.SetActive(true);
        gamePaused = false;
        // Time.timeScale = 1f;
        LockCursor();
    }

    public void EndGame()
    {
        gamePaused = true;
        gameOver = true;
        gameOverScoreText.text = player.score.ToString();
        UnlockCursor();

        StartCoroutine(OpenGameOverScreen());
    }

    public void LoadScene(String _name)
    {
        ResumeGame();
        UnlockCursor();
        SceneManager.LoadSceneAsync(_name);
    }

    public void EnemyDied()
    {
        instantiateTime = Mathf.Clamp(instantiateTime - instantiateTimeLoss, instantiateTimeMin, float.MaxValue);
        activeEnemys--;
    }

    private void SpawnEnemy()
    {
        Vector3 rndPos = GetRandomSpawnPosition();
        EnemyController enemy = GetRandomEnemy();
        Instantiate(enemy.portalPrefab, rndPos, Quaternion.identity);
        EnemyController newEnemy = Instantiate(enemy, rndPos, Quaternion.identity);
        newEnemy.TakePlayer(player);
        actualInstantiateTimer = instantiateTime;
        numEnemys++;
        totalEnemys = numEnemys + activeEnemys;
    }

    public void ChangeMasterVolumeMusic(float _volume = 1f) {
        AudioManager.instance.SetMasterVolume(_volume, AudioManagement.AudioType.MUSIC);
    }

    public void ChangeMasterVolumeSFX(float _volume = 1f) {
        AudioManager.instance.SetMasterVolume(_volume, AudioManagement.AudioType.SFX);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int rnd = UnityEngine.Random.Range(0, enemySpawnPoints.Count);
        return enemySpawnPoints[rnd].position;
    }

    private EnemyController GetRandomEnemy()
    {
        int rnd = UnityEngine.Random.Range(0, enemyPrefabs.Count);
        return enemyPrefabs[rnd];
    }

    IEnumerator OpenGameOverScreen()
    {
        yield return new WaitForSeconds(openGameOverTime);
        gameCanvas.SetActive(false);
        menuCanvas.SetActive(true);
        hud.OpenGameOver();
        // menuButtons.SetActive(false);
        // pauseButtons.SetActive(false);
        // gameOverCanvas.SetActive(true);
    }
}
