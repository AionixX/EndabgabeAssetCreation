using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    [SerializeField] PlayerController player = null;
    [SerializeField] List<Transform> enemySpawnPoints = new List<Transform>();
    [SerializeField] GameObject enemyPrefab = null;
    [SerializeField] int startEnemys = 5;

    private List<GameObject> enemys = new List<GameObject>();

    void Start()
    {
        if(GameManager.instance) {
            Debug.LogError("Already found an instance of GameManager!");
            return;
        }
        GameManager.instance = this;
    }

    public void StartGame() {
        for(int i = 0; i < startEnemys; i++) {
            SpawnEnemy();
        }
    }

    private void SpawnEnemy() {
        GameObject newEnemy = Instantiate(enemyPrefab, GetRandomSpawnPosition(), Quaternion.identity);
    }

    private Vector3 GetRandomSpawnPosition() {
        int rnd = Random.Range(0, enemySpawnPoints.Count);
        return enemySpawnPoints[rnd].position;
    }
}
