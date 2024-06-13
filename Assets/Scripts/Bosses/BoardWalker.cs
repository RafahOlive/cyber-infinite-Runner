using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BoardWalker : MonoBehaviour
{
    PlayerController playerController;
    LevelGenerator levelGenerator;
    GameManagerCine gameManager;
    Transform playerTransform;
    public float followSpeed = .5f;
    public float speed = 0f;
    public Transform firePoint;
    public GameObject bulletPrefab;
    public int health = 10;
    public int currentHealth;
    public Image[] healthBars;
    AudioManager audioManager;
    [SerializeField] AudioClip explosionSfx;
    [SerializeField] AudioClip hurtSfx;
    Animator anim;
    void Start()
    {
        if (!GameObject.Find("Player").TryGetComponent<PlayerController>(out playerController))
        {
            Debug.LogError("PlayerController not found!");
        }
        else
        {
            playerTransform = playerController.transform;
        }
        levelGenerator = GameObject.Find("LevelGenerator").GetComponent<LevelGenerator>();
        gameManager = GameObject.Find("GameManagerCine").GetComponent<GameManagerCine>();
        currentHealth = health;
        anim = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
    }
    void Update()
    {
        speed = playerController.speed;

        if (!playerController.isHit && !playerController.gameOver)
        {
            Vector3 movement = speed * Time.deltaTime * Vector3.right;
            transform.Translate(movement);
        }
        else if (playerController.isHit && playerController.gameOver)
        {
            Vector3 movement = Vector3.zero;
            transform.Translate(movement);
        }

        if (!levelGenerator.boardWalkerIsOn)
        {
            return;
        }
        else
        {
            float newYPosition = Mathf.Lerp(transform.position.y, playerTransform.position.y + 0.1f, followSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newYPosition, transform.position.z);
        }
    }
    public void Shoot()
    {
        audioManager.PlayAudio(explosionSfx);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    public void UpdateHealthUI()
    {
        for (int i = 0; i < healthBars.Length; i++)
        {
            if (i < currentHealth)
            {
                healthBars[i].gameObject.SetActive(true);
            }
            else
            {
                healthBars[i].gameObject.SetActive(false);
            }
        }
    }
    public void TakeHit()
    {
        audioManager.PlayAudio(hurtSfx);
        anim.Play("Hurt");
        currentHealth--;
        UpdateHealthUI();
        if (currentHealth <= 0)
        {
            anim.Play("Die");
            levelGenerator.boardWalkerIsOn = false;
        }
    }
    public void WalkerPosReturn()
    {
        transform.position = new Vector3(playerTransform.position.x + 3f, playerTransform.position.y + 3.5f, transform.position.z);
        gameManager.blueCard = 0;
        gameManager.blueCardText.text = gameManager.blueCard.ToString();
        anim.SetBool("on", false);
        anim.Play("Idle");
        playerController.StartMovePlayerToLevelGenerator();
    }
}
