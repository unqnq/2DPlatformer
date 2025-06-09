using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveParallax : MonoBehaviour
{
    [SerializeField] private float speed = 0.5f;
    void Update()
    {
        transform.position += Vector3.left * Time.deltaTime * speed;
        if (transform.position.x < -41f)
        {
            transform.position = new Vector3(46f, transform.position.y, transform.position.z);
        }
    }
}