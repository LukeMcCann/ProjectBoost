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

    // Game States 

    enum State { Alive, Dying, Transcending };
    State state = State.Alive;

    // Tweakables

    [SerializeField] private float mainThrust = 10f, rcsThrust = 100f;
    private float rotationThisFrame;

    // Level Tracking

    private int currentSceneIndex;

    // Resources

    public SimpleHealthBar fuelBar;
    [SerializeField] float fuel = 100;
    [SerializeField] float consumptionRate = 5;

    void Start()
    {
        this.currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        this.rigidBody = GetComponent<Rigidbody>();
        this.audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey(KeyCode.Space)) // Can thrust whilst rotating
        {
            if(!fuelIsEmpty())
            {
                EngageThrusters();
                PlayThrusterSound();
            }
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
                AddFuel();
                break;
            case "Finish":
                state = State.Transcending;
                Invoke("LoadNextLevelByIndex", 1f);
                break;
            default:
                state = State.Dying;
                Invoke("ResetLevel", 1f);
                break;
        }
    }


    // Progression Methods

    private void ResetLevel()
    {
        SceneManager.LoadScene(currentSceneIndex);
    }

    private void LoadNextLevelByIndex()
    {
        if(currentSceneIndex == SceneManager.sceneCountInBuildSettings-1)
        {
            print("End of Game!");
        }
        else
        {
            int nextScene = currentSceneIndex + 1;
            SceneManager.LoadScene(nextScene);
        }
    }

    // Fuel Control

    private void DepleteFuel()
    {
        if(fuel > 0)
        {
            this.fuel -= this.consumptionRate * Time.deltaTime;
            this.fuelBar.UpdateBar(fuel, 100);
        }
    }

    private void AddFuel()
    {
        if(this.fuel < 1f)
        {
            this.fuel += (this.consumptionRate*2)* Time.deltaTime;
            this.fuelBar.UpdateBar(fuel, 100);
        }
    }

    private bool fuelIsEmpty()
    {
        if(fuel <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Thrusters

    private void EngageThrusters()
    {
        this.rigidBody.AddRelativeForce(Vector3.up * this.mainThrust);
        DepleteFuel();
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
