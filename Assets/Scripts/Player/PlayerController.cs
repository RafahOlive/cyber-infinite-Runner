using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    public float speed = 0f;
    [SerializeField] int jumpPower;
    public Transform groundCheck;
    public LayerMask groundLayer;
    [SerializeField] bool isGrounded;
    int maxHealth = 3;
    public int currentHealth;
    public Image[] healthBars;
    public bool gameOver = false;
    public bool isHit = false;
    public GameObject levelUpPanel;
    GameManagerCine gameManager;
    Animator wallsAnim;
    [SerializeField] AudioClip jumpSfx;
    [SerializeField] AudioClip punchSfx;
    [SerializeField] AudioClip takeDamageSfx;
    [SerializeField] AudioClip blueCardSfx;
    [SerializeField] AudioClip moneySfx;
    AudioManager audioManager;
    public GameObject aeroLad;
    void Start()
    {
        gameManager = GameObject.Find("GameManagerCine").GetComponent<GameManagerCine>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
    }
    void Update()
    {
        //ATTACK BUTONS ON KEYBOARD
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Attack();
        }

        if (!isHit && !gameOver)
        {
            Vector3 movement = speed * Time.deltaTime * Vector3.right;
            transform.Translate(movement);
        }
        else if (isHit || gameOver)
        {
            Vector3 movement = Vector3.zero;
            transform.Translate(movement);
        }

        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.18f, 0.04f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isJumping", true);
        }

        if (gameManager.blueCard == 3)
        {
            wallsAnim = GameObject.Find("WallsAnim").GetComponent<Animator>();
            wallsAnim.SetBool("openWall", true);
        }
    }

    public void Jump()
    {
        if (isGrounded)
        {
            audioManager.PlayAudio(jumpSfx);
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }
    public void Slide()
    {
        if (isGrounded)
        {
            anim.SetTrigger("slide");
        }
    }
    public void Attack()
    {
        audioManager.PlayAudio(punchSfx);
        anim.SetTrigger("attack");
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isHit)
        {
            if (aeroLad != null && aeroLad.activeSelf)
            {
                aeroLad.SetActive(false);
                gameManager.SaveAeroLadState();
            }
            else
            {
                audioManager.PlayAudio(takeDamageSfx);
                anim.SetTrigger("isHurt");
                currentHealth -= damageAmount;
                if (currentHealth < 0)
                {
                    currentHealth = 0;
                }
                UpdateHealthUI();
                if (currentHealth <= 0)
                {
                    gameManager.Die();
                }
                isHit = true;
                StartCoroutine(ResetHitState());
            }
        }
    }
    public void CallLoseOnGameManager()
    {
        gameManager.Lose();
    }
    IEnumerator ResetHitState()
    {
        yield return new WaitForSeconds(.7f);
        isHit = false;
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
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Money"))
        {
            audioManager.PlayAudio(moneySfx);
            gameManager.stageMoney++;
            gameManager.stageMoneyText.text = gameManager.stageMoney.ToString();
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("CardBlue"))
        {
            audioManager.PlayAudio(blueCardSfx);
            gameManager.blueCard++;
            gameManager.blueCardText.text = gameManager.blueCard.ToString();
            wallsAnim = GameObject.Find("WallsAnim").GetComponent<Animator>();
            Destroy(other.gameObject);
        }
    }
}
