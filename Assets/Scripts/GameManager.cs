using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    bool gameIsStarted = false;
    public ParalaxTimer bgPrlx2;
    public ParalaxTimer bgPrlx22;
    public ParalaxTimer bgPrlx3;
    public ParalaxTimer bgPrlx33;
    public ParalaxTimer bgPrlx4;
    public ParalaxTimer bgPrlx44;
    public ParalaxTimer bgPrlx5;
    public ParalaxTimer bgPrlx55;
    public ParalaxTimer groundPrlx1;
    public ParalaxTimer groundPrlx11;
    public PlayerController playerController;
    public Animator beginPanel;
    public Animator gameUI;
    public BobSpawner spawnBob;
    void Start()
    {
        // bgPrlx2 = GetComponent<ParalaxTimer>();
    }
    void Update()
    {
        if (!playerController.GetComponent<PlayerController>().gameOver)
        {
            if (gameIsStarted)
            {
                spawnBob.GetComponent<BobSpawner>().SpawnBob();
            }
        }
        else if (playerController.GetComponent<PlayerController>().gameOver)
        {
            bgPrlx2.GetComponent<ParalaxTimer>().speed = 0f;
            bgPrlx22.GetComponent<ParalaxTimer>().speed = 0f;

            bgPrlx3.GetComponent<ParalaxTimer>().speed = 0f;
            bgPrlx33.GetComponent<ParalaxTimer>().speed = 0f;

            bgPrlx4.GetComponent<ParalaxTimer>().speed = 0f;
            bgPrlx44.GetComponent<ParalaxTimer>().speed = 0f;

            bgPrlx5.GetComponent<ParalaxTimer>().speed = 0f;
            bgPrlx55.GetComponent<ParalaxTimer>().speed = 0f;

            groundPrlx1.GetComponent<ParalaxTimer>().speed = 0f;
            groundPrlx11.GetComponent<ParalaxTimer>().speed = 0f;
        }

    }
    public void StartGame()
    {
        StartCoroutine(StartGameLevel1());
    }
    public IEnumerator StartGameLevel1()
    {
        beginPanel.GetComponent<Animator>().SetBool("playGame", true);
        yield return new WaitForSeconds(1f);

        gameUI.GetComponent<Animator>().SetBool("playGame", true);
        yield return new WaitForSeconds(1f);

        playerController.GetComponent<Animator>().SetBool("playGame", true);
        yield return new WaitForSeconds(1.5f);
        bgPrlx2.GetComponent<ParalaxTimer>().speed = 0.2f;
        bgPrlx22.GetComponent<ParalaxTimer>().speed = 0.2f;

        bgPrlx3.GetComponent<ParalaxTimer>().speed = 0.4f;
        bgPrlx33.GetComponent<ParalaxTimer>().speed = 0.4f;

        bgPrlx4.GetComponent<ParalaxTimer>().speed = 0.6f;
        bgPrlx44.GetComponent<ParalaxTimer>().speed = 0.6f;

        bgPrlx5.GetComponent<ParalaxTimer>().speed = 0.8f;
        bgPrlx55.GetComponent<ParalaxTimer>().speed = 0.8f;

        groundPrlx1.GetComponent<ParalaxTimer>().speed = 0.8f;
        groundPrlx11.GetComponent<ParalaxTimer>().speed = 0.8f;

        gameIsStarted = true;
    }

    public void BeginScene()
    {
        SceneManager.LoadScene("Game");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
