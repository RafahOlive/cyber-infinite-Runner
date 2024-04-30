using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    Animator anim;
     void Start()
    {
        anim = GetComponent<Animator>();
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
}
