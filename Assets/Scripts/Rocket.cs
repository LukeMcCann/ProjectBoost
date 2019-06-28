using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

/**
 * Rocket

 * Author: Luke McCann
 * 
 * handles rocket controls.
 * 
 * While the keyord 'this' is not neccessary for all cases in this file, 
 * I like to be explicit for case of clarity and in case of future changes
 * (e.g. a local var of the same name is placed).
 */ 
public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    [SerializeField] private float mainThrust = 10f;
    [SerializeField] private float rcsThrust = 100f;
    private float rotationThisFrame;

    private int initialLevelIndex = 0;
    private int currentLevelIndex;

    void Start()
    {
        this.rigidBody = GetComponent<Rigidbody>();
        this.audioSource = GetComponent<AudioSource>();
        currentLevelIndex = 0;
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
            this.audioSource.Stop();
        }
        ListenForRotate();
    }

    // Special tags are friendly
    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Fuel":
                print("Acquired Fuel!");
                break;
            case "Finish":
                print("You win!");
                LoadNextLevelByIndex();
                break;
            default:
                print("You are dead!");
                ResetProgress();
                break;
        }
    }


    // Progression Methods

    private void ResetProgress()
    {
        currentLevelIndex = initialLevelIndex;
        SceneManager.LoadScene(currentLevelIndex);
    }

    private void LoadNextLevelByIndex()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        int previousLevelIndex = currentLevelIndex - 1;

        if (nextLevelIndex > SceneManager.sceneCount)
        {
            SceneManager.LoadScene(previousLevelIndex);
            print("End of game!");
        }
        else
        {
            SceneManager.LoadScene(nextLevelIndex);
        }
    }


    // Thrusters

    private void EngageThrusters()
    {
        this.rigidBody.AddRelativeForce(Vector3.up * this.mainThrust);
    }

    private void PlayThrusterSound()
    {
        if (!audioSource.isPlaying)
        {
            this.audioSource.Play();
        }
    }


    // Rotational Controls

    private void ListenForRotate()
    {
        this.rigidBody.freezeRotation = true; // take manual control of rotation

        this.rotationThisFrame = this.rcsThrust * Time.deltaTime;
        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            RotateLeft(rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        {
            RotateRight(rotationThisFrame);
        }
        this.rigidBody.freezeRotation = false; // Resume natural physics rotation controls
    }

    private void RotateLeft(float rotationThisFrame)
    {
        transform.Rotate(Vector3.forward * rotationThisFrame);
    }

    private void RotateRight(float rotationThisFrame)
    {
        transform.Rotate(-Vector3.forward * rotationThisFrame);
    }
}
