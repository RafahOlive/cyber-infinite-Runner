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
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
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
            anim.SetBool("shoot", true);
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            anim.SetBool("shoot", false);
        }
    }
}
