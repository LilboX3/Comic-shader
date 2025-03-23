using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpikeRoller : MonoBehaviour
{
    private Rigidbody rb;
    private float rollSpeed = 2f;
    private bool movingForward = true;
    private bool allowedToMove;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = Vector3.right * rollSpeed;
        allowedToMove = true;
    }

    void FixedUpdate()
    {
        if (allowedToMove)
        {
            if (movingForward)
            {
                rb.linearVelocity = Vector3.right * rollSpeed;
            }
            else
            {
                rb.linearVelocity = Vector3.left * rollSpeed;
            }
        } 
        else
        {
            rb.linearVelocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Reverse"))
        {
            movingForward = !movingForward;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(WaitUntilMovementAgain(2f));
        }
    }

    IEnumerator WaitUntilMovementAgain(float seconds)
    {
        allowedToMove = false;
        yield return new WaitForSeconds(seconds);
        allowedToMove = true;
    }
}
