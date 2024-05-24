using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public PlayerController playerController;
    GameManagerCine gameManager;
    Animator wallsAnim;
    private bool spawnLevelOneNext = false;
    public GameObject spawnLevelOne;
    public GameObject spawnLevelTwo;
    private Vector2 spawnPosition;
    [SerializeField] private float spawnDistance = 103.68f;
    [SerializeField] private float lastSpawnPosition = 0;
    [SerializeField] private bool hasSpawned = false;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentLevelToDestroy;
    GameObject stageLoop;
    void Start()
    {
        gameManager = GameObject.Find("GameManagerCine").GetComponent<GameManagerCine>();
    }

    void SetHasSpawnedToTrue()
    {
        hasSpawned = false;
    }
    void MoveMeToNextStage()
    {
        Vector2 currentPosition = transform.position;
        currentPosition.x += spawnDistance;
        transform.position = currentPosition;
    }
    void IncreasePlayerSpeed()
    {
        playerController.speed += 0.2f;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !hasSpawned)
        {
            lastSpawnPosition += spawnDistance;
            spawnPosition = new Vector2(lastSpawnPosition, 0f);
            if (gameManager.blueCard != 3)
            {
                if (spawnLevelOneNext)
                {
                    stageLoop = Instantiate(spawnLevelOne, spawnPosition, Quaternion.identity);
                    gameManager.ChangeGlobalLight(1f, 10f);
                }
                else
                {
                    stageLoop = Instantiate(spawnLevelTwo, spawnPosition, Quaternion.identity);
                    gameManager.ChangeGlobalLight(0.5f, 10f);
                }
                spawnLevelOneNext = !spawnLevelOneNext;

                wallsAnim = GameObject.Find("WallsAnim").GetComponent<Animator>();
                currentLevel++;
                stageLoop.name = "Stage" + currentLevel.ToString();
                hasSpawned = true;
                Invoke(nameof(SetHasSpawnedToTrue), 4);
                Invoke(nameof(MoveMeToNextStage), 4);
                Invoke(nameof(IncreasePlayerSpeed), 5);
            }
            // else if (gameManager.blueCard == 3)
            // {
            //     stageLoop = Instantiate(spawnLevelTwo, spawnPosition, Quaternion.identity);
            //     currentLevel++;
            //     stageLoop.name = "Stage" + currentLevel.ToString();
            //     gameManager.blueCard = 0;
            //     hasSpawned = true;
            //     Invoke(nameof(SetHasSpawnedToTrue), 4);
            //     Invoke(nameof(MoveMeToNextStage), 4);
            //     Invoke(nameof(IncreasePlayerSpeed), 5);
            // }
        }

        currentLevelToDestroy = currentLevel - 1;

        GameObject objectToDelete = GameObject.Find("Stage" + currentLevelToDestroy.ToString());
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
