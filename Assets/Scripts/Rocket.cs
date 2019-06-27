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
    private Rigidbody rigidbody;
    private AudioSource audioSource;

    void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if(Input.GetKey(KeyCode.Space)) // Can thrust whilst rotating
        {
            EngageThrusters();
            PlayThrusterSound();
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
        this.audioSource = GetComponent<AudioSource>();
        if (!this.audioSource.isPlaying) // prevent layering audio
        {
            this.audioSource.Play();
        }
        else
        {
            this.audioSource.Stop();
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
