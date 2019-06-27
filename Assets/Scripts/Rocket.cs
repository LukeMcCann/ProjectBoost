using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Rocket

 * Author: Luke McCann
 * 
 * handles rocket controls.
 */ 
public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space)) // Can thrust whilst rotating
        {
            EngageThrusters();
            PlayThrusterSound();
        }
        else
        {
            audioSource.Stop();
        }

        ListenForRotate();
    }


    // Thrusters

    private void EngageThrusters()
    {
        rigidBody.AddRelativeForce(Vector3.up);
    }

    private void PlayThrusterSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }


    // Rotational Controls

    private void ListenForRotate()
    {
        rigidBody.freezeRotation = true;
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            RotateRight();
        }
        rigidBody.freezeRotation = false; // Resume natural physics rotation controls
    }

    private void RotateLeft()
    {
        transform.Rotate(Vector3.forward);
    }

    private void RotateRight()
    {
        transform.Rotate(-Vector3.forward);
    }
}
