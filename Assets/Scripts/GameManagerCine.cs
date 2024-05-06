using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerCine : MonoBehaviour
{
    public PlayerController playerController;
    public Animator beginPanel;
    public Animator gameUI;
    [SerializeField] GameObject beginPanelMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject shopMenu;
    public ParalaxTimer bgPrlx2;
    public ParalaxTimer bgPrlx22;
    public ParalaxTimer bgPrlx3;
    public ParalaxTimer bgPrlx33;
    public ParalaxTimer bgPrlx5;
    public ParalaxTimer bgPrlx55;
    float actualParalax1x2;
    float actualParalax1x22;
    float actualParalax1x3;
    float actualParalax1x33;
    float actualParalax1x5;
    float actualParalax1x55;
    public int money;
    public int blueCard;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI blueCardText;
    // void Update()
    // {
    //     if (playerController.isHit)
    //     {
    //         actualParalax1x2 = bgPrlx2.GetComponent<ParalaxTimer>().speed;
    //         actualParalax1x22 = bgPrlx22.GetComponent<ParalaxTimer>().speed;

    //         actualParalax1x3 = bgPrlx3.GetComponent<ParalaxTimer>().speed;
    //         actualParalax1x33 = bgPrlx33.GetComponent<ParalaxTimer>().speed;

    //         actualParalax1x5 = bgPrlx5.GetComponent<ParalaxTimer>().speed;
    //         actualParalax1x55 = bgPrlx55.GetComponent<ParalaxTimer>().speed;

    //         bgPrlx2.GetComponent<ParalaxTimer>().speed = 0f;
    //         bgPrlx22.GetComponent<ParalaxTimer>().speed = 0f;

    //         bgPrlx3.GetComponent<ParalaxTimer>().speed = 0f;
    //         bgPrlx33.GetComponent<ParalaxTimer>().speed = 0f;

    //         bgPrlx5.GetComponent<ParalaxTimer>().speed = 0f;
    //         bgPrlx55.GetComponent<ParalaxTimer>().speed = 0f;
    //     }
    //     // else if (!playerController.isHit)
    //     else
    //     {
    //         bgPrlx22.GetComponent<ParalaxTimer>().speed = actualParalax1x2;
    //         bgPrlx22.GetComponent<ParalaxTimer>().speed = actualParalax1x22;

    //         bgPrlx3.GetComponent<ParalaxTimer>().speed = actualParalax1x3;
    //         bgPrlx33.GetComponent<ParalaxTimer>().speed = actualParalax1x33;

    //         bgPrlx5.GetComponent<ParalaxTimer>().speed = actualParalax1x5;
    //         bgPrlx55.GetComponent<ParalaxTimer>().speed = actualParalax1x55;
    //     }
    // }
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

        bgPrlx2.GetComponent<ParalaxTimer>().speed = 0.2f;
        bgPrlx22.GetComponent<ParalaxTimer>().speed = 0.2f;

        bgPrlx3.GetComponent<ParalaxTimer>().speed = 0.4f;
        bgPrlx33.GetComponent<ParalaxTimer>().speed = 0.4f;

        bgPrlx5.GetComponent<ParalaxTimer>().speed = 0.6f;
        bgPrlx55.GetComponent<ParalaxTimer>().speed = 0.6f;
    }

    public void BeginScene()
    {
        SceneManager.LoadScene("Game Camera Moving");
        Time.timeScale = 1f;
    }
    public void OpenShop()
    {
        shopMenu.SetActive(true);
        beginPanelMenu.SetActive(false);
    }
    public void CloseShop()
    {
        shopMenu.SetActive(false);
        beginPanelMenu.SetActive(true);
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
