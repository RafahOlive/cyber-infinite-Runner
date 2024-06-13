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
                Clank clankController = other.gameObject.GetComponent<Clank>();
                clankController.TakeDamage();
            }
            if (other.gameObject.name == "Bomber")
            {
                Bomber bomberbobController = other.gameObject.GetComponent<Bomber>();
                bomberbobController.TakeDamage();
            }
        }

    }
}
