using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    Animator anim;
    AudioManager audioManager;
    [SerializeField] AudioClip explosionSfx;
    [SerializeField] AudioClip deathSfx;
    bool animTrigger = false;
    bool isDead = false;
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
        isDead = false;
    }
    public void Shoot()
    {
        audioManager.PlayAudio(explosionSfx);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!animTrigger && !isDead)
            {
                animTrigger = true;
                StartCoroutine(ShootAnimation());
            }
        }
    }
    IEnumerator ShootAnimation()
    {
        anim.Play("Attack");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        anim.Play("Idle");
        yield return new WaitForSeconds(1);
        animTrigger = false;
    }

     public void TakeDamage()
    {
        {
            isDead = true;
            anim.Play("Die");
            audioManager.PlayAudio(deathSfx);
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
