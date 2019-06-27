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
    // Start is called before the first frame update
    void Start()
    {
        
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

    private void EngageThrusters()
    {
        print("Thrust Active");
    }

    private void RotateLeft()
    {
        print("Rotating Left");
    }

    private void RotateRight()
    {
        print("Rotating Right");
    }
}
