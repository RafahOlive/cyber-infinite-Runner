using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerCine : MonoBehaviour
{
    bool gameIsStarted = false;
    public PlayerController playerController;
    public Animator beginPanel;
    public Animator gameUI;
    [SerializeField] GameObject pauseMenu;
    public int money;
    public int blueCard;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI blueCardText;
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

        playerController.speed = 1f;

        gameIsStarted = true;
    }

    public void BeginScene()
    {
        SceneManager.LoadScene("Game Camera Moving");
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

}
