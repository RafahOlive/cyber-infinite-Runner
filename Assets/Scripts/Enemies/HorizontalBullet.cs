using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalBullet : MonoBehaviour
{
   public float speed = 2f;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 direction = Vector2.left;
        rb.velocity = direction * speed;
        Destroy(gameObject, 3f);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {  
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.TakeDamage(1);
            Destroy(gameObject);
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {  
            Destroy(gameObject);
        }
    }
}
