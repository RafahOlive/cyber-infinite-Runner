using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public PlayerController playerController;
    GameManagerCine gameManager;
    public int currentXP = 0;
    public int maxXP = 100;
    public int level = 1;
    public Image xpBar;
    public TextMeshProUGUI levelText;
    public GameObject levelUpPanel;
    [SerializeField] GameObject[] skills;
    List<GameObject> selectedSkills = new();
    AudioManager audioManager;
    [SerializeField] AudioClip levelUpSfx;
    [SerializeField] AudioClip riseAeroladSfx;
    void Start()
    {
        gameManager = GameObject.Find("GameManagerCine").GetComponent<GameManagerCine>();
        audioManager = GetComponent<AudioManager>();
    }
    public void AddXP(int amount)
    {
        currentXP += amount;
        currentXP = Mathf.Clamp(currentXP, 0, maxXP);
        UpdateXPBar();
        if (currentXP >= maxXP)
        {
            xpBar.fillAmount = 0;
            LevelUp();
        }
    }
    public void LevelUp()
    {
        audioManager.PlayAudio(levelUpSfx);
        level++;
        maxXP = level * 100;
        currentXP = 0;
        levelText.text = level.ToString();
        LevelUpPanel();
    }
    void LevelUpPanel()
    {
        levelUpPanel.SetActive(true);
        gameManager.gameIsPaused = true;
        Time.timeScale = 0;
        SelectRandomSkills(3);
        ActivateSelectedSkills();
    }

    void UpdateXPBar()
    {
        if (xpBar != null)
        {
            Debug.Log("A barra de XP foi referenciada corretamente.");
            float progress = (float)currentXP / maxXP;
            xpBar.fillAmount = progress;
        }
        else
        {
            Debug.LogWarning("Referência à barra de XP não está configurada em PlayerExperience.");
        }
    }

    void SelectRandomSkills(int count)
    {
        List<GameObject> availableSkills = new(skills);

        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, availableSkills.Count);
            selectedSkills.Add(availableSkills[randomIndex]);
            availableSkills.RemoveAt(randomIndex);
        }
    }
    void ActivateSelectedSkills()
    {
        for (int i = 0; i < selectedSkills.Count; i++)
        {
            selectedSkills[i].SetActive(true);
        }
        selectedSkills[0].GetComponent<RectTransform>().anchoredPosition = new Vector3(349.7976f, 0f, 0f);
        selectedSkills[1].GetComponent<RectTransform>().anchoredPosition = new Vector3(0f, 0f, 0f);
        selectedSkills[2].GetComponent<RectTransform>().anchoredPosition = new Vector3(-349.7976f, 0f, 0f);
    }

    void DisableAllSkills()
    {
        foreach (GameObject skill in selectedSkills)
        {
            skill.SetActive(false);
        }
        selectedSkills.Clear();
    }

    public void SkillRecovery()
    {
        playerController.currentHealth += 1;
        playerController.UpdateHealthUI();
        Time.timeScale = 1f;
        gameManager.gameIsPaused = false;
        DisableAllSkills();
        levelUpPanel.SetActive(false);
    }
    public void SkillAeroLad()
    {
        audioManager.PlayAudio(riseAeroladSfx);
        playerController.aeroLad.SetActive(true);
        Time.timeScale = 1f;
        gameManager.gameIsPaused = false;
        DisableAllSkills();
        levelUpPanel.SetActive(false);
        playerController.ActivateInvincibility(1f);
    }

    public void SkillSequencialPunch()
    {
        playerController.GetComponent<Animator>().SetBool("attackDouble", true);
        Time.timeScale = 1f;
        gameManager.gameIsPaused = false;
        DisableAllSkills();
        levelUpPanel.SetActive(false);
    }
    public void SkillOnePunch()
    {
        playerController.GetComponent<Animator>().SetBool("attackDouble", false);
        Time.timeScale = 1f;
        gameManager.gameIsPaused = false;
        DisableAllSkills();
        levelUpPanel.SetActive(false);
    }
    public void SkillNone()
    {
        Time.timeScale = 1f;
        gameManager.gameIsPaused = false;
        DisableAllSkills();
        levelUpPanel.SetActive(false);
    }
}
