using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobAttack : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            PlayerController playerController = other.gameObject.GetComponent<PlayerController>();
            playerController.TakeDamage(1);
        }
    }
}
