using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            if (other.gameObject.name == "Bob")
            {
                Bob bobController = other.gameObject.GetComponent<Bob>();
                bobController.TakeDamage();
            }
            if (other.gameObject.name == "Slider")
            {
                Destroy(other.gameObject);
            }
            if (other.gameObject.name == "Clank")
            {
                Destroy(other.gameObject);
            }
            if (other.gameObject.name == "Bomber")
            {
                Destroy(other.gameObject);
            }
        }

    }
}
