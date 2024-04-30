using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    GameManagerCine gameManager;
    Animator wallsAnim;
    public GameObject spawnLevelOne;
    public GameObject spawnLevelTwo;
    private Vector2 spawnPosition;
    [SerializeField] private float spawnDistance = 46.08f;
    [SerializeField] private float lastSpawnPosition = 0;
    [SerializeField] private bool hasSpawned = false;
    void Start()
    {
        gameManager = GameObject.Find("GameManagerCam").GetComponent<GameManagerCine>();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !hasSpawned)
        {
            lastSpawnPosition += spawnDistance;
            spawnPosition = new Vector2(lastSpawnPosition, 0f);
            if (gameManager.blueCard != 3)
            {
                GameObject stageLoop = Instantiate(spawnLevelOne, spawnPosition, Quaternion.identity);
                wallsAnim = GameObject.Find("WallsAnim").GetComponent<Animator>();
                stageLoop.name = "Stage one";
                hasSpawned = true;
            } else if (gameManager.blueCard == 3)
            {
                GameObject stageLoop = Instantiate(spawnLevelTwo, spawnPosition, Quaternion.identity);
                stageLoop.name = "Stage one";
                hasSpawned = true;
            }
        }

        GameObject objectToDelete = GameObject.Find("Stage one");
        if (objectToDelete != null)
        {
            Destroy(objectToDelete, 20f);
        }
        else
        {
            Debug.Log("GameObject not found.");
        }
    }
}
