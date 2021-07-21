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

    public bool gamePaused = false;
    public bool gameStarted = false;
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
        gamePaused = false;

        baseCameraSpeedX = playerCam.m_XAxis.m_MaxSpeed;
        baseCameraSpeedY = playerCam.m_YAxis.m_MaxSpeed;

        ChangeMasterVolumeMusic();
        ChangeMasterVolumeSFX();
        UpdateCameraSpeed();

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

    public void UpdateCameraSpeed() {
        playerCam.m_XAxis.m_MaxSpeed = mouseSlider.value * baseCameraSpeedX;
        playerCam.m_YAxis.m_MaxSpeed = mouseSlider.value * baseCameraSpeedY;
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
        pauseButtons.SetActive(true);
        gameCanvas.SetActive(false);
        gamePaused = true;
        // Time.timeScale = 0f;
        UnlockCursor();
    }

    public void ResumeGame()
    {
        // pauseCanvas.SetActive(false);
        menuCanvas.SetActive(false);
        pauseButtons.SetActive(false);
        gameCanvas.SetActive(true);
        gamePaused = false;
        // Time.timeScale = 1f;
        LockCursor();
    }

    public void EndGame()
    {
        gamePaused = true;
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
    }

    private void SpawnEnemy()
    {
        Vector3 rndPos = GetRandomSpawnPosition();
        EnemyController enemy = GetRandomEnemy();
        Instantiate(enemy.portalPrefab, rndPos, Quaternion.identity);
        EnemyController newEnemy = Instantiate(enemy, rndPos, Quaternion.identity);
        newEnemy.TakePlayer(player);
        actualInstantiateTimer = instantiateTime;
    }

    public void ChangeMasterVolumeMusic() {
        AudioManager.instance.SetMasterVolume(volumeSliderMusic.value, AudioManagement.AudioType.MUSIC);
    }

    public void ChangeMasterVolumeSFX() {
        AudioManager.instance.SetMasterVolume(volumeSliderSFX.value, AudioManagement.AudioType.SFX);
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
        menuCanvas.SetActive(false);
        menuButtons.SetActive(false);
        pauseButtons.SetActive(false);
        gameOverCanvas.SetActive(true);
    }
}
