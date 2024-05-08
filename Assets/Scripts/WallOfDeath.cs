using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfDeath : MonoBehaviour
{
    GameManagerCine gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManagerCine").GetComponent<GameManagerCine>();
    }
    void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            gameManager.Die();
        }
    }
}
