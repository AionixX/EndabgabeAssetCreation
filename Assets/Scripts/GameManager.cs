using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] PlayerController player = null;
    [SerializeField] List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] List<GameObject> enemyPrefabs = new List<GameObject>();
    [SerializeField] int startEnemys = 5;
    [SerializeField] float instantiateStartTime = 3f;
    [SerializeField] float instantiateTimeLoss = 0.05f;
    [SerializeField] float instantiateTimeMin = 0.2f;

    private List<GameObject> enemys = new List<GameObject>();
    bool gamePaused = false;
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
        if (Input.GetMouseButton(0) && gamePaused)
            gamePaused = false;
        
        if(Input.GetKeyDown(KeyCode.Escape))
            gamePaused = true;
            
        if (!gamePaused)
            LockCursor();

        actualInstantiateTimer -= Time.deltaTime;
        if(actualInstantiateTimer <= 0f)
            SpawnEnemy();
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void StartGame()
    {
        instantiateTime = instantiateStartTime;
        player.StartGame();

        for (int i = 0; i < startEnemys; i++)
        {
            SpawnEnemy();
        }
    }
    public void EnemyDied() {
        // SpawnEnemy();
        instantiateTime = Mathf.Clamp(instantiateTime - instantiateTimeLoss, instantiateTimeMin, float.MaxValue);
    }

    private void SpawnEnemy()
    {
        EnemyController newEnemy = Instantiate(GetRandomEnemy(), GetRandomSpawnPosition(), Quaternion.identity).GetComponent<EnemyController>();
        newEnemy.TakePlayer(player);
        actualInstantiateTimer = instantiateTime;
    }

    private Vector3 GetRandomSpawnPosition()
    {
        int rnd = Random.Range(0, enemySpawnPoints.Count);
        return enemySpawnPoints[rnd].position;
    }

    private GameObject GetRandomEnemy() {
        int rnd = Random.Range(0, enemyPrefabs.Count);
        return enemyPrefabs[rnd];
    }
}
