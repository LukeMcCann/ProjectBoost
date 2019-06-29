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
 * This code does not utilise all best practices. This project is a thrown together prototype for
 * the Udemy Course (C# and Unity 3D). 
 */ 
public class Rocket : MonoBehaviour
{
    Rigidbody rigidBody;
    AudioSource audioSource;

    private float rotationThisFrame;

    // Game States 

    enum State { Alive, Dying, Transcending, Falling, Charging};
    State state = State.Alive;

    // Tweakables

    [SerializeField] private float mainThrust = 10f, rcsThrust = 100f;
    [SerializeField] float fuel = 100;
    [SerializeField] float consumptionRate = 5;
    [SerializeField] float refreshRate = 20;

    // Sound Files 

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip explosion;
    [SerializeField] AudioClip success;
    [SerializeField] AudioClip fall;
    [SerializeField] AudioClip rotate;

    // Particles

    [SerializeField] ParticleSystem explode;
    [SerializeField] ParticleSystem completion;
    [SerializeField] ParticleSystem engineFumes;

    // Level Tracking

    private int currentSceneIndex;

    // Resources

    public SimpleHealthBar fuelBar;

    void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        state = State.Alive;
    }

    void Update()
    {
        if (state != State.Dying)
        {
            ProcessInput();
        }
    }

    private void ProcessInput()
    {
        ListenForRotate();
        ListenForFreeFall();
        if (Input.GetKey(KeyCode.Space)) // Can thrust whilst rotating
        {
            if(FuelIsEmpty()) { return; }
            EngageThrusters();
            engineFumes.Play();
            PlayThrusterSound();
        
        }
        else
        {
            audioSource.Stop();
            engineFumes.Stop();
        }
    }

    // Special tags are friendly
    private void OnCollisionEnter(Collision collision)
    {
        if(state == State.Dying) { return; } // ignore collisions when dead
        switch(collision.gameObject.tag)
        {
            case "Friendly":
                // do nothing
                break;
            case "Charger":
                recoverFuel();
                break ;
            case "Fuel":
                print("Acquired Fuel!");
                AddFuel();
                break;
            case "Finish":
                StartSuccessSequence();
                break;
            default:
                StartDeathSequence();
                break;
        }
    }

    // Successful Completion Methods

    private void StartSuccessSequence()
    {
        state = State.Transcending;

        if(state != State.Transcending) { return; }
         PlayTranscendSound();
         completion.Play();
         Invoke("LoadNextLevelByIndex", 2f);

    }

    private void PlayTranscendSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(success);
    }


    // Fall methods

    private void ListenForFreeFall()
    {
        if (FuelIsEmpty())
        {
            state = State.Falling;
            FreeFall();
        }
    }

    private void FreeFall()
    {
        if (state == State.Falling)
        {
            print("Falling");
            RotateRight(rotationThisFrame);
            PlaySpinSound();
            PlayFallSound();
        }
    }

    private void PlayFallSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(fall);
    }

    private void PlaySpinSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(rotate);
    }

    // Death Methods


    private void StartDeathSequence()
    {
        state = State.Dying;
        ExplodeShip();
        Invoke("ResetLevel", 1f);
    }

    private void ExplodeShip()
    {
        if(state != State.Dying) { return; }
        PlayExplosionSound();
        explode.Play();
    }

    private void PlayExplosionSound()
    {
        audioSource.Stop();
        audioSource.PlayOneShot(explosion);
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

    private bool FuelIsEmpty()
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

    public void recoverFuel()
    {
        if (fuel < 100)
        {
            while(fuel < 100)
            {
                this.fuel += this.refreshRate * Time.deltaTime;
                this.fuelBar.UpdateBar(fuel, 100);
            }
        }
    }

    // Thrusters

    private void EngageThrusters()
    {
        this.rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        DepleteFuel();
        engineFumes.Play();
    }

    private void PlayThrusterSound()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(mainEngine);
        }
    }

    private void StopThrusterSound()
    {
        if(audioSource.isPlaying)
        {
            audioSource.Stop();
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

    // Getters and Setter

    public float GetFuel()
    {
        return this.fuel;
    }

    public void SetFuel(float fuel)
    {
        this.fuel = fuel;
    }
}
