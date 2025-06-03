using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParallax : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * speed;
        if (transform.position.x < -38f)
        {
            transform.position = new Vector3(42f, transform.position.y, transform.position.z);
        }
    }
}