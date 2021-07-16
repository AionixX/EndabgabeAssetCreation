using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] public PlayerController player = null;
    [SerializeField] GameObject gameCanvas = null;
    [SerializeField] GameObject pauseCanvas = null;
    [SerializeField] GameObject gameOverCanvas = null;
    [SerializeField] GameObject startGameHintText = null;
    [SerializeField] float openGameOverTime = 2f;
    [SerializeField] TMP_Text gameOverScoreText = null;
    [SerializeField] List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] List<EnemyController> enemyPrefabs = new List<EnemyController>();
    [SerializeField] int startEnemys = 5;
    [SerializeField] float instantiateStartTime = 3f;
    [SerializeField] float instantiateTimeLoss = 0.05f;
    [SerializeField] float instantiateTimeMin = 0.2f;

    public bool gamePaused = true;
    private List<GameObject> enemys = new List<GameObject>();
    float instantiateTime = 2f;
    float actualInstantiateTimer = 0f;

    void Start()
    {
        if (GameManager.instance)
        {
            Debug.LogError("Already found an instance of GameManager!");
            return;
        }
        GameManager.instance = this;

        StartGame();
    }

    void Update()
    {
        if (gamePaused) return;

        if (Input.GetKeyDown(KeyCode.Escape))
            PauseGame();

        actualInstantiateTimer -= Time.deltaTime;
        if (actualInstantiateTimer <= 0f)
            SpawnEnemy();
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

    public void StartGame()
    {
        LockCursor();

        gameOverCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        startGameHintText.SetActive(false);

        gamePaused = false;
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
        pauseCanvas.SetActive(true);
        gamePaused = true;
        Time.timeScale = 0f;
        UnlockCursor();
    }

    public void ResumeGame()
    {
        pauseCanvas.SetActive(false);
        gamePaused = false;
        Time.timeScale = 1f;
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
        gameOverCanvas.SetActive(true);
    }
}
