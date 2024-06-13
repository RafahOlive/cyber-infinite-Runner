using System.Collections;
using System.Collections.Generic;

using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    Animator anim;
    [SerializeField] Collider2D playerCol;
    public float speed = 0f;
    public float distanceTraveled = 0f;
    [SerializeField] int jumpPower;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private Vector2 touchStartPos;
    [SerializeField] bool isGrounded;
    public bool isOnPlatform = false;
    private Collider2D platformCollider;
    int maxHealth = 3;
    public int currentHealth;
    public Image[] healthBars;
    public bool gameOver = false;
    public bool isHit = false;
    public bool isAttacking = false;
    public bool isSliding = false;
    public bool isInvincible = false;
    public GameObject levelUpPanel;
    GameManagerCine gameManager;
    [SerializeField] AudioClip jumpSfx;
    [SerializeField] AudioClip punchSfx;
    [SerializeField] AudioClip takeDamageSfx;
    [SerializeField] AudioClip blueCardSfx;
    [SerializeField] AudioClip moneySfx;
    [SerializeField] AudioClip deathAeroladSfx;
    AudioManager audioManager;
    public GameObject aeroLad;
    // [SerializeField] Animator fingerAnimator;
    [SerializeField] Animator readyTextAnimator;
    [SerializeField] GameObject portal;
    [SerializeField] Transform portalPos;
    [SerializeField] Transform playerPortalPos;


    void Start()
    {
        gameManager = GameObject.Find("GameManagerCine").GetComponent<GameManagerCine>();
        // fingerAnimator = GameObject.Find("Point finger").GetComponent<Animator>();
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
    }
    void Update()
    {
        //BUTONS CONFIGURATIONS ON KEYBOARD
        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.F))
        {
            Attack();
        }
        if (isGrounded && Input.GetKeyDown(KeyCode.S))
        {
            Slide();
            StartCoroutine(DisableCollision());
        }
        //TOUCH CONFIGURATION ON MOBILE SCREEN
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector2 touchEndPos = touch.position;
                float deltaY = touchEndPos.y - touchStartPos.y;
                if (deltaY > 0)
                {
                    Jump();
                }
                else if (deltaY < 0)
                {
                    Slide();
                    if (platformCollider != null)
                    {
                        if (!isHit && !isAttacking && isGrounded)
                        {
                            StartCoroutine(DisableCollision());
                        }
                    }
                }
            }
        }
        //VELOCITY CONTROL ON GAME STATS. LIKE GAMEOVER
        if (!isHit && !gameOver)
        {
            Vector3 movement = speed * Time.deltaTime * Vector3.right;
            transform.Translate(movement);

            if (gameManager.gameIsStarted)
            {
                gameManager.bgPrlx2.GetComponent<ParalaxTimer>().speed = 0.2f;
                gameManager.bgPrlx22.GetComponent<ParalaxTimer>().speed = 0.2f;

                gameManager.bgPrlx3.GetComponent<ParalaxTimer>().speed = 0.4f;
                gameManager.bgPrlx33.GetComponent<ParalaxTimer>().speed = 0.4f;

                gameManager.bgPrlx5.GetComponent<ParalaxTimer>().speed = 0.6f;
                gameManager.bgPrlx55.GetComponent<ParalaxTimer>().speed = 0.6f;
            }
            distanceTraveled += Mathf.Abs(movement.x);
        }
        else if (isHit || gameOver)
        {
            Vector3 movement = Vector3.zero;
            transform.Translate(movement);

            gameManager.bgPrlx2.GetComponent<ParalaxTimer>().speed = 0f;
            gameManager.bgPrlx22.GetComponent<ParalaxTimer>().speed = 0f;

            gameManager.bgPrlx3.GetComponent<ParalaxTimer>().speed = 0f;
            gameManager.bgPrlx33.GetComponent<ParalaxTimer>().speed = 0f;

            gameManager.bgPrlx5.GetComponent<ParalaxTimer>().speed = 0f;
            gameManager.bgPrlx55.GetComponent<ParalaxTimer>().speed = 0f;
        }
        //CONTROL IF THE PLAYER IS ON GROUND, FOR JUMPING OR ATTACK
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.18f, 0.04f), CapsuleDirection2D.Horizontal, 0, groundLayer);
        if (anim == null)
        {
            Debug.LogError("Animator is not found on the GameObject!");
        }
        else
        {
            if (isGrounded)
            {
                anim.SetBool("isJumping", false);
            }
            else
            {
                anim.SetBool("isJumping", true);
            }
        }

    }
    public void Jump()
    {
        // if (gameManager.isOnJumpTutorial)
        // {
        //     return;
        // }
        // else
        // {
        // Time.timeScale = 1;
        // fingerAnimator = GameObject.Find("Point finger").GetComponent<Animator>();
        // fingerAnimator.Play("Idle");
        if (gameManager.gameIsStarted && !gameManager.gameIsPaused && !isHit && !isAttacking)
        {
            if (isGrounded && rb.velocity.y <= 0)
            {
                audioManager.PlayAudio(jumpSfx);
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }
        // }
    }
    public void Slide()
    {
        // if (gameManager.isOnSlideTutorial)
        // {
        //     return;
        // }
        // else
        // {
        //     Time.timeScale = 1;
        //     fingerAnimator = GameObject.Find("Point finger").GetComponent<Animator>();
        //     fingerAnimator.Play("Idle");
        if (gameManager.gameIsStarted && !gameManager.gameIsPaused && !isHit && !isAttacking && !isSliding)
        {
            if (isGrounded)
            {
                isSliding = true;
                anim.SetTrigger("slide");
            }
            // }
        }
    }
    public void Attack()
    {
        // if (gameManager.isOnAttackTutorial)
        // {
        //     return;
        // }
        // else
        // {
        // Time.timeScale = 1;
        // fingerAnimator = GameObject.Find("Point finger").GetComponent<Animator>();
        // fingerAnimator.Play("Idle");
        if (gameManager.gameIsStarted && !gameManager.gameIsPaused && !isHit && !isSliding && !isAttacking)
        {
            if (isGrounded)
            {
                isAttacking = true;
                audioManager.PlayAudio(punchSfx);
                anim.SetTrigger("attack");
            }
            // }
        }
    }
    public void ChangeVariableIsAttacking()
    {
        isAttacking = false;
    }
    public void ChangeVariableIsSliding()
    {
        isSliding = false;
    }
    public void ChangeVariableGameIsStarted()
    {
        gameManager.gameIsStarted = true;
    }
    public void TakeDamage(int damageAmount)
    {
        if (isInvincible)
        {
            return;
        }

        if (!isHit)
        {
            if (aeroLad != null && aeroLad.activeSelf)
            {
                audioManager.PlayAudio(deathAeroladSfx);
                aeroLad.GetComponent<Animator>().SetBool("hit", true);
                aeroLad.transform.SetParent(null);

                Rigidbody2D rb = aeroLad.GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;

                Invoke(nameof(ReactivateAeroLad), .5f);
                gameManager.SaveAeroLadState();
            }
            else
            {
                isHit = true;
                isAttacking = false;
                isSliding = false;
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
            }
            StartCoroutine(ResetHitState());
        }
    }
    public void ActivateInvincibility(float duration)
    {
        StartCoroutine(InvincibilityCoroutine(duration));
    }
    IEnumerator InvincibilityCoroutine(float duration)
    {
        isInvincible = true;
        yield return new WaitForSeconds(duration);
        isInvincible = false;
    }
    void ReactivateAeroLad()
    {
        aeroLad.transform.SetParent(transform);
        aeroLad.transform.localPosition = new Vector3(-0.309f, 0.153f, 0f);

        aeroLad.SetActive(false);
        gameManager.SaveAeroLadState();
        aeroLad.GetComponent<Animator>().SetBool("hit", false);

        Rigidbody2D rb = aeroLad.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
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
    public void ReadyText()
    {
        readyTextAnimator.Play("Start");
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
            Destroy(other.gameObject);
        }

        if (other.gameObject.CompareTag("GunToKillBoardWalker"))
        {
            GameObject boardWalkerObject = GameObject.Find("BoardWalker");
            BoardWalker boardWalker = boardWalkerObject.GetComponent<BoardWalker>();
            Destroy(other.gameObject);
            boardWalker.TakeHit();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            platformCollider = collision.collider;
        }
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Debug.Log("Colidi");
            TakeDamage(1);
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            platformCollider = null;
        }
    }
    private IEnumerator DisableCollision()
    {
        if (platformCollider != null)
        {
            Collider2D platformCol = platformCollider.GetComponent<Collider2D>();

            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platformCol);
            yield return new WaitForSeconds(0.5f);
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platformCol, false);
        }
    }
    public void StartMovePlayerToLevelGenerator()
    {
        StartCoroutine(MovePlayerToLevelGenerator());
    }
    IEnumerator MovePlayerToLevelGenerator()
    {
        float actualSpeed = speed;
        speed = 0;
        // while (portal.transform.position.y != playerPortalPos.position.y)
        // {
        //     portal.transform.position = Vector2.MoveTowards(portal.transform.position, playerPortalPos.transform.position, 4f * Time.deltaTime);
        //     yield return null; // Espera até o próximo frame
        // }
        // yield return new WaitForSeconds(5f);
        rb.isKinematic = true;
        playerCol.enabled = false;
        portal.GetComponent<Animator>().Play("In");

        yield return new WaitForSeconds(1f);
        Vector2 playerPortalPos1 = new(portalPos.position.x, transform.position.y);
        while (transform.position.x != playerPortalPos1.x)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPortalPos1, 5f * Time.deltaTime);
            yield return null; // Espera até o próximo frame
        }

        yield return new WaitForSeconds(1f);
        rb.isKinematic = false;
        playerCol.enabled = true;
        portal.GetComponent<Animator>().Play("Out");

        yield return new WaitForSeconds(.5f);
        speed = actualSpeed;
    }
}
