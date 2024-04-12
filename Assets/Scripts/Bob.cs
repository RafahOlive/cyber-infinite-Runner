using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bob : MonoBehaviour
{
    Animator anim;
    public float speed;
    bool canMove = true;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (canMove)
        {
            transform.Translate(speed * Time.deltaTime * Vector2.left);
        }
        else
        {
            transform.Translate(0.03f * Time.deltaTime * Vector2.left);
        }
    }
    void Attack()
    {
        canMove = false;
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
        anim.SetBool("die", true);
        transform.Translate(0.01f * Time.deltaTime * Vector2.left);
        Destroy(gameObject, .5f);
    }
}
