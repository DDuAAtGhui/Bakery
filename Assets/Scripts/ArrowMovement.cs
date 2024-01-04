using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovement : MonoBehaviour
{
    [SerializeField] float speed = 3f;
    [SerializeField] float distance = 2f;
    Vector3 initialPosition;
    private void Start()
    {
        initialPosition = transform.position;
    }
    private void Update()
    {
        transform.position = initialPosition +
            new Vector3(transform.position.x,
            Mathf.PingPong(Time.time * speed, distance),
            transform.position.z);
    }
}
