using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    // TODO: remove from inspector later
    [Range(-1, 1)][SerializeField] float movementFactor;

    private Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2;

        float rawSinWave = Mathf.Sin(cycles * tau);
        movementFactor = rawSinWave;

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
