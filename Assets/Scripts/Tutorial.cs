using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    Animator fingerAnimator;
    GameManagerCine gameManager;
    void Start()
    {
        gameManager = GameObject.Find("GameManagerCine").GetComponent<GameManagerCine>();
        fingerAnimator = GameObject.Find("Point finger").GetComponent<Animator>();
    }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("JumpTutorial") && gameManager.isOnJumpTutorial)
        {
            Time.timeScale = 0;
            fingerAnimator.Play("JumpTutorial");
            gameManager.isOnJumpTutorial = false;
            gameManager.SaveTutorialState();
            Debug.Log(gameManager.isOnJumpTutorial);
        }
        if (other.CompareTag("SlideTutorial") && gameManager.isOnSlideTutorial)
        {
            Time.timeScale = 0;
            fingerAnimator.Play("SlideTutorial");
            gameManager.isOnSlideTutorial = false;
            gameManager.SaveTutorialState();
            Debug.Log(gameManager.isOnSlideTutorial);
        }
        if (other.CompareTag("AttackTutorial") && gameManager.isOnAttackTutorial)
        {
            Time.timeScale = 0;
            fingerAnimator.Play("AttackTutorial");
            gameManager.isOnAttackTutorial = false;
            gameManager.SaveTutorialState();
            Debug.Log(gameManager.isOnAttackTutorial);
        }
    }
}
