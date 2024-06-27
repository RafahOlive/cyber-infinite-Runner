using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Rendering.Universal;
using System;

public class GameManagerCine : MonoBehaviour
{
    public bool gameIsStarted = false;
    public bool gameIsPaused = false;
    public bool isOnJumpTutorial = true;
    public bool isOnSlideTutorial = true;
    public bool isOnAttackTutorial = true;
    PlayerController playerController;
    public Animator beginPanel;
    public GameObject gameUI;
    Animator losePanelAnim;
    Animator pauseMenuAnim;
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
    public static int money;
    public int stageMoney;
    public int blueCard;
    public TextMeshProUGUI shopMoneyText;
    public TextMeshProUGUI stageMoneyText;
    public TextMeshProUGUI stageMoneyCollectedText;
    public TextMeshProUGUI totalMoneyCollectedText;
    public TextMeshProUGUI doubleMoneyText;
    public TextMeshProUGUI blueCardText;
    public TextMeshProUGUI distanceTraveledText;
    private Light2D globalLight;
    AdManager adManager;
    void Awake()
    {
        // ClearAllPlayerPrefs();
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
        // LoadTutorialState();

        GameObject globalLightObj = GameObject.Find("Global Light 2D");
        globalLight = globalLightObj.GetComponent<Light2D>();

        adManager = GameObject.Find("Ads").GetComponent<AdManager>();
        losePanelAnim = losePanel.GetComponent<Animator>();
        pauseMenuAnim = pauseMenu.GetComponent<Animator>();
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
        beginPanel.GetComponent<Animator>().SetBool("playGame", true);
        yield return new WaitForSeconds(1f);

        gameUI.SetActive(true);
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
    public void ChangeGlobalLight(float intensity, float duration)
    {
        GameObject globalLightObj = GameObject.Find("Global Light 2D");
        globalLight = globalLightObj.GetComponent<Light2D>();
        StartCoroutine(ChangeLightIntensity(intensity, duration));
    }
    private IEnumerator ChangeLightIntensity(float targetIntensity, float duration)
    {
        float startIntensity = globalLight.intensity;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            globalLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, elapsedTime / duration);
            yield return null; // Espera até o próximo frame
        }

        // Garante que a intensidade final seja exatamente a desejada
        globalLight.intensity = targetIntensity;
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
        pauseMenuAnim.Play("Pause");
        gameIsPaused = true;
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        pauseMenuAnim.Play("Idle");
        gameIsPaused = false;
        gameIsStarted = true;
        Time.timeScale = 1f;
    }
    public void Restart()
    {
        SceneManager.LoadScene("Game Camera Moving");
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
        int doubleGains = stageMoney * 2 + money;
        money = totalMoney;
        stageMoneyCollectedText.text = "Money collected: " + stageMoney.ToString();
        totalMoneyCollectedText.text = "Total money : " + money.ToString();
        doubleMoneyText.text = doubleGains.ToString();
        SaveMoney();
        LoadAeroLadState();
        losePanelAnim.Play("Lose");
        gameIsStarted = false;
        Time.timeScale = 0f;
        
    }

    //ADS AND REWARDEDS STUFFS
    public void RewardedAdDoubleMoney()
    {
        adManager.ShowRewardedAd();
    }
    public void StuffsToDoAfterRewardPlayer()
    {
        int totalMoney = stageMoney + money;
        money = totalMoney;
        SaveMoney();
        LoadAeroLadState();
        losePanelAnim.Play("Idle");
        // Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameIsStarted = true;
        Time.timeScale = 1f;
    }

    //SAVE AND LOAD STUFFS
    private static void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs have been cleared");
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
    // public void SaveTutorialState()
    // {
    //     PlayerPrefs.SetInt("IsOnJumpTutorial", isOnJumpTutorial ? 1 : 0);
    //     PlayerPrefs.SetInt("IsOnSlideTutorial", isOnSlideTutorial ? 1 : 0);
    //     PlayerPrefs.SetInt("IsOnAttackTutorial", isOnAttackTutorial ? 1 : 0);

    //     PlayerPrefs.Save();
    // }
    // public void LoadTutorialState()
    // {
    //     isOnJumpTutorial = PlayerPrefs.GetInt("IsOnJumpTutorial", 1) == 1;
    //     isOnSlideTutorial = PlayerPrefs.GetInt("IsOnSlideTutorial", 1) == 1;
    //     isOnAttackTutorial = PlayerPrefs.GetInt("IsOnAttackTutorial", 1) == 1;
    // }
}
