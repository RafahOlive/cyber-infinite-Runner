using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    float timeBtwnSpawn;
    public float startTimeBtwnSpawn;
    public Transform spawnPoint;

    public void SpawnBob()
    {
        if (timeBtwnSpawn <= 0)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            timeBtwnSpawn = startTimeBtwnSpawn;
        }
        else
        {
            timeBtwnSpawn -= Time.deltaTime;
        }
    }
}
