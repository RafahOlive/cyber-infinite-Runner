using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public float speed = 5f; 

    void Update()
    {
        Vector3 movement = Vector3.right * speed * Time.deltaTime;
        transform.Translate(movement);
    }
}
