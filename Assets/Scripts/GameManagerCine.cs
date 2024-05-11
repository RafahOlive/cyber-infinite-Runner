using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManagerCine : MonoBehaviour
{
    public bool gameIsStarted = false;
    public bool gameIsPaused = false;
    PlayerController playerController;
    public Animator beginPanel;
    public Animator gameUI;
    [SerializeField] GameObject losePanel;
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
    public static int money;
    public int stageMoney;
    public int blueCard;
    public TextMeshProUGUI shopMoneyText;
    public TextMeshProUGUI stageMoneyText;
    public TextMeshProUGUI stageMoneyCollectedText;
    public TextMeshProUGUI totalMoneyCollectedText;
    public TextMeshProUGUI blueCardText;
    public TextMeshProUGUI distanceTraveledText;
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
    void Awake()
    {
        if (!GameObject.Find("Player").TryGetComponent<PlayerController>(out playerController))
        {
            Debug.LogError("PlayerController not found!");
        }
        else
        {
            Debug.Log("PlayerController found!");
        }
        LoadMoney();
        LoadAeroLadState();
    }
    void Start()
    {
        shopMoneyText.text = money.ToString();
    }
    void Update()
    {
        distanceTraveledText.text = "Distance: " + playerController.distanceTraveled.ToString("F1");
    }
    public void StartGame()
    {
        StartCoroutine(StartGameLevel1());
    }

    public IEnumerator StartGameLevel1()
    {
        gameIsStarted = true;
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
        SaveMoney();
        LoadAeroLadState();
        SceneManager.LoadScene("Game Camera Moving");
        Time.timeScale = 1f;
    }
    public void OpenShop()
    {
        shopMoneyText.text = money.ToString();
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
        PlayerPrefs.SetInt("Money", money);
        Application.Quit();
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        gameIsPaused = true;
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        gameIsPaused = false;
        gameIsStarted = true;
        Time.timeScale = 1f;
    }
    public void Die()
    {
        playerController.gameOver = true;
        playerController.GetComponent<Animator>().SetTrigger("die");
    }
    public void Lose()
    {
        int totalMoney = stageMoney + money;
        money = totalMoney;
        stageMoneyCollectedText.text = "Money collected: " + stageMoney.ToString();
        totalMoneyCollectedText.text = "Total money : " + money.ToString();
        losePanel.SetActive(true);
        gameIsStarted = false;
        Time.timeScale = 0f;
    }
    public static void SaveMoney()
    {
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }
    public static void LoadMoney()
    {
        money = PlayerPrefs.GetInt("Money", 0); // O segundo argumento é um valor padrão caso "Money" não exista
    }
    public void SaveAeroLadState()
    {
        int aeroladState = playerController.aeroLad.activeSelf ? 1 : 0;
        PlayerPrefs.SetInt("AeroLadState", aeroladState);
        PlayerPrefs.Save();
    }
    public void LoadAeroLadState()
    {
        int aeroladState = PlayerPrefs.GetInt("AeroLadState", 0);
        bool isActive = aeroladState == 1 ? true : false;
        playerController.aeroLad.SetActive(isActive);
    }
}
