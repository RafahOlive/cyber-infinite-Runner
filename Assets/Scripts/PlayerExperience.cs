using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour
{
    public int currentXP = 0;
    public int maxXP = 100;
    public int level = 1;
    public Image xpBar;
    public TextMeshProUGUI levelText;
    public GameObject levelUpPanel;
    public void AddXP(int amount)
    {
        currentXP += amount;
        currentXP = Mathf.Clamp(currentXP, 0, maxXP);
        UpdateXPBar();
        if (currentXP >= maxXP)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        level++;
        maxXP = level * 100;
        currentXP = 0;
        levelText.text = level.ToString();
        LevelUpPanel();
        Debug.Log("Level Up! Current level: " + level);
    }

    void LevelUpPanel()
    {
        levelUpPanel.SetActive(true);
        Time.timeScale = 0;
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
}
