using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxTimer : MonoBehaviour
{
    public float speed;
    public float endX;
    public float startX;
    void Update()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        Vector2 spriteSize = spriteRenderer.size;

        spriteSize.x += speed * Time.deltaTime;

        spriteRenderer.size = spriteSize;

        transform.Translate(speed * Time.deltaTime * Vector2.left);
    }

    // private void Update()
    // {
    //     transform.Translate(speed * Time.deltaTime * Vector2.left);

    //     if (transform.position.x <= endX)
    //     {
    //         Vector2 pos = new(startX, transform.position.y);
    //         transform.position = pos;
    //     }
    // }
}
