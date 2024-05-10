using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 2f;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        transform.eulerAngles = new Vector3(0f, 0f, 45f);
        Vector2 diagonalDirection = new Vector2(-1f, -1f).normalized;
        rb.velocity = diagonalDirection * speed;
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
