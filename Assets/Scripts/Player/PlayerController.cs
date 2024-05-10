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
        //BUTONS CONFIGURATIONS ON KEYBOARD
        if (isGrounded && Input.GetKeyDown(KeyCode.W))
        {
            Jump();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Attack();
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
                    if (platformCollider != null)
                    {
                        StartCoroutine(DisableCollision());
                    }
                }
            }
        }
        //VELOCITY CONTROL ON GAME STATS. LIKE GAMEOVER
        if (!isHit && !gameOver)
        {
            Vector3 movement = speed * Time.deltaTime * Vector3.right;
            transform.Translate(movement);

            distanceTraveled += Mathf.Abs(movement.x);
        }
        else if (isHit || gameOver)
        {
            Vector3 movement = Vector3.zero;
            transform.Translate(movement);
        }
        //CONTROL IF THE PLAYER IS ON GROUND, FOR JUMPING OR ATTACK
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.18f, 0.04f), CapsuleDirection2D.Horizontal, 0, groundLayer);
        if (isGrounded)
        {
            anim.SetBool("isJumping", false);
        }
        else
        {
            anim.SetBool("isJumping", true);
        }
        //MANAGE WALL OF COLLECTABLE FOR PASS THROUGH NEXT LEVEL
        if (gameManager.blueCard == 3)
        {
            wallsAnim = GameObject.Find("WallsAnim").GetComponent<Animator>();
            wallsAnim.SetBool("openWall", true);
        }
    }
    public void Jump()
    {
        if (gameManager.gameIsStarted && !gameManager.gameIsPaused && !isHit && !isAttacking)
        {
            if (isGrounded)
            {
                audioManager.PlayAudio(jumpSfx);
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
            }
        }
    }
    public void Slide()
    {
        if (gameManager.gameIsStarted && !gameManager.gameIsPaused && !isHit && !isAttacking)
        {
            if (isGrounded)
            {
                isSliding = true;
                anim.SetTrigger("slide");
            }
        }
    }
    public void Attack()
    {
        if (gameManager.gameIsStarted && !gameManager.gameIsPaused && !isHit && !isSliding)
        {
            if (isGrounded)
            {
                isAttacking = true;
                audioManager.PlayAudio(punchSfx);
                anim.SetTrigger("attack");
            }
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
    public void TakeDamage(int damageAmount)
    {
        if (!isHit)
        {
            if (aeroLad != null && aeroLad.activeSelf)
            {
                aeroLad.GetComponent<Animator>().SetBool("hit", true);
                aeroLad.transform.SetParent(null);

                Rigidbody2D rb = aeroLad.GetComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Dynamic;

                Invoke(nameof(DeactivateAeroLad), 2);

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
    void DeactivateAeroLad()
    {
        aeroLad.transform.SetParent(transform);
        aeroLad.transform.localPosition = new Vector3(-0.309f, 0.153f, 0f);

        aeroLad.SetActive(false);
        aeroLad.GetComponent<Animator>().SetBool("hit", false);

        Rigidbody2D rb = aeroLad.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        gameManager.SaveAeroLadState();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            platformCollider = collision.collider;
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
        Collider2D platformCol = platformCollider.GetComponent<Collider2D>();

        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platformCol);
        yield return new WaitForSeconds(0.25f);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), platformCol, false);
    }
}
