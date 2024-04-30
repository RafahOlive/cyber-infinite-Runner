using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomber : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Shoot()
    {
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Debug.Log("colidi");
            anim.SetTrigger("shoot");
        }
    }
}
