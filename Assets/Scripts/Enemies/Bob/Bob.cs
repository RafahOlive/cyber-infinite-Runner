using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    Animator anim;
    AudioManager audioManager;
    [SerializeField] AudioClip bobDeathfx;
    public float speed;

    public PlayerExperience playerExp;

    bool isHit;
    void Start()
    {
        anim = GetComponent<Animator>();
        audioManager = GetComponent<AudioManager>();
    }
    void Attack()
    {
        anim.SetTrigger("hit");
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Attack();
        }
    }
    public void TakeDamage()
    {
        if (!isHit)
        {
            isHit = true;
            anim.SetBool("die", true);
            audioManager.PlayAudio(bobDeathfx);

            GameObject playerExperienceGameObject = GameObject.FindGameObjectWithTag("PlayerExperience");
            PlayerExperience playerExp = playerExperienceGameObject.GetComponent<PlayerExperience>();

            if (playerExp != null)
            {
                playerExp.AddXP(50);
            }
            Destroy(gameObject, .5f);
        }

    }
}
