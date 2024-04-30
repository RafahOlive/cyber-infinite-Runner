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
    int currentHealth;
    public Image[] healthBars;
    public bool gameOver = false;
    bool isHit = false;
    public GameObject levelUpPanel;
    [SerializeField] GameObject losePanel;
    GameManagerCine gameManager;
    Animator wallsAnim;
    void Start()
    {
        gameManager = GameObject.Find("GameManagerCam").GetComponent<GameManagerCine>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
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
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }
    }
    public void Attack()
    {
        anim.SetTrigger("attack");
    }

    public void TakeDamage(int damageAmount)
    {
        if (!isHit)
        {
            anim.SetTrigger("isHurt");
            currentHealth -= damageAmount;
            if (currentHealth < 0)
            {
                currentHealth = 0;
            }
            UpdateHealthUI();
            if (currentHealth <= 0)
            {
                Die();
            }
            isHit = true;
            StartCoroutine(ResetHitState());
        }
    }
    IEnumerator ResetHitState()
    {
        yield return new WaitForSeconds(.7f);
        isHit = false;
    }
    void UpdateHealthUI()
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
            gameManager.money++;
            gameManager.moneyText.text = gameManager.money.ToString();
            Destroy(other.gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("CardBlue"))
        {
            gameManager.blueCard++;
            gameManager.blueCardText.text = gameManager.blueCard.ToString();
            wallsAnim = GameObject.Find("WallsAnim").GetComponent<Animator>();
            Destroy(other.gameObject);
        }
    }
    void Die()
    {
        gameOver = true;
        anim.SetTrigger("die");
    }
    public void Lose()
    {
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void SkillSequencialPunch()
    {
        anim.SetBool("attackDouble", true);
        Time.timeScale = 1f;
        levelUpPanel.SetActive(false);
    }
    public void SkillOnePunch()
    {
        anim.SetBool("attackDouble", false);
        Time.timeScale = 1f;
        levelUpPanel.SetActive(false);
    }
    public void SkillNone()
    {
        Time.timeScale = 1f;
        levelUpPanel.SetActive(false);
    }
}
