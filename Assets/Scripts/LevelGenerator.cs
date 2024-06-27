using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public PlayerController playerController;
    GameManagerCine gameManager;
    private bool spawnLevelOneNext = false;
    public GameObject[] dayStages;
    public GameObject[] nightStages;
    public GameObject spawnBossLevel;
    private Vector2 spawnPosition;
    [SerializeField] private float spawnDistance = 103.68f;
    [SerializeField] private float lastSpawnPosition = 0;
    [SerializeField] private bool hasSpawned = false;
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentLevelToDestroy;
    [SerializeField] private GameObject bossBoardWalkerHealthUI;
    GameObject stageLoop;
    public bool boardWalkerIsOn = false;
    Transform playerTransform;
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
        playerController.speed += 0.1f;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") && !hasSpawned)
        {
            lastSpawnPosition += spawnDistance;
            spawnPosition = new Vector2(lastSpawnPosition, 0f);
            GameObject[] selectedStages = spawnLevelOneNext ? dayStages : nightStages; // Seleciona o grupo de telas
            int randomIndex = Random.Range(0, selectedStages.Length); // Escolhe uma tela aleatoriamente
            
            if (gameManager.blueCard != 3)
            {
                if (spawnLevelOneNext)
                {
                    stageLoop = Instantiate(selectedStages[randomIndex], spawnPosition, Quaternion.identity);
                    gameManager.ChangeGlobalLight(0.3f, 10f);
                }
                else
                {
                    stageLoop = Instantiate(selectedStages[randomIndex], spawnPosition, Quaternion.identity);
                    gameManager.ChangeGlobalLight(0.5f, 10f);
                }
                spawnLevelOneNext = !spawnLevelOneNext;

                currentLevel++;
                stageLoop.name = "Stage" + currentLevel.ToString();
                hasSpawned = true;
                Invoke(nameof(SetHasSpawnedToTrue), 4);
                Invoke(nameof(MoveMeToNextStage), 4);
                Invoke(nameof(IncreasePlayerSpeed), 5);
            }
            else if (gameManager.blueCard == 3)
            {
                stageLoop = Instantiate(spawnBossLevel, spawnPosition, Quaternion.identity);
                currentLevel++;
                stageLoop.name = "Stage" + currentLevel.ToString();
                // gameManager.blueCard = 0;
                hasSpawned = true;
                Invoke(nameof(SetHasSpawnedToTrue), 4);
                Invoke(nameof(MoveMeToNextStage), 4);
                Invoke(nameof(IncreasePlayerSpeed), 5);

                GameObject blockedPassage = GameObject.Find("PassageBlock");
                Destroy(blockedPassage);

                StartCoroutine(ShowBossBoardWalker());
            }
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

    IEnumerator ShowBossBoardWalker()
    {
        if (boardWalkerIsOn)
        {
            yield break;
        }
        else
        {
            GameObject boss = GameObject.Find("BoardWalker");

            yield return new WaitForSeconds(10f);
            playerTransform = playerController.transform;
            boss.transform.position = new Vector3(playerTransform.position.x + 3f, playerTransform.position.y + 3.5f, boss.transform.position.z);
            // boss.transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, boss.transform.position.z);
            boardWalkerIsOn = true;

            if (boss.TryGetComponent<Animator>(out var bossAnimator))
            {
                bossAnimator.SetBool("on", true);
                bossBoardWalkerHealthUI.SetActive(true);
            }
            else
            {
                Debug.LogError("Animator component not found on BoardWalker!");
            }

            yield return new WaitForSeconds(2f);
            bossAnimator.Play("Attack2");
        }
    }
}
