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
    Rigidbody rigidbody;
    AudioSource audioSource;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
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

        if (Input.GetKey(KeyCode.A)&& !Input.GetKey(KeyCode.D))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            RotateRight();
        }
    }


    // Thrusters

    private void EngageThrusters()
    {
        rigidbody.AddRelativeForce(Vector3.up);
        //        Debug.Log("Thrusters Engaged!");
    }

    private void PlayThrusterSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }


    // Rotational Controls

    private void RotateLeft()
    {
        transform.Rotate(Vector3.forward);
//        Debug.Log("Rotating Left");
    }

    private void RotateRight()
    {
        transform.Rotate(-Vector3.forward);
//        Debug.Log("Rotating Right");
    }
}
