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

    private List<GameObject> enemys = new List<GameObject>();
    bool gamePaused = false;

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
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    public void StartGame()
    {
        player.StartGame();

        for (int i = 0; i < startEnemys; i++)
        {
            SpawnEnemy();
        }
    }
    public void EnemyDied() {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        EnemyController newEnemy = Instantiate(GetRandomEnemy(), GetRandomSpawnPosition(), Quaternion.identity).GetComponent<EnemyController>();
        newEnemy.TakePlayer(player);
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
