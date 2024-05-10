using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassThroughPlatform : MonoBehaviour
{
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            player.isOnPlatform = false;

            GetComponent<PlatformEffector2D>().enabled = false;

            StartCoroutine(EnableEffectorAfterDelay(0.1f));

            // Restaura a colisão entre o jogador e a plataforma
            // Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>(), false);
        }
    }

    private IEnumerator EnableEffectorAfterDelay(float delay)
{
    yield return new WaitForSeconds(delay);
    
    // Ativa o effector novamente
    GetComponent<PlatformEffector2D>().enabled = true;
}
}
