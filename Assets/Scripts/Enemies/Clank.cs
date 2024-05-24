using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clank : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    Animator anim;
    AudioManager audioManager;
    [SerializeField] AudioClip shootSfx;
    void Start()
    {
        audioManager = GetComponent<AudioManager>();
        anim = GetComponent<Animator>();
    }
    public void Shoot()
    {
        audioManager.PlayAudio(shootSfx);
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }
    void OnTriggerEnter2D(Collider2D other)
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
