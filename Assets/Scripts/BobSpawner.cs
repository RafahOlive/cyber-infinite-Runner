using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    float timeBtwnSpawn;
    public float startTimeBtwnSpawn;
    public Transform spawnPoint;
    public PlayerExperience playerExp;

    public void SpawnBob()
    {
        if (timeBtwnSpawn <= 0)
        {
            GameObject bobPrefab = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            PlayerExperience playerExp = bobPrefab.GetComponent<PlayerExperience>();
            if(playerExp == null)
            {
                playerExp = bobPrefab.AddComponent<PlayerExperience>();
            }
            timeBtwnSpawn = startTimeBtwnSpawn;
        }
        else
        {
            timeBtwnSpawn -= Time.deltaTime;
        }
    }
}
