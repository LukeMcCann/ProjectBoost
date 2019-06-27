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

    // Start is called before the first frame update
    void Start()
    {
        this.rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        if(Input.GetKey(KeyCode.Space)) // Can thrust whilst rotating
        {
            EngageThrusters();
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


    // Control Methods

    private void EngageThrusters()
    {
        rigidbody.AddRelativeForce(Vector3.up);
//        Debug.Log("Thrusters Engaged!");
    }

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
