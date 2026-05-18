using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int enemyCount = 5;

    private List<GameObject> spawnedEnemies = new List<GameObject>();

    public void SpawnEnemies()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];

            GameObject enemy = Instantiate(enemyPrefab, point.position, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }
    }

    public void RemoveEnemies()
    {
        foreach (GameObject enemy in spawnedEnemies)
        {
            if (enemy != null)
                Destroy(enemy);
        }

        spawnedEnemies.Clear();
    }
}
