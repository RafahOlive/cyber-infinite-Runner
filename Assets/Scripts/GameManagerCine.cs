using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerCine : MonoBehaviour
{
    bool gameIsStarted = false;
    public PlayerController playerController;
    public Animator beginPanel;
    public Animator gameUI;

    void Start()
    {
         if (!playerController.GetComponent<PlayerController>().gameOver)
        {
            if (gameIsStarted)
            {
                
            }
        }
    }

    void Update()
    {
        
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

        playerController.speed = 1;

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
