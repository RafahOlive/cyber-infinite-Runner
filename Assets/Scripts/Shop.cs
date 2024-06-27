using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] GameManagerCine gameManager;
    [SerializeField] PlayerController player;
    [SerializeField] Button buyAeroladOnceButton;
    void Start()
    {
        Debug.Log(GameManagerCine.money);
    }
    public void BuyAerolad()
    {
        // int currentMoney = GameManagerCine.money;
        if (GameManagerCine.money >= 50)
        {
            if (player.aeroLad.activeSelf)
            {
                buyAeroladOnceButton.interactable = false;
            }
            else
            {
                player.aeroLad.SetActive(true);
                buyAeroladOnceButton.interactable = false;
                GameManagerCine.money -= 50;
                gameManager.shopMoneyText.text = GameManagerCine.money.ToString();
                GameManagerCine.SaveMoney();
                gameManager.SaveAeroLadState();
            }
        }
    }
}
