using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] int jumpPower;
    public Transform groundCheck;
    public LayerMask groundLayer;
    [SerializeField] bool isGrounded;
    int maxHealth = 3;
    int currentHealth;
    public Image[] healthBars;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject losePanel;
    public bool gameOver = false;
    void Start()
    {
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.18f, 0.04f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isJumping", true);
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
    void Die()
    {
        anim.SetTrigger("die");
        gameOver = true;
        Invoke(nameof(Lose), 2f);
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }
    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }
    public void Lose()
    {
        Debug.Log("Aconteci");
        losePanel.SetActive(true);
        Time.timeScale = 0f;
    }
}
